using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Interfaces
{
    public interface IVentaService
    {
        Venta Create(Venta venta);
        Venta Read(long id);
        IDictionary<long,Venta> List();
        Venta Update(Venta venta);
        Venta Delete(long id);
        double CalcularTotales(long idSesion = -1, long idSala = -1);
        int CalcularEntradas(long idSesion = -1, long idSala = -1);
    }
}
