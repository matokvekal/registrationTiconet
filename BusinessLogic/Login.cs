//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Business_Logic
{
    using System;
    using System.Collections.Generic;
    
    public partial class Login
    {
        public int userId { get; set; }
        public string userName { get; set; }
        public string Password { get; set; }
        public bool emailConfirm { get; set; }
        public string passwordUnSecure { get; set; }
        public Nullable<int> familyId { get; set; }
    }
}