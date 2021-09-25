using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Lokacija
    {
        private double geografskaDuzina;
        private double geografskaSirina;
        private MestoOdrzavanja mestoOdrzavanja;

        public Lokacija()
        {

        }

        public Lokacija(double geografskaDuzina, double geografskaSirina, MestoOdrzavanja mestoOdrzavanja)
        {
            GeografskaDuzina = geografskaDuzina;
            GeografskaSirina = geografskaSirina;
            MestoOdrzavanja = mestoOdrzavanja;
        }

        public double GeografskaDuzina { get => geografskaDuzina; set => geografskaDuzina = value; }
        public double GeografskaSirina { get => geografskaSirina; set => geografskaSirina = value; }
        public MestoOdrzavanja MestoOdrzavanja { get => mestoOdrzavanja; set => mestoOdrzavanja = value; }
    }
}