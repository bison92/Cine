using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class VentaRepositoryTest
    {
        private VentaRepository sut;

        [TestInitialize]
        public void TestInicializa()
        {
            sut = VentaRepository.GetInstance();
            sut.Clean();
            sut = VentaRepository.GetInstance();
        }

        [TestMethod]
        public void TestGetInstance()
        {
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void TestGetInstanceSingleton()
        {
            VentaRepository secondSut = VentaRepository.GetInstance();
            Assert.AreEqual(sut, secondSut);
        }

        [TestMethod]
        public void TestGetInstanceClean()
        {
            VentaRepository secondSut = VentaRepository.GetInstance();
            sut.Clean();
            sut = VentaRepository.GetInstance();
            Assert.AreNotEqual(sut, secondSut);
        }

        [TestMethod]
        public void TestCreate()
        {
            Venta venta = new Venta(1, 10);
            Venta res = sut.Create(venta);
            Assert.AreEqual(1, res.Id);
        }

        [TestMethod]
        public void TestRead()
        {
            CrearVentas();
            Venta res = sut.Read(1);
            Venta res2 = sut.Read(12);
            Assert.AreEqual(1, res.Id);
            Assert.AreEqual(12, res2.Id);
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
            CrearVentas();
            IDictionary<long, Venta> res = sut.List();
            Assert.AreEqual(18, res.Count);
        }

        [TestMethod]
        public void TestListBySesion()
        {
            CrearVentas();
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
        }
        [TestMethod]
        public void TestListNoHayVentas()
        {
            IDictionary<long,Venta> res = sut.List();
            IDictionary<long,Venta> resSesion1 = sut.List(1);
            Assert.AreEqual(0, res.Count);
            Assert.AreEqual(0, resSesion1.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            CrearVentas();
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.Id = 1;
            Venta antigua = sut.Read(1);
            Venta actualizada = sut.Update(ventaAActualizar);
            Assert.AreEqual(10, actualizada.NumeroEntradas);
            Assert.AreNotEqual(antigua.NumeroEntradas, actualizada.NumeroEntradas);
        }

        [TestMethod]
        public void TestUpdateNoExisteVenta()
        {
            Venta ventaAActualizar = new Venta(1, 10);
            ventaAActualizar.Id = 1;
            Venta actualizada = sut.Update(ventaAActualizar);
            Assert.IsNull(actualizada);
        }

        [TestMethod]
        public void TestDelete()
        {
            CrearVentas();
            Venta venta = sut.Read(1);
            Venta ventaBorrada = sut.Delete(1);
            Assert.AreEqual(venta, ventaBorrada);
            venta = sut.Read(1);
            Assert.IsNull(venta);
        }

        [TestMethod]
        public void TestDeleteNoExisteVenta()
        {
            Venta venta = sut.Delete(1);
            Assert.IsNull(venta);
        }

        private void CrearVentas()
        {
            sut.Create(new Venta(1, 2));
            sut.Create(new Venta(1, 7));
            sut.Create(new Venta(2, 3));
            sut.Create(new Venta(2, 4));
            sut.Create(new Venta(3, 5));
            sut.Create(new Venta(3, 7));

            sut.Create(new Venta(4, 2));
            sut.Create(new Venta(4, 7));
            sut.Create(new Venta(5, 3));
            sut.Create(new Venta(5, 4));
            sut.Create(new Venta(6, 5));
            sut.Create(new Venta(6, 7));

            sut.Create(new Venta(7, 2));
            sut.Create(new Venta(7, 7));
            sut.Create(new Venta(8, 3));
            sut.Create(new Venta(8, 4));
            sut.Create(new Venta(9, 5));
            sut.Create(new Venta(9, 7));

        }
    }
}
