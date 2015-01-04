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
        public SesionRepository()
        {
        }

        private Sesion Create(long id, long salaId, string hora)
        {
            Sesion nuevaSesion = new Sesion(id, salaId, hora);
            using (var context = new CineDB())
            {
                context.Sesiones.Add(nuevaSesion);
                context.SaveChanges();
            }
            return nuevaSesion;
        }

        public Sesion Read(long id)
        {
            Sesion resultado = null;
            using (var context   = new CineDB())
            {
                resultado = context.Sesiones.Find(id);
            }
            return resultado;
        }

        public IDictionary<long,Sesion> List(long idSala = -1)
        {
            IEnumerable<KeyValuePair<long, Sesion>> subconjunto;
            using (var context = new CineDB())
            {
                if (idSala != -1)
                    subconjunto = context.Sesiones.Where<Sesion>((ses) => (ses.SalaId == idSala)).ToDictionary<Sesion, long>(skp => skp.Id);
                else
                    subconjunto = context.Sesiones.ToDictionary<Sesion, long>(skp => skp.Id);
            }
            IDictionary<long,Sesion> resultado = subconjunto.Select(sKP => sKP.Value).ToDictionary<Sesion,long>(sKP => sKP.Id);
            return resultado;
        }

        public Sesion Update(long id, bool abierta)
        {
            Sesion sesion = null;
            using (var ctx = new CineDB())
            {
                // provoca un InvalidOperationException en lugar de devolver null.
                //sesion = ctx.Sesiones.Single(s => s.Id == id);
                // mejor Find() como en el Read;
                sesion = ctx.Sesiones.Find(id);
                if (sesion == null)
                {
                    Logger.Log(String.Format("Se ha intentado actualizar una sesion con id {0} que no existe, se lanza SesionException.", id));
                    throw new SesionException(id);
                }
                sesion.EstaAbierta = abierta;
                ctx.SaveChanges();
            }       
            return sesion;
        }
    }
}
