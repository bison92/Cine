using System;
using Cine.Utils;
using Cine.Interfaces;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
namespace Cine
{
    public class SesionService : ISesionService
    {
        public ISesionRepository _sesionRepository;
        private ISalaService _salaService;
        public SesionService(ISesionRepository sesionRepository, ISalaService salaService)
        {
            _salaService = salaService;
            _sesionRepository = sesionRepository;
        }

        public Sesion Read(long id)
        {
            Sesion sesion = _sesionRepository.Read(id);
            if (sesion == null)
            {
                Logger.Log(String.Format("Se ha intentado leer una sesion con id {0} que no existe, se lanza SesionException.", id));
                throw new SesionException(id);
            }
            if (sesion.Sala == null)
            {
                sesion.Sala = _salaService.Read(sesion.SalaId);
            }
            return sesion;
        }
        public IDictionary<long, Sesion> List(long salaId = -1)
        {
            return _sesionRepository.List(salaId);
        }

        public Sesion Cerrar(long id)
        {
            Sesion sesion = _sesionRepository.Update(id, false);
            return sesion;
        }

        public Sesion Abrir(long id)
        {
            Sesion sesion = _sesionRepository.Update(id, true);
            return sesion;
        }
    }
}
