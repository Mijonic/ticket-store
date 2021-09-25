using Projekat.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace Projekat
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Application_PostAuthorizeRequest(); //mora ovo da bi se omogucio rad sa sesijama



            /*
             List<Manifestacija> pom = new List<Manifestacija>();
            pom.Add(new Manifestacija("A1", ManifestacijaTip.FESTIVAL, 5000, DateTime.Now, 5000, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "ANovi Sad", 21000)), "bice slika"));
            pom.Add(new Manifestacija("EXIT", ManifestacijaTip.FESTIVAL, 0, new DateTime(2020,10,2,12,30,0), 50, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Bogdana Suputa",115,"Novi Sad",21000)), "nekifestival.jpg"));
            pom.Add(new Manifestacija("Guca", ManifestacijaTip.FESTIVAL, 2600, new DateTime(2019, 8, 5, 15, 30, 0), 100, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Marka Markovica", 115, "Beograd", 21000)), "Summer-festival.jpg"));
            pom.Add(new Manifestacija("NISVILE", ManifestacijaTip.FESTIVAL, 2800, DateTime.Now, 20, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "NIS", 21000)), "bice slika"));
            pom.Add(new Manifestacija("SUMER", ManifestacijaTip.FESTIVAL, 3000, DateTime.Now, 5, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "Vrbas", 21000)), "bice slika"));
            pom.Add(new Manifestacija("koncert", ManifestacijaTip.KONCERT, 3200, DateTime.Now, 3, StatusManifestacijeTip.AKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "Novi Sad", 21000)), "bice slika"));
            pom.Add(new Manifestacija("pozoriste", ManifestacijaTip.POZORISTE, 4000, DateTime.Now, 25, StatusManifestacijeTip.NEAKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "Novi Sad", 21000)), "bice slika"));
            pom.Add(new Manifestacija("Z6", ManifestacijaTip.FESTIVAL, 0, DateTime.Now, 5000, StatusManifestacijeTip.NEAKTIVNO, new MestoOdrzavanja(new Adresa("Neka najduza ulicaaaaaa", 115, "ZNovi Sad", 21000)), "bice slika"));

            ManipulacijaPodacima.UpisiSveManifestacije(pom, "~/App_Data/manifestacije.txt");
                   */


            //List<Korisnik> korisnici = ManipulacijaPodacima.UcitajKorisnike("~/App_Data/korisnici.txt");
            //HttpContext.Current.Application["korisnici"] = korisnici;

            List<Administrator> admini = ManipulacijaPodacima.UcitajAdministratore("~/App_Data/administratori.txt");
            List<Prodavac> prodavci = ManipulacijaPodacima.UcitajProdavce("~/App_Data/prodavci.txt");
            List<Kupac> kupci = ManipulacijaPodacima.UcitajKupce("~/App_Data/kupci.txt");


            List<Korisnik> korisnici = new List<Korisnik>();

            admini.ForEach(x => korisnici.Add(x));
            prodavci.ForEach(x => korisnici.Add(x));
            kupci.ForEach(x => korisnici.Add(x));


            HttpContext.Current.Application["admini"] = admini;
            HttpContext.Current.Application["prodavci"] = prodavci;
            HttpContext.Current.Application["kupci"] = kupci;
            HttpContext.Current.Application["korisnici"] = korisnici;

            

            List<Manifestacija> manifestacije = ManipulacijaPodacima.UcitajManifestacije("~/App_Data/manifestacije.txt");
            HttpContext.Current.Application["manifestacije"] = manifestacije;

            List<Komentar> komentari = ManipulacijaPodacima.UcitajKomentare("~/App_Data/komentari.txt");
            HttpContext.Current.Application["komentari"] = komentari;

        }


        protected void Application_PostAuthorizeRequest()
        {
            System.Web.HttpContext.Current.SetSessionStateBehavior(System.Web.SessionState.SessionStateBehavior.Required);
        }
    }
}
