using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Interfaces
{
    public interface IVentaRepository
    {
        Venta Create(Venta venta);
        Venta Read(long id);
        IDictionary<long,Venta> List(long sesionId = -1, bool devuelta = false);
        Venta Update(Venta venta);
        Venta Delete(long id);
    }
}
