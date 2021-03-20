using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Models {
    public class Account {
        string username;
        string password;
        string fullname;
        string address;
        int age;
        string gender;
        string roleId;

        public Account() {
        }

        public Account(string username, string password, string fullname, string address, int age, string gender, string roleId) {
            this.Username = username;
            this.Password = password;
            this.Fullname = fullname;
            this.Address = address;
            this.Age = age;
            this.Gender = gender;
            this.RoleId = roleId;
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Fullname { get => fullname; set => fullname = value; }
        public string Address { get => address; set => address = value; }
        public int Age { get => age; set => age = value; }
        public string Gender { get => gender; set => gender = value; }
        public string RoleId { get => roleId; set => roleId = value; }
    }
}
