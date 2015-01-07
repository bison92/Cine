using Cine.Interfaces;
using Cine.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class SesionRepository: ISesionRepository
    {
        public CineDB Context { get; set; }
        public SesionRepository(CineDB context)
        {
            Context = context;
        }

        private Sesion Create(long id, long salaId, string hora)
        {
            Sesion nuevaSesion = new Sesion(id, salaId, hora);
            Context.Sesiones.Add(nuevaSesion);
            Context.SaveChanges();
            return nuevaSesion;
        }

        public Sesion Read(long id)
        {
            Sesion resultado = null;
            resultado = Context.Sesiones.Find(id);
            return resultado;
        }

        public IEnumerable<KeyValuePair<long,Sesion>> List(long idSala = -1)
        {
            IEnumerable<KeyValuePair<long, Sesion>> resultado;
            if (idSala != -1)
                resultado = Context.Sesiones.Where<Sesion>((ses) => (ses.SalaId == idSala)).ToDictionary<Sesion, long>(skp => skp.SesionId);
            else
                resultado = Context.Sesiones.ToDictionary<Sesion, long>(skp => skp.SesionId);
            return resultado;
        }

        public Sesion Update(long id, bool abierta)
        {
            Sesion sesion = null;
            // provoca un InvalidOperationException en lugar de devolver null.
            //sesion = ctx.Sesiones.Single(s => s.Id == id);
            // mejor Find() como en el Read;
            sesion = Context.Sesiones.Find(id);
            if (sesion == null)
            {
                Logger.Log(String.Format("Se ha intentado actualizar una sesion con id {0} que no existe, se lanza SesionException.", id));
                throw new SesionException(id);
            }
            sesion.EstaAbierta = abierta;
            Context.SaveChanges();
            return sesion;
        }
    }
}
