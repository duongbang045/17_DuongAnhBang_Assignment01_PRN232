using System.Text.Json;
using FUNewsManagement.Web.Models;

namespace FUNewsManagement.Web.Helpers
{
    public static class SessionExtensions
    {
        public static void SetUserSession(this ISession session, UserSession user)
        {
            session.SetString("CurrentUser", JsonSerializer.Serialize(user));
        }

        public static UserSession GetUserSession(this ISession session)
        {
            var data = session.GetString("CurrentUser");
            return data == null ? null : JsonSerializer.Deserialize<UserSession>(data);
        }

        public static void ClearUserSession(this ISession session)
        {
            session.Remove("CurrentUser");
        }
    }
}
