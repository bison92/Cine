using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.Threading;

namespace CineTest
{
    [TestClass]
    public class VentaRepositoryTest
    {
        private VentaRepository sut;
        private static CineDB context;
        private static DbContextTransaction transaction;

        [TestInitialize]
        public void TestInicializa()
        {
            context = new CineDB();
            transaction = context.Database.BeginTransaction();
            context.Ventas.Add(new Venta(1, 4));
            context.Ventas.Add(new Venta(1, 10));
            context.Ventas.Add(new Venta(2, 4));
            context.Ventas.Add(new Venta(2, 10));
            context.Ventas.Add(new Venta(3, 4));
            context.Ventas.Add(new Venta(3, 10));
            context.Ventas.Add(new Venta(4, 4));
            context.Ventas.Add(new Venta(4, 10));
            context.Ventas.Add(new Venta(5, 4));
            context.Ventas.Add(new Venta(5, 10));
            context.Ventas.Add(new Venta(6, 4));
            context.Ventas.Add(new Venta(6, 10));
            context.Ventas.Add(new Venta(7, 4));
            context.Ventas.Add(new Venta(7, 10));
            context.Ventas.Add(new Venta(8, 4));
            context.Ventas.Add(new Venta(8, 10));
            context.Ventas.Add(new Venta(9, 4));
            context.Ventas.Add(new Venta(9, 10));
            sut = new VentaRepository(context);
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }
        [TestMethod]
        public void TestCreate()
        {
            Venta venta = new Venta(1, 10);
            Venta res = sut.Create(venta);
            Assert.AreEqual(19, res.Id);
        }

        [TestMethod]
        public void TestRead()
        {
            Venta res = sut.Read(1);
            Venta res2 = sut.Read(2);
            Assert.AreEqual(1, res.Id);
            Assert.AreEqual(2, res2.Id);
        }

        [TestMethod]
        public void TestReadNoExisteVenta()
        {
            Venta res = sut.Read(1);
            Assert.IsNull(res);
        }

        [TestMethod]
        public void TestList()
        {
            IDictionary<long, Venta> res = sut.List();
            Assert.AreEqual(18, res.Count);
        }

        [TestMethod]
        public void TestListBySesion()
        {
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                IDictionary<long, Venta> resSesion = sut.List(Constantes.Sesiones[i]);
                Assert.AreEqual(2, resSesion.Count);
                foreach (var pareja in resSesion)
                {
                    Venta venta = pareja.Value;
                    Assert.AreEqual(Constantes.Sesiones[i], venta.SesionId);
                }
            }
        }
        [TestMethod]
        public void TestListNoHayVentas()
        {
            IDictionary<long, Venta> res = sut.List();
            IDictionary<long, Venta> resSesion1 = sut.List(1);
            Assert.AreEqual(0, res.Count);
            Assert.AreEqual(0, resSesion1.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {         
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.Id = 1;
            Venta antigua = sut.Read(1);
            Venta actualizada = sut.Update(ventaAActualizar);
            Assert.AreEqual(10, actualizada.NumeroEntradas);
            Assert.AreNotEqual(antigua.NumeroEntradas, actualizada.NumeroEntradas);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoExisteVenta()
        {
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.Id = 1;
            Venta actualizada = sut.Update(ventaAActualizar);
        }

        [TestMethod]
        public void TestDelete()
        {
            Venta venta = sut.Read(1);
            Venta ventaBorrada = sut.Delete(1);
            Assert.IsTrue(_ventasIguales(venta, ventaBorrada));
            venta = sut.Read(1);
            Assert.IsNull(venta);
        }

        private bool _ventasIguales(Venta venta, Venta ventaBorrada)
        {
            bool iguales = true;
            if (venta.AppliedDiscount != ventaBorrada.AppliedDiscount)
                iguales = false;
            if (venta.DiferenciaDevolucion != ventaBorrada.DiferenciaDevolucion)
                iguales = false;
            if (venta.Id != ventaBorrada.Id)
                iguales = false;
            if (venta.NumeroEntradas != ventaBorrada.NumeroEntradas)
                iguales = false;
            if (venta.PrecioEntrada != ventaBorrada.PrecioEntrada)
                iguales = false;
            if (venta.SesionId != ventaBorrada.SesionId)
                iguales = false;
            if (venta.TotalVenta != ventaBorrada.TotalVenta)
                iguales = false;
            return iguales;
        }

        [TestMethod]
        public void TestDeleteNoExisteVenta()
        {
            Venta venta = sut.Delete(1);
            Assert.IsNull(venta);
        }
    }
}
