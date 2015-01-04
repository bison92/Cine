using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public class CineDBInitializer : DropCreateDatabaseAlways<CineDB>
    {
        protected override void Seed(CineDB context)
        {
            IList<Sala> defaultSalas = new List<Sala>();
            for (int i = 0; i < Constantes.Salas.Length; i++)
            {
                defaultSalas.Add(new Sala() { Id = Constantes.Salas[i], Aforo = Constantes.Aforos[i] });
            }
            foreach (Sala sala in defaultSalas)
                context.Salas.Add(sala);

            IList<Sesion> defaultSesiones = new List<Sesion>();
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                defaultSesiones.Add(new Sesion(Constantes.Sesiones[i], Constantes.Salas[i % 3], Constantes.Horas[i]));
            }
            foreach (Sesion sesion in defaultSesiones)
                context.Sesiones.Add(sesion);

            base.Seed(context);
        }
    }
}
