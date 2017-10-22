using DentMex.Domain;
using DentMex.Logic.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentMex.Logic
{
    public class VisitService : IVisitService
    {
        private readonly IRepository<Account> AccountRepository;
        private readonly IRepository<Visit> VisitRepository;
        public VisitService(IRepository<Account> AccountRepository, IRepository<Visit> VisitRepository)
        {
            this.AccountRepository = AccountRepository;
            this.VisitRepository = VisitRepository;
        }

        public bool Save2Visit(int dentistId, int pateintId, DateTime dayOfVisit, int timeOfVisit, string makeServices)
        {
            try
            {
                Visit result = new Visit()
                {
                    DentistId = dentistId,
                    PateintId = pateintId,
                    DateOfVisit = dayOfVisit,
                    TimeOfVisit = timeOfVisit,
                    MakeServices = makeServices,
                    VisitStateId = 1
                };

                VisitRepository.Add(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool EditSave2Visit(int visitId, int dentistId, int pateintId, DateTime dayOfVisit, int timeOfVisit, string makeServices)
        {
            try
            {
                Visit result = GetVisitById(visitId);
                 result.DentistId = dentistId;
                 result.PateintId = pateintId;
                 result.DateOfVisit = dayOfVisit;
                 result.TimeOfVisit = timeOfVisit;
                 result.MakeServices = makeServices;
               

                VisitRepository.Update(result);
                return true;
            }
            catch
            {
                return false;
            }
        }

         public Visit GetVisitById(int id)
        {
            return VisitRepository.FindBy(x => x.VisitId == id);
        }

        public List<Visit> GetVisistByDentist(int id)
        {
            return VisitRepository.GetBy(x => x.DentistId == id).ToList();
        }
        public List<Visit> GetUnavaiableVisits(int dentistdId, DateTime date)
        {
            return VisitRepository.GetBy(x => x.DentistId == dentistdId && x.DateOfVisit.Value.Equals(date) && x.VisitStateId != 2 ).ToList();
        }

        public List<Visit> GetVisitsByPatient(int id)
        {
            return VisitRepository.GetBy(x => x.PateintId == id && x.VisitStateId != 4).ToList();
        }

        public bool ChangeVisitState(int visitId, int state)
        {
            try
            {
                Visit v = GetVisitById(visitId);
                v.VisitStateId = state;
                VisitRepository.Update(v);
            return true;
            }  
             catch 
            {
                return false;
            }

        }
    }
}
