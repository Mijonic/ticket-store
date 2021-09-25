using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Kupac : Korisnik
    {
        private List<Karta> sveKarte;
        private double brojSakupljenihBodova;
        private TipKorisnika tipKorisnika;

        public Kupac(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja) : base(username, password, ime, prezime, pol, datumRodjenja)
        {
            this.Uloga = UlogaTip.KUPAC;
            BrojSakupljenihBodova = 0;
            TipKorisnika = new TipKorisnika(ImeTipaKorisnika.BRONZANI,0,250);

            SveKarte = new List<Karta>();
        }

        public Kupac(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja, UlogaTip uloga, bool logickiObrisan,double brojSakupljenihBodova,TipKorisnika tipKorisnika) : base(username, password, ime, prezime, pol, datumRodjenja,uloga,logickiObrisan)
        {

            BrojSakupljenihBodova = brojSakupljenihBodova;
            TipKorisnika = tipKorisnika;

            SveKarte = new List<Karta>();
        }



        public override string ToString()
        {
            return base.ToString() + $"|{BrojSakupljenihBodova}|{tipKorisnika}";
        }



        public List<Karta> SveKarte { get => sveKarte; set => sveKarte = value; }
        public double BrojSakupljenihBodova { get => brojSakupljenihBodova; set => brojSakupljenihBodova = value; }
        public TipKorisnika TipKorisnika { get => tipKorisnika; set => tipKorisnika = value; }
    }
}