using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using Cine.Interfaces;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class SesionServiceTest
    {
        private SesionService sut;
        private Mock<ISesionRepository> _mockSesionRepositorio;
        private Mock<ISalaService> _mockSalaService;
        [TestInitialize]
        public void TestInicializa()
        {
            _mockSesionRepositorio = new Mock<ISesionRepository>();
            _mockSalaService = new Mock<ISalaService>();
            sut = new SesionService(_mockSesionRepositorio.Object, _mockSalaService.Object);
        }
        //sala service 
        private void SetupSalaRead()
        {
            _mockSalaService.Setup(sService => sService.Read(1))
                .Returns(new Sala(1, 100));
        }
        private void VerifySalaRead(int times)
        {
            _mockSalaService.Verify(sService => sService.Read(1), Times.Exactly(times));
        }

        [TestMethod]
        public void TestRead()
        {
            SetupSalaRead();
            _mockSesionRepositorio.Setup(sRepository => sRepository.Read(It.IsIn<long>(Constantes.Sesiones))).Returns(new Sesion(1, 1, "17:00"));
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                sut.Read(Constantes.Sesiones[i]);
            }
            _mockSesionRepositorio.Verify(sRepository => sRepository.Read(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(Constantes.Sesiones.Length));
            VerifySalaRead(1);
        }
        [TestMethod]
        public void TestList()
        {
            _mockSesionRepositorio.Setup(sRepository => sRepository.List(It.IsIn<long>(Constantes.Salas)))
                .Returns(new Dictionary<long,Sesion>() {
                    {1, new Sesion(1,1,"17:00") },
                    {2, new Sesion(2,1,"20:00") },
                    {3, new Sesion(3,1,"22:00") }
                });
            sut.List(1);
            _mockSesionRepositorio.Verify(sRepository => sRepository.List(It.IsIn<long>(Constantes.Salas)),Times.Once());

        }
        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestReadNoExisteSesion()
        {
            _mockSesionRepositorio.Setup(sRepository => sRepository.Read(It.IsNotIn<long>(Constantes.Sesiones))).Returns<Sesion>(null);
            sut.Read(Constantes.SesionNoExiste);
        }

        [TestMethod]
        public void TestCerrar()
        {
            _mockSesionRepositorio.Setup(sRepository => sRepository.Update(It.IsIn<long>(Constantes.Sesiones), false))
                .Returns(() => { Sesion devuelta = new Sesion(1, 1, "17:00"); devuelta.EstaAbierta = false; return devuelta; });
            sut.Cerrar(Constantes.Sesiones[0]);
            _mockSesionRepositorio.Verify(sRepository => sRepository.Update(It.IsIn<long>(Constantes.Sesiones), false), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestCerrarNoExisteSesion()
        {
            _mockSesionRepositorio.Setup(sesionRepositorio => sesionRepositorio.Update(It.IsNotIn<long>(Constantes.Sesiones), false)).Throws(new SesionException(1));
            sut.Cerrar(Constantes.SesionNoExiste);
        }

        [TestMethod]
        public void TestAbrir()
        {
            _mockSesionRepositorio.Setup(sRepository => sRepository.Update(It.IsIn<long>(Constantes.Sesiones), true)).Returns(new Sesion { SesionId = 1, SalaId = 1, Hora = "17:30", EstaAbierta = true });
            sut.Abrir(Constantes.Sesiones[0]);
            _mockSesionRepositorio.Verify(sRepository => sRepository.Update(It.IsIn<long>(Constantes.Sesiones), true), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestAbrirNoExisteSesion()
        {
            _mockSesionRepositorio.Setup(sesionRepositorio => sesionRepositorio.Update(It.IsNotIn<long>(Constantes.Sesiones), true)).Throws(new SesionException(1));
            sut.Abrir(Constantes.SesionNoExiste);
        }
    }
}
