using Projekat.Models;
using Projekat.Models.PomocniModeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using static System.Net.WebRequestMethods;

namespace Projekat.Controllers
{
    [RoutePrefix("index")]
    public class HomeController : ApiController
    {

        [HttpGet]
        [Route("vratiManifestacije")]
        public List<Manifestacija> vratiManifestacije()
        {
            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            manifestacije = manifestacije.FindAll(x => !x.LogickiObrisan);
            manifestacije.Sort((x, y) => x.DatumVremeManifestacije.CompareTo(y.DatumVremeManifestacije));
            return manifestacije;

        }


        [HttpPost]
        [Route("pretraziManifestacije")]
        public List<Manifestacija> pretraziManifestacije([FromBody] PretragaManifestacijaModel parametri)
        {

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];


            if (!parametri.Naziv.Equals(""))
                manifestacije = manifestacije.FindAll(x => x.Naziv.ToLower().Contains(parametri.Naziv));
            if (!parametri.Lokacija.Equals(""))
                manifestacije = manifestacije.FindAll(x => x.MestoOdrzavanja.Adresa.Mesto.ToString().ToLower().Contains(parametri.Lokacija));

            if (!parametri.MestoOdrzavanja.Equals(""))
                manifestacije = manifestacije.FindAll(x => x.MestoOdrzavanja.Adresa.Ulica.ToString().ToLower().Contains(parametri.MestoOdrzavanja));

            if (parametri.Datum_od != DateTime.MinValue && parametri.Datum_do != DateTime.MinValue)
                manifestacije = manifestacije.FindAll(x => (x.DatumVremeManifestacije >= parametri.Datum_od) && (x.DatumVremeManifestacije <= parametri.Datum_do));

            manifestacije = manifestacije.FindAll(x => (x.Cena >= parametri.Cena_od) && (x.Cena <= parametri.Cena_do));

            manifestacije = manifestacije.FindAll(x => !x.LogickiObrisan);

            manifestacije.Sort((x, y) => x.DatumVremeManifestacije.CompareTo(y.DatumVremeManifestacije));

            return manifestacije;
        }

        [HttpPost]
        [Route("vratiKoordinateManifestacije")]
        public Koordinate vratiKoordinateManifestacije([FromBody] KomentarisanjeModel parametri)
        {
            List<Koordinate> koordinate = ManipulacijaPodacima.UcitajKoordinate("~/App_Data/koordinate.txt");
            Koordinate k = koordinate.Find(x => x.ManifestacijaNaziv.Equals(parametri.NazivManifestacije));

            return k;

          
        }

        [HttpPost]
        [Route("registrujKorisnika")]
        public IHttpActionResult registrujKorisnika([FromBody] RegistracijaKorisnikaModel parametri)
        {
            string poruka = "";
            bool validacija = true;

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];
            

            Korisnik pomocni = korisnici.Find(x => x.Username.Equals(parametri.Username));
            if (pomocni != null)
            {
                poruka += $"Vec postoji korisnik ciji je username = {parametri.Username}!";
                validacija = false;
            }

            if (parametri.Username.Length < 6)
            {
                poruka += "Username mora imati 6 karaktera!";
                validacija = false;
            }

            if (parametri.Username.Equals(""))
            {
                poruka += "Morate uneti username!";
                validacija = false;
            }

            if (parametri.Password.Length < 6)
            {
                poruka += "Password mora imati 6 karaktera!";
                validacija = false;
            }

            if (parametri.Password.Equals(""))
            {
                poruka += "Morate uneti password!";
                validacija = false;
            }

            if (parametri.Ime.Equals(""))
            {
                poruka += "Morate uneti ime!";
                validacija = false;
            }

            if (parametri.Prezime.Equals(""))
            {
                poruka += "Morate uneti prezime!";
                validacija = false;
            }

            if (parametri.DatumRodjenja.Equals(DateTime.MinValue))
            {
                poruka += "Morate izabrati datum rodjenja!";
                validacija = false;
            }

            if (parametri.DatumRodjenja > DateTime.Now)
            {
                poruka += "Datum rodjenja mora biti u proslosti!";
                validacija = false;
            }


            if (!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }
       

            Kupac k = new Kupac(parametri.Username, parametri.Password, parametri.Ime, parametri.Prezime, parametri.Pol, parametri.DatumRodjenja);
            korisnici.Add(k);
            ManipulacijaPodacima.UpisiKupca(k, "~/App_Data/kupci.txt");
            HttpContext.Current.Application["kupci"] = ManipulacijaPodacima.UcitajKupce("~/App_Data/kupci.txt");

            return Ok(k);
        }


        [HttpPost]
        [Route("ulogujKorisnika")]
        public IHttpActionResult ulogujKorisnika([FromBody] LogovanjeModel parametri)
        {
            string poruka = "";
            bool validacija = true;

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];


            if (parametri.Username.Length < 6)
            {
                poruka += "Username mora imati 6 karaktera!";
                validacija = false;
            }

            if (parametri.Username.Equals(""))
            {
                poruka += "Morate uneti username!";
                validacija = false;
            }

            if (parametri.Password.Length < 6)
            {
                poruka += "Password mora imati 6 karaktera!";
                validacija = false;
            }

            if (parametri.Password.Equals(""))
            {
                poruka += "Morate uneti password!";
                validacija = false;
            }


            Korisnik pomocni = korisnici.Find(x => x.Username.Equals(parametri.Username) && x.Password.Equals(parametri.Password) && !x.LogickiObrisan);
            if (pomocni == null)
            {
                poruka += "Ne postoji korisnik sa zadatom sifrom i korisnickim imenom!";
                validacija = false;
            }

            if (!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }


            HttpContext.Current.Session["korisnik"] = pomocni;


            return Ok(pomocni);
        }


        [HttpGet]
        [Route("izlogujSe")]
        public IHttpActionResult izlogujSe()
        {
            HttpContext.Current.Session["korisnik"] = null;
            return Ok();
        }

        

        [HttpGet]
        [Route("vratiDetalje")]
        public IHttpActionResult vratiDetalje([FromUri] string naziv)
        {
            string poruka = "";
            bool validacija = true;

           

            

            if (naziv.Equals(""))
            {
                poruka += "Prosledjeni parametar nije odgovarajuci!";
                validacija = false;
            }


            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];

            Manifestacija pomocni = manifestacije.Find(x => x.Naziv.Equals(naziv));
            if (pomocni == null)
            {
                poruka += "Ne postoji korisnik sa zadatom sifrom i korisnickim imenom!";
                validacija = false;            
            }

            if (!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }


            

            return Ok(pomocni);
        }

      


        [HttpGet]
        [Route("vratiUlogovanogKorisnika")]
        public IHttpActionResult vratiUlogovanogKorisnika()
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Ni jedan korisnik nije ulogovan!");
            }   

            return Ok(trenutnoUlogovan);
        }


        [HttpGet]
        [Route("vratiSveKomentareManifestacije")]
        public List<Komentar> vratiSveKomentareManifestacije([FromUri] string naziv)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Current.Application["komentari"];
            komentari = komentari.FindAll(x => !x.Obrisan && x.Odobren && !x.Odbijen && x.ManifestacijaNaziv.Equals(naziv));
           
            komentari.Reverse();
    
            return komentari;

        }



    }
}
