using System;
using static System.Net.WebRequestMethods;

namespace Automation.Config.QA
{
    public class ConfigQA
    {
        private string urlPage = "https://app-web-itkmx-scus-qa-01.azurewebsites.net";

        public string UrlPage { get => urlPage; set => urlPage = value; }

        public class Credentials
        {
            public  string user;
            public  string password;
            public  string rol;

            public Credentials()
            {
                user = "Aumenta";
                password = "Aument4D3v1";
                rol = "";
            }
            //lor27jac
            //FWDB9EZ9

            //jacobtest
            //6HY7UJ5O
            public Credentials(string User, string Password, string Rol)
            {
                user = User;
                password = Password;
                rol = Rol;
            }
        }
    }
}
