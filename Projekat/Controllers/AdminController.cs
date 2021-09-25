using Projekat.Models;
using Projekat.Models.PomocniModeli;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Projekat.Controllers
{
    [RoutePrefix("adminPrikaz")]
    public class AdminController : ApiController
    {
        [HttpGet]
        [Route("vratiUlogovanogAdmina")]
        public IHttpActionResult vratiUlogovanogAdmina()
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Ni jedan korisnik nije ulogovan!");
            }

            List<Administrator> admini = (List<Administrator>)HttpContext.Current.Application["admini"];

            Administrator a = admini.Find(x => x.Username.Equals(trenutnoUlogovan.Username));

            if(a == null)
                return Content(HttpStatusCode.Unauthorized, "Ni jedan admin nije ulogovan!");

            return Ok(a);
        }

        [HttpPost]
        [Route("registrujProdavca")]
        public IHttpActionResult registrujProdavca([FromBody] RegistracijaKorisnikaModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if(trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
             return  Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


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


            Prodavac k = new Prodavac(parametri.Username, parametri.Password, parametri.Ime, parametri.Prezime, parametri.Pol, parametri.DatumRodjenja);
            korisnici.Add(k);
            ManipulacijaPodacima.UpisiProdavca(k, "~/App_Data/prodavci.txt");
            HttpContext.Current.Application["prodavci"] = ManipulacijaPodacima.UcitajProdavce("~/App_Data/prodavci.txt");

            return Ok(k);
        }

        [HttpPost]
        [Route("izmeniAdmina")]
        public IHttpActionResult izmeniAdmina([FromBody] RegistracijaKorisnikaModel parametri)
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            if (parametri.DatumRodjenja == null || parametri.Ime == null || parametri.Password == null || parametri.Prezime == null || parametri.Username == null)
                return Content(HttpStatusCode.Forbidden, "Nevalidni podaci");

            List<Administrator> admini = (List<Administrator>)HttpContext.Current.Application["admini"];

            Administrator p = admini.Find(x => !x.LogickiObrisan && x.Username.Equals(parametri.Username));
            p.Password = parametri.Password;
            p.Ime = parametri.Ime;
            p.Prezime = parametri.Prezime;
            p.Pol = parametri.Pol;
            p.DatumRodjenja = parametri.DatumRodjenja;

            ManipulacijaPodacima.UpisiSveAdministratore(admini, "~/App_Data/administratori.txt");
            HttpContext.Current.Application["korisnici"] = ManipulacijaPodacima.UcitajSveKorisnike();




            return Ok();

        }

        [HttpGet]
        [Route("vratiKorisnikeSistema")]
        public List<Korisnik> vratiKorisnikeSistema()
        {
           

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];

            List<Korisnik> bezUlogovanog = korisnici.FindAll(x => x.Username != trenutnoUlogovan.Username && !x.LogickiObrisan);
            List<Korisnik> trazeni = new List<Korisnik>();
            foreach( Korisnik k in bezUlogovanog)
            {
                if (!trazeni.Contains(k))
                    trazeni.Add(k);
            }

            return bezUlogovanog;


        }


        [HttpPost]
        [Route("pretraziKorisnikeSistema")]
        public List<Korisnik> pretraziKorisnikeSistema([FromBody] PretragaKorisnikaModel parametri)
        {

            List<Korisnik> korisnici = (List<Korisnik>)HttpContext.Current.Application["korisnici"];
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];


            if (!parametri.Username.Equals(""))
                korisnici = korisnici.FindAll(x => x.Username.ToLower().Contains(parametri.Username));

            if (!parametri.Ime.Equals(""))
                korisnici = korisnici.FindAll(x => x.Ime.ToLower().Contains(parametri.Ime));

            if (!parametri.Prezime.Equals(""))
                korisnici = korisnici.FindAll(x => x.Prezime.ToLower().Contains(parametri.Prezime));

            korisnici = korisnici.FindAll(x => x.Username != trenutnoUlogovan.Username);
            korisnici = korisnici.FindAll(x => !x.LogickiObrisan);

            return korisnici;

        }



        [HttpGet]
        [Route("vratiSumnjiveKorisnike")]
        public List<Kupac> vratiSumnjiveKorisnike()
        {
            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            List<Kupac> sumnjiviKupci = new List<Kupac>();

            int brojOtkazanih = 0;

            foreach ( Kupac k in kupci)
            {
                if(!k.LogickiObrisan)
                {
                    foreach(Karta karta in k.SveKarte)
                    {
                        if(!karta.LogickiObrisana && karta.StatusKarte == StatusKarteTip.ODUSTANAK)
                        {
                            if(karta.DatumOtkazivanja <= DateTime.Now && karta.DatumOtkazivanja >= DateTime.Now.AddMonths(-1) )
                                brojOtkazanih++;
                        }
                    }

                    if (brojOtkazanih >= 5)
                        sumnjiviKupci.Add(k);
                }

                brojOtkazanih = 0;
            }

            return sumnjiviKupci;



        }

        [HttpPost]
        [Route("blokirajKupca")]                       //koristim logovanjeModel nije potrebna validacija
        public IHttpActionResult blokirajKupca([FromBody] LogovanjeModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


            if (parametri.Username.Equals(""))
                return  Content(HttpStatusCode.Forbidden, "Nevalidna vrednost je prosledjena za username korisnika!");

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(parametri.Username));
            k.LogickiObrisan = true;

            ManipulacijaPodacima.UpisiSveKupce(kupci, "~/App_Data/kupci.txt");
            HttpContext.Current.Application["kupci"] = ManipulacijaPodacima.UcitajKupce("~/App_Data/kupci.txt");
            HttpContext.Current.Application["korisnici"] = ManipulacijaPodacima.UcitajSveKorisnike();


            return Ok();

        }

        [HttpPost]
        [Route("blokirajProdavca")]                       //koristim logovanjeModel nije potrebna validacija
        public IHttpActionResult blokirajProdavca([FromBody] LogovanjeModel parametri)
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            if (parametri.Username.Equals(""))
                return Content(HttpStatusCode.Forbidden, "Nevalidna vrednost je prosledjena za username korisnika!");

            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];
            Prodavac k = prodavci.Find(x => x.Username.Equals(parametri.Username));
            k.LogickiObrisan = true;

            ManipulacijaPodacima.UpisiSveProdavce(prodavci, "~/App_Data/prodavci.txt");
            HttpContext.Current.Application["prodavci"] = ManipulacijaPodacima.UcitajProdavce("~/App_Data/prodavci.txt");
            HttpContext.Current.Application["korisnici"] = ManipulacijaPodacima.UcitajSveKorisnike();


            return Ok();

        }



        [HttpGet]
        [Route("vratiNeodobreneManifestacije")]
        public List<Manifestacija> vratiNeodobreneManifestacije()
        {
            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            manifestacije = manifestacije.FindAll(x => x.Status == StatusManifestacijeTip.NEAKTIVNO);
            manifestacije = manifestacije.FindAll(x => x.DatumVremeManifestacije > DateTime.Now);
            manifestacije = manifestacije.FindAll(x => !x.LogickiObrisan);
            manifestacije.Sort((x, y) => x.DatumVremeManifestacije.CompareTo(y.DatumVremeManifestacije));
            return manifestacije;

        }


        [HttpPost]
        [Route("odobriManifestaciju")]
        public IHttpActionResult odobriManifestaciju([FromBody] DodavanjeManifestacijeModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


            if (parametri.Naziv == null)
            {
                return Content(HttpStatusCode.Forbidden, "Mora postojati naziv manifestacije!");
            }

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija trazena = manifestacije.Find(x => x.Naziv.Equals(parametri.Naziv) && !x.LogickiObrisan);
            trazena.Status = StatusManifestacijeTip.AKTIVNO;

            ManipulacijaPodacima.UpisiSveManifestacije(manifestacije, "~/App_Data/manifestacije.txt");
            HttpContext.Current.Application["manifestacije"] = ManipulacijaPodacima.UcitajManifestacije("~/App_Data/manifestacije.txt");


            return Ok();

        }

        
        [HttpGet]
        [Route("vratiSveKarte")]
        public List<Karta> vratiSveKarte()
        {
            List<Karta> sveKarte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
            sveKarte = sveKarte.FindAll(x => !x.LogickiObrisana);
          
            return sveKarte;

        }



        

        [HttpPost]
        [Route("logickiObrisiProdavca")]
        public IHttpActionResult logickiObrisiProdavca([FromBody] LogovanjeModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


            if (parametri.Username == null)
            {
                return Content(HttpStatusCode.Forbidden, "Mora postojati username prodavca!");
            }

            List<Prodavac> prodavci = (List<Prodavac>)HttpContext.Current.Application["prodavci"];
            Prodavac trazeni = prodavci.Find(x => x.Username.Equals(parametri.Username));
            trazeni.LogickiObrisan = true;

            ManipulacijaPodacima.UpisiSveProdavce(prodavci, "~/App_Data/prodavci.txt");



            return Ok();

        }


        [HttpPost]
        [Route("logickiObrisiKupca")]
        public IHttpActionResult logickiObrisiKupca([FromBody] LogovanjeModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            if (parametri.Username == null)
            {
                return Content(HttpStatusCode.Forbidden, "Mora postojati username kupca!");
            }

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac trazeni = kupci.Find(x => x.Username.Equals(parametri.Username));
            trazeni.LogickiObrisan = true;

            ManipulacijaPodacima.UpisiSveKupce(kupci, "~/App_Data/kupci.txt");

            trazeni.SveKarte.ForEach(x => x.LogickiObrisana = true);

            List<Karta> sveKarte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");

            for(int i = 0; i < sveKarte.Count; i++)
            {
                for(int j = 0; j < trazeni.SveKarte.Count; j++)
                {
                    if(sveKarte[i].Id.Equals(trazeni.SveKarte[j].Id))
                    {
                        sveKarte[i] = trazeni.SveKarte[j];
                    }
                }
            }
            

            ManipulacijaPodacima.UpisiSveKarte(sveKarte, "~/App_Data/karte.txt");


            List<Komentar> komentari = ManipulacijaPodacima.UcitajKomentare("~/App_Data/komentari.txt");

            foreach (Komentar k in komentari)
            {
                if (!k.Obrisan && k.UsernameKupacKometarisao.Equals(trazeni.Username)) 
                    k.Obrisan = true;
            }

            ManipulacijaPodacima.UpisiSveKomentare(komentari, "~/App_Data/komentari.txt");






            return Ok();

        }


        
        [HttpPost]
        [Route("logickiObrisiManifestaciju")]
        public IHttpActionResult logickiObrisiManifestaciju([FromBody] DodavanjeManifestacijeModel parametri)
        {


            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


            if (parametri.Naziv == null)
            {
                return Content(HttpStatusCode.Forbidden, "Mora postojati naziv nanifestacije!");
            }

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija trazeni = manifestacije.Find(x => x.Naziv.Equals(parametri.Naziv));
            trazeni.LogickiObrisan = true;

            ManipulacijaPodacima.UpisiSveManifestacije(manifestacije, "~/App_Data/manifestacije.txt");

            List<Karta> karte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
         

            foreach(Karta k in karte)
            {
                if (!k.LogickiObrisana && k.NazivManifestacije.Equals(trazeni.Naziv))
                    k.LogickiObrisana = true;
            }

            ManipulacijaPodacima.UpisiSveKarte(karte, "~/App_Data/karte.txt");



            List<Komentar> komentari = ManipulacijaPodacima.UcitajKomentare("~/App_Data/komentari.txt");
            
            foreach(Komentar k in komentari)
            {
                if (!k.Obrisan && k.ManifestacijaNaziv.Equals(trazeni.Naziv))
                    k.Obrisan = true;
            }

            ManipulacijaPodacima.UpisiSveKomentare(komentari, "~/App_Data/komentari.txt");






            return Ok();

        }


        [HttpPost]
        [Route("logickiObrisiKartu")]
        public IHttpActionResult logickiObrisiKartu([FromBody] LogovanjeModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.ADMINISTRATOR)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");


            if (parametri.Username == null)
            {
                return Content(HttpStatusCode.Forbidden, "Mora postojati id karte!");
            }

            List<Karta> karte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");
            Karta trazeni = karte.Find(x => x.Id.Equals(parametri.Username));
            trazeni.LogickiObrisana = true;

            ManipulacijaPodacima.UpisiSveKarte(karte, "~/App_Data/karte.txt");



            return Ok();

        }

    }

}
