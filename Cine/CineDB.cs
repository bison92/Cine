using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class CineDB: DbContext
    {
        public CineDB()
            : base()
        {
            Database.SetInitializer<CineDB>(new CineDBInitializer());
        }

        public DbSet<Sala> Salas { get; set; }
        public DbSet<Sesion> Sesiones { get; set; }
        public DbSet<Venta> Ventas { get; set; }
    }
}
