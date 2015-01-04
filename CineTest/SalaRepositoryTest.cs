using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using System.Collections.Generic;
using System.Data.Entity;

namespace CineTest
{
    [TestClass]
    public class SalaRepositoryTest
    {
        private SalaRepository sut;
        private CineDB ctx;
        private DbContextTransaction transaction;
        [TestInitialize]
        public void TestInicializa()
        {
           
            ctx = new CineDB();
            transaction = ctx.Database.BeginTransaction();
            sut = new SalaRepository(ctx);
        }
        [TestCleanup]
        public void TestFinaliza()
        {
            transaction.Rollback();
            transaction.Dispose();
            ctx.Dispose();
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
