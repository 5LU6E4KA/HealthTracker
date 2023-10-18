using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HealthTracker.DatabaseContext
{
    static class DBContext
    {
        private static readonly Entities.HealthTrackerEntities _context = new Entities.HealthTrackerEntities();

        public static Entities.HealthTrackerEntities Context => _context;
    }
}
