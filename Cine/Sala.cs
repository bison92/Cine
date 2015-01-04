using System.ComponentModel.DataAnnotations;
namespace Cine
{
    public class Sala
    {
        [Key]
        public long Id { get; set; }
        public int Aforo { get; set; }

        public Sala(long id, int aforo)
        {
            this.Id = id;
            this.Aforo = aforo;
        }
        public Sala()
        {

        }
    }
}
