using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hemtenta_Alisina_Housela.blog
{
    public class Blog : IBlog
    {
        User user;
        public bool UserIsLoggedIn
        {
            get
            {
                    return user != null;
            }
        }
        public IAuthenticator Authenticator { get; set; }
        public void LoginUser(User u)
        {
            if (u == null)
            {
                throw new UserNullException();
            }

            var user = Authenticator.GetUserFromDatabase(u.Name);

            if (user.Password == u.Password)
            {
                this.user = user;
            }
        }

        public void LogoutUser(User u)
        {
            if (u == null)
            {
                throw new UserNullException();
            }

            this.user = null;
        }

        public bool PublishPage(Page p)
        {
            if (p == null || string.IsNullOrEmpty(p.Title) || string.IsNullOrEmpty(p.Content))
            {
                throw new PageException();
            }

            if (!UserIsLoggedIn)
            {
                return false;
            }
            return true;
        }

        public int SendEmail(string address, string caption, string body)
        {
            if (String.IsNullOrEmpty(address) || String.IsNullOrEmpty(caption) || String.IsNullOrEmpty(body))
            {
                return 0;
            }
            if (!UserIsLoggedIn)
            {
                return 0;
            }
            return 1;
        }

    }
    // Implementera IBlog när du skrivit testerna
    public interface IBlog
    {
        // Försöker logga in en användare. Man kan
        // se om inloggningen lyckades på property
        // UserIsLoggedIn.
        // Kastar ett exception om User är null.
        void LoginUser(User u);

        // Försöker logga ut en användare. Kastar
        // exception om User är null.
        void LogoutUser(User u);

        // True om användaren är inloggad (behöver
        // inte testas separat)
        bool UserIsLoggedIn { get; }

        // För att publicera en sida måste Page vara
        // ett giltigt Page-objekt och användaren
        // måste vara inloggad.
        // Returnerar true om det gick att publicera,
        // false om publicering misslyckades och
        // exception om Page har ett ogiltigt värde.
        bool PublishPage(Page p);

        // För att skicka e-post måste användaren vara
        // inloggad och alla parametrar ha giltiga värden.
        // Returnerar 1 om det gick att skicka mailet,
        // 0 annars.
        int SendEmail(string address, string caption, string body);
    }

    // Förbjudet att implementera!!
    public interface IAuthenticator
    {
        // Söker igenom databasen efter en användare med
        // namnet "username". Returnerar ett giltigt
        // User-objekt om den hittade en användare,
        // null annars.
        User GetUserFromDatabase(string username);
    }

    public class User
    {
        public string Name { get; set; }
        public string Password { get; set; }

        public User(string name)
        {
            this.Name = name;
            this.Password = "guest";
        }
    }

    public class Page
    {
        public string Title { get; set; }
        public string Content { get; set; }
    }

    public class UserNullException : Exception { }

    public class PageException : Exception { }
}
