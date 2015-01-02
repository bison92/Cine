using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cine
{
    public static class Constantes
    {
        // el código de los repositorios esta ideado para que dependan de estos datos hardcodeados 
        // que supuestamente estarían en la base de datos. La relación sesiones por Sala debe de ser 3.
        public static int SesionesPorSala = 3;
        public static long[] Salas = new long[] { 1, 2, 3 };
        public static int[] Aforos = new int[] { 100, 50, 20 };
        public static long[] Sesiones = new long[] { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public static string[] Horas = new string[] {"5:30", "7:30", "10","5","7","10","5","7:30","10:30"};
        public static long SalaNoExiste = 4;
        public static long SesionNoExiste = 10;
        public const int Discount = 10; // percentual points
        public const int DiscountThreshold = 5; // inclusive
        public const double TicketPrice = 7.0d; 
    }
}