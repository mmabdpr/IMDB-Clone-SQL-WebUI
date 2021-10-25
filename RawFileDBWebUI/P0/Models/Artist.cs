using System;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace P0.Models
{
    public class Artist
    {
        private List<string> artistFilms = new List<string>();

        [Range(1000, 9999)]
        public int ArtistId { get; set; } // 4 digits, unique
        [Required]
        [StringLength(100)]
        public string ArtistName { get; set; } // max 100 chars
        [Range(1, 999)]
        public int Age { get; set; } // 3 digits
        public IEnumerable<string> ArtistFilms { get => artistFilms.Select(x => x.Trim()).ToList();
            set
            {
                var x = value.Select(x => x.Trim()).ToList();
                artistFilms = x;
            }
             }

        public Artist(int id, string name, int age, List<string> films)
        {
            ArtistId = id;
            ArtistName = name;
            Age = age;
            ArtistFilms = films;
        }

        public override string ToString()
        {
            var films = string.Join(',', ArtistFilms.ToList());
            var r = $"{ArtistId.ToString()}/{ArtistName}/{Age.ToString()}/{films}";
            return r;
        }

        public static Artist Parse(string s)
        {
            var sp = s.Split('/').ToArray();
            return new Artist(int.Parse(sp[0]), sp[1], int.Parse(sp[2]), sp[3].Split(',', StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).ToList());
        }
    }
}
