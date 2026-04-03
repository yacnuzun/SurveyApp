using System.Security.Claims;          
using Microsoft.AspNetCore.Http;       

namespace SurveyApp.Shared.Helpers;
public static class HttpContextExtensions
{
    public static int GetUserId(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("User identity not found.");

        return int.Parse(claim.Value);
    }

    public static string GetUserMail(this HttpContext context)
    {
        var claim = context.User.FindFirst(ClaimTypes.Email)
            ?? throw new UnauthorizedAccessException("User identity not found.");

        return claim.Value;
    }
}