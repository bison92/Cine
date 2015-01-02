using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class SesionExceptionCerrada: Exception
    {
        public long Id { get; set; }
        public SesionExceptionCerrada(long id): base("La sesión está cerrada.")
        {
            this.Id = id;
        }
    }    
}
