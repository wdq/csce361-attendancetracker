//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace AttendanceTracker
{
    using System;
    using System.Collections.Generic;
    
    public partial class RoomDevice
    {
        public System.Guid Id { get; set; }
        public string IpAddress { get; set; }
        public System.Guid RoomId { get; set; }
    
        public virtual Room Room { get; set; }
    }
}