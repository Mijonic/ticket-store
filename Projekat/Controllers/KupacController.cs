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
    [RoutePrefix("kupacPrikaz")]
    public class KupacController : ApiController
    {


        [HttpGet]
        [Route("vratiManifestacijeRezervacija")]
        public List<Manifestacija> vratiManifestacijeRezervacija()
        {

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];            
            List<Manifestacija> trazeneManifestacije = manifestacije.FindAll(x => x.Status == StatusManifestacijeTip.AKTIVNO && x.PreostaloKarata > 0 && x.DatumVremeManifestacije > DateTime.Now);
            trazeneManifestacije = trazeneManifestacije.FindAll(x => !x.LogickiObrisan);



            return trazeneManifestacije;

        }

        [HttpPost]
        [Route("rezervisiKarte")]
        public IHttpActionResult rezervisiKarte([FromBody] RezervacijaKarata parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan.Uloga != UlogaTip.KUPAC)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];

            Manifestacija m = manifestacije.Find(x => x.Naziv.Equals(parametri.NazivManifestacije) && !x.LogickiObrisan);

            if(m == null)
                return Content(HttpStatusCode.Forbidden, "Nije pronadjena manifestacija za koju se rezervisu karte!");

            trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            if (trenutnoUlogovan == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Ni jedan korisnik nije ulogovan!");
            }

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];

            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            if(k == null)
                return Content(HttpStatusCode.Forbidden, "Nije pronadjen kupac!");


            for (int i = 0; i < parametri.BrojRegular; i++)
            {
                double cena = IzracunajCenuKarte(parametri.Cena, k.TipKorisnika.ImeTipa);
                Karta karta = new Karta(Guid.NewGuid().ToString("N"), cena, StatusKarteTip.REZERVISANA, KartaTip.REGULAR, parametri.NazivManifestacije,m.DatumVremeManifestacije, trenutnoUlogovan.Username,trenutnoUlogovan.Ime,trenutnoUlogovan.Prezime,DateTime.MinValue,false);
                ManipulacijaPodacima.UpisiKartu(karta, "~/App_Data/karte.txt");
                k.SveKarte.Add(karta);
            }

            for (int i = 0; i < parametri.BrojFanPit; i++)
            {
                double cena = IzracunajCenuKarte(parametri.Cena * 2, k.TipKorisnika.ImeTipa);
                Karta karta = new Karta(Guid.NewGuid().ToString("N"), cena, StatusKarteTip.REZERVISANA, KartaTip.FAN_PIT, parametri.NazivManifestacije, m.DatumVremeManifestacije, trenutnoUlogovan.Username, trenutnoUlogovan.Ime, trenutnoUlogovan.Prezime, DateTime.MinValue, false);
                ManipulacijaPodacima.UpisiKartu(karta, "~/App_Data/karte.txt");
                k.SveKarte.Add(karta);
            }

            for (int i = 0; i < parametri.BrojVip; i++)
            {
                double cena = IzracunajCenuKarte(parametri.Cena * 4, k.TipKorisnika.ImeTipa);
                Karta karta = new Karta(Guid.NewGuid().ToString("N"), cena, StatusKarteTip.REZERVISANA, KartaTip.VIP, parametri.NazivManifestacije, m.DatumVremeManifestacije, trenutnoUlogovan.Username, trenutnoUlogovan.Ime, trenutnoUlogovan.Prezime, DateTime.MinValue, false);
                ManipulacijaPodacima.UpisiKartu(karta, "~/App_Data/karte.txt");
                k.SveKarte.Add(karta);
            }

            k.BrojSakupljenihBodova += parametri.OsvojeniBodovi;

            if (k.BrojSakupljenihBodova >= k.TipKorisnika.BrojBodova)
            {
                k.TipKorisnika.ImeTipa = ImeTipaKorisnika.SREBRNI;
                k.TipKorisnika.Popust = 3;
            }

            if (k.BrojSakupljenihBodova >= (k.TipKorisnika.BrojBodova *2))
            {
                k.TipKorisnika.ImeTipa = ImeTipaKorisnika.ZLATNI;
                k.TipKorisnika.Popust = 5;
            }

            m.PreostaloKarata -= parametri.BrojFanPit + parametri.BrojRegular + parametri.BrojVip;

            ManipulacijaPodacima.UpisiSveManifestacije(manifestacije, "~/App_Data/manifestacije.txt");
            ManipulacijaPodacima.UpisiSveKupce(kupci,"~/App_Data/kupci.txt");








            return Ok("Uspesno rezervisanje!");

        }

        [HttpGet]
        [Route("vratiKarteKupca")]
        public List<Karta> vratiKarteKupca()
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];    
            
            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            if (k == null)
                return new List<Karta>();



            return k.SveKarte.FindAll(x => !x.LogickiObrisana);

        }

        [HttpGet]
        [Route("vratiUlogovanogKupca")]
        public IHttpActionResult vratiUlogovanogKupca()
        {
            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];

           
            if (trenutnoUlogovan == null)
            {
                return Content(HttpStatusCode.Unauthorized, "Ni jedan korisnik nije ulogovan!");
            }

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];

            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            if(k == null)
                return Content(HttpStatusCode.Unauthorized, "Nije pronadjen kupac!");


            return Ok(k);
        }


        [HttpPost]
        [Route("pretraziKarteKupca")]
        public List<Karta> pretraziKarteKupca([FromBody] PretragaKarataModel parametri)
        {

            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            if (k == null)
                return new List<Karta>();


            List<Karta> pretrazeneKarte = k.SveKarte.FindAll(x => !x.LogickiObrisana);

            if (!parametri.Naziv.Equals(""))
                pretrazeneKarte = pretrazeneKarte.FindAll(x => x.NazivManifestacije.ToLower().Contains(parametri.Naziv.ToLower()));

            if (parametri.Datum_od != DateTime.MinValue && parametri.Datum_do != DateTime.MinValue)
                pretrazeneKarte = pretrazeneKarte.FindAll(x => x.DatumVremeManifestacije >= parametri.Datum_od && x.DatumVremeManifestacije <= parametri.Datum_do);

            pretrazeneKarte = pretrazeneKarte.FindAll(x => (x.Cena >= parametri.Cena_od) && (x.Cena <= parametri.Cena_do));
            pretrazeneKarte = pretrazeneKarte.FindAll(x => !x.LogickiObrisana);

            return pretrazeneKarte;

        }

        

        [HttpPost]
        [Route("otkaziRezervacijuKarte")]
        public List<Karta> otkaziRezervacijuKarte([FromBody] OtkazivanjeRezervacijeModel parametri)
        {

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(parametri.UsernameKupca) && !x.LogickiObrisan);
            k.BrojSakupljenihBodova -= parametri.BrojIzgubljenihBodova;

            List<Manifestacija> manifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija manifestacijaKarta = manifestacije.Find(x => x.Naziv.Equals(parametri.NazivManifestacije) && !x.LogickiObrisan);
            manifestacijaKarta.PreostaloKarata++;

            Karta kartaKupca = k.SveKarte.Find(x => x.Id.Equals(parametri.Id) && !x.LogickiObrisana);
            kartaKupca.StatusKarte = StatusKarteTip.ODUSTANAK;
            kartaKupca.DatumOtkazivanja = DateTime.Now;
          


            ManipulacijaPodacima.UpisiSveManifestacije(manifestacije, "~/App_Data/manifestacije.txt");
            ManipulacijaPodacima.UpisiSveKupce(kupci,"~/App_Data/kupci.txt");

            List<Karta> sveKarte = ManipulacijaPodacima.UcitajKarte("~/App_Data/karte.txt");

            for(int i =0; i < sveKarte.Count; i++)
            {
                if(sveKarte[i].Id.Equals(kartaKupca.Id))
                {
                    sveKarte[i] = kartaKupca;
                }
            }

            ManipulacijaPodacima.UpisiSveKarte(sveKarte, "~/App_Data/karte.txt");

            return k.SveKarte;


        }

        [HttpGet]
        [Route("vratiPoseceneManifestacijeKupca")]
        public List<Manifestacija> vratiPoseceneManifestacijeKupca()
        {



            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];
            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username));

            List<Manifestacija> sveManifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            List<Manifestacija> posecene = new List<Manifestacija>();

            foreach(Karta karta in k.SveKarte)
            {
                foreach(Manifestacija m in sveManifestacije)
                {
                    if (karta.NazivManifestacije.Equals(m.Naziv))
                    {
                        if(m.DatumVremeManifestacije < DateTime.Now)
                        {
                            if (!posecene.Contains(m) && !m.LogickiObrisan)
                                posecene.Add(m);
                        }
                        
                    }
                }
            }


            return posecene;

        }

        

        [HttpPost]
        [Route("postavljanjeKomentara")]
        public IHttpActionResult postavljanjeKomentara([FromBody] KomentarisanjeModel parametri)
        {


            string poruka = "";
            bool validacija = true;
            
            if(parametri.NazivManifestacije.Equals(""))
            {
                poruka += "Naziv manifestacije mora postojati!";
                validacija = false;

            }

            if(parametri.OcenaKorisnika < 1 || parametri.OcenaKorisnika > 5)
            {
                poruka += "Ocena mora biti u intervalu od 1 do 5!";
                validacija = false;
            }

            if(parametri.TextKomentara.Equals(""))
            {
                poruka += "Morate uneti tekst komentara!";
                validacija = false;
            }

            if(!validacija)
            {
                return Content(HttpStatusCode.Forbidden, poruka);
            }


            Korisnik trenutnoUlogovan = (Korisnik)HttpContext.Current.Session["korisnik"];

            if (trenutnoUlogovan.Uloga != UlogaTip.KUPAC)
                return Content(HttpStatusCode.Unauthorized, "Nemate prava pristupa ovoj funkcionalnosti!");

            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];
            Kupac k = kupci.Find(x => x.Username.Equals(trenutnoUlogovan.Username) && !x.LogickiObrisan);

            

            Komentar kom = new Komentar(Guid.NewGuid().ToString("N"),trenutnoUlogovan.Username, parametri.NazivManifestacije, parametri.TextKomentara,parametri.OcenaKorisnika, false, false, false);
            ManipulacijaPodacima.UpisiKomentar(kom, "~/App_Data/komentari.txt");

            List<Manifestacija> sveManifestacije = (List<Manifestacija>)HttpContext.Current.Application["manifestacije"];
            Manifestacija m = sveManifestacije.Find(x => x.Naziv.Equals(parametri.NazivManifestacije) && !x.LogickiObrisan);

            List<Komentar> komentari = (List<Komentar>)HttpContext.Current.Application["komentari"];
            komentari.Add(kom);

            int zbirOcena = 0;
            komentari.ForEach(x => zbirOcena += x.Ocena);
            double noviProsek = (double)zbirOcena / komentari.Count;

            m.ProsecnaOcena = Math.Round(noviProsek,1);

            ManipulacijaPodacima.UpisiSveManifestacije(sveManifestacije, "~/App_Data/manifestacije.txt");
            



            return Ok();


        }

        [Route("izmeniKupca")]
        public IHttpActionResult izmeniKupca([FromBody] RegistracijaKorisnikaModel parametri)
        {

            if (parametri.DatumRodjenja == null || parametri.Ime == null || parametri.Password == null || parametri.Prezime == null || parametri.Username == null)
                return Content(HttpStatusCode.Forbidden, "Nevalidni podaci");



            List<Kupac> kupci = (List<Kupac>)HttpContext.Current.Application["kupci"];

            Kupac p = kupci.Find(x => !x.LogickiObrisan && x.Username.Equals(parametri.Username));
            p.Password = parametri.Password;
            p.Ime = parametri.Ime;
            p.Prezime = parametri.Prezime;
            p.Pol = parametri.Pol;
            p.DatumRodjenja = parametri.DatumRodjenja;

            ManipulacijaPodacima.UpisiSveKupce(kupci, "~/App_Data/kupci.txt");
            HttpContext.Current.Application["korisnici"] = ManipulacijaPodacima.UcitajSveKorisnike();

           


            return Ok();

        }


        public double IzracunajCenuKarte(double pocetnaCena, ImeTipaKorisnika imeTipa)
        {


            if (imeTipa == ImeTipaKorisnika.BRONZANI)
            {
                return pocetnaCena;
            }
            else if (imeTipa == ImeTipaKorisnika.SREBRNI)
            {
                return pocetnaCena - (pocetnaCena * 0.03);
            }
            else
            {
                return pocetnaCena - (pocetnaCena * 0.05);
            }
        }

        
    }
}
