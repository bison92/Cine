using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Interfaces
{
    public interface ISesionRepository
    {
        Sesion Read(long id);
        IEnumerable<KeyValuePair<long,Sesion>> List(long salaId = -1);
        Sesion Update(long id, bool abierta);
    }
}
