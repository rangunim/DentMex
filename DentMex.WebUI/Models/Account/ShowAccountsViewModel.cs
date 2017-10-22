using System.Collections.Generic;

namespace DentMex.WebUI.Models.Account
{
    public class ShowAccountsViewModel
    {
        public List<DentMex.Domain.Account> Accounts { get; set; }
        public List<string> CityNames { get; set; }
        public List<string> GenderNames { get; set; }
        public List<string> RoleNames { get; set; }
    }
}