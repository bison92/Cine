using Cine.Interfaces;
using System.Collections.Generic;

namespace Cine
{
    public class SalaRepository : ISalaRepository
    {
        public SalaRepository()
        {
           
        }
        //public static SalaRepository GetInstance()
        //{
        //    if (_instance == null)
        //    {
        //        _instance = new SalaRepository();
        //    }
        //    return _instance;
        //}
        //public static void Clean()
        //{
        //    _instance = null;
        //}
        public Sala Read(long id)
        {
            Sala resultado = null;
            using (var context = new CineDB())
            {
                resultado = context.Salas.Find(id);
            }
            return resultado;
        }
        private Sala Create(long id, int nButacas)
        {
            Sala sala = new Sala(id, nButacas);
            using (var context= new CineDB())
            {
                sala = context.Salas.Add(sala);
                context.SaveChanges();
            }
            return sala;
        }
    }
}
