namespace Cine
{
    public class Sesion
    {
        public long Id { get; set; }
        public long SalaId { get; set; }
        public bool EstaAbierta { get; set; }
        public string Hora { get; set; }

        public Sesion(long id, long salaId, string hora)
        {
            this.Id = id;
            this.SalaId = salaId;
            this.EstaAbierta = false;
            this.Hora = hora;
        }

        public Sesion()
        {
            this.EstaAbierta = false;
        }
    }
}
