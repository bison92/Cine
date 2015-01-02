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
        private ISalaService _salaService;

        public VentaService(IVentaRepository ventaRepository, ISesionService sesionService, ISalaService salaService)
        {
            _ventaRepository = ventaRepository;
            _sesionService = sesionService;
            _salaService = salaService;
        }

        public Venta Create(Venta venta)
        {
            // Chequea que la sesión está abierta
            Sesion sesion = LeeSesionYThrowExceptionSiSesionCerrada(venta.SesionId, "crear");
            // Chequea que hay suficiente aforo
            if (!HaySuficientesButacas(sesion, venta))
            {
                Logger.Log(String.Format("Se ha intentado vender {0} entradas, para la sesión {1} y no hay suficiente aforo, se lanza VentaNoAforoException.", venta.NumeroEntradas, sesion.Id));
                throw new VentaExceptionNoAforo(venta.NumeroEntradas);
            }
            venta = CalculaPrecioYDescuento(venta);
            return _ventaRepository.Create(venta);
        }

        public Venta Read(long id)
        {
            Venta venta = LeeVentaYThrowExceptionSiVentaNoExiste(id, "leer");
            return venta;
        }
        public IDictionary<long, Venta> List()
        {
            return _ventaRepository.List();
        }

        public Venta Update(Venta venta)
        {
            string action = "cambiar";
            Venta antiguosDatos = LeeVentaYThrowExceptionSiVentaNoExiste(venta.Id, action);
            Sesion sesion = LeeSesionYThrowExceptionSiSesionCerrada(venta.SesionId, action);
            if (!HaySuficientesButacas(sesion, venta, antiguosDatos))
            {
                Logger.Log(String.Format("Se ha intentado cambiar {0} por {1} entradas ,"
                + "para la sesión {2} pero no hay suficiente aforo, se lanza VentaNoAforoException.", antiguosDatos.NumeroEntradas, venta.NumeroEntradas, sesion.Id)); // sonar casca
                throw new VentaExceptionNoAforo(venta.NumeroEntradas);
            }
            venta = CalculaPrecioYDescuento(venta, antiguosDatos);
            return _ventaRepository.Update(venta);
        }

        public Venta Delete(long id)
        {
            string action = "devolver";
            Venta venta = LeeVentaYThrowExceptionSiVentaNoExiste(id, action);
            LeeSesionYThrowExceptionSiSesionCerrada(venta.SesionId, action);
            return _ventaRepository.Delete(id);
        }
        public bool HaySuficientesButacas(Sesion sesion, Venta venta, Venta antiguaVenta = null)
        {
            int butacasVendidas = 0;
            IDictionary<long, Venta> diccionario = _ventaRepository.List(venta.SesionId);
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
            Sala sala = _salaService.Read(sesion.SalaId);
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

        public double CalcularTotales(long idSesion = -1, long idSala = -1)
        {
            double total = 0;
            IDictionary<long, Venta> diccionario;
            if (idSala != -1)
            {
                diccionario = new Dictionary<long, Venta>();
                IDictionary<long, Sesion> sesionesDeLaSala = _sesionService.List(idSala);
                foreach (var pareja in sesionesDeLaSala)
                {
                    IDictionary<long,Venta> parte = _ventaRepository.List(pareja.Value.Id);
                    diccionario = diccionario.Concat(parte).ToDictionary(x => x.Key, x => x.Value);
                }
            }
            else
            {
                diccionario = _ventaRepository.List(idSesion);
            }
            foreach (var pareja in diccionario)
            {
                total += pareja.Value.TotalVenta;
            }
            return total;
        }

        public int CalcularEntradas(long idSesion = -1, long idSala = -1)
        {
            int entradas = 0;
            IDictionary<long, Venta> diccionario;
            if (idSala != -1)
            {
                diccionario = new Dictionary<long, Venta>();
                IDictionary<long, Sesion> sesionesDeLaSala = _sesionService.List(idSala);
                foreach(var pareja in sesionesDeLaSala){
                    IDictionary<long, Venta> parte = _ventaRepository.List(pareja.Value.Id);
                    diccionario = diccionario.Concat(parte).ToDictionary(x => x.Key, x => x.Value);
                }
            }
            else
            {
                diccionario = _ventaRepository.List(idSesion);
            }
            foreach (var pareja in diccionario)
            {
                entradas += pareja.Value.NumeroEntradas;
            }
            return entradas;
        }
        /// <summary>
        /// Intenta obtener la sesión del repositorio de sesiones, lanza una excepción si no existe.
        /// </summary>
        /// <param name="sesionId">El id de sesión</param>
        /// <param name="action">La acción para el Log</param>
        /// <returns>La sesión en caso de obtenerla</returns>
        /// <exception>SesionCerradaException</exception>
        private Sesion LeeSesionYThrowExceptionSiSesionCerrada(long sesionId, string action)
        {
            Sesion sesion = _sesionService.Read(sesionId);
            if (!sesion.EstaAbierta)
            {
                Logger.Log(String.Format("Se ha intentado {0} la venta, pero la correspondiente sesión {1} ya está cerrada, se lanza SesionCerradaException.", action, sesion.Id)); 
                throw new SesionExceptionCerrada(sesion.Id);
            }
            return sesion;
        }
        /// <summary>
        /// Intenta obtener la venta del repositorio de ventas, lanza una excepción si no existe.
        /// </summary>
        /// <param name="sesionId">El id de venta</param>
        /// <param name="action">La acción para el Log</param>
        /// <returns>La venta en caso de obtenerla</returns>
        /// <exception>VentaException</exception>
        private Venta LeeVentaYThrowExceptionSiVentaNoExiste(long id, string action)
        {
            Venta venta = _ventaRepository.Read(id);
            if (venta == null)
            {
                Logger.Log(String.Format("Se ha intentado {0} una venta con id {1} que no existe, se lanza VentaException.", action, id));
                throw new VentaException(id);
            }
            return venta;
        }
    }
}
