using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5B
{
    public class Companion
    {
        public string Name { get; set; }
        public string Actor { get; set; }
        public int DoctorID { get; set; }
        public string StoryID { get; set; }

        public Companion(string name, string actor, int doctorId, string storyId)
        {
            Name = name;
            Actor = actor;
            DoctorID = doctorId;
            StoryID = storyId;
        }
    }
}
