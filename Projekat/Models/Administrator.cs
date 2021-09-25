using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Administrator : Korisnik
    {

        public Administrator(string username, string password, string ime, string prezime, PolTip pol,DateTime datumRodjenja) : base( username,  password,  ime,  prezime,  pol,  datumRodjenja)
        {
            this.Uloga = UlogaTip.ADMINISTRATOR;
        }

        public Administrator(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja, UlogaTip uloga, bool logickiObrisan) : base(username, password, ime, prezime, pol, datumRodjenja,  uloga,  logickiObrisan)
        {
            
        }
      


        public override string ToString()
        {
            return base.ToString();
        }



    }
}