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
            using (var ctx = new CineDB())
            {
                ctx.Sesiones.Add(nuevaSesion);
                ctx.SaveChanges();
            }
            return nuevaSesion;
        }

        public Sesion Read(long id)
        {
            Sesion resultado = null;
            using (var ctx = new CineDB())
            {
                resultado = ctx.Sesiones.Find(id);
            }
            return resultado;
        }

        public IDictionary<long,Sesion> List(long idSala = -1)
        {
            IEnumerable<KeyValuePair<long, Sesion>> subconjunto;
            using (var ctx = new CineDB())
            {
                if (idSala != -1)
                    subconjunto = ctx.Sesiones.Where<Sesion>((ses) => (ses.SalaId == idSala)).ToDictionary<Sesion, long>(skp => skp.Id);
                else
                    subconjunto = ctx.Sesiones.ToDictionary<Sesion, long>(skp => skp.Id);
            }
            IDictionary<long,Sesion> resultado = subconjunto.Select(sKP => sKP.Value).ToDictionary<Sesion,long>(sKP => sKP.Id);
            return resultado;
        }

        public Sesion Update(long id, bool abierta)
        {
            Sesion sesion = null;
            using (var ctx = new CineDB())
            {
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
