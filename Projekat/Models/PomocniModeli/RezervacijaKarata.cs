using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.PomocniModeli
{
    public class RezervacijaKarata
    {
        public int BrojRegular { get; set; }
        public int BrojVip { get; set; }
        public int BrojFanPit { get; set; }
        public double Cena { get; set; }
        public double OsvojeniBodovi { get; set; }
        public string NazivManifestacije { get; set; }


        
    }
}