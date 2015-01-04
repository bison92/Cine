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
        public VentaRepository()
        {
        }
        public Venta Create(Venta venta)
        {
            Venta nueva = new Venta();
            using (var ctx = new CineDB())
            {
                nueva = ctx.Ventas.Add(venta);
                ctx.SaveChanges();
            }
            return nueva;
        }

        public Venta Read(long id)
        {
            Venta venta = null;
            using (var ctx = new CineDB())
            {
                venta = ctx.Ventas.Find(id);
            }
            return venta;
        }

        public IDictionary<long,Venta> List(long sesionId = -1)
        {

            IEnumerable<KeyValuePair<long, Venta>> subconjunto;
            using (var ctx = new CineDB())
            {
                if (sesionId != -1)
                    subconjunto = ctx.Ventas.Where<Venta>(v => v.SesionId == sesionId).ToDictionary<Venta, long>(v => v.Id);
                else
                    subconjunto = ctx.Ventas.ToDictionary<Venta, long>(v => v.Id);
            }
            IDictionary<long, Venta> resultado = subconjunto.Select(vkp => vkp.Value).ToDictionary<Venta, long>(vkp => vkp.Id);
            return resultado;
        }

        public Venta Update(Venta venta)
        {
            Venta ventaUpdate = null;
            using (var ctx = new CineDB())
            {
                ventaUpdate = ctx.Ventas.Find(venta.Id);
                if (ventaUpdate == null)
                {
                    Logger.Log(String.Format("Se ha intentado actualizar una venta con id {0} que no existe, se lanza VentaException.", venta.Id));
                    throw new VentaException();
                }
                ctx.Entry(ventaUpdate).CurrentValues.SetValues(venta);
                ctx.SaveChanges();
            }
            return ventaUpdate;
        }

        public Venta Delete(long id)
        {
            Venta borrado = null;
            using (var ctx = new CineDB()){
                borrado = ctx.Ventas.Find(id);
                if (borrado != null)
                {
                    ctx.Ventas.Remove(borrado);
                    ctx.SaveChanges();
                }
            }
            return borrado;
        }
    }
}
