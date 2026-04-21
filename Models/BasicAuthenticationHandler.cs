using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace SchoolDbCoreWbAPI.Models
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly SchoolDbContext _context;

        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            SchoolDbContext context)
            : base(options, logger, encoder)
        {
            _context = context;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            try
            {
                if (!Request.Headers.ContainsKey("Authorization"))
                {
                    return AuthenticateResult.Fail("Missing Authorization Header");
                }
                var authorizationHeader = Request.Headers["Authorization"].ToString();

                if (!AuthenticationHeaderValue.TryParse(authorizationHeader, out var headerValue))
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                if(!"Basic".Equals(headerValue.Scheme, StringComparison.OrdinalIgnoreCase))
                {
                    return AuthenticateResult.Fail("Invalid Authorization Scheme");
                }

                var credentialsBytes = Convert.FromBase64String(headerValue.Parameter!);
                var credentials = System.Text.Encoding.UTF8.GetString(credentialsBytes).Split(':', 2);

                if (credentials.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header");
                }

                var email = credentials[0];
                var password = credentials[1];

                var user = await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
                if (user == null || !PasswordHasher.VerifyPassword(user.PasswordHash, password))
                {
                    return AuthenticateResult.Fail("Invalid Email or Password");
                }

                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
                    new Claim(ClaimTypes.Name, user.Email),
                };
                
                var roles = user.Role.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                foreach (var role in roles)
                {
                    claims.Add(new Claim(ClaimTypes.Role, role));
                }

                var claimsIdentity = new ClaimsIdentity(claims, Scheme.Name);
                var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                var authenticationTicket = new AuthenticationTicket(claimsPrincipal, Scheme.Name);

                return AuthenticateResult.Success(authenticationTicket);
            }
            catch
            {
                return AuthenticateResult.Fail("Error occurred during authentication");
            }
        }
    }
}
