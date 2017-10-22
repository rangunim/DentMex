using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DentMex.WebUI.Models.Visit
{
    public class ShowVisitsViewModel
    {
        public List<DentMex.Domain.Visit> Visits { get; set; }

        public string PatientName { get; set; }
        public List<string> DentistsNames { get; set; }
  
    }
}