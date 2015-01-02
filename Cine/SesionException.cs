using System;

namespace Cine
{
    public class SesionException: Exception
    {
        public long Id { get; set; }

        public SesionException(long id): base("El identificador de sesión no es válido.")
        {
            this.Id = id;
        }
    }
}
