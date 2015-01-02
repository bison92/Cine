using Cine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class VentaController : IVentaController
    {
        private IVentaService _ventaService;
        public VentaController(IVentaService ventaService)
        {
            _ventaService = ventaService;
        }

        public Venta Create(Venta venta)
        {
            //NMCN
            return this.Create(venta.SesionId, venta.NumeroEntradas);
        }

        private Venta Create(long sesionId, int numeroEntradas)
        {
            Venta venta = new Venta(sesionId, numeroEntradas);
            return _ventaService.Create(venta);
        }

        public Venta Read(long id)
        {
            return _ventaService.Read(id);
        }

        public IList<Venta> List()
        {
            IDictionary<long,Venta> diccionario = _ventaService.List();
            ICollection<Venta> valores = diccionario.Values;
            return valores.ToList();
        }

        public Venta Update(Venta venta)
        {
            //NMCN
            return this.Update(venta.Id, venta.SesionId, venta.NumeroEntradas);
        }
        public Venta Update(long id, long sesionId, int numeroEntradas)
        {
            Venta venta = new Venta { Id = id, SesionId = sesionId, NumeroEntradas = numeroEntradas };
            return _ventaService.Update(venta);
        }

        public Venta Delete(long id)
        {
            return _ventaService.Delete(id);
        }
        public double CalcularTotales()
        {
            return _ventaService.CalcularTotales();
        }
        public double CalcularTotalesSala(long salaId)
        {
            return _ventaService.CalcularTotales(-1, salaId);
        }
        public double CalcularTotalesSesion(long sesionId)
        {
            return _ventaService.CalcularTotales(sesionId);
        }

        public int CalcularEntradas()
        {
            return _ventaService.CalcularEntradas();
        }
        public int CalcularEntradasSala(long salaId)
        {
            return _ventaService.CalcularEntradas(-1, salaId);
        }
        public int CalcularEntradasSesion(long sesionId)
        {
            return _ventaService.CalcularEntradas(sesionId);
        }
    }
}
