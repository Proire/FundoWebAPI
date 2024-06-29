using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace UserModelLayer
{
    public class UserModel
    {
        [DefaultValue("John")]
        public string Name { get; set; } = "";

        [DefaultValue("dummy_username")]
        public string UserName { get; set; } = "";

        [DefaultValue("dummy_password")]
        public string Password { get; set; } = "";

        [DefaultValue("1234567890")]
        public string PhoneNumber { get; set; } = "1234567890";

        [DefaultValue("user")]
        public string Role { get; set; } = "";

        [DefaultValue("ABC@gmail.com")]
        public string Email { get; set; } = "";
    }
}
