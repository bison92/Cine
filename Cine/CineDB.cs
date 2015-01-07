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

        public virtual DbSet<Sala> Salas { get; set; }
        public virtual DbSet<Sesion> Sesiones { get; set; }
        public virtual DbSet<Venta> Ventas { get; set; }
    }
}
