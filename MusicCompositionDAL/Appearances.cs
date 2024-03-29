//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MusicCompositionDAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Appearances
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Appearances()
        {
            this.PlayersInAppearances = new HashSet<PlayersInAppearances>();
        }
    
        public int codeA { get; set; }
        public Nullable<System.DateTime> dateA { get; set; }
        public int codePlays { get; set; }
        public int codeCli { get; set; }
        public string pelPlays { get; set; }
        public Nullable<System.TimeSpan> startHour { get; set; }
        public Nullable<System.TimeSpan> endHour { get; set; }
        public int codeComp { get; set; }
        public int codeConductor { get; set; }
        public Nullable<int> cost { get; set; }
    
        public virtual Clients Client { get; set; }
        public virtual Compositions Composition { get; set; }
        public virtual Places Place { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PlayersInAppearances> PlayersInAppearances { get; set; }
        public virtual Players Player { get; set; }
    }
}
