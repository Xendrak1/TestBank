using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

public class BasicAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public BasicAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> opt, ILoggerFactory log, UrlEncoder enc, ISystemClock clk)
        : base(opt, log, enc, clk) { }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        if (!Request.Headers.ContainsKey("Authorization"))
            return Task.FromResult(AuthenticateResult.Fail("Missing Authorization Header"));

        try
        {
            var auth = Request.Headers["Authorization"].ToString();
            if (!auth.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
                return Task.FromResult(AuthenticateResult.Fail("Invalid Scheme"));

            var token = auth.Substring("Basic ".Length).Trim();
            var creds = Encoding.UTF8.GetString(Convert.FromBase64String(token)).Split(':', 2);
            var user = creds[0]; var pass = creds[1];

            if (user == "admin" && pass == "admin123")
            {
                var claims = new[] { new Claim(ClaimTypes.Name, user) };
                var id = new ClaimsIdentity(claims, Scheme.Name);
                var principal = new ClaimsPrincipal(id);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            return Task.FromResult(AuthenticateResult.Fail("Invalid credentials"));
        }
        catch
        {
            return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header"));
        }
    }
}
