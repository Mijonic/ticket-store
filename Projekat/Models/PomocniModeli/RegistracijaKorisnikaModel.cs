using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.PomocniModeli
{
    public class RegistracijaKorisnikaModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public PolTip Pol { get; set; }
        public DateTime DatumRodjenja { get; set; }

        
    }
}