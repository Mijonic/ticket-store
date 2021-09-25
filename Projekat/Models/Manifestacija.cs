using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Manifestacija
    {
        private string naziv;
        private ManifestacijaTip tipManifestacije;
        private int brojMesta;
        private DateTime datumVremeManifestacije;
        private double cena; // regularna bez popusta       
        private StatusManifestacijeTip status; // aktivna ili neaktivna
        private MestoOdrzavanja mestoOdrzavanja;
        private string posterManifestacije; // putanja ili Image
        private double prosecnaOcena;
        private bool logickiObrisan;
        private int preostaloKarata;

        private string usernameProdavca; // on je dodao manifestaciju


        public Manifestacija()
        {

            LogickiObrisan = false;
        }

        public Manifestacija(string naziv, ManifestacijaTip tipManifestacije, int brojMesta, DateTime datumVremeManifestacije, double cena, StatusManifestacijeTip status, MestoOdrzavanja mestoOdrzavanja, string posterManifestacije, string usernameProdavca)
        {
            Naziv = naziv;
            TipManifestacije = tipManifestacije;
            BrojMesta = brojMesta;
            DatumVremeManifestacije = datumVremeManifestacije;
            Cena = cena;
            Status = status;
            MestoOdrzavanja = mestoOdrzavanja;
            PosterManifestacije = posterManifestacije;
            ProsecnaOcena = -1;
            LogickiObrisan = false;
            PreostaloKarata = BrojMesta;
            UsernameProdavca = usernameProdavca;
           
            
        }

                      
        public Manifestacija(string naziv, ManifestacijaTip tipManifestacije, int brojMesta, DateTime datumVremeManifestacije, double cena, StatusManifestacijeTip status, MestoOdrzavanja mestoOdrzavanja, string posterManifestacije,double prosecnaOcena, int preostaloKarata, string usernameProdavca, bool logickiObrisan)
        {
            Naziv = naziv;
            TipManifestacije = tipManifestacije;
            BrojMesta = brojMesta;
            DatumVremeManifestacije = datumVremeManifestacije;
            Cena = cena;
            Status = status;
            MestoOdrzavanja = mestoOdrzavanja;
            PosterManifestacije = posterManifestacije;
            ProsecnaOcena = prosecnaOcena;
            LogickiObrisan = logickiObrisan;
            PreostaloKarata = preostaloKarata;
            UsernameProdavca = usernameProdavca;
            

        }



        public override string ToString()
        {
            string rez = $"{Naziv}|{TipManifestacije}|{BrojMesta}|{DatumVremeManifestacije.ToString("dd-MM-yyyy hh:mm:ss")}|{Cena}|{Status}|{MestoOdrzavanja}|{PosterManifestacije}|{ProsecnaOcena}|{PreostaloKarata}|{UsernameProdavca}|{LogickiObrisan}";
            return rez;
        }

        public string Naziv { get => naziv; set => naziv = value; }
        public ManifestacijaTip TipManifestacije { get => tipManifestacije; set => tipManifestacije = value; }
        public int BrojMesta { get => brojMesta; set => brojMesta = value; }
        public DateTime DatumVremeManifestacije { get => datumVremeManifestacije; set => datumVremeManifestacije = value; }
        public double Cena { get => cena; set => cena = value; }
        public StatusManifestacijeTip Status { get => status; set => status = value; }
        public MestoOdrzavanja MestoOdrzavanja { get => mestoOdrzavanja; set => mestoOdrzavanja = value; }
        public string PosterManifestacije { get => posterManifestacije; set => posterManifestacije = value; }
        public double ProsecnaOcena { get => prosecnaOcena; set => prosecnaOcena = value; }
        public bool LogickiObrisan { get => logickiObrisan; set => logickiObrisan = value; }
        public int PreostaloKarata { get => preostaloKarata; set => preostaloKarata = value; }
        public string UsernameProdavca { get => usernameProdavca; set => usernameProdavca = value; }
        
    }
}