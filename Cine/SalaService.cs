using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using Cine.Utils;
using Cine.Interfaces;
namespace Cine
{
    public class SalaService: ISalaService
    {
        private ISalaRepository _salaRepository;
        public SalaService(ISalaRepository salaRepository)
        {
            _salaRepository = salaRepository;
        }
        public Sala Read(long id)
        {
            Sala resultado = _salaRepository.Read(id);
            if (resultado == null)
            {
                Logger.Log(String.Format("Se ha intentado leer una sala con id {0} que no existe, se lanza SalaException.",id));
                throw new SalaException(id);
            }
            return resultado;
        }
    }
}
