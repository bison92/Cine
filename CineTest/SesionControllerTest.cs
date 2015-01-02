using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using Cine.Interfaces;

namespace CineTest
{
    [TestClass]
    public class SesionControllerTest
    {
        private SesionController sut;
        private Mock<ISesionService> mock;

        [TestInitialize]
        public void TestInicializa()
        {
            mock = new Mock<ISesionService>();
            sut = new SesionController(mock.Object);
        }

        [TestMethod]
        public void TestCerrar()
        {
            mock.Setup(sService => sService.Cerrar(It.IsIn<long>(Constantes.Sesiones)))
                .Returns(
                    (long id) => 
                    { 
                        Sesion sesion = new Sesion(id, 1, "20:00"); 
                        sesion.EstaAbierta = false; 
                        return sesion; 
                    }
                );
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                sut.Cerrar(Constantes.Sesiones[i]);
            }
            mock.Verify(sService => sService.Cerrar(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(Constantes.Sesiones.Length));
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestCerrarNoExisteSesion()
        {
            mock.Setup(sService => sService.Cerrar(It.IsNotIn<long>(Constantes.Sesiones))).Callback((long id) => { throw new SesionException(id); });
            sut.Cerrar(Constantes.SesionNoExiste);
        }
        [TestMethod]
        public void TestAbrir()
        {
            mock.Setup(sService => sService.Abrir(It.IsIn<long>(Constantes.Sesiones))).Returns((long id) => { Sesion sesion = new Sesion(id, 1, "20:00"); sesion.EstaAbierta = true; return sesion; });
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                sut.Abrir(Constantes.Sesiones[i]);
            }
            mock.Verify(sService => sService.Abrir(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(Constantes.Sesiones.Length));
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestAbrirNoExisteSesion()
        {
            mock.Setup(sService => sService.Abrir(It.IsNotIn<long>(Constantes.Sesiones))).Callback((long id) => { throw new SesionException(id); });
            sut.Abrir(Constantes.SesionNoExiste);
        }
    }
}
