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
    
    public partial class TemperatureInformations
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TemperatureInformations()
        {
            this.VitalSigns = new HashSet<VitalSigns>();
        }
    
        public int TemperatureInformationID { get; set; }
        public Nullable<System.TimeSpan> MeasurementTimeTemperature { get; set; }
        public decimal BodyTemperature { get; set; }
        public string MeasurementPlace { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<VitalSigns> VitalSigns { get; set; }
    }
}