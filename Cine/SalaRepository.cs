using Cine.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;

namespace Cine
{
    public class SalaRepository : ISalaRepository
    {
        public CineDB Context { get; set; }
        public SalaRepository(CineDB context)
        {
            Context = context;
        }
        public Sala Read(long id)
        {
            Sala resultado = null;
            resultado = Context.Salas.Find(id);
            return resultado;
        }
        // no usado.
        private Sala Create(long id, int nButacas)
        {
            Sala sala = new Sala(id, nButacas);
            sala = Context.Salas.Add(sala);
            Context.SaveChanges();
            return sala;
        }
    }
}
