using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Korisnik
    {
        private string username; // jedinstveno
        private string password;
        private string ime;
        private string prezime;
        private PolTip pol;
        private DateTime datumRodjenja;
        private UlogaTip uloga;
        private bool logickiObrisan;



        public Korisnik()
        {

            logickiObrisan = false;
        }


        public Korisnik(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            LogickiObrisan = false;
           
        }


        public Korisnik(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja, UlogaTip uloga)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            LogickiObrisan = false;
        }

        public Korisnik(string username, string password, string ime, string prezime, PolTip pol, DateTime datumRodjenja, UlogaTip uloga, bool logickiObrisan)
        {
            Username = username;
            Password = password;
            Ime = ime;
            Prezime = prezime;
            Pol = pol;
            DatumRodjenja = datumRodjenja;
            Uloga = uloga;
            LogickiObrisan = logickiObrisan;
        }

        public override string ToString()
        {
            return $"{Username}|{Password}|{Ime}|{Prezime}|{Pol}|{DatumRodjenja.ToString("dd-MM-yyyy hh:mm:ss")}|{Uloga}|{LogickiObrisan}";
        }

        public string Username { get => username; set => username = value; }
        public string Password { get => password; set => password = value; }
        public string Ime { get => ime; set => ime = value; }
        public string Prezime { get => prezime; set => prezime = value; }
        public PolTip Pol { get => pol; set => pol = value; }
        public DateTime DatumRodjenja { get => datumRodjenja; set => datumRodjenja = value; }
        public UlogaTip Uloga { get => uloga; set => uloga = value; }
        public bool LogickiObrisan { get => logickiObrisan; set => logickiObrisan = value; }
    }
}