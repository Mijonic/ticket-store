using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class MestoOdrzavanja
    {
        private Adresa adresa;

        public MestoOdrzavanja()
        {
                
        }

        public MestoOdrzavanja(string nazivUlice, int broj, string mesto, int postanskiBroj)
        {
             this.adresa = new Adresa(nazivUlice, broj, mesto, postanskiBroj);
        }

        public MestoOdrzavanja(Adresa adresa)
        {
            Adresa = adresa;
        }
        
        public Adresa Adresa { get => adresa; set => adresa = value; }

        public override string ToString()
        {
            return $"{ Adresa.Ulica}|{Adresa.Broj}|{Adresa.Mesto}|{Adresa.PostanskiBroj}";
        }
    }
}