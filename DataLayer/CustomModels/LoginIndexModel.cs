using System.ComponentModel.DataAnnotations;
using CommonLayer;

namespace DataLayer
{
    public class LoginIndexModel
    {
        #region Constructor
        public LoginIndexModel()
        {

        }

        public LoginIndexModel(LoginIndexModel obj)
        {
            UserName = obj.UserName;
            Password = obj.Password;
            Email = obj.Email;
        }
        #endregion

        #region Properties
        [Required(ErrorMessage = "Username is required")]
        [StringLength(100)]
        public string UserName { get; set; }

        [DataType(DataType.Password)]
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [StringLength(100)]
        [DataType(DataType.EmailAddress)]
        [RegularExpression(RegularExpression.RegexEmail, ErrorMessage = "Invalid email")]
        public string Email { get; set; }

        public bool RememberMe { get; set; }

        public string Message { get; set; }
        #endregion
    }
}
