//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace se.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    
    public partial class Dat
    {
        public Dat()
        {
            this.Coordinates = new HashSet<Coordinates>();
        }
    
        public string Id { get; set; }
        public string Path { get; set; }
        [UIHint("Html")]
        public string Description { get; set; }
    
        public virtual ICollection<Coordinates> Coordinates { get; set; }
    }
}
