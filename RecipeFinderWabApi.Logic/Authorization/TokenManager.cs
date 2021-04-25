using MlkPwgen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Logic.Authorization
{
    public static class TokenManager
    {
        private static List<UserTokens> registeredTokens = new List<UserTokens>();
        private static List<AuthorizingTokens> authorizationTokens = new List<AuthorizingTokens>();

        public static string GetUserIdByAccessToken(string accessToken)
        {
            return registeredTokens.FirstOrDefault(x => x.AccessToken.Value == accessToken)?.UserId;
        }

        public static string RegisterUser(string userId)
        {
            if (registeredTokens.Any(x => x.UserId == userId))
            {
                registeredTokens.RemoveAll(x => x.UserId == userId);
            }

            AuthorizingTokens authTokens = new AuthorizingTokens();

            authTokens.UserId = userId;
            authTokens.AuthorizationToken = new AuthorizationToken(GenerateAuthorizationToken(userId));

            authorizationTokens.Add(authTokens);

            return authTokens.AuthorizationToken.Value;
        }

        private static string GenerateAuthorizationToken(string userId)
        {
            return PasswordGenerator.Generate(14, Sets.Alphanumerics);
        }

        public static void UnregisterUserTokens(string userId, string accessToken = "")
        {
            registeredTokens.RemoveAll(x => (x.UserId == userId || x.AccessToken.Value == accessToken) ||
            x.AccessToken.Expired || x.RefreshToken.Expired);
        }

        public static string[] GetTokens(string userId, string authorizationToken)
        {
            var authToken = authorizationTokens.FirstOrDefault(x => x.UserId == userId);

            if (authToken == null)
            {
                return new string[] { "FAIL", "User has not authorised yet. Authorize first before requesting tokens." };
            }
            else if (authToken.AuthorizationToken.Expired)
            {
                authorizationTokens.RemoveAll(x => x.UserId == userId);
                return new string[] { "FAIL", "Authorization token is expired. You have 30 minutes to request tokens after authorizing the user." };
            }
            else if (authToken.AuthorizationToken.Value != authorizationToken)
            {
                return new string[] { "FAIL", "Authorization tokens dont match." };
            }
            else
            {
                UserTokens tokens = new UserTokens();

                tokens.UserId = userId;
                tokens.RefreshToken = new RefreshToken(GenerateRefreshToken(authorizationToken));
                tokens.AccessToken = new AccessToken(GenerateAccessToken(tokens.RefreshToken.Value));

                registeredTokens.Add(tokens);
                authorizationTokens.RemoveAll(x => (x.UserId == userId) ||
                    x.AuthorizationToken.Expired);

                return new string[] { tokens.RefreshToken.Value, tokens.AccessToken.Value };
            }
        }

        private static string GenerateRefreshToken(string authToken)
        {
            return PasswordGenerator.Generate(32, Sets.Alphanumerics + Sets.Symbols);
        }

        public static bool ValidateAccessToken(string accessToken)
        {
            return registeredTokens.Any(x => x.AccessToken.Value == accessToken && x.AccessToken.Valid);
        }

        private static string GenerateAccessToken(string refreshToken)
        {
            return PasswordGenerator.Generate(48, Sets.Alphanumerics + Sets.Symbols);
        }

        public static string RefreshAccessToken(string refreshToken)
        {
            var knownTokens = registeredTokens.FirstOrDefault(x => x.RefreshToken.Value == refreshToken);

            if (knownTokens != null && knownTokens.RefreshToken.Valid)
            {
                knownTokens.AccessToken = new AccessToken(GenerateAccessToken(refreshToken));

                return knownTokens.AccessToken.Value;
            }
            else
            {
                return "FAIL";
            }
        }

        class UserTokens
        {
            public string UserId { get; set; }
            
            public RefreshToken RefreshToken { get; set; }
            public AccessToken AccessToken { get; set; }
        }

        class AccessToken
        {
            public AccessToken(string value)
            {
                Value = value;

                ExpiratedDate = DateTime.Now.AddHours(2);
            }

            public string Value { get; set; }

            public DateTimeOffset ExpiratedDate { get; set; }
            public bool Expired { get { return ExpiratedDate < DateTime.Now; } }
            public bool Valid { get { return ExpiratedDate > DateTime.Now; } }
        }

        class RefreshToken
        {
            public RefreshToken(string value)
            {
                Value = value;

                ExpiratedDate = DateTime.Now.AddDays(2);
            }

            public string Value { get; set; }

            public DateTimeOffset ExpiratedDate { get; set; }
            public bool Expired { get { return ExpiratedDate < DateTime.Now; } }
            public bool Valid { get { return ExpiratedDate > DateTime.Now; } }
        }

        class AuthorizingTokens
        {
            public string UserId { get; set; }
            
            public AuthorizationToken AuthorizationToken { get; set; }
        }

        class AuthorizationToken
        {
            public AuthorizationToken(string value)
            {
                Value = value;

                ExpiratedDate = DateTime.Now.AddMinutes(30);
            }

            public string Value { get; set; }

            public DateTimeOffset ExpiratedDate { get; set; }
            public bool Expired { get { return ExpiratedDate < DateTime.Now; } }
            public bool Valid { get { return ExpiratedDate > DateTime.Now; } }
        }
    }
}
