using System;

namespace Cine
{
    public class VentaException: Exception
    {
        public long Id { get; set; }

        public VentaException(long id): base("El identificador de venta no es válido.")
        {
            this.Id = id;
        }
    }
}
