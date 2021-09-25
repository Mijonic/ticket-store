using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Projekat.Models
{
    public class TipKorisnika
    {
        private ImeTipaKorisnika imeTipa; // zlatni, srebrni, bronzani
        private double popust; //
        private int brojBodova;

        public TipKorisnika()
        {

        }

        public TipKorisnika(ImeTipaKorisnika imeTipa, double popust, int brojBodova)
        {
            ImeTipa = imeTipa;
            Popust = popust;
            BrojBodova = brojBodova;
        }

        public override string ToString()
        {
            return $"{ImeTipa}|{Popust}|{BrojBodova}";
        }

        public ImeTipaKorisnika ImeTipa { get => imeTipa; set => imeTipa = value; }
        public double Popust { get => popust; set => popust = value; }
        public int BrojBodova { get => brojBodova; set => brojBodova = value; }
    }
}