using DentMex.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DentMex.WebUI.Models.Account
{
    public class EditAccountViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        [Key]
        public int AccountId { get; set; }

        [EmailAddress]
        [Display(Name = "Email")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Email { get; set; }

        [Display(Name = "Hasło")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Nowe hasło")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Display(Name = "Powtórz hasło")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Hasła nie są zgodne")]
        public string RepeatPassword { get; set; }

        [Display(Name = "Nr ubezpieczenia")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string InsuranceNumber { get; set; }

        [Display(Name = "Adres")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string Address { get; set; }


        [Display(Name = "Kod pocztowy")]
        [Required(ErrorMessage = "To pole jest wymagane!")]

        public string PostalCode { get; set; }

        public List<City> City { get; set; }

        [Display(Name = "Miasto")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public short CityId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Telefon")]
        public string Phone { get; set; }
       
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string PreviousPage { get; set; }
    }
}