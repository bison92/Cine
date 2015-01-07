using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using Cine;
using Cine.Interfaces;
namespace CineTest
{
    [TestClass]
    public class VentaServiceTest
    {
        private VentaService sut;
        private Mock<IVentaRepository> mockVentaRepository;
        private Mock<ISesionService> mockSesionService;
        
        [TestInitialize]
        public void TestInicializa()
        {
            mockVentaRepository = new Mock<IVentaRepository>();
            mockSesionService = new Mock<ISesionService>();
            sut = new VentaService(mockVentaRepository.Object, mockSesionService.Object);

        }
        #region mockConfigurations
        //venta repository
        private void SetupRepoCreate()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Create(It.IsAny<Venta>()))
                .Returns(
                    (Venta v) =>
                    {
                        v.VentaId = 1;
                        return v;
                    }
                );
        }
        private void VerifyRepoCreate(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Create(It.IsAny<Venta>()), Times.Exactly(times));
        }
        private void SetupRepoRead()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(It.IsAny<long>()))
                .Returns((long id) => { return new Venta { SesionId = 1, NumeroEntradas = 20, VentaId = id, AppliedDiscount = 10, PrecioEntrada = 7.0d, DiferenciaDevolucion = 0, TotalVenta = 126.0d }; });
        }
        private void SetupRepoReadNull()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(It.IsAny<long>()))
                .Returns<Venta>(null);
        }
        private void VerifyRepoRead(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Read(It.IsAny<long>()), Times.Exactly(times));
        }
        private void SetupRepoList20Spots()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>(), false))
                .Returns(new Dictionary<long, Venta>(){
                    {1, new Venta(1, 40)},
                    {2, new Venta(1, 40)}
                });
        }
        private void SetupRepoListFull()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>(), false))
                .Returns(new Dictionary<long, Venta>(){
                    {1,new Venta(1,50)},
                    {2,new Venta(1,50)}
                });
        }
        private void SetupRepoListTotales()
        {
            mockVentaRepository.Setup(vRepository => vRepository.List(It.IsAny<long>(), false))
                .Returns(
                    (long caso, bool devuelta) =>
                    {
                        if (caso == -1)
                        {
                            return new Dictionary<long, Venta>(){
                                {1, new Venta{VentaId = 1, SesionId = 1, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {2, new Venta{VentaId = 1, SesionId = 2, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {3, new Venta{VentaId = 1, SesionId = 3, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {4, new Venta{VentaId = 1, SesionId = 4, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {5, new Venta{VentaId = 1, SesionId = 5, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {6, new Venta{VentaId = 1, SesionId = 6, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {7, new Venta{VentaId = 1, SesionId = 7, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {8, new Venta{VentaId = 1, SesionId = 8, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                                {9, new Venta{VentaId = 1, SesionId = 9, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                            };
                        }
                        else
                        {
                            return new Dictionary<long, Venta>()
                            {
                                {caso, new Venta{VentaId = 1, SesionId = caso, AppliedDiscount = 10, PrecioEntrada = 7, TotalVenta = 126, NumeroEntradas = 20, DiferenciaDevolucion = 0, Devuelta = devuelta} },
                            };
                        }

                    });
        }
        private void VerifyRepoList(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.List(It.IsAny<long>(), false), Times.Exactly(times));
        }
        private void SetupRepoUpdate()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Update(It.IsAny<Venta>()))
                .Returns((Venta v) => (v));
        }
        private void VerifyRepoUpdate(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Update(It.IsAny<Venta>()), Times.Exactly(times));
        }
        private void SetupRepoDelete()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Delete(It.IsAny<long>()))
                .Returns((long id) => { return new Venta { SesionId = 1, VentaId = id, NumeroEntradas = 20 }; });
        }
        private void VerifyRepoDelete(int times)
        {
            mockVentaRepository.Verify(vRepository => vRepository.Delete(It.IsAny<long>()), Times.Exactly(times));
        }
        //sesion service 
        private void SetupSesionRead(bool abierta = false)
        {
            mockSesionService.Setup(sesService => sesService.Read(It.IsIn<long>(Constantes.Sesiones)))
               .Returns(
                   (long id) =>
                   {
                       Sesion sesion = new Sesion(id, 1, "17:00");
                       sesion.EstaAbierta = abierta;
                       sesion.Sala = new Sala(1, 100);
                       return sesion;
                   }
               );
        }
        private void VerifySesionRead(int times)
        {
            mockSesionService.Verify(sesService => sesService.Read(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(times));
        }
        private void SetupSesionList()
        {
            mockSesionService.Setup(sesService => sesService.List(It.IsIn<long>(Constantes.Salas)))
                .Returns(
                    (long id) =>
                    {
                        long first = 1 + 3 * (id-1);
                        return new Dictionary<long, Sesion>() {
                            {first, new Sesion(first++,id,"unahora")},
                            {first, new Sesion(first++,id,"unahora")},
                            {first, new Sesion(first,id,"unahora")}
                        };
                    }
                );
        }
        private void VerifySesionList(int times)
        {
            mockSesionService.Verify(sesService => sesService.List(It.IsIn<long>(Constantes.Sesiones)), Times.Exactly(times));
        }
       
        #endregion

        #region helperTests
        [TestMethod]
        public void TestHaySuficientesButacas()
        {
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion{SesionId=1, SalaId = 1, Hora= "17:00", Sala = new Sala{SalaId = 1, Aforo=100}}, new Venta(1, 20));
            Assert.IsTrue(isEnough);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasNoHay()
        {
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion { SesionId = 1, SalaId = 1, Hora = "17:00", Sala = new Sala { SalaId = 1, Aforo = 100 } }, new Venta(1, 21));
            Assert.IsFalse(isEnough);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasDevolucion()
        {
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion { SesionId = 1, SalaId = 1, Hora = "17:00", Sala = new Sala { SalaId = 1, Aforo = 100 } }, new Venta(1, 30), new Venta(1, 10));
            Assert.IsTrue(isEnough);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasDevolucionCambioDeSesionCorrecto()
        {
            SetupRepoListFull();
            bool isEnough = sut.HaySuficientesButacas(new Sesion { SesionId = 1, SalaId = 1, Hora = "17:00", Sala = new Sala { SalaId = 1, Aforo = 100 } }, new Venta(1, 10), new Venta(2, 20));
            Assert.IsFalse(isEnough, "No queda espacio en la sala, pero se esta restando de otra sala.");
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestHaySuficientesButacasNoHayDevolucion()
        {
            SetupRepoList20Spots();
            bool isEnough = sut.HaySuficientesButacas(new Sesion { SesionId = 1, SalaId = 1, Hora = "17:00", Sala = new Sala { SalaId = 1, Aforo = 100 } }, new Venta(1, 31), new Venta(1, 10));
            Assert.IsFalse(isEnough);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuento()
        {
            Venta venta = new Venta(1, 4);
            venta = sut.CalculaPrecioYDescuento(venta);
            Assert.AreEqual(28.0d, venta.TotalVenta);
            Assert.AreEqual(0, venta.AppliedDiscount);
        }

        [TestMethod]
        public void TestCalculaPrecioYDescuentoConDescuento()
        {
            Venta venta = new Venta(1, 6);
            venta = sut.CalculaPrecioYDescuento(venta);
            Assert.AreEqual(37.8d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(10, venta.AppliedDiscount);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuentoDiferencia()
        {
            SetupRepoRead();
            Venta venta = new Venta { VentaId = 1, SesionId = 1, NumeroEntradas = 4 };
            Venta antiguaVenta = sut.Read(1);
            venta = sut.CalculaPrecioYDescuento(venta, antiguaVenta);
            Assert.AreEqual(28.0d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(0, venta.AppliedDiscount);
            Assert.AreEqual(-98d, venta.DiferenciaDevolucion, 0.001d);
        }
        [TestMethod]
        public void TestCalculaPrecioYDescuentoDiferenciaConDescuento()
        {
            SetupRepoRead();
            Venta venta = new Venta { VentaId = 1, SesionId = 1, NumeroEntradas = 6 };
            Venta antiguaVenta = sut.Read(1);
            venta = sut.CalculaPrecioYDescuento(venta, antiguaVenta);
            Assert.AreEqual(37.8d, venta.TotalVenta, 0.01d);
            Assert.AreEqual(10, venta.AppliedDiscount);
            Assert.AreEqual(-88.2d, venta.DiferenciaDevolucion, 0.001d);
        }
        #endregion

        #region CreateTests
        [TestMethod]
        public void TestCreate()
        {
            SetupSesionRead(true);
            SetupRepoCreate();
            SetupRepoList20Spots();

            Venta res = sut.Create(new Venta(1, 2));
            Assert.AreEqual(14.0, res.TotalVenta);
            Assert.AreEqual(0, res.AppliedDiscount);
            Assert.AreEqual(Constantes.TicketPrice, res.PrecioEntrada);

            VerifySesionRead(1);
            VerifyRepoCreate(1);
            VerifyRepoList(1);
        }

        [TestMethod]
        public void TestCreateConDescuento()
        {
            SetupSesionRead(true);
            SetupRepoCreate();
            SetupRepoList20Spots();

            Venta res = sut.Create(new Venta(1, 8));
            Assert.AreEqual(50.4d, res.TotalVenta, 0.01d);
            Assert.AreEqual(10, res.AppliedDiscount);
            Assert.AreEqual(Constantes.TicketPrice, res.PrecioEntrada);

            VerifySesionRead(1);
            VerifyRepoCreate(1);
            VerifyRepoList(1);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestCreateSesionCerrada()
        {
            SetupSesionRead(false);
            sut.Create(new Venta(1, 2));
        }

        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestCreateNoHayButacas()
        {
            SetupSesionRead(true);
            SetupRepoListFull();
            sut.Create(new Venta(1, 2));
        }
        #endregion

        #region ReadTests
        [TestMethod]
        public void TestRead()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(1))
                .Returns((long id) => { Venta venta = new Venta(1, 20); venta.VentaId = id; return venta; });
            sut.Read(1);
            mockVentaRepository.Verify(vRepository => vRepository.Read(1), Times.Once());
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestReadNoExisteVenta()
        {
            mockVentaRepository.Setup(vRepository => vRepository.Read(1)).Returns<Venta>(null);
            sut.Read(1);
        }
        #endregion

        #region ListTests
        [TestMethod]
        public void TestList()
        {
            SetupRepoList20Spots();
            sut.List();
            VerifyRepoList(1);
        }
        #endregion

        #region UpdateTests
        [TestMethod]
        public void TestUpdate()
        {
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            SetupRepoUpdate();

            Venta paraActualizar = new Venta(1, 4);
            paraActualizar.VentaId = 1;
            Venta res = sut.Update(paraActualizar);
            Assert.AreEqual(28.0d, res.TotalVenta, 0.01d);
            Assert.AreEqual(0, res.AppliedDiscount);
            Assert.AreEqual(-98d, res.DiferenciaDevolucion, 0.001d);

            VerifySesionRead(1);
            VerifyRepoList(1);
            VerifyRepoRead(1);
            VerifyRepoUpdate(1);
        }
        [TestMethod]
        public void TestUpdateConDescuento()
        {
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            SetupRepoUpdate();

            Venta paraActualizar = new Venta(1, 19);
            paraActualizar.VentaId = 1;
            Venta res = sut.Update(paraActualizar);
            Assert.AreEqual(119.7d, res.TotalVenta, 0.01d);
            Assert.AreEqual(10, res.AppliedDiscount);
            Assert.AreEqual(-6.3d, res.DiferenciaDevolucion, 0.001d);

            VerifySesionRead(1);
            VerifyRepoList(1);
            VerifyRepoRead(1);
            VerifyRepoUpdate(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoExisteVenta()
        {
            SetupRepoReadNull();

            Venta paraActualizar = new Venta(1, 20);
            paraActualizar.VentaId = 1;
            sut.Update(paraActualizar);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestUpdateSesionCerrada()
        {
            SetupRepoRead();
            SetupSesionRead(false);

            Venta paraActualizar = new Venta(1, 20);
            paraActualizar.VentaId = 1;
            sut.Update(paraActualizar);

            VerifyRepoRead(1);
            VerifySesionRead(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestUpdateNoHayButacas()
        {
            SetupSesionRead(true);
            SetupRepoList20Spots();
            SetupRepoRead();
            Venta paraActualizar = new Venta(1, 41);
            paraActualizar.VentaId = 1;
            sut.Update(paraActualizar);
        }
        #endregion

        #region DeleteTest
        [TestMethod]
        public void TestDelete()
        {
            SetupSesionRead(true);
            SetupRepoRead();
            SetupRepoDelete();

            sut.Delete(1);

            VerifySesionRead(1);
            VerifyRepoRead(1);
            VerifyRepoDelete(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestDeleteNoExisteVenta()
        {
            SetupRepoReadNull();
            sut.Delete(1);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestDeleteSesionCerrada()
        {
            SetupRepoRead();
            SetupSesionRead(false);
            sut.Delete(1);
        }
        #endregion
        #region TotalizeTests
        [TestMethod]
        public void TestCalCularTotales()
        {
            SetupRepoListTotales();
            double res = sut.CalcularTotales();
            Assert.AreEqual(1134.0d, res, 0.001d);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularTotalesBySesion()
        {
            SetupRepoListTotales();
            double res = sut.CalcularTotales(1);
            Assert.AreEqual(126.0d, res, 0.001d);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularTotalesBySala()
        {
            SetupRepoListTotales();
            SetupSesionList();
            double res = sut.CalcularTotales(-1, 1);
            Assert.AreEqual(378.0d, res, 0.001d);
            VerifyRepoList(3);
            VerifySesionList(1);
        }
        [TestMethod]
        public void TestCalCularEntradas()
        {
            SetupRepoListTotales();
            double res = sut.CalcularEntradas();
            Assert.AreEqual(180, res);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularEntradasBySesion()
        {
            SetupRepoListTotales();
            double res = sut.CalcularEntradas(1);
            Assert.AreEqual(20, res);
            VerifyRepoList(1);
        }
        [TestMethod]
        public void TestCalcularEntradasBySala()
        {
            SetupRepoListTotales();
            SetupSesionList();
            double res = sut.CalcularEntradas(-1, 1);
            Assert.AreEqual(60, res);
            VerifyRepoList(3);
            VerifySesionList(1);
        }
        #endregion
    }
}
