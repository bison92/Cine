using Cine.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class VentaRepository : IVentaRepository
    {
        private Dictionary<long, Venta> _almacenVentas;
        private Dictionary<long, Venta> _almacenDevoluciones;
        private long _idAuto;
        private static VentaRepository _instance = null;
        private VentaRepository()
        {
            _almacenVentas = new Dictionary<long, Venta>();
            _idAuto = 1; // Id Auto incremental para las nuevas ventas
            _almacenDevoluciones = new Dictionary<long, Venta>();
        }
        public static VentaRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new VentaRepository();
            }
            return _instance;
        }
        public void Clean()
        {
            _instance = null;
        }
        public Venta Create(Venta venta)
        {
            venta.Id = _idAuto++;
            _almacenVentas.Add(venta.Id, venta);
            return venta;
        }

        public Venta Read(long id)
        {
            Venta venta = null;
            if (_almacenVentas.ContainsKey(id))
            {
                venta = _almacenVentas[id];
            }
            return venta;
        }

        public IDictionary<long,Venta> List(long sesionId = -1)
        {
            
            IEnumerable<KeyValuePair<long, Venta>> subconjunto = _almacenVentas.AsEnumerable<KeyValuePair<long, Venta>>();
            if (sesionId != -1)
            {
                subconjunto = subconjunto.Where(vKP => vKP.Value.SesionId == sesionId);
            }
            IDictionary<long, Venta> resultado = subconjunto.Select(vkp => vkp.Value).ToDictionary<Venta, long>(vkp => vkp.Id);
            return resultado;
        }

        public Venta Update(Venta venta)
        {
            Venta res = null;
            if (_almacenVentas.ContainsKey(venta.Id))
            {
                _almacenVentas[venta.Id] = venta;
                res = venta;
            }
            return res;
        }

        public Venta Delete(long id)
        {
            Venta borrado = null;
            if (_almacenVentas.ContainsKey(id))
            {
                borrado = _almacenVentas[id];
                _almacenVentas.Remove(id);
                _almacenDevoluciones.Add(borrado.Id, borrado);
            }
            return borrado;
        }
    }
}
