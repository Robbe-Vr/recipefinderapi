using RecipeFinderWebApi.Exchange.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Auth
{
    public static class AuthManager
    {
        private static List<SignedInToken> signedInTokens = new List<SignedInToken>();

        public static void SignIn(User user)
        {
            string accessToken = GenerateAccessToken();

            if (!signedInTokens.Any(s => s.UserId == user.Id))
            {
                signedInTokens.Add(new SignedInToken(user, accessToken));
            }
            else
            {
                signedInTokens.First(s => s.UserId == user.Id).ResetToken(accessToken);
            }
        }

        public static string GetToken(string userId)
        {
            SignedInToken token = signedInTokens.FirstOrDefault(s => s.UserId == userId);

            if (token == null)
            {
                return "Not found.";
            }
            else if (token.Expired)
            {
                return "Expired.";
            }
            else
            {
                return token.AccessToken;
            }
        }

        public static User GetUser(string accessToken)
        {
            SignedInToken token = signedInTokens.FirstOrDefault(s => s.AccessToken == accessToken);

            return token?.User;
        }

        public static string RefreshToken(string userId)
        {
            if (signedInTokens.Any(s => s.UserId == userId))
            {
                string newAccessToken = GenerateAccessToken();

                signedInTokens.First(s => s.UserId == userId).ResetToken(newAccessToken);

                return newAccessToken;
            }
            else return null;
        }

        public static void SignOut(string userId)
        {
            signedInTokens.RemoveAll(s => s.UserId == userId);
        }

        private static string GenerateAccessToken()
        {
            string chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789!@#%&?+";

            Random r = new Random();

            return new string(Enumerable.Repeat(chars, 19).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 9).Select(c => c[r.Next(c.Length)]).ToArray()) + "-" +
                   new string(Enumerable.Repeat(chars, 15).Select(c => c[r.Next(c.Length)]).ToArray());
        }
    }

    public class SignedInToken
    {
        public SignedInToken(User user, string accessToken)
        {
            User = user;
            UserId = user.Id;

            AccessToken = accessToken;

            ExpiratedDate = DateTime.Now.AddHours(2);
        }

        public void ResetToken(string newAccessToken)
        {
            AccessToken = newAccessToken;

            ExpiratedDate = DateTime.Now.AddHours(2);
        }

        public string UserId { get; set; }
        public User User { get; set; }

        public string AccessToken { get; set; }
        public DateTimeOffset ExpiratedDate { get; private set; }

        public bool Expired { get { return ExpiratedDate < DateTime.Now; } }
        public bool Valid { get { return !String.IsNullOrEmpty(UserId) && !String.IsNullOrEmpty(AccessToken) && ExpiratedDate > DateTime.Now; } }
    }
}
