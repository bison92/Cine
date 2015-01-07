using Cine.Utils;
using System;
using System.Linq;
using System.Collections.Generic;
using Cine.Interfaces;
using System.Runtime.CompilerServices;

namespace Cine
{
    public class VentaService : IVentaService
    {
        private IVentaRepository _ventaRepository;
        private ISesionService _sesionService;

        public VentaService(IVentaRepository ventaRepository, ISesionService sesionService)
        {
            _ventaRepository = ventaRepository;
            _sesionService = sesionService;
        }

        public Venta Create(Venta venta)
        {
            Sesion sesion = _sesionService.Read(venta.SesionId);
            // Chequea que la sesión está abierta
            CompruebaSesionAbierta(sesion, "crear"); // throws exception
            // Chequea que hay suficiente aforo
            if (!HaySuficientesButacas(sesion, venta))
            {
                Logger.Log(String.Format("Se ha intentado vender {0} entradas, para la sesión {1} y no hay suficiente aforo, se lanza VentaNoAforoException.", venta.NumeroEntradas, sesion.SesionId));
                throw new VentaExceptionNoAforo(venta.NumeroEntradas);
            }
            venta = CalculaPrecioYDescuento(venta);
            return _ventaRepository.Create(venta);
        }

        public Venta Read(long ventaId)
        {
            Venta venta = _ventaRepository.Read(ventaId);
            CompruebaVentaExiste(venta, "leer"); // throws exception
            return venta;
        }
        public IEnumerable<KeyValuePair<long, Venta>> List(bool devuelta = false)
        {
            return _ventaRepository.List(-1, devuelta);
        }

        public Venta Update(Venta venta)
        {
            string logAction = "cambiar";
            Venta antiguosDatos = _ventaRepository.Read(venta.VentaId);
            CompruebaVentaExiste(antiguosDatos, logAction); // throws exception
            Sesion sesion = _sesionService.Read(venta.SesionId);
            CompruebaSesionAbierta(sesion, logAction); // throws exception
            if (!HaySuficientesButacas(sesion, venta, antiguosDatos))
            {
                Logger.Log(String.Format("Se ha intentado cambiar {0} por {1} entradas ,"
                + "para la sesión {2} pero no hay suficiente aforo, se lanza VentaNoAforoException.", antiguosDatos.NumeroEntradas, venta.NumeroEntradas, sesion.SesionId)); // sonar casca
                throw new VentaExceptionNoAforo(venta.NumeroEntradas);
            }
            venta = CalculaPrecioYDescuento(venta, antiguosDatos);
            return _ventaRepository.Update(venta);
        }

        public Venta Delete(long ventaId)
        {
            string action = "devolver";
            Venta venta = _ventaRepository.Read(ventaId);
            CompruebaVentaExiste(venta, action); // throws exception
            Sesion sesion = _sesionService.Read(venta.SesionId);
            CompruebaSesionAbierta(sesion, action); // throws exception
            return _ventaRepository.Delete(ventaId);
        }
        public bool HaySuficientesButacas(Sesion sesion, Venta venta, Venta antiguaVenta = null)
        {
            int butacasVendidas = 0;
            IEnumerable<KeyValuePair<long, Venta>> diccionario = _ventaRepository.List(venta.SesionId);
            foreach (var pareja in diccionario)
            {
                butacasVendidas += pareja.Value.NumeroEntradas;
            }
            if (antiguaVenta != null)
            {
                if (venta.SesionId == antiguaVenta.SesionId)
                {
                    butacasVendidas -= antiguaVenta.NumeroEntradas;
                }
            }
            Sala sala = sesion.Sala;
            int aforo = sala.Aforo;
            return (butacasVendidas + venta.NumeroEntradas) <= aforo;
        }

        public Venta CalculaPrecioYDescuento(Venta venta, Venta antiguaVenta = null)
        {
            double total = 0;
            venta.PrecioEntrada = Constantes.TicketPrice;
            if (venta.NumeroEntradas >= Constantes.DiscountThreshold)
            {
                total = (venta.PrecioEntrada * venta.NumeroEntradas) * ((100 - Constantes.Discount) / 100.0d);
                venta.AppliedDiscount = Constantes.Discount;
            }
            else
            {
                total = venta.PrecioEntrada * venta.NumeroEntradas;
                venta.AppliedDiscount = 0;
            }
            if(antiguaVenta != null)
            {
                venta.DiferenciaDevolucion = total - antiguaVenta.TotalVenta;
            }
            venta.TotalVenta = total;
            return venta;
        }

        public double CalcularTotales(long sesionId = -1, long salaId = -1)
        {
            double total = 0;
            IEnumerable<KeyValuePair<long, Venta>> diccionario = ListaVentas(salaId, sesionId);
            total = diccionario.Sum<KeyValuePair<long,Venta>>((venta) => (venta.Value.TotalVenta));
            return total;
        }

        public int CalcularEntradas(long sesionId = -1, long salaId = -1)
        {
            int entradas = 0;
            IEnumerable<KeyValuePair<long, Venta>> diccionario = ListaVentas(salaId, sesionId);
            entradas = diccionario.Sum<KeyValuePair<long, Venta>>((venta) => (venta.Value.NumeroEntradas));
            return entradas;
        }
        private IEnumerable<KeyValuePair<long, Venta>> ListaVentas(long salaId, long sesionId)
        {
            IEnumerable<KeyValuePair<long, Venta>> diccionario;
            if (salaId != -1)
            {
                diccionario = this.ListaVentasSala(salaId);
            }
            else
            {
                diccionario = _ventaRepository.List(sesionId);
            }
            return diccionario;
        }
        private IEnumerable<KeyValuePair<long, Venta>> ListaVentasSala(long salaId)
        {
            IEnumerable<KeyValuePair<long, Venta>> diccionario = new Dictionary<long,Venta>();
            IEnumerable<KeyValuePair<long, Sesion>> sesionesDeLaSala = _sesionService.List(salaId);
            foreach (var pareja in sesionesDeLaSala)
            {
                IEnumerable<KeyValuePair<long, Venta>> parte = _ventaRepository.List(pareja.Value.SesionId);
                diccionario = diccionario.Concat(parte).ToDictionary(x => x.Key, x => x.Value);
            }
            return diccionario;
        }
        /// <summary>
        /// Comprueba que la sesión está abierta, lanza una excepción si no es así.
        /// </summary>
        /// <param name="sesionId">la sesión a comprobar</param>
        /// <param name="action">La acción que estamos realizando con la venta para el Log</param>
        /// <returns>La sesión en caso de obtenerla</returns>
        /// <exception>SesionCerradaException</exception>
        private bool CompruebaSesionAbierta(Sesion sesion, string action)
        {
            if (!sesion.EstaAbierta)
            {
                Logger.Log(String.Format("Se ha intentado {0} la venta, pero la correspondiente sesión {1} ya está cerrada, se lanza SesionCerradaException.", action, sesion.SesionId)); 
                throw new SesionExceptionCerrada(sesion.SesionId);
            }
            return true;
        }
        /// <summary>
        /// Intenta obtener la venta del repositorio de ventas, lanza una excepción si no existe.
        /// </summary>
        /// <param name="sesionId">El id de venta</param>
        /// <param name="action">La acción para el Log</param>
        /// <returns>La venta en caso de obtenerla</returns>
        /// <exception>VentaException</exception>
        private Venta CompruebaVentaExiste(Venta venta, string action)
        {
            if (venta == null)
            {
                Logger.Log(String.Format("Se ha intentado {0} una venta con id que no existe, se lanza VentaException.", action));
                throw new VentaException();
            }
            return venta;
        }
    }
}
