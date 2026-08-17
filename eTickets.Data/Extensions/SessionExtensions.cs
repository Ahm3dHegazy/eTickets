using Microsoft.AspNetCore.Http;
using System.Text;

namespace eTickets.Data.Extensions
{
    public static class SessionExtensions
    {
        public static void SetString(this ISession session, string key, string value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(value));
        }

        public static string? GetString(this ISession session, string key)
        {
            return session.TryGetValue(key, out var data)
                ? Encoding.UTF8.GetString(data)
                : null;
        }
    }
}