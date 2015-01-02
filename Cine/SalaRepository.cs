using Cine.Interfaces;
using System.Collections.Generic;

namespace Cine
{
    public class SalaRepository : ISalaRepository
    {
        private IDictionary<long, Sala> _almacen;
        private static SalaRepository _instance = null;
        private SalaRepository()
        {
            _almacen = new Dictionary<long, Sala>();
            for (int i = 0; i < Constantes.Salas.Length; i++)
            {
                Create(Constantes.Salas[i], Constantes.Aforos[i]);
            }
        }
        public static SalaRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SalaRepository();
            }
            return _instance;
        }
        public static void Clean()
        {
            _instance = null;
        }
        public Sala Read(long id)
        {
            Sala resultado = null;
            if (_almacen.ContainsKey(id))
            {
                resultado = _almacen[id];
            }
            return resultado;
        }
        private Sala Create(Sala sala)
        {
            return Create(sala.Id, sala.Aforo);
        }
        private Sala Create(long id, int nButacas)
        {
            Sala nuevaSala = new Sala(id, nButacas);
            _almacen.Add(id, nuevaSala);
            return nuevaSala;
        }
    }
}
