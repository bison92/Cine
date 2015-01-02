using System;
using System.Runtime.Serialization;

namespace Cine
{
    public class SalaException: Exception
    {
        private long Id { get; set; }
        public SalaException(long id): base("El identificador de sala no es válido.")
        {
            this.Id = id;
        }
    }
}
