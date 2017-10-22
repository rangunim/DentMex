using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DentMex.Domain;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;


namespace DentMex.WebUI.Models.Visit
{
    public class Save2VisitViewModel
    {
        public int VisitId;
        public int DentistId { get; set; }

        [Display(Name = "Wybierz stomatologa")]
        public List<DentMex.Domain.Account> Dentists { get; set; }

        [Display(Name = "Dzień wizyty")]
        [Required(ErrorMessage="To pole jest wymagane!")]
        public string DateOfVisit { get; set; }

        [Display(Name = "Planowane usługi do wykonania")]
        public string MakeServices { get; set; }

        public double Price { get; set; }

        public string ReturnLink { get; set; }

        
        [Display(Name = "Czas wizyty")]
        [Required(ErrorMessage = "To pole jest wymagane!")]
        public string TimeOfVisitValue { get; set; }
        public IEnumerable<SelectListItem> TimeOfVisitValues
        {
            get
            {
                return new []
            {
                new SelectListItem { Value = "9", Text = "09:00" },
                new SelectListItem { Value = "10", Text = "10:00" },
                new SelectListItem { Value = "11", Text = "11:00" },
                new SelectListItem { Value = "12", Text = "12:00" },
                new SelectListItem { Value = "13", Text = "13:00" },
                new SelectListItem { Value = "14", Text = "14:00" },
                new SelectListItem { Value = "15", Text = "15:00" },
                new SelectListItem { Value = "16", Text = "16:00" },
                new SelectListItem { Value = "17", Text = "17:00" },
                new SelectListItem { Value = "18", Text = "18:00" },
                new SelectListItem { Value = "19", Text = "19:00" },
                new SelectListItem { Value = "20", Text = "20:00" },
                new SelectListItem { Value = "21", Text = "21:00" },
            };
            }
        }
    }
}