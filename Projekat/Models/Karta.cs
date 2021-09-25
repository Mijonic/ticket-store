using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Karta
    {
        private string id; // jedinstveno 10 karakterta 
        private string nazivManifestacije;
        private DateTime datumVremeManifestacije;
        // datum i vreme manifestacije  sta ce mi ovo kad imam citavu manifestaciju?
        private double cena;
        private string usernameKupca;
        private string imeKupca;
        private string prezimeKupca;
        private StatusKarteTip statusKarte; //rezervisana/odustanak
        private KartaTip tipKarte; //vip, regular, fan pit
        private DateTime datumOtkazivanja;
        private bool logickiObrisana;
       

        public Karta()
        {

        }

        public Karta(string id, double cena, StatusKarteTip statusKarte, KartaTip tipKarte, string nazivManifestacije, DateTime datumVremeManifestacije, string usernameKupca, string imeKupca, string prezimeKupca, DateTime datumOtkazivanja, bool logickiObrisana)
        {
            Id = id;
            Cena = cena;
            StatusKarte = statusKarte;
            TipKarte = tipKarte;
            NazivManifestacije = nazivManifestacije;
            DatumVremeManifestacije = datumVremeManifestacije;
            UsernameKupca = usernameKupca;
            ImeKupca = imeKupca;
            PrezimeKupca = prezimeKupca;
            DatumOtkazivanja = datumOtkazivanja;
            LogickiObrisana = logickiObrisana;
        }

        public override string ToString()
        {
            return $"{Id}|{Cena}|{StatusKarte}|{TipKarte}|{NazivManifestacije}|{DatumVremeManifestacije.ToString("dd-MM-yyyy hh:mm:ss")}|{UsernameKupca}|{ImeKupca}|{PrezimeKupca}|{DatumOtkazivanja.ToString("dd-MM-yyyy hh:mm:ss")}|{LogickiObrisana}";
        }

        public string Id { get => id; set => id = value; }
        public double Cena { get => cena; set => cena = value; } 
        public StatusKarteTip StatusKarte { get => statusKarte; set => statusKarte = value; }
        public KartaTip TipKarte { get => tipKarte; set => tipKarte = value; }
        public string NazivManifestacije { get => nazivManifestacije; set => nazivManifestacije = value; }
        public DateTime DatumVremeManifestacije { get => datumVremeManifestacije; set => datumVremeManifestacije = value; }
        public string UsernameKupca { get => usernameKupca; set => usernameKupca = value; }
        public string ImeKupca { get => imeKupca; set => imeKupca = value; }
        public string PrezimeKupca { get => prezimeKupca; set => prezimeKupca = value; }
        public DateTime DatumOtkazivanja { get => datumOtkazivanja; set => datumOtkazivanja = value; }
        public bool LogickiObrisana { get => logickiObrisana; set => logickiObrisana = value; }
    }
}