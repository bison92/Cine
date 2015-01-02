using Cine.Interfaces;
namespace Cine
{
    public class SesionController: ISesionController
    {
        private ISesionService _sesionService;

        public SesionController(ISesionService sesionService)
        {
            _sesionService = sesionService;
        }
        public Sesion Cerrar(Sesion sesion)
        {
            return this.Cerrar(sesion.Id);
        }

        public Sesion Cerrar(long id)
        {
            return _sesionService.Cerrar(id);
        }


        public Sesion Abrir(Sesion sesion)
        {
            return this.Abrir(sesion.Id);
        }

        public Sesion Abrir(long id)
        {
            return _sesionService.Abrir(id);
        }
    }
}
