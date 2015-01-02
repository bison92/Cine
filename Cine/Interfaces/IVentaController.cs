using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Interfaces
{
    public interface IVentaController
    {
        Venta Create(Venta venta);
        Venta Read(long id);
        IList<Venta> List();
        Venta Update(Venta venta);
        Venta Delete(long id);        
        double CalcularTotales();
        double CalcularTotalesSala(long salaId);
        double CalcularTotalesSesion(long sesionId);
        int CalcularEntradas();
        int CalcularEntradasSala(long salaId);
        int CalcularEntradasSesion(long sesionId);
    }
}
