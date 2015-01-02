using System;

namespace Cine
{
    public class VentaExceptionNoAforo: Exception
    {
        public int ButacasFaltantes { get; set; }
        public VentaExceptionNoAforo(int butacasFaltantes): base("No hay suficientes butacas para completar la venta.")
        {
            this.ButacasFaltantes = butacasFaltantes;
        }
    }
}
