using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class VentaIntegracionTest
    {
        private SalaRepository _salaRepository;
        private SalaService _salaService;

        private SesionRepository _sesionRepository;
        private SesionService _sesionService;
        private SesionController sutSesion;

        private VentaRepository _ventaRepository;
        private VentaService _ventaService;
        private VentaController sutVenta;

        [TestInitialize]
        public void TestInicializa()
        {
            //SalaRepository.Clean();
            _salaRepository = new SalaRepository();
            _salaService = new SalaService(_salaRepository);

            _sesionRepository = new SesionRepository();
            _sesionService = new SesionService(_sesionRepository);
            sutSesion = new SesionController(_sesionService);

            _ventaRepository = new VentaRepository();
            _ventaService = new VentaService(_ventaRepository, _sesionService, _salaService);
            sutVenta = new VentaController(_ventaService);
        }

        [TestMethod]
        public void TestAbrirYCerrarSesiones()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    Sesion res = sutSesion.Abrir(1);
                    Assert.IsTrue(res.EstaAbierta);
                    res = sutSesion.Cerrar(1);
                    Assert.IsFalse(res.EstaAbierta);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        public void TestCreateConYSinDescuento()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    //sin descuento
                    Venta res1 = sutVenta.Create(new Venta(1, 1));
                    //con descuento
                    Venta res2 = sutVenta.Create(new Venta(1, 5));

                    Assert.AreEqual(1, res1.Id);
                    Assert.AreEqual(2, res2.Id);
                    Assert.AreEqual(28.0d, res1.TotalVenta, 0.01d);
                    Assert.AreEqual(63.0d, res2.TotalVenta, 0.01d);
                    Assert.AreEqual(7.0d, res1.PrecioEntrada);
                    Assert.AreEqual(7.0d, res2.PrecioEntrada);
                    Assert.AreEqual(0, res1.AppliedDiscount);
                    Assert.AreEqual(10, res2.AppliedDiscount);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        public void TestUpdateConYSinDescuento()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    //sin descuento
                    Venta res1 = sutVenta.Create(new Venta(1, 4));
                    //con descuento
                    Venta res2 = sutVenta.Create(new Venta(1, 10));
                    res1.NumeroEntradas = 10;
                    Venta upd1 = sutVenta.Update(res1);
                    Assert.AreEqual(10, upd1.NumeroEntradas);
                    Assert.AreEqual(63.0d, upd1.TotalVenta);
                    Assert.AreEqual(10, upd1.AppliedDiscount);
                    Assert.AreEqual(35, upd1.DiferenciaDevolucion);
                    res2.NumeroEntradas = 4;
                    Venta upd2 = sutVenta.Update(res2);
                    Assert.AreEqual(4, upd2.NumeroEntradas);
                    Assert.AreEqual(28.0d, upd2.TotalVenta);
                    Assert.AreEqual(0, upd2.AppliedDiscount);
                    Assert.AreEqual(-35, upd2.DiferenciaDevolucion);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestDelete()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    //sin descuento
                    Venta res1 = sutVenta.Create(new Venta(1, 4));
                    //con descuento
                    sutVenta.Create(new Venta(1, 10));
                    IList<Venta> listaAntes = sutVenta.List();
                    Venta deleted = sutVenta.Delete(res1.Id);
                    IList<Venta> listaDespues = sutVenta.List();
                    Assert.AreNotEqual(listaAntes.Count, listaDespues.Count);
                    Assert.AreEqual(listaAntes.Count - 1, listaDespues.Count);
                    sutVenta.Read(deleted.Id);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestCreateSesionCerrada()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Cerrar(1);
                    sutVenta.Create(new Venta(1, 2));
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestUpdateSesionCerrada()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    Venta res = sutVenta.Create(new Venta(1, 2));
                    sutSesion.Cerrar(1);
                    res.NumeroEntradas = 5;
                    sutVenta.Update(res);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestDeleteSesionCerrada()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    Venta res = sutVenta.Create(new Venta(1, 2));
                    sutSesion.Cerrar(1);
                    sutVenta.Delete(res.Id);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestCreateNoHayButacas()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    sutVenta.Create(new Venta(1, 50));
                    sutVenta.Create(new Venta(1, 50));
                    sutVenta.Create(new Venta(1, 1));
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestUpdateNoHayButacas()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = new CineDB().Database.BeginTransaction())
                {
                    sutSesion.Abrir(1);
                    sutVenta.Create(new Venta(1, 50));
                    Venta res = sutVenta.Create(new Venta(1, 50));
                    res.NumeroEntradas = 51;
                    sutVenta.Update(res);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SalaException))]
        public void TestSalaReadNoExiste()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = new CineDB().Database.BeginTransaction())
                {
                    _salaService.Read(Constantes.SalaNoExiste);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestSesionReadNoExiste()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = new CineDB().Database.BeginTransaction())
                {
                    _sesionService.Read(Constantes.SesionNoExiste);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestVentaReadNoExiste()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = new CineDB().Database.BeginTransaction())
                {
                    sutVenta.Read(1);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestVentaUpdateNoExiste()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = new CineDB().Database.BeginTransaction())
                {
                    sutVenta.Update(new Venta { Id = 1, SesionId = 1, NumeroEntradas = 10 });
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
    }
}
