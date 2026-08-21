using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Options;

namespace eTickets.Services;

public class PayPalService
{
    private readonly HttpClient httpClient;
    private readonly PayPalOptions options;

    public PayPalService(HttpClient httpClient, IOptions<PayPalOptions> options)
    {
        this.httpClient = httpClient;
        this.options = options.Value;
    }

    public bool IsConfigured => !string.IsNullOrWhiteSpace(options.ClientId) && !string.IsNullOrWhiteSpace(options.ClientSecret);
    public string ClientId => options.ClientId;

    public async Task<string> CreateOrderAsync(decimal total)
    {
        var accessToken = await GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, "/v2/checkout/orders");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("PayPal-Request-Id", Guid.NewGuid().ToString());
        request.Content = JsonContent.Create(new
        {
            intent = "CAPTURE",
            application_context = new
            {
                shipping_preference = "NO_SHIPPING",
                user_action = "PAY_NOW"
            },
            purchase_units = new[] { new { amount = new { currency_code = options.Currency, value = total.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) } } }
        });

        using var response = await httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        EnsureSuccess(response, responseContent);
        using var document = JsonDocument.Parse(responseContent);
        return document.RootElement.GetProperty("id").GetString()!;
    }

    public async Task<(bool Completed, string? CaptureId)> CaptureOrderAsync(string payPalOrderId)
    {
        var accessToken = await GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{Uri.EscapeDataString(payPalOrderId)}/capture");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("PayPal-Request-Id", Guid.NewGuid().ToString());
        request.Headers.Add("Prefer", "return=representation");
        request.Content = JsonContent.Create(new { });

        using var response = await httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        EnsureSuccess(response, responseContent);
        using var document = JsonDocument.Parse(responseContent);
        var completed = document.RootElement.GetProperty("status").GetString() == "COMPLETED";
        string? captureId = null;
        if (completed &&
            document.RootElement.TryGetProperty("purchase_units", out var units) &&
            units.GetArrayLength() > 0 &&
            units[0].TryGetProperty("payments", out var payments) &&
            payments.TryGetProperty("captures", out var captures) &&
            captures.GetArrayLength() > 0 &&
            captures[0].TryGetProperty("id", out var captureIdElement))
        {
            captureId = captureIdElement.GetString();
        }
        return (completed, captureId);
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.ClientId}:{options.ClientSecret}"));
        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["grant_type"] = "client_credentials" });
        using var response = await httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        EnsureSuccess(response, responseContent);
        using var document = JsonDocument.Parse(responseContent);
        return document.RootElement.GetProperty("access_token").GetString()!;
    }

    private static void EnsureSuccess(HttpResponseMessage response, string responseContent)
    {
        if (!response.IsSuccessStatusCode)
            throw new InvalidOperationException($"PayPal API returned {(int)response.StatusCode}: {responseContent}");
    }
}
