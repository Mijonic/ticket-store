using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.PomocniModeli
{
    public class PretragaKarataModel
    {
        public string Naziv { get; set; }
        public DateTime Datum_od { get; set; }
        public DateTime Datum_do { get; set; }
        public double Cena_od { get; set; }
        public double Cena_do { get; set; }
   
      
    }
}