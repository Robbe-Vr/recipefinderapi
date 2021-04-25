using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.Logic.Authorization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace RecipeFinderWabApi.Logic.signInHandlers
{
    public class SignInHandler
    {
        private Encryption enc;

        private IUserRepo _user_repo;
        private IRoleRepo _role_repo;

        public SignInHandler(IUserRepo user_repo, IRoleRepo role_repo)
        {
            enc = new Encryption();

            _user_repo = user_repo;
            _role_repo = role_repo;
        }

        public User Register(string name, string email, string password, DateTime DOB)
        {
            try
            {
                User newUser = new User() { Name = name, Email = email, Roles = new Role[] { _role_repo.GetByName("Default") }, DOB = DOB, CreationDate = DateTime.Now };

                newUser.Salt = enc.CreateSalt(8);
                newUser.PasswordHashed = enc.HashString(new EncryptionObject() { Text = password, Salt = newUser.Salt })?.Result;

                if (!ValidateUser(newUser)) return null;

                int rowsAffected = _user_repo.Create(newUser);
                if (rowsAffected > 0)
                {
                    var user = _user_repo.GetByName(newUser.Name);

                    return user;
                }
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to register user. Reason: " + e.Message);
            }

            return null;
        }

        private bool ValidateUser(User user)
        {
            return
                (!String.IsNullOrEmpty(user.Name) && user.Name.Length > 2 && user.Name.ToCharArray().Any(c => true)) &&
                (!String.IsNullOrEmpty(user.Email) && user.Email.Length > 4 && user.Email.Count(c => c == '@') == 1 && user.Email.Count(c => c == '.') >= 1 && user.Email.IndexOf("@") > 2 && user.Email.LastIndexOf('.') > user.Email.IndexOf('@')) &&
                (user.DOB <= DateTime.Now.AddYears(-13));
        }

        public string[] Login(string name, string password)
        {
            try
            {
                User targetUser = _user_repo.GetByName(name);
                if (targetUser == null) return null;

                if (targetUser.PasswordHashed != enc.HashString(new EncryptionObject() { Text = password, Salt = targetUser.Salt })?.Result) return null;

                string authToken = TokenManager.RegisterUser(targetUser.Id);

                return new string[] { authToken, targetUser.Id };
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to log in for '" + name + "'. Reason: " + e.Message);
                return null;
            }
        }

        public string[] RequestTokens(string userId, string authToken)
        {
            string[] tokens = TokenManager.GetTokens(userId, authToken);

            if (tokens[0] != "FAIL")
            {
                return tokens;
            }
            else
            {
                return tokens;
            }
        }

        public string RefreshAccessToken(string refreshToken)
        {
            return TokenManager.RefreshAccessToken(refreshToken);
        }

        public User GetUserByAccessToken(string accessToken)
        {
            return _user_repo.GetById(TokenManager.GetUserIdByAccessToken(accessToken));
        }

        public bool ValidateAccessToken(string accessToken)
        {
            return TokenManager.ValidateAccessToken(accessToken);
        }
    }
}
