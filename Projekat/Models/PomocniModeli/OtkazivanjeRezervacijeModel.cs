using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models.PomocniModeli
{
    public class OtkazivanjeRezervacijeModel
    {

        public string Id { get; set; }
        public string UsernameKupca { get; set; }
        public string NazivManifestacije { get; set; }
        public double BrojIzgubljenihBodova { get; set; }

    }
}