using System;

namespace Cine
{
    public class VentaException: Exception
    {
        public VentaException(): base("El identificador de venta no es válido.")
        {
        }
    }
}
