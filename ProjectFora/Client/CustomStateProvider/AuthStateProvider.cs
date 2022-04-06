using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace ProjectFora.Client.CustomStateProvider
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorage;

        public AuthStateProvider(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var state = new AuthenticationState(new ClaimsPrincipal());

            string token = await _localStorage.GetItemAsStringAsync("token");
            if (!string.IsNullOrEmpty(token))
            {
                var identity = new ClaimsIdentity(new[]
                {
                new Claim(ClaimTypes.Name, token)
                }, "test authentication type");

                state = new AuthenticationState(new ClaimsPrincipal(identity));
            }

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }
    }
}
