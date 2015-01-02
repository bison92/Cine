using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine.Interfaces
{
    public interface ISalaService
    {
        /// <summary>
        /// Busca la sala con el identificador dado.
        /// </summary>
        /// <param name="id">Id. de la sala a buscar</param>
        /// <returns>La sala buscada</returns>
        /// <exception cref="SalaException">Cuando no encuentra la sala</exception>
        Sala Read(long id);
    }
}
