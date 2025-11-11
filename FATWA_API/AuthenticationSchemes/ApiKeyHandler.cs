using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace FATWA_API.AuthenticationSchemes
{

    public class ApiKeyHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly IConfiguration _configuration;

        public ApiKeyHandler(  
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock,
            IConfiguration configuration)
            : base(options, logger, encoder, clock)
        {
            _configuration = configuration;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var apiKey = _configuration.GetSection("FatwaApiKey")?.Get<List<string>>() ?? new List<string>();
            // Check for API key in the header
            if(Request.Headers.ContainsKey("FatwaApiKey"))
            {
                if (Request.Headers.TryGetValue("FatwaApiKey", out var extractedApiKey))
                {
                    if (apiKey.Contains(extractedApiKey))
                    {
                        var claims = new[] { new Claim(ClaimTypes.Name, "FatwaApiKey") };
                        var identity = new ClaimsIdentity(claims, Scheme.Name);
                        var principal = new ClaimsPrincipal(identity);
                        var ticket = new AuthenticationTicket(principal, Scheme.Name);

                        return AuthenticateResult.Success(ticket);
                    }
                }
                return AuthenticateResult.Fail("Invalid API key");
            }
            else
            {
                return AuthenticateResult.Fail("Request does not contain an API Key");
            }
        }
    }
}