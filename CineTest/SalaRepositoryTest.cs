using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using System.Collections.Generic;

namespace CineTest
{
    [TestClass]
    public class SalaRepositoryTest
    {
        private long[] _salas = new long[] { 1, 2, 3 };
        private SalaRepository sut;

        [TestInitialize]
        public void TestInicializa()
        {
            sut = SalaRepository.GetInstance();
        }

        [TestMethod]
        public void TestGetInstance()
        {
            Assert.IsNotNull(sut);
        }

        [TestMethod]
        public void TestGetInstanceSingleton()
        {
            SalaRepository secondSut = SalaRepository.GetInstance();
            Assert.AreEqual(sut, secondSut);
        }

        [TestMethod]
        public void TestGetInstanceClean()
        {
            SalaRepository secondSut = SalaRepository.GetInstance();
            sut.Clean();
            sut = SalaRepository.GetInstance();
            Assert.AreNotEqual(sut, secondSut);
        }
        [TestMethod]
        public void TestRead()
        {
            // lectura
            for (int i = 0; i < Constantes.Salas.Length; i++)
            {
                Sala sala = sut.Read(Constantes.Salas[i]);
                Assert.IsNotNull(sala);
                //id ok.
                Assert.AreEqual(Constantes.Salas[i], sala.Id);
                //aforo ok.
                Assert.AreEqual(Constantes.Aforos[i], sala.Aforo);
            }
        }

        [TestMethod]
        public void TestReadNoExisteSala()
        {
            Sala sala4 = sut.Read(4);
            Assert.IsNull(sala4);
        }
    }
}
