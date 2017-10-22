using System.Collections.Generic;

namespace DentMex.WebUI.Models.Account
{
    public class EditPermissionsViewModel
    {
        [System.Web.Mvc.HiddenInput(DisplayValue = false)]
        public string AccountLogin { get; set; }
        public List<DentMex.Domain.UserRole> UserRoleList { get; set; }
        public short UserRoleId { get; set; }
        public string PreviousPage { get; set; }
    }
}