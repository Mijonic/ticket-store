using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.PomocniModeli
{
    public class DodavanjeManifestacijeModel
    {
        public string Naziv { get; set; }
        public ManifestacijaTip TipManifestacije { get; set; }
        public int BrojMesta { get; set; }
        public DateTime DatumManifestacije { get; set; }
        public double Cena { get; set; } 
        public string Ulica { get; set; }
        public int Broj { get; set; }
        public string Mesto { get; set; }
        public int PostanskiBroj { get; set; }
        public string Poster { get; set; }

    }

   
}