using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Koordinate
    {
        private double sirina;
        private double duzina;
        private string manifestacijaNaziv;

        public Koordinate()
        {

        }

        public Koordinate(double sirina, double duzina, string manifestacijaNaziv)
        {
            Sirina = sirina;
            Duzina = duzina;
            ManifestacijaNaziv = manifestacijaNaziv;
        }

        public override string ToString()
        {
            return $"{Sirina}|{Duzina}|{ManifestacijaNaziv}";
        }

        public double Sirina { get => sirina; set => sirina = value; }
        public double Duzina { get => duzina; set => duzina = value; }
        public string ManifestacijaNaziv { get => manifestacijaNaziv; set => manifestacijaNaziv = value; }
    }
}