using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Venta
    {
        public long VentaId { get; set; }
        public long SesionId { get; set; }
        public Sesion sesion { get; set; }
        public int NumeroEntradas { get; set; }
        public double TotalVenta { get; set; }
        public double PrecioEntrada { get; set; }
        public double AppliedDiscount { get; set; }
        public double DiferenciaDevolucion { get; set; }
        public bool Devuelta { get; set; }
        public Venta(long sesionId, int nEntradas)
        {
            this.VentaId = -1;
            this.SesionId = sesionId;
            this.NumeroEntradas = nEntradas;
            this.TotalVenta = 0;
            this.AppliedDiscount = 0;
            this.DiferenciaDevolucion = 0;
            this.Devuelta = false;
        }

        public Venta()
        {
            this.VentaId = -1;
            this.TotalVenta = 0;
            this.AppliedDiscount = 0;
            this.DiferenciaDevolucion = 0;
            this.Devuelta = false;
        }
    }
}
