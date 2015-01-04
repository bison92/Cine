﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Cine;
using System.Collections.Generic;
using System.Data.Entity;

namespace CineTest
{
    [TestClass]
    public class SesionRepositoryTest
    {
        private SesionRepository sut;
        private CineDB context;
        private DbContextTransaction transaction;

        [TestInitialize]
        public void TestInicializa()
        {
            context = new CineDB();
            transaction = context.Database.BeginTransaction();
            sut = new SesionRepository(context);
        }
        [TestCleanup]
        public void TestCleanUp()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }
        [TestMethod]
        public void TestRead()
        {
            for (int i = 0; i < Constantes.Sesiones.Length; i++)
            {
                Sesion sesion = sut.Read(Constantes.Sesiones[i]);
                Assert.IsNotNull(sesion);
                Assert.AreEqual(Constantes.Sesiones[i], sesion.Id);
                Assert.AreEqual(Constantes.Salas[i % Constantes.Salas.Length], sesion.SalaId);
                Assert.AreEqual(Constantes.Horas[i], sesion.Hora);
            }
        }

        [TestMethod]
        public void TestReadNoExisteSesion()
        {
            Sesion sesionNoExiste = sut.Read(Constantes.SesionNoExiste);
            Assert.IsNull(sesionNoExiste);
        }

        [TestMethod]
        public void TestList()
        {
            IDictionary<long,Sesion> sesionesAll = sut.List();
            Assert.AreEqual(Constantes.Sesiones.Length, sesionesAll.Count);
            foreach (int salaId in Constantes.Salas)
            {
                IDictionary<long,Sesion> sesiones = sut.List(salaId);
                Assert.AreEqual(Constantes.SesionesPorSala, sesiones.Count, "Sesiones por sala no coinciden");
                foreach (var pareja in sesiones)
                {
                    Assert.AreEqual(salaId, pareja.Value.SalaId, "Id de las salas no es correcto");
                }
            }
        }

        [TestMethod]
        public void TestListNoExisteSala()
        {
            IDictionary<long,Sesion> sesiones4 = sut.List(Constantes.SalaNoExiste);
            Assert.AreEqual(0, sesiones4.Count);
        }

        [TestMethod]
        public void TestUpdate()
        {
            Sesion sesion1 = sut.Update(Constantes.Sesiones[0], true);
            Assert.AreEqual(true, sesion1.EstaAbierta);
            sesion1 = sut.Update(Constantes.Sesiones[0], false);
            Assert.AreEqual(false, sesion1.EstaAbierta);
        }

        [TestMethod]
        [ExpectedException(typeof(SesionException))]
        public void TestUpdateNoExisteSesion()
        {
            Sesion sesion = sut.Update(Constantes.SesionNoExiste, false);
        }

    }
}
