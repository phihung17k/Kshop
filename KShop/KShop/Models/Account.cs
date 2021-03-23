using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace KShop.Models {
    public class Account{
        string fullname;
        string address;
        int age;
        string gender;
        string role;

        public Account() {
        }

        public Account(string fullname, string address, int age, string gender, string role) {
            this.fullname = fullname;
            this.address = address;
            this.age = age;
            this.gender = gender;
            this.role = role;
        }

        public string Fullname { get => fullname; set => fullname = value; }
        public string Address { get => address; set => address = value; }
        public int Age { get => age; set => age = value; }
        public string Gender { get => gender; set => gender = value; }
        public string Role { get => role; set => role = value; }
    }
}
