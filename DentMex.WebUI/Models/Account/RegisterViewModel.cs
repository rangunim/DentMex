using DentMex.Domain;
using Student2Course.WebUI.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DentMex.WebUI.Models.Account
{
    public class RegisterViewModel
    {

        [Display(Name = "Login: (*)")]
        [Required(ErrorMessage = "To pole jest wymagane!")]   
        public string AccountLogin { get; set; }

        [Display(Name = "Hasło: (*)")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [StringLength(30, MinimumLength = 6, ErrorMessage = "Hasło musi się składać z  minimum 6 znaków w tym jednej cyfry oraz jednej litery i nie może być dłuższe niż 30 znaków.")]
        [RegularExpression(@"(([0-9]).*([a-zA-z]).*)|(([a-zA-z]).*([0-9]).*)", ErrorMessage = "Hasło musi się składać z  minimum 6 znaków w tym jednej cyfry oraz jednej litery. i nie może być dłuższe niż 30 znaków")]
        public string Password { get; set; }

        [Display(Name = "Powtórz hasło: (*)")]
        [DataType(DataType.Password)]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Compare("Password", ErrorMessage = "Hasła nie są zgodne")]
        public string RepeatPassword { get; set; }

        [EmailAddress]
        [Display(Name = "Email: (*)")]
        [DataType(DataType.EmailAddress)]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        [StringLength(30, MinimumLength = 4, ErrorMessage = "Maksymalna dlugość maila nie może być dłuższa niż 30 znaków.")]

        public string Email { get; set; }


        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Nr ubezpieczenia: (*)")]
        public string InsuranceNumber { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [RegularExpression("([0-9]*)", ErrorMessage = "Pesel musi się składać z samych cyfr!")]       
        [Display(Name = "Pesel: (*)")]
        public string Pesel { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Imię: (*)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Nazwisko: (*)")]
        public string LastName { get; set; }

        public List<Gender> Gender { get; set; }

        [Display(Name = "Płeć:")]
        public short? GenderId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Adres: (*)")]
        public string Address { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [RegularExpression("([0-9][0-9]-[0-9][0-9][0-9])", ErrorMessage = "Kod musi być w postaci 00-000!")]      
        [Display(Name = "Kod pocztowy: (*)")]
        public string PostalCode { get; set; }

        public List<City> City { get; set; }

        [Display(Name = "Miasto:")]
        public short CityId { get; set; }

        [Required(ErrorMessage = "To pole jest wymagane!")]
        [Display(Name = "Telefon: (*)")]
        public string Phone { get; set; }

        [IsTrue(ErrorMessage = "Aby zarejestrować się w serwisie, musisz zaakcpetować regulamin.")]
        public bool IsAcceptRule { get; set; }
    
    }
}