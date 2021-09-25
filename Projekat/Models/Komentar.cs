using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class Komentar
    {
        private string idKomentara;
        private string usernameKupacKometarisao;
        private string manifestacijaNaziv; // na nju se komentar odnosi
        private string tekstKomentara;
        private int ocena;  // od 1 do 5
        private bool odobren;
        private bool odbijen;
        private bool obrisan;

        public Komentar()
        {

        }

        public Komentar(string idKomentara, string usernameKupacKometarisao, string manifestacijaNaziv, string tekstKomentara, int ocena, bool odobren, bool odbijen)
        {
            IdKomentara = idKomentara;
            UsernameKupacKometarisao = usernameKupacKometarisao;
            ManifestacijaNaziv = manifestacijaNaziv;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Odobren = odobren;
            Odbijen = odbijen;
        }

        public Komentar(string idKomentara, string usernameKupacKometarisao, string manifestacijaNaziv, string tekstKomentara, int ocena, bool odobren, bool odbijen, bool obrisan)
        {
            IdKomentara = idKomentara;
            UsernameKupacKometarisao = usernameKupacKometarisao;
            ManifestacijaNaziv = manifestacijaNaziv;
            TekstKomentara = tekstKomentara;
            Ocena = ocena;
            Odobren = odobren;
            Odbijen = odbijen;
            Obrisan = obrisan;
        }

        public override string ToString()
        {
            return $"{IdKomentara}|{UsernameKupacKometarisao}|{ManifestacijaNaziv}|{TekstKomentara}|{Ocena}|{Odobren}|{Odbijen}|{Obrisan}";
        }

        public string UsernameKupacKometarisao { get => usernameKupacKometarisao; set => usernameKupacKometarisao = value; }
        public string ManifestacijaNaziv { get => manifestacijaNaziv; set => manifestacijaNaziv = value; }
        public string TekstKomentara { get => tekstKomentara; set => tekstKomentara = value; }
        public int Ocena { get => ocena; set => ocena = value; }
        public bool Odobren { get => odobren; set => odobren = value; }
        public bool Odbijen { get => odbijen; set => odbijen = value; }
        public bool Obrisan { get => obrisan; set => obrisan = value; }
        public string IdKomentara { get => idKomentara; set => idKomentara = value; }
    }
}