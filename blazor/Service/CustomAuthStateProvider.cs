using blazor.Models;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace blazor.Service
{
    public class CustomAuthStateProvider : AuthenticationStateProvider
    {
        private readonly HttpClient httpClient;
        private readonly ISyncLocalStorageService localStorage;
        public string? userId;
        private string? _token;

        public CustomAuthStateProvider(HttpClient httpClient, ISyncLocalStorageService localStorage)
        {
            this.httpClient = httpClient;
            this.localStorage = localStorage;

            var accessToken = localStorage.GetItem<string>("accessToken");
            if (accessToken != null)
            {
                this.httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
                _token = accessToken;
                userId = localStorage.GetItem<string>("userId");
                NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            }
        }

        public async Task<FormResult> LoginAsync(string username, string password)
        {
            try
            {
                LoginDto loginDto = new LoginDto
                {
                    UserName = username,
                    Password = password
                };

                var response = await httpClient.PostAsJsonAsync("api/account/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    var token = await response.Content.ReadAsStringAsync();
                    var jsonObject = JsonSerializer.Deserialize<Dictionary<string, string>>(token);

                    _token = jsonObject["token"];
                    userId = jsonObject["id"];
                    localStorage.SetItem("userId", userId);
                    localStorage.SetItem("accessToken", _token);

                    NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());

                    return new FormResult { Succeeded = true };
                }

                return new FormResult { Succeeded = false, Errors = ["Bad Email or Password"] };
            }
            catch
            {
                return new FormResult { Succeeded = false, Errors = ["Connection Error"] };
            }
        }

        public void Logout()
        {
            _token = null;
            userId = null;
            localStorage.RemoveItem("accessToken");
            localStorage.RemoveItem("userId");
            localStorage.Clear();
            this.httpClient.DefaultRequestHeaders.Authorization = null;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            if (string.IsNullOrEmpty(_token))
                return Task.FromResult(new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity())));

            var claims = JwtParser.ParseClaimsFromJwt(_token);
            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return Task.FromResult(new AuthenticationState(user));
        }

        public async Task<FormResult> RegisterAsync(RegisterDto registerDto)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync("api/account/register", registerDto);
                if (response.IsSuccessStatusCode)
                {
                    var loginResponse = await LoginAsync(registerDto.Username, registerDto.Password);
                    return loginResponse;
                }

                var strResponse = await response.Content.ReadAsStringAsync();
                Console.WriteLine(strResponse);
                var jsonResponse = JsonNode.Parse(strResponse);
                var errorObject = jsonResponse!["errors"]!.AsObject();
                var errorsList = new List<string>();

                foreach (var error in errorObject)
                {
                    errorsList.Add(error.Value![0]!.ToString());
                }

                var formResult = new FormResult
                {
                    Succeeded = false,
                    Errors = errorsList.ToArray()
                };

                return formResult;
            }
            catch { }

            return new FormResult { Succeeded = false, Errors = ["Connection Error"] };
        }
    }

    public static class JwtParser
    {
        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = Convert.FromBase64String(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);

            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }
    }

    public class FormResult
    {
        public bool Succeeded { get; set; }
        public string[] Errors { get; set; } = [];
    }

}
