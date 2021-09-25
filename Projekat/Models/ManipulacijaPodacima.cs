using Projekat.Models.PomocniModeli;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;

namespace Projekat.Models
{
    public class ManipulacijaPodacima
    {

        
        #region  Manifestacija
        public static List<Manifestacija> UcitajManifestacije(string putanja)
        {
            List<Manifestacija> manifestacije = new List<Manifestacija>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 15)
                {   
                    int index = podaci[10].LastIndexOf("\\");
                    string poster = "C:\\fakepath" + podaci[10].Substring(index);
                    MestoOdrzavanja mesto = new MestoOdrzavanja(new Adresa(podaci[6], int.Parse(podaci[7]), podaci[8], int.Parse(podaci[9])));
                    Manifestacija m = new Manifestacija(podaci[0], (ManifestacijaTip)Enum.Parse(typeof(ManifestacijaTip), podaci[1]), int.Parse(podaci[2]), DateTime.ParseExact(podaci[3], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), Double.Parse(podaci[4]), (StatusManifestacijeTip)Enum.Parse(typeof(StatusManifestacijeTip), podaci[5]), mesto, poster,Double.Parse(podaci[11]), int.Parse(podaci[12]), podaci[13], Boolean.Parse(podaci[14]));
                    manifestacije.Add(m);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return manifestacije;

        }

        public static void UpisiManifestaciju(Manifestacija m, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(m);
            }
        }


        public static void UpisiSveManifestacije(List<Manifestacija> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Manifestacija m in lista)
                    sw.WriteLine(m);
            }


        }

        #endregion

        #region Koordinate

        public static List<Koordinate> UcitajKoordinate(string putanja)
        {
            List<Koordinate> koordinate = new List<Koordinate>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 3)
                {
                    Koordinate k = new Koordinate(Double.Parse(podaci[0]), Double.Parse(podaci[1]), podaci[2]);
                    koordinate.Add(k);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return koordinate;

        }

        public static void UpisiKoordinate(Koordinate k, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(k);
            }
        }

        public static void UpisiSveKoordinate(List<Koordinate> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Koordinate k in lista)
                    sw.WriteLine(k);
            }


        }

        #endregion

        #region Admin

        public static List<Administrator> UcitajAdministratore(string putanja)
        {
            List<Administrator> administratori = new List<Administrator>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 8)
                {
                    //MestoOdrzavanja mesto = new MestoOdrzavanja(new Adresa(podaci[6], int.Parse(podaci[7]), podaci[8], int.Parse(podaci[9])));
                    // Manifestacija m = new Manifestacija(podaci[0], (ManifestacijaTip)Enum.Parse(typeof(ManifestacijaTip), podaci[1]), int.Parse(podaci[2]), DateTime.ParseExact(podaci[3], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), Double.Parse(podaci[4]), (StatusManifestacijeTip)Enum.Parse(typeof(StatusManifestacijeTip), podaci[5]), mesto, podaci[10], Double.Parse(podaci[11]), Boolean.Parse(podaci[12]));

                    // manifestacije.Add(m);

                    /*
                    Korisnik k = null;

                    UlogaTip tip = (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[6]);
                    if( tip == UlogaTip.ADMINISTRATOR)
                    {
                         k = new Administrator(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture));
                    }else if( tip == UlogaTip.KUPAC )
                    {
                        k = new Kupac(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture));
                    }else if(tip == UlogaTip.PRODAVAC)
                    {
                        k = new Prodavac(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture));
                    }
                    */

                    Administrator admin = new Administrator(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[6]), Boolean.Parse(podaci[7]));
                    administratori.Add(admin);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return administratori;

        }

        public static void UpisiAdministratora(Administrator admin, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(admin);
            }
        }

        public static void UpisiSveAdministratore(List<Administrator> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Administrator k in lista)
                    sw.WriteLine(k);
            }


        }



        #endregion

        #region Prodavac

        public static List<Prodavac> UcitajProdavce(string putanja)
        {
            List<Prodavac> prodavci = new List<Prodavac>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 8)
                {
                    

                    Prodavac prodavac = new Prodavac(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[6]), Boolean.Parse(podaci[7]));
                    List<Manifestacija> sve = UcitajManifestacije("~/App_Data/manifestacije.txt");
                    List<Manifestacija> manifestacijeProdavca = sve.FindAll(x => x.UsernameProdavca.Equals(prodavac.Username));
                    prodavac.Manifestacije = manifestacijeProdavca;
                    prodavci.Add(prodavac);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return prodavci;

        }


        public static void UpisiProdavca(Prodavac prodavac, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(prodavac);
            }
        }

        public static void UpisiSveProdavce(List<Prodavac> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Prodavac k in lista)
                    sw.WriteLine(k);
            }


        }
        #endregion

        #region Kupac

        public static void UpisiKupca(Kupac kupac, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(kupac);
            }
        }

        public static List<Kupac> UcitajKupce(string putanja)
        {
            List<Kupac> kupci = new List<Kupac>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 12)
                {

                    //Zoka|Zokic|jeje|sada|MUSKI|20-08-2020 05:12:41|KUPAC|False|0|BRONZANI|0|100
                    TipKorisnika tip = new TipKorisnika((ImeTipaKorisnika)Enum.Parse(typeof(ImeTipaKorisnika), podaci[9]), Double.Parse(podaci[10]), int.Parse(podaci[11]));
                    Kupac kupac = new Kupac(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[6]), Boolean.Parse(podaci[7]),double.Parse(podaci[8]),tip);
                    List<Karta> sveKarte = UcitajKarte("~/App_Data/karte.txt");
                    List<Karta> karteKupca = sveKarte.FindAll(x => x.UsernameKupca.Equals(kupac.Username));
                    kupac.SveKarte = karteKupca;
                    kupci.Add(kupac);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();


            

            return kupci;

        }

        public static void UpisiSveKupce(List<Kupac> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Kupac k in lista)
                    sw.WriteLine(k);
            }


        }

        #endregion

        #region Karte

        public static void UpisiKartu(Karta karta, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(karta);
            }
        }

        public static List<Karta> UcitajKarte(string putanja)
        {
            List<Karta> kupci = new List<Karta>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 11)
                {

                    

                    Karta k = new Karta(podaci[0], Double.Parse(podaci[1]), (StatusKarteTip)Enum.Parse(typeof(StatusKarteTip), podaci[2]), (KartaTip)Enum.Parse(typeof(KartaTip), podaci[3]), podaci[4], DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), podaci[6], podaci[7], podaci[8], DateTime.ParseExact(podaci[9], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), Boolean.Parse(podaci[10]));
                    kupci.Add(k);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return kupci;

        }

        public static void UpisiSveKarte(List<Karta> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Karta k in lista)
                    sw.WriteLine(k);
            }


        }

        public static void UpisiSveNoveKarte(List<Karta> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                foreach (Karta k in lista)
                    sw.WriteLine(k);
            }


        }

        #endregion

        #region Komentari

        public static List<Komentar> UcitajKomentare(string putanja)
        {
            List<Komentar> komentari = new List<Komentar>();

            putanja = HostingEnvironment.MapPath(putanja);

            FileStream stream = new FileStream(putanja, FileMode.Open);
            StreamReader sr = new StreamReader(stream);
            string line = "";

            while ((line = sr.ReadLine()) != null)
            {
                string[] podaci = line.Split('|');

                if (podaci.Length == 8 )
                {


                    //Komentar komentar = new Komentar(podaci[0], podaci[1], podaci[2], podaci[3], (PolTip)Enum.Parse(typeof(PolTip), podaci[4]), DateTime.ParseExact(podaci[5], "dd-MM-yyyy hh:mm:ss", CultureInfo.CurrentCulture), (UlogaTip)Enum.Parse(typeof(UlogaTip), podaci[6]), Boolean.Parse(podaci[7]));
                    Komentar kom = new Komentar(podaci[0],podaci[1], podaci[2], podaci[3], int.Parse(podaci[4]), Boolean.Parse(podaci[5]), Boolean.Parse(podaci[6]), Boolean.Parse(podaci[7]));
                    komentari.Add(kom);
                }
                else
                {
                    continue;
                }



            }

            sr.Close();
            stream.Close();

            return komentari;

        }


        public static void UpisiKomentar(Komentar komentar, string putanja)
        {
            putanja = HostingEnvironment.MapPath(putanja);

            using (StreamWriter sw = File.AppendText(putanja))
            {
                sw.WriteLine(komentar);
            }
        }

        public static void UpisiSveKomentare(List<Komentar> lista, string putanja)
        {

            putanja = HostingEnvironment.MapPath(putanja);
            File.WriteAllText(putanja, String.Empty);

            using (StreamWriter sw = File.CreateText(putanja))
            {
                foreach (Komentar k in lista)
                    sw.WriteLine(k);
            }


        }

        #endregion

        public static List<Korisnik> UcitajSveKorisnike()
        {
            List<Administrator> admini = ManipulacijaPodacima.UcitajAdministratore("~/App_Data/administratori.txt");
            List<Prodavac> prodavci = ManipulacijaPodacima.UcitajProdavce("~/App_Data/prodavci.txt");
            List<Kupac> kupci = ManipulacijaPodacima.UcitajKupce("~/App_Data/kupci.txt");


            List<Korisnik> korisnici = new List<Korisnik>();

            admini.ForEach(x => korisnici.Add(x));
            prodavci.ForEach(x => korisnici.Add(x));
            kupci.ForEach(x => korisnici.Add(x));

            return korisnici;
        }
    }
}