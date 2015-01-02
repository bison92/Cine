using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class Venta
    {
        public long Id { get; set; }
        public long SesionId { get; set; }
        public int NumeroEntradas { get; set; }
        public double TotalVenta { get; set; }
        public double PrecioEntrada { get; set; }
        public double AppliedDiscount { get; set; }
        public double DiferenciaDevolucion { get; set; }
        public Venta(long sesionId, int nEntradas)
        {
            this.Id = -1;
            this.SesionId = sesionId;
            this.NumeroEntradas = nEntradas;
            this.TotalVenta = 0;
            this.AppliedDiscount = 0;
            this.DiferenciaDevolucion = 0;
        }

        public Venta()
        {
            this.Id = -1;
            this.TotalVenta = 0;
            this.AppliedDiscount = 0;
            this.DiferenciaDevolucion = 0;
        }
    }
}
