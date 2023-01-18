using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Services.DTO
{
    public class UserDto
    {
        public long Id { get; set; }
        public string Name { get;  set; }
        public string Email { get;  set; }
        public string Password { get;  set; }


        public UserDto()
        {}
        public UserDto(long id, string name, string email, string password)
        {
            Id = id;
            Name = name;
            Email = email;
            Password = password;
        }


    }

   
}
