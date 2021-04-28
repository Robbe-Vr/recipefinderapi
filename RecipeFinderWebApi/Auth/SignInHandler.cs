using RecipeFinderWebApi.Exchange.DTOs;
using RecipeFinderWebApi.Exchange.Interfaces.Repos;
using RecipeFinderWebApi.Logic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace RecipeFinderWebApi.UI.Auth
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

        public User Login(string name, string password)
        {
            try
            {
                User targetUser = _user_repo.GetByName(name);
                if (targetUser == null) return null;

                if (targetUser.PasswordHashed != enc.HashString(new EncryptionObject() { Text = password, Salt = targetUser.Salt })?.Result) return null;

                AuthManager.SignIn(targetUser);

                return targetUser;
            }
            catch (Exception e)
            {
                Trace.WriteLine("Failed to log in for '" + name + "'. Reason: " + e.Message);
                return null;
            }
        }

        public void Logout(string accessToken)
        {
            User user = AuthManager.GetUser(accessToken);

            if (user != null)
            {
                AuthManager.SignOut(user.Id);
            }
        }

        public string GetToken(string userId)
        {
            return AuthManager.GetToken(userId);
        }

        public User GetUserByToken(string accessToken)
        {
            return AuthManager.GetUser(accessToken);
        }

        public string ValidateToken(string accessToken)
        {
            User user = GetUserByToken(accessToken);

            if (user == null) return "Not found.";

            return AuthManager.GetToken(user.Id);
        }

        public string RefreshToken(string accessToken)
        {
            User user = GetUserByToken(accessToken);

            if (user != null)
            {
                return AuthManager.RefreshToken(user.Id);
            }

            return "Unkown access token.";
        }
    }
}
