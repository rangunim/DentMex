using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DentMex.WebUI.Models.Account
{
    public class LoginViewModel
    {
        [Display(Name = "Login: (*)")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string AccountLogin { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}