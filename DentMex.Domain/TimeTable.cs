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
    
    public partial class TimeTable
    {
        public int TimeTableId { get; set; }
        public int AccountId { get; set; }
        public Nullable<bool> Active { get; set; }
        public Nullable<short> DayOfWeek { get; set; }
        public Nullable<short> Time { get; set; }
    
        public virtual Account Account { get; set; }
    }
}
