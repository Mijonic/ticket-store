using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Adresa
    {
        private string ulica;
        private int broj;
        private string mesto;
        private int postanskiBroj;

        public Adresa()
        {

        }

        public Adresa(string ulica, int broj, string mesto, int postanskiBroj)
        {
            Ulica = ulica;
            Broj = broj;
            Mesto = mesto;
            PostanskiBroj = postanskiBroj;
        }

        public override string ToString()
        {
            
            return $"{Ulica} {Broj}, {Mesto} {PostanskiBroj}";
        }

        public string Ulica { get => ulica; set => ulica = value; }
        public int Broj { get => broj; set => broj = value; }
        public string Mesto { get => mesto; set => mesto = value; }
        public int PostanskiBroj { get => postanskiBroj; set => postanskiBroj = value; }
    }
}