using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace Cine
{
    public class Sala
    {
        [Key]
        public long SalaId { get; set; }
        public int Aforo { get; set; }

        public ICollection<Sesion> Sesiones { get; set; }

        public Sala(long id, int aforo)
        {
            this.SalaId = id;
            this.Aforo = aforo;
        }
        public Sala()
        {

        }
    }
}
