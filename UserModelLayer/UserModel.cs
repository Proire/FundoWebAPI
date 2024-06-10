using System.ComponentModel.DataAnnotations;

namespace UserModelLayer
{
    public class UserModel
    { 
        public string Name { get; set; } = string.Empty;

        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public override string ToString()
        {
            return $"{Name}, {UserName}, {Password}, {PhoneNumber}";
        }
    }
}
