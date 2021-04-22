using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.Logic.Authorization
{
    public static class TokenManager
    {
        private static List<UserTokens> registeredTokens = new List<UserTokens>();

        public static string GetUserIdByToken(string accessToken)
        {
            return registeredTokens.FirstOrDefault(x => x.AccessToken == accessToken && x.Valid)?.UserId;
        }

        public static void RegisterUserTokens(string userId, string accessToken, string refreshToken)
        {
            var knownTokens = registeredTokens.FirstOrDefault(x => x.UserId == userId);

            if (knownTokens != null)
            {
                knownTokens.AccessToken = accessToken;
                knownTokens.RefreshToken = refreshToken;
            }
            else
            {
                registeredTokens.Add(new UserTokens()
                {
                    UserId = userId,
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,

                    AccessTokenExpiratedDate = DateTime.Now.AddHours(2),
                });
            }
        }

        public static void UnregisterUserTokens(string userId)
        {
            registeredTokens.RemoveAll(x => x.UserId == userId);
        }

        public static void UpdateAccessToken(string userId, string accessToken)
        {
            var knownTokens = registeredTokens.FirstOrDefault(x => x.UserId == userId);

            if (knownTokens != null)
            {
                knownTokens.AccessToken = accessToken;
            }
            else
            {
                registeredTokens.RemoveAll(x => x.UserId == userId);
            }
        }

        public class UserTokens
        {
            public string UserId { get; set; }
            public string AccessToken { get; set; }
            public string RefreshToken { get; set; }

            public DateTimeOffset AccessTokenExpiratedDate { get; set; }
            public bool Expired { get { return AccessTokenExpiratedDate < DateTime.Now; } }
            public bool Valid { get { return AccessTokenExpiratedDate > DateTime.Now; } }
        }
    }
}
