﻿using System;
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
            
            _salaRepository = SalaRepository.GetInstance();
            _salaRepository.Clean();
            _salaRepository = SalaRepository.GetInstance();
            _salaService = new SalaService(_salaRepository);
            

            _sesionRepository = SesionRepository.GetInstance();
            _sesionRepository.Clean();
            _sesionRepository = SesionRepository.GetInstance();
            _sesionService = new SesionService(_sesionRepository);
            sutSesion = new SesionController(_sesionService);

            _ventaRepository = VentaRepository.GetInstance();
            _ventaRepository.Clean();
            _ventaRepository = VentaRepository.GetInstance();
            _ventaService = new VentaService(_ventaRepository, _sesionService, _salaService);
            sutVenta = new VentaController(_ventaService);
        }

        [TestMethod]
        public void TestAbrirYCerrarSesiones()
        {
            Sesion res = sutSesion.Abrir(1);
            Assert.IsTrue(res.EstaAbierta);
            sutSesion.Cerrar(1);
            Assert.IsFalse(res.EstaAbierta);
        }

        [TestMethod]
        public void TestCreateConYSinDescuento()
        {
            sutSesion.Abrir(1);
            //sin descuento
            Venta res1 = sutVenta.Create(new Venta(1,4));
            //con descuento
            Venta res2 = sutVenta.Create(new Venta(1,10));

            Assert.AreEqual(1, res1.Id);
            Assert.AreEqual(2, res2.Id);
            Assert.AreEqual(28.0d, res1.TotalVenta, 0.01d);
            Assert.AreEqual(63.0d, res2.TotalVenta, 0.01d);
            Assert.AreEqual(7.0d, res1.PrecioEntrada);
            Assert.AreEqual(7.0d, res2.PrecioEntrada);
            Assert.AreEqual(0, res1.AppliedDiscount);
            Assert.AreEqual(10, res2.AppliedDiscount);
        }
        [TestMethod]
        public void TestUpdateConYSinDescuento()
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
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestDelete()
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
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestCreateSesionCerrada()
        {
            sutSesion.Cerrar(1);
            sutVenta.Create(new Venta(1,2));
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestUpdateSesionCerrada()
        {
            sutSesion.Abrir(1);
            Venta res = sutVenta.Create(new Venta(1, 2));
            sutSesion.Cerrar(1);
            res.NumeroEntradas = 5;
            sutVenta.Update(res);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionExceptionCerrada))]
        public void TestDeleteSesionCerrada()
        {
            sutSesion.Abrir(1);
            Venta res = sutVenta.Create(new Venta(1, 2));
            sutSesion.Cerrar(1);
            sutVenta.Delete(res.Id);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestCreateNoHayButacas()
        {
            sutSesion.Abrir(1);
            sutVenta.Create(new Venta(1, 50));
            sutVenta.Create(new Venta(1, 50));
            sutVenta.Create(new Venta(1, 1));
        }
        [TestMethod]
        [ExpectedException(typeof(VentaExceptionNoAforo))]
        public void TestUpdateNoHayButacas()
        {
            sutSesion.Abrir(1);
            sutVenta.Create(new Venta(1, 50));
            Venta res = sutVenta.Create(new Venta(1, 50));
            res.NumeroEntradas = 51;
            sutVenta.Update(res);
        }
        [TestMethod]
        [ExpectedException(typeof(SalaException))]
        public void TestSalaReadNoExiste()
        {
            _salaService.Read(Constantes.SalaNoExiste);
        }
        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestSesionReadNoExiste()
        {
            _sesionService.Read(Constantes.SesionNoExiste);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestVentaReadNoExiste()
        {
            sutVenta.Read(1);
        }
        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestVentaUpdateNoExiste()
        {
            sutVenta.Update(new Venta { Id = 1, SesionId = 1, NumeroEntradas = 10 });
        }
    }
}
