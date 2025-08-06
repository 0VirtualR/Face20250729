using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared
{
    public class UserDto:BaseDto
    {
        private string account;

        public string Account
        {
            get { return account; }
            set { account = value;OnPropertyChanged(); }
        }
        private string username;

        public string Username
        {
            get { return username; }
            set { username = value; OnPropertyChanged(); }
        }
        private string password;

        public string Password
        {
            get { return password; }
            set { password = value; OnPropertyChanged(); }
        }
        private string token;

        public string Token
        {
            get { return token; }
            set { token = value;OnPropertyChanged(); }
        }

    }
}
