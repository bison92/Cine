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
        private IDictionary<long, Sesion> _almacen;
        private static SesionRepository _instance = null;
        private long _idAuto;
        private SesionRepository()
        {
            _idAuto = 1;
            _almacen = new Dictionary<long, Sesion>();
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                Create(_idAuto++, Constantes.Salas[i % Constantes.Salas.Length], Constantes.Horas[i]);
            }
        }

        public static void Clean()
        {
            _instance = null;
        }

        public static SesionRepository GetInstance()
        {
            if (_instance == null)
            {
                _instance = new SesionRepository();
            }
            return _instance;
        }

        public Sesion Read(long id)
        {
            Sesion resultado = null;
            if(_almacen.ContainsKey(id)){
                resultado = _almacen[id];
            }
            return resultado;
        }

        private Sesion Create(long id, long salaId, string hora)
        {
            Sesion nuevaSesion = new Sesion(id, salaId, hora );
            _almacen.Add(id, nuevaSesion);
            return nuevaSesion;
        }

        public IDictionary<long,Sesion> List(long idSala = -1)
        {
            IEnumerable<KeyValuePair<long, Sesion>> subconjunto = _almacen.AsEnumerable<KeyValuePair<long, Sesion>>();
            if (idSala != -1)
            {
                subconjunto = subconjunto.Where(sKP => sKP.Value.SalaId == idSala);
            }
            IDictionary<long,Sesion> resultado = subconjunto.Select(sKP => sKP.Value).ToDictionary<Sesion,long>(sKP => sKP.Id);
            return resultado;
        }

        public Sesion Update(long id, bool abierta)
        {
            Sesion sesion = null;
            if(_almacen.ContainsKey(id))
            {
                _almacen[id].EstaAbierta = abierta;
                sesion = _almacen[id];
            }
            else
            {
                Logger.Log(String.Format("Se ha intentado actualizar una sesion con id {0} que no existe, se lanza SesionException.", id));
                throw new SesionException(id);
            }           
            return sesion;
        }
    }
}
