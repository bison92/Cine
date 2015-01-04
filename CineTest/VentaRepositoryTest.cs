using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;

namespace CineTest
{
    [TestClass]
    public class VentaRepositoryTest
    {
        private VentaRepository sut;

        [TestInitialize]
        public void TestInicializa()
        {
            sut = new VentaRepository();
        }
        [TestMethod]
        public void TestCreate()
        {
            using (var ctx =  new CineDB()){
                using(var transaction = ctx.Database.BeginTransaction()){
                    Venta venta = new Venta(1, 10);
                    Venta res = sut.Create(venta);
                    Assert.AreEqual(1, res.Id);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }             
        }

        [TestMethod]
        public void TestRead()
        {
            using (var ctx =  new CineDB())
            {
                using(var transaction = ctx.Database.BeginTransaction())
                {
                    Venta res = sut.Read(1);
                    Venta res2 = sut.Read(12);
                    Assert.AreEqual(1, res.Id);
                    Assert.AreEqual(12, res2.Id);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestReadNoExisteVenta()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    Venta res = sut.Read(1);
                    Assert.IsNull(res);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestList()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    IDictionary<long, Venta> res = sut.List();
                    Assert.AreEqual(18, res.Count);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestListBySesion()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    for (int i = 1; i <= 9; i++)
                    {
                        IDictionary<long, Venta> resSesion = sut.List(i);
                        Assert.AreEqual(2, resSesion.Count);
                        foreach (var pareja in resSesion)
                        {
                            Venta venta = pareja.Value;
                            Assert.AreEqual(i, venta.SesionId);
                        }
                    }
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
        [TestMethod]
        public void TestListNoHayVentas()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    IDictionary<long, Venta> res = sut.List();
                    IDictionary<long, Venta> resSesion1 = sut.List(1);
                    Assert.AreEqual(0, res.Count);
                    Assert.AreEqual(0, resSesion1.Count);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestUpdate()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    Venta ventaAActualizar = new Venta(1, 10);
                    ventaAActualizar.Id = 1;
                    Venta antigua = sut.Read(1);
                    Venta actualizada = sut.Update(ventaAActualizar);
                    Assert.AreEqual(10, actualizada.NumeroEntradas);
                    Assert.AreNotEqual(antigua.NumeroEntradas, actualizada.NumeroEntradas);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestUpdateNoExisteVenta()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    Venta ventaAActualizar = new Venta(1, 10);
                    ventaAActualizar.Id = 1;
                    Venta actualizada = sut.Update(ventaAActualizar);
                    Assert.IsNull(actualizada);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
        }

        [TestMethod]
        public void TestDelete()
        {
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                    Venta venta = sut.Read(1);
                    Venta ventaBorrada = sut.Delete(1);
                    Assert.IsTrue(_ventasIguales(venta, ventaBorrada));
                    venta = sut.Read(1);
                    Assert.IsNull(venta);
                    transaction.Rollback();
                    transaction.Dispose();
                }
                ctx.Dispose();
            }
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
            using (var ctx = new CineDB())
            {
                using (var transaction = ctx.Database.BeginTransaction())
                {
                Venta venta = sut.Delete(1);
                Assert.IsNull(venta);
                transaction.Rollback();
                transaction.Dispose();
                }
                ctx.Dispose();
            }
        }
    }
}
