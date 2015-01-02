using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using Moq;
using Cine.Interfaces;

namespace CineTest
{
    [TestClass]
    public class SalaServiceTest
    {
        private ISalaService _sut;
        private Mock<ISalaRepository> _salaRepository;
        [TestInitialize]
        public void TestInitialize()
        {

            _salaRepository = new Mock<ISalaRepository>();
            _sut = new SalaService(_salaRepository.Object);
        }
        [TestMethod]
        public void TestRead()
        {
            _salaRepository.Setup(r => r.Read(It.IsIn<long>(Cine.Constantes.Salas))).Returns((long id) => { return new Sala(id, Constantes.Aforos[id - 1]); });
            for (long i = 0; i < Constantes.Salas.Length; i++)
            {
                _sut.Read(Constantes.Salas[i]);
            }
            _salaRepository.Verify(r => r.Read(It.IsIn<long>(Cine.Constantes.Salas)), Times.Exactly(Constantes.Salas.Length));
        }
        [TestMethod]
        [ExpectedException(typeof(SalaException))]
        public void TestReadNoExisteSala()
        {
            _salaRepository.Setup(r => r.Read(It.IsNotIn<long>(Constantes.Salas))).Returns<Sala>(null);
            _sut.Read(Constantes.Salas.Length + 1);
        }
    }
}