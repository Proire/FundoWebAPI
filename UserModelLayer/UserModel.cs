using System.ComponentModel.DataAnnotations;

namespace UserModelLayer
{
    public class UserModel
    { 
        public string Name { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string PhoneNumber { get; set; }
    }
}
