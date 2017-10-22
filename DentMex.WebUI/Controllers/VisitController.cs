using DentMex.Domain;
using DentMex.Logic;
using DentMex.WebUI.Models.Visit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DentMex.WebUI.Controllers
{
    public class VisitController : AbstractController
    {
        public VisitController(IAccountService AccountService, IVisitService VisitService) : base(AccountService, VisitService) { }
      
        [Authorize(Roles = "Pacjent")]
        [HttpGet]
        public ActionResult Save2Visit()
        {
            Save2VisitViewModel model = new Save2VisitViewModel();
            model.Dentists = AccountService.GetAccountsByUserRole("Stomatolog");
            model.ReturnLink  = Request.ServerVariables ["HTTP_REFERER"];
            model.DateOfVisit  = (DateTime.Now).AddDays(1).Date.ToString("yyyy-MM-dd");

            return View(model);
        }

        [Authorize(Roles = "Pacjent")]
        [HttpPost]
        public ActionResult Save2Visit(Save2VisitViewModel model)
        {
            model.Dentists = AccountService.GetAccountsByUserRole("Stomatolog");
            bool isError = false;
            string message = "Nie wybrano daty!";

            if(model.DateOfVisit != null && ModelState.IsValid)
            {
                message = "";
                int dentistId = model.DentistId;
                int pateintId =  AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name)).AccountId;
                DateTime dateOfVisit = DateTime.Parse(model.DateOfVisit);
                int timeOfVisit = Convert.ToInt32(model.TimeOfVisitValue);
                
                if(dateOfVisit.CompareTo(DateTime.Now) < 0)
                {
                    isError = true;
                    message = "Podana data jest błędna (wcześniejsza niż dziś)";
                }

                List<Visit>  incorrectVisits = VisitService.GetUnavaiableVisits(dentistId, dateOfVisit);
                    incorrectVisits = incorrectVisits.Where(x => x.TimeOfVisit == timeOfVisit).ToList();

                    if(incorrectVisits.Count > 0)
                    {
                        isError = true;
                        message += "Podany termin jest już zajęty!";
                    }
                    else if(!isError)
                    {
                        isError = !VisitService.Save2Visit(dentistId, pateintId, dateOfVisit, timeOfVisit, model.MakeServices);
                        if(isError)
                        {
                            message += "Wystapil blad podczas zapisu!. Spróbuj ponownie";
                        }
                        else
                        {
                            message = "Pomyślnie zapisnao na wizytę!";
                        }
                    }
            }


            ViewBag.IsError = isError;
            ViewBag.Message = message;
            return View(model);
        }

        [Authorize(Roles = "Pacjent")]
        public ActionResult ShowVisits()
        {
            List<Visit> visits = VisitService.GetVisitsByPatient(AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name)).AccountId);
            List<string> dentistsNames = new List<string>();
            foreach(Visit v in visits)
            {
                dentistsNames.Add("dr " + AccountService.GetAccountById(v.DentistId).FullName);
            }

            ShowVisitsViewModel model = new ShowVisitsViewModel
            {
                DentistsNames = dentistsNames,
                Visits = visits,
                PatientName = AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name)).FullName
            };
            return View(model);
        }

        [Authorize(Roles = "Pacjent")]
        public ActionResult AcceptVisit(string id)
        {
            VisitService.ChangeVisitState(Convert.ToInt32(id), 2);
            TempData ["isError"] = false;
            TempData ["message"] = "Zaakceptowano wizytę o id = " + id + "!";
            return RedirectToAction("ShowVisits");
        }

        [Authorize(Roles = "Pacjent")]
        public ActionResult DeleteVisit(string id)
        {
            VisitService.ChangeVisitState(Convert.ToInt32(id), 4);
            TempData ["isError"] = false;
            TempData ["message"] = "Usunięto wizytę o id = " + id + "!";
            return RedirectToAction("ShowVisits");
        }

        [Authorize(Roles = "Pacjent")]
        public ActionResult EditVisit(int id)
        {
            Visit v = VisitService.GetVisitById(id);
            Save2VisitViewModel model = new Save2VisitViewModel
            {
                DentistId = v.DentistId,
                Dentists = AccountService.GetAccountsByUserRole("Stomatolog"),
                ReturnLink  = Request.ServerVariables ["HTTP_REFERER"],
                DateOfVisit = v.DateOfVisit.Value.ToString("yyyy-MM-dd"),
                MakeServices = v.MakeServices,
                VisitId = id
            };

            return View("Save2Visit", model);
        }

        [Authorize(Roles = "Pacjent")]
        [HttpPost]
        public ActionResult EditVisit(Save2VisitViewModel model,int visitId)
        {
            model.Dentists = AccountService.GetAccountsByUserRole("Stomatolog");
            
            bool isError = false;
            string message = "Nie wybrano daty!";

            if(model.DateOfVisit != null && ModelState.IsValid)
            {
                message = "";
                int dentistId = model.DentistId;
                int pateintId = AccountService.GetAccountByLogin(GetIdFromIdentity(HttpContext.User.Identity.Name)).AccountId;
                DateTime dateOfVisit = DateTime.Parse(model.DateOfVisit);
                int timeOfVisit = Convert.ToInt32(model.TimeOfVisitValue);

                if(dateOfVisit.CompareTo(DateTime.Now) < 0)
                {
                    isError = true;
                    message = "Podana data jest błędna (wcześniejsza niż dziś)";
                }

                List<Visit> incorrectVisits = VisitService.GetUnavaiableVisits(dentistId, dateOfVisit);
                incorrectVisits = incorrectVisits.Where(x => x.TimeOfVisit == timeOfVisit).ToList();

                if(incorrectVisits.Count > 0)
                {
                    isError = true;
                    message += "Podany termin jest już zajęty!";
                }
                else if(!isError)
                {
                    isError = !VisitService.EditSave2Visit(visitId, dentistId, pateintId, dateOfVisit, timeOfVisit, model.MakeServices);
                    if(isError)
                    {
                        message += "Wystapil blad podczas edycji!. Spróbuj ponownie";
                    }
                    else
                    {
                        message = "Pomyślnie zedytowano na wizytę!";
                    }
                }
            }


            ViewBag.IsError = isError;
            ViewBag.Message = message;
            return View("Save2Visit",model);

        }
    }
}
