using txts.Types.Entities;

namespace txts.Database;

public partial class DatabaseContext
{
    private AdminUserEntity? UserFromSessionToken(string sessionToken)
    {
        WebSessionEntity? webSession = this.WebSessions.FirstOrDefault(s => s.Token == sessionToken);
        
        return webSession == null
            ? null
            : this.AdminUsers.FirstOrDefault(u => u.UserId == webSession.UserId);
    }

    public AdminUserEntity? UserFromWebRequest(HttpRequest request)
    {
        string? sessionToken = request.Cookies["token"];
        return sessionToken == null ? null : this.UserFromSessionToken(sessionToken);
    }
}