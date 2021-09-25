using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Prodavac : Korisnik
    {
        private List<Manifestacija> manifestacije;


        public Prodavac(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja) : base(username, password, ime, prezime, pol, datumRodjenja)
        {
            this.Uloga = UlogaTip.PRODAVAC;
            Manifestacije = new List<Manifestacija>();
        }

        public Prodavac(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja, UlogaTip uloga, bool logickiObrisan) : base(username, password, ime, prezime, pol, datumRodjenja,uloga,logickiObrisan)
        {
            Manifestacije = new List<Manifestacija>();
        }



        public List<Manifestacija> Manifestacije { get => manifestacije; set => manifestacije = value; }
    }
}