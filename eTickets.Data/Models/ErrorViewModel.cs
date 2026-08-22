namespace eTickets.Models
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }

        public int? StatusCode { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public string Title => StatusCode switch
        {
            404 => "Page not found",
            403 => "Access denied",
            401 => "Unauthorized",
            400 => "Bad request",
            _ => "Something went wrong"
        };

        public string Message => StatusCode switch
        {
            404 => "The page you're looking for doesn't exist or may have been moved.",
            403 => "You don't have permission to view this page.",
            401 => "You need to sign in to view this page.",
            400 => "We couldn't process that request. Please check the details and try again.",
            _ => "An unexpected error occurred while processing your request. Please try again in a moment."
        };
    }
}
