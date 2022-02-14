using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5B
{
    public class Doctor
    {
        public int DoctorID { get; private set; }
        public string Actor { get; private set; }
        public int Series { get; private set; }
        public int Age { get; private set; }
        public string Debut { get; private set; }
        public Image Picture { get; private set; }

        public Doctor(int doctoreId, string actor, int series, int age, string debut, Image picture)
        {
            DoctorID = doctoreId;
            Actor = actor;
            Series = series;
            Age = age;
            Debut = debut;
            Picture = picture;
        }
        public override string ToString()
        {
            return $" {DoctorID} {Debut} {Actor}";
        }
    }
}
