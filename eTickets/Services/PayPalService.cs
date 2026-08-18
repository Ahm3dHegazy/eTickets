using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using eTickets.Configuration;
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
            purchase_units = new[] { new { amount = new { currency_code = options.Currency, value = total.ToString("0.00", System.Globalization.CultureInfo.InvariantCulture) } } }
        });

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("id").GetString()!;
    }

    public async Task<(bool Completed, string? CaptureId)> CaptureOrderAsync(string payPalOrderId)
    {
        var accessToken = await GetAccessTokenAsync();
        using var request = new HttpRequestMessage(HttpMethod.Post, $"/v2/checkout/orders/{Uri.EscapeDataString(payPalOrderId)}/capture");
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
        request.Headers.Add("PayPal-Request-Id", Guid.NewGuid().ToString());
        request.Content = JsonContent.Create(new { });

        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        var completed = document.RootElement.GetProperty("status").GetString() == "COMPLETED";
        string? captureId = null;
        if (completed && document.RootElement.TryGetProperty("purchase_units", out var units))
            captureId = units[0].GetProperty("payments").GetProperty("captures")[0].GetProperty("id").GetString();
        return (completed, captureId);
    }

    private async Task<string> GetAccessTokenAsync()
    {
        var credentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{options.ClientId}:{options.ClientSecret}"));
        using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/oauth2/token");
        request.Headers.Authorization = new AuthenticationHeaderValue("Basic", credentials);
        request.Content = new FormUrlEncodedContent(new Dictionary<string, string> { ["grant_type"] = "client_credentials" });
        using var response = await httpClient.SendAsync(request);
        response.EnsureSuccessStatusCode();
        using var document = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        return document.RootElement.GetProperty("access_token").GetString()!;
    }
}
