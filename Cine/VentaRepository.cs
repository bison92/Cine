using Cine.Interfaces;
using Cine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class VentaRepository : IVentaRepository
    {
        public CineDB Context { get; set; }
        public VentaRepository(CineDB context)
        {
            Context = context;
        }
        public Venta Create(Venta venta)
        {
            Venta nueva = new Venta();
            nueva = Context.Ventas.Add(venta);
            // try 
            Context.SaveChanges();
            return nueva;
        }

        public Venta Read(long id)
        {
            Venta venta = null;
            venta = Context.Ventas.Find(id);
            return venta;
        }
        
        public IEnumerable<KeyValuePair<long,Venta>> List(long sesionId = -1, bool devuelta = false)
        {
            IEnumerable<KeyValuePair<long, Venta>> resultado;
            if (sesionId != -1)
                resultado = Context.Ventas.Where<Venta>(v => v.SesionId == sesionId && v.Devuelta == devuelta).ToDictionary<Venta, long>(v => v.VentaId);
            else
                resultado = Context.Ventas.Where<Venta>(v => v.Devuelta == devuelta).ToDictionary<Venta, long>(v => v.VentaId);
            return resultado;
        }
        public Venta Update(Venta venta)
        {
            Venta ventaUpdate = null;
            ventaUpdate = Context.Ventas.Find(venta.VentaId);
            if (ventaUpdate == null)
            {
                Logger.Log(String.Format("Se ha intentado actualizar una venta con id {0} que no existe, se lanza VentaException.", venta.VentaId));
                throw new VentaException();
            }
            Context.Entry(ventaUpdate).CurrentValues.SetValues(venta);
            Context.SaveChanges();
            return ventaUpdate;
        }

        public Venta Delete(long id)
        {
            Venta borrado = null;
            borrado = Context.Ventas.Find(id);
            if (borrado != null && borrado.Devuelta == false)
            {
                borrado.Devuelta = true;
                Context.SaveChanges();
            }
            return borrado;
        }
    }
}
