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
        private CineDB context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void TestInicializa()
        {
            context = new CineDB();
            transaction = context.Database.BeginTransaction();
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
            int ventas = sut.List().Count();
            Venta res = sut.Create(new Venta(1, 10));
            Assert.AreEqual(ventas+1, sut.List().Count());
        }

        [TestMethod]
        public void TestRead()
        {
            Venta cr = sut.Create(new Venta(1, 10)); // unit guarro testing?
            Venta res = sut.Read(cr.VentaId);
            Assert.AreEqual(cr.VentaId, res.VentaId);
            Assert.AreEqual(10, res.NumeroEntradas);
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
            sut.Create(new Venta(1, 20));
            sut.Create(new Venta(1, 20));
            IDictionary<long, Venta> res = (Dictionary<long,Venta>)sut.List();
            Assert.AreEqual(2, res.Count);
        }

        [TestMethod]
        public void TestListBySesion()
        {
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                sut.Create(new Venta(Constantes.Sesiones[i], 10));
                sut.Create(new Venta(Constantes.Sesiones[i], 10));
                IDictionary<long, Venta> resSesion = (Dictionary<long,Venta>)sut.List(Constantes.Sesiones[i]);
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
            IDictionary<long, Venta> res = (Dictionary<long, Venta>)sut.List();
            IDictionary<long, Venta> resSesion1 = (Dictionary<long, Venta>)sut.List(1);
            Assert.AreEqual(0, res.Count);
            Assert.AreEqual(0, resSesion1.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Venta noVinculado = new Venta(1, 20);
            Venta cr = sut.Create(new Venta(1,20));
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.VentaId = cr.VentaId;
            Venta actualizada = sut.Update(ventaAActualizar);
            Assert.AreEqual(10, actualizada.NumeroEntradas);
            Assert.AreNotEqual(noVinculado.NumeroEntradas, actualizada.NumeroEntradas);
        }

        [TestMethod]
        [ExpectedException(typeof(VentaException))]
        public void TestUpdateNoExisteVenta()
        {
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.VentaId = 1;
            Venta actualizada = sut.Update(ventaAActualizar);
        }

        [TestMethod]
        public void TestDelete()
        {
            Venta cr = sut.Create(new Venta(1, 20));
            Venta ventaBorrada = sut.Delete(cr.VentaId);
            Assert.AreEqual(cr, ventaBorrada);
            cr = sut.Read(1);
            Assert.IsNull(cr);
        }
        [TestMethod]
        public void TestDeleteNoExisteVenta()
        {
            Venta venta = sut.Delete(1);
            Assert.IsNull(venta);
        }
    }
}
