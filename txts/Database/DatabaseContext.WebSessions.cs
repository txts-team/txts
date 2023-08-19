using Microsoft.EntityFrameworkCore;
using txts.Types.Entities;

namespace txts.Database;

public partial class DatabaseContext
{
    private async Task<AdminUserEntity?> UserFromSessionToken(string sessionToken)
    {
        WebSessionEntity? webSession = this.WebSessions.FirstOrDefault(s => s.Token == sessionToken);

        return webSession == null
            ? null
            : await this.AdminUsers.FirstOrDefaultAsync(u => u.UserId == webSession.UserId);
    }

    public async Task<AdminUserEntity?> UserFromWebRequest(HttpRequest request)
    {
        string? sessionToken = request.Cookies["token"];
        return sessionToken == null
            ? null
            : await this.UserFromSessionToken(sessionToken);
    }
}