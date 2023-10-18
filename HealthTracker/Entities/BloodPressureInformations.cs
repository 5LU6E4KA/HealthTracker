//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HealthTracker.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class BloodPressureInformations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public BloodPressureInformations()
        {
            this.VitalSigns = new HashSet<VitalSigns>();
        }
    
        public int BloodPressureInformationID { get; set; }
        public Nullable<System.TimeSpan> MeasurementTimeBloodPressure { get; set; }
        public int SystolicPressure { get; set; }
        public int DiastolicPressure { get; set; }
        public string BodyPosition { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VitalSigns> VitalSigns { get; set; }
    }
}