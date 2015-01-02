using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Cine.Interfaces;
using Moq;



namespace CineTest
{
    [TestClass]
    public class VentaControllerTest
    {
        #region mocks
        private Mock<IVentaService> mockVentaService;
        private VentaController sut;
        #endregion
        [TestInitialize]
        public void TestInicializa()
        {
            mockVentaService = new Mock<IVentaService>();
            sut = new VentaController(mockVentaService.Object);
        }
        [TestMethod]
        public void TestCreate()
        {
            mockVentaService.Setup(vService => vService.Create(It.IsAny<Venta>()))
                .Returns((Venta v) => (v));
            sut.Create(new Venta(1, 20));
            mockVentaService.Verify(vService => vService.Create(It.IsAny<Venta>()), Times.Once());
        }

        [TestMethod]
        public void TestRead()
        {
            mockVentaService.Setup(vService => vService.Read(It.IsAny<long>()))
                .Returns((long id) => new Venta { Id = id, SesionId = 1, NumeroEntradas = 20 });
            sut.Read(1);
            mockVentaService.Verify(vService => vService.Read(It.IsAny<long>()), Times.Once());
        }
        [TestMethod]
        public void TestList()
        {
            mockVentaService.Setup(vService => vService.List())
                .Returns(
                    () =>
                    {
                        return new Dictionary<long, Venta>() { 
                            { 1, new Venta { Id = 1, SesionId = 1, NumeroEntradas = 20 } }, 
                            { 2, new Venta { Id = 1, SesionId = 1, NumeroEntradas = 20 } }
                        };
                    });
            sut.List();
            mockVentaService.Verify(vService => vService.List(), Times.Once());
        }
        [TestMethod]
        public void TestUpdate()
        {
            mockVentaService.Setup(vService => vService.Update(It.IsAny<Venta>()))
                .Returns((Venta v) => (v));
            sut.Update(new Venta { Id = 1, SesionId = 1, NumeroEntradas = 1 });
            mockVentaService.Verify(vService => vService.Update(It.IsAny<Venta>()), Times.Once());
        }
        [TestMethod]
        public void TestDelete()
        {
            mockVentaService.Setup(vService => vService.Delete(It.IsAny<long>()))
                .Returns((long id) => (new Venta { Id = id, SesionId = 1, NumeroEntradas = 20 }));
            sut.Delete(1);
            mockVentaService.Verify(vService => vService.Delete(It.IsAny<long>()), Times.Once());
        }
        [TestMethod]
        public void TestCalcularTotales()
        {
            mockVentaService.Setup(vService => vService.CalcularTotales(-1, -1)).Returns(42.0d);
            sut.CalcularTotales();
            mockVentaService.Verify(vService => vService.CalcularTotales(-1, -1), Times.Once());
        }
        [TestMethod]
        public void TestCalcularTotalesSala()
        {
            mockVentaService.Setup(vService => vService.CalcularTotales(-1, 1)).Returns(42.0d);
            sut.CalcularTotalesSala(1);
            mockVentaService.Verify(vService => vService.CalcularTotales(-1, 1), Times.Once());
        }
        [TestMethod]
        public void TestCalcularTotalesSesion()
        {
            mockVentaService.Setup(vService => vService.CalcularTotales(1, -1)).Returns(42.0d);
            sut.CalcularTotalesSesion(1);
            mockVentaService.Verify(vService => vService.CalcularTotales(1, -1), Times.Once());
        }
        [TestMethod]
        public void TestCalcularEntradas()
        {
            mockVentaService.Setup(vService => vService.CalcularEntradas(-1, -1)).Returns(12);
            sut.CalcularEntradas();
            mockVentaService.Verify(vService => vService.CalcularEntradas(-1, -1), Times.Once());
        }
        [TestMethod]
        public void TestCalcularEntradasSala()
        {
            mockVentaService.Setup(vService => vService.CalcularEntradas(-1, 1)).Returns(12);
            sut.CalcularEntradasSala(1);
            mockVentaService.Verify(vService => vService.CalcularEntradas(-1, 1), Times.Once());
        }
        [TestMethod]
        public void TestCalcularEntradasSesion()
        {
            mockVentaService.Setup(vService => vService.CalcularEntradas(1, -1)).Returns(12);
            sut.CalcularEntradasSesion(1);
            mockVentaService.Verify(vService => vService.CalcularEntradas(1, -1), Times.Once());
        }
    }
}
