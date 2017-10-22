using DentMex.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DentMex.Logic
{
    public interface IVisitService
    {
        bool Save2Visit(int dentistId, int pateintId, DateTime dayOfVisit, int timeOfVisit, string makeServices);
        List<Visit> GetVisistByDentist(int id);
        List<Visit> GetUnavaiableVisits(int dentistdId, DateTime date);
        List<Visit> GetVisitsByPatient(int id);
        Visit GetVisitById(int id);

        bool ChangeVisitState(int visitId, int state);
        bool EditSave2Visit(int visitId, int dentistId, int pateintId, DateTime dayOfVisit, int timeOfVisit, string makeServices);
    }
}
