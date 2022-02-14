using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab5B
{
    public class Episode
    {
        public string StoryId { get; set; }
        public string Season { get; set; }
        public int SeasonYear { get; set; }
        public string Title { get; set; }

        public Episode(string storyId, string season, int seasonYear, string title)
        {
            StoryId = storyId;
            Season = season;
            SeasonYear = seasonYear;
            Title = title;
        }
    }
}
