using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class SesionCerradaException: Exception
    {
        public long Id { get; set; }

        public SesionCerradaException(long id): base("La sesión está cerrada.")
        {
            this.Id = id;
        }
    }    
}
