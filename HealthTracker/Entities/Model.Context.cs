﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class HealthTrackerEntities : DbContext
    {
        public HealthTrackerEntities()
            : base("name=HealthTrackerEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<BloodPressureInformations> BloodPressureInformations { get; set; }
        public virtual DbSet<FoodInformations> FoodInformations { get; set; }
        public virtual DbSet<Meals> Meals { get; set; }
        public virtual DbSet<PulseInformations> PulseInformations { get; set; }
        public virtual DbSet<SleepInformations> SleepInformations { get; set; }
        public virtual DbSet<SleepModes> SleepModes { get; set; }
        public virtual DbSet<sysdiagrams> sysdiagrams { get; set; }
        public virtual DbSet<TemperatureInformations> TemperatureInformations { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<VitalSigns> VitalSigns { get; set; }
        public virtual DbSet<WaterInformations> WaterInformations { get; set; }
    }
}