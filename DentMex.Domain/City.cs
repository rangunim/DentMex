//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DentMex.Domain
{
    using System;
    using System.Collections.Generic;
    
    public partial class City
    {
        public City()
        {
            this.Account = new HashSet<Account>();
        }
    
        public short CityId { get; set; }
        public string CityName { get; set; }
    
        public virtual ICollection<Account> Account { get; set; }
    }
}
