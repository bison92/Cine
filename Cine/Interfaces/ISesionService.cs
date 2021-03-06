﻿using System.Collections.Generic;
namespace Cine.Interfaces
{
    public interface ISesionService
    {
        Sesion Read(long id);
        IEnumerable<KeyValuePair<long, Sesion>> List(long salaId = -1);
        Sesion Cerrar(long id);
        Sesion Abrir(long id);
    }
}
