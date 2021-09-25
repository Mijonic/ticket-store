using Projekat.Models;
using Projekat.Models.PomocniModeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;

namespace Projekat.Controllers
{
    [RoutePrefix("prodavacPrikaz")]
    public class ProdavacController : ApiController
    {

        [HttpGet]
        [Route("vratiUlogovanogProdavca")]
        public IHttpActionResult vratiUlogovanogProdavca()
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Ni jedan korisnik nije ulogovan!");
            }

            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];

            Prodavac a = prodavci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            if (a == null)
                return Content(HttpStatusCode.Unauthorized, "Ni jedan prodavac nije ulogovan!");

            return Ok(a);
        }


        [HttpPost]
        [Route("dodajManifestaciju")]
        public IHttpActionResult dodajManifestaciju([FromBody]  DodavanjeManifestacijeModel parametri)
        {

            string poruka = "";
            bool validacija = true;

            if(parametri.Naziv.Equals(""))
            {
                poruka += "Morate uneti naziv manifestacije";
                validacija = false;

            }

            if(parametri.BrojMesta < 0)
            {
                poruka += "Broj mesta na manifestaciji mora biti veci od 0!\n";
                validacija = false;
            }

            if(parametri.DatumManifestacije.Equals(DateTime.MinValue))
            {
                poruka += "Morate izabrati datum i vreme manifestacije!\n";
                validacija = false;
            }

            if(parametri.Cena < 0)
            {
                poruka += "Cena manifestacije mora biti veca od 0$\n";
                validacija = false;
            }

            if (parametri.Ulica.Trim().Equals(""))
            {
                poruka += "Morate uneti ulicu u kojoj se odrzava manifestacija!\n";
                validacija = false;
            }

            if(parametri.Broj < 0)
            {
                poruka += "Morate uneti broj ulice u kojoj se odrzava manifestacija!\n";
                validacija = false;
            }

            if(parametri.PostanskiBroj < 0)
            {
                poruka += "Postanski broj ne moze biti negativan!\n";
                validacija = false;
            }


            if(parametri.Poster.Trim().Equals(""))
            {
                poruka += "Izaberite sliku!";
                validacija = false;
            }

           

            

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija m = manifestacije.Find(x => x.MestoOdrzavanja.Adresa.Mesto.Equals(parametri.Mesto) && x.DatumVremeManifestacije.Equals(parametri.DatumManifestacije) && !x.LogickiObrisan);


            if(m != null)
            {
                poruka += "Vec postoji zakazana manifestacija na zadatoj lokaciji i zadatog datuma!";
                validacija = false;
            }

            m = manifestacije.Find(x => x.Naziv.Equals(parametri.Naziv) && !x.LogickiObrisan);

            if(m != null)
            {
                poruka += $"Vec postoji manifestacija sa nazivom {parametri.Naziv}!";
                validacija = false;
            }

            if (!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }


            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.PRODAVAC)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            Manifestacija novaManifestacija = new Manifestacija(parametri.Naziv, parametri.TipManifestacije, parametri.BrojMesta, parametri.DatumManifestacije, parametri.Cena,  StatusManifestacijeTip.NEAKTIVNO, new MestoOdrzavanja(parametri.Ulica, parametri.Broj, parametri.Mesto, parametri.PostanskiBroj), parametri.Poster, trenutnoUlogovan.Username);
            manifestacije.Add(novaManifestacija);
            ManipulacijaPodacima.UpisiManifestaciju(novaManifestacija, "~/App_Data/manifestacije.txt");
            Koordinate k = (Koordinate)HttpContext.Current.Session["koordinate"];

            if (k != null)
            {


                k.ManifestacijaNaziv = parametri.Naziv;
            }else
            {
                k = new Koordinate();
              
                k.Sirina = 45.249059739446885;
                k.Duzina = 19.825387001037598;
                k.ManifestacijaNaziv = parametri.Naziv;
            }
            ManipulacijaPodacima.UpisiKoordinate(k, "~/App_Data/koordinate.txt");

            HttpContext.Current.Application["manifestacije"] = ManipulacijaPodacima.UcitajManifestacije("~/App_Data/manifestacije.txt");

            return Ok(novaManifestacija);


        }


        [HttpPost]
        [Route("izmeniManifestaciju")]
        public IHttpActionResult izmeniManifestaciju([FromBody]  DodavanjeManifestacijeModel parametri)
        {

            string poruka = "";
            bool validacija = true;

            if (parametri.Naziv.Equals(""))
            {
                poruka += "Morate uneti naziv manifestacije";
                validacija = false;

            }

            if (parametri.BrojMesta < 0)
            {
                poruka += "Broj mesta na manifestaciji mora biti veci od 0!\n";
                validacija = false;
            }

            if (parametri.DatumManifestacije.Equals(DateTime.MinValue))
            {
                poruka += "Morate izabrati datum i vreme manifestacije!\n";
                validacija = false;
            }

            if (parametri.Cena < 0)
            {
                poruka += "Cena manifestacije mora biti veca od 0$\n";
                validacija = false;
            }

            if (parametri.Ulica.Trim().Equals(""))
            {
                poruka += "Morate uneti ulicu u kojoj se odrzava manifestacija!\n";
                validacija = false;
            }

            if (parametri.Broj < 0)
            {
                poruka += "Morate uneti broj ulice u kojoj se odrzava manifestacija!\n";
                validacija = false;
            }

            if (parametri.PostanskiBroj < 0)
            {
                poruka += "Postanski broj ne moze biti negativan!\n";
                validacija = false;
            }


           



            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija m = manifestacije.Find(x => x.MestoOdrzavanja.Adresa.Mesto.Equals(parametri.Mesto) && x.DatumVremeManifestacije.Equals(parametri.DatumManifestacije) && !x.LogickiObrisan && x.Naziv != parametri.Naziv);


            if (m != null)
            {
                poruka += "Vec postoji zakazana manifestacija na zadatoj lokaciji i zadatog datuma!";
                validacija = false;
            }

            m = manifestacije.Find(x => x.Naziv.Equals(parametri.Naziv) && !x.LogickiObrisan);

           

            if (!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }


            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.PRODAVAC)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            Manifestacija trazena = manifestacije.Find(x => !x.LogickiObrisan && x.Naziv.Equals(parametri.Naziv));

            trazena.MestoOdrzavanja.Adresa.Mesto = parametri.Mesto;
            trazena.Naziv = parametri.Naziv;

            if (parametri.Poster != null && parametri.Poster != "")
            {
                trazena.PosterManifestacije = parametri.Poster;
            }

            trazena.PreostaloKarata = parametri.BrojMesta;
            trazena.BrojMesta = parametri.BrojMesta;
            trazena.Cena = parametri.Cena;
            trazena.DatumVremeManifestacije = parametri.DatumManifestacije;
            trazena.TipManifestacije = parametri.TipManifestacije;
            trazena.MestoOdrzavanja.Adresa.Broj = parametri.Broj;
            trazena.MestoOdrzavanja.Adresa.PostanskiBroj = parametri.PostanskiBroj;
            trazena.MestoOdrzavanja.Adresa.Ulica = parametri.Ulica;

           
            

            Koordinate trenutnoIzabrane = (Koordinate)HttpContext.Current.Session["koordinate"];
            
            List<Koordinate> koordinate = ManipulacijaPodacima.UcitajKoordinate("~/App_Data/koordinate.txt");
            Koordinate k = koordinate.Find(x => x.ManifestacijaNaziv.Equals(parametri.Naziv));


            if (trenutnoIzabrane != null)
            {
                if (trenutnoIzabrane.Duzina != k.Duzina || trenutnoIzabrane.Sirina != k.Sirina)
                {
                    k.Sirina = trenutnoIzabrane.Sirina;
                    k.Duzina = trenutnoIzabrane.Duzina;
                    ManipulacijaPodacima.UpisiSveKoordinate(koordinate, "~/App_Data/koordinate.txt");
                }
            }

            ManipulacijaPodacima.UpisiSveManifestacije(manifestacije, "~/App_Data/manifestacije.txt");
            HttpContext.Current.Application["manifestacije"] = ManipulacijaPodacima.UcitajManifestacije("~/App_Data/manifestacije.txt");





          

            return Ok(trazena);


        }


        [HttpGet]
        [Route("vratiManifestaciju")]
        public Manifestacija vratiManifestaciju([FromUri] string naziv)
        {
            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija m = manifestacije.Find(x => !x.LogickiObrisan && x.Naziv.Equals(naziv));

            return m;

        }



        [HttpGet]
        [Route("vratiManifestacijeProdavca")]
        public List<Manifestacija> vratiManifestacijeProdavca()
        {
            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Manifestacija> trazeneManifestacije = manifestacije.FindAll(x => x.UsernameProdavca.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            return trazeneManifestacije;

        }

        [Route("izmeniProdavca")]
        public IHttpActionResult izmeniProdavca([FromBody] RegistracijaKorisnikaModel parametri)
        {

            if(parametri.DatumRodjenja == null || parametri.Ime == null || parametri.Password == null || parametri.Prezime == null || parametri.Username == null)
                return  Content(HttpStatusCode.Forbidden, "Nevalidni podaci");

            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];

            Prodavac p = prodavci.Find(x => !x.LogickiObrisan && x.Username.Equals(parametri.Username));       
            p.Password = parametri.Password;
            p.Ime = parametri.Ime;
            p.Prezime = parametri.Prezime;
            p.Pol = parametri.Pol;
            p.DatumRodjenja = parametri.DatumRodjenja;

            ManipulacijaPodacima.UpisiSveProdavce(prodavci, "~/App_Data/prodavci.txt");
            HttpContext.Current.Application["korisnici"] = ManipulacijaPodacima.UcitajSveKorisnike();

            


            return Ok();

        }


        [HttpGet]
        [Route("vratiRezervisaneKarteMojihManifestacija")]
        public List<Karta> vratiRezervisaneKarteMojihManifestacija()
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];

            Prodavac p = prodavci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            List<Karta> karte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
            List<Karta> trazeneKarte = new List<Karta>();

            foreach(Manifestacija m in p.Manifestacije)
            {
                foreach(Karta k in karte)
                {
                    if ( !m.LogickiObrisan && !k.LogickiObrisana && k.NazivManifestacije.Equals(m.Naziv))
                        trazeneKarte.Add(k);
                }
            }



            return trazeneKarte;

        }


        [HttpGet]
        [Route("vratiKupceKarata")]
        public List<Kupac> vratiKupceKarata()
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];

            Prodavac p = prodavci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            List<Karta> karte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
            List<Karta> trazeneKarte = new List<Karta>();

            foreach (Manifestacija m in p.Manifestacije)
            {
                foreach (Karta k in karte)
                {
                    if (!m.LogickiObrisan && !k.LogickiObrisana && k.NazivManifestacije.Equals(m.Naziv))
                        trazeneKarte.Add(k);
                }
            }

            List<string> kupciPom = new List<string>();

            trazeneKarte.ForEach(x => kupciPom.Add(x.UsernameKupca));

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            List<Kupac> trazeniKupc = new List<Kupac>();


            foreach (Kupac k in kupci)
            {
                foreach(string username in kupciPom)
                {
                    if (k.Username.Equals(username) && !k.LogickiObrisan)
                    {
                        if (!trazeniKupc.Contains(k))
                            trazeniKupc.Add(k);
                    }

                }
            }

            return trazeniKupc;

        }



        
        [HttpPost]
        [Route("pretraziKarteProdavac")]
        public List<Karta> pretraziKarteKupca([FromBody] PretragaKarataModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];

            Prodavac p = prodavci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            List<Karta> karte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
            List<Karta> trazeneKarte = new List<Karta>();

            foreach (Manifestacija m in p.Manifestacije)
            {
                foreach (Karta k in karte)
                {
                    if (!m.LogickiObrisan && !k.LogickiObrisana && k.NazivManifestacije.Equals(m.Naziv))
                        trazeneKarte.Add(k);
                }
            }

            

            if (!parametri.Naziv.Equals(""))
                trazeneKarte = trazeneKarte.FindAll(x => x.NazivManifestacije.Equals(parametri.Naziv));

            if (parametri.Datum_od != DateTime.MinValue && parametri.Datum_do != DateTime.MinValue)
                trazeneKarte = trazeneKarte.FindAll(x => x.DatumVremeManifestacije >= parametri.Datum_od && x.DatumVremeManifestacije <= parametri.Datum_do);

            trazeneKarte = trazeneKarte.FindAll(x => (x.Cena >= parametri.Cena_od) && (x.Cena <= parametri.Cena_do));

            return trazeneKarte;

        }


        [HttpPost]
        [Route("sacuvajKoordinate")]
        public void sacuvajKoordinate([FromBody] KoordinateModel parametri)
        {

            Koordinate model = new Koordinate();
            if (parametri.Duzina == 0 || parametri.Sirina == 0)
            {
                model.Sirina = 45.249059739446885;
                model.Duzina = 19.825387001037598;
            }else
            {
                model.Sirina = parametri.Sirina;
                model.Duzina = parametri.Duzina;
            }


                                         
            HttpContext.Current.Session["koordinate"] = model;
            
        }

        [HttpGet]
        [Route("vratiKomentare")]
        public List<Komentar> vratiKomentare([FromUri] string naziv)
        {
            List<Komentar> komentari = (List<Komentar>)HttpContext.Current.Application["komentari"];
            komentari = komentari.FindAll(x => !x.Obrisan && x.ManifestacijaNaziv.Equals(naziv));

            komentari.Reverse();

            return komentari;

        }

        
        [HttpPost]
        [Route("odobriKomentar")]                //koristim LogovanjeModel ne treba validacija
        public IHttpActionResult odobriKomentar([FromBody] LogovanjeModel parametri)
        {

            if (parametri.Username == null)
            {
                return Content(HttpStatusCode.Forbidden, "Nevalidan id komentara!");
            }

            List<Komentar> komentari = (List<Komentar>)HttpContext.Current.Application["komentari"];
            Komentar trazeni = komentari.Find(x => x.IdKomentara.Equals(parametri.Username) && !x.Obrisan);
            trazeni.Odobren = true;

            ManipulacijaPodacima.UpisiSveKomentare(komentari, "~/App_Data/komentari.txt");




            return Ok();

        }

       

        [HttpPost]
        [Route("odbijanjeKomentara")]                       //koristim logovanjeModel nije potrebna validacija
        public IHttpActionResult odbijanjeKomentara([FromBody] LogovanjeModel parametri)
        {


            List<Komentar> komentari = (List<Komentar>)HttpContext.Current.Application["komentari"];
            Komentar trazeni = komentari.Find(x => x.IdKomentara.Equals(parametri.Username) && !x.Obrisan);
            trazeni.Odbijen = true;

            ManipulacijaPodacima.UpisiSveKomentare(komentari, "~/App_Data/komentari.txt");

           
          


            return Ok();

        }

        






    }
}
