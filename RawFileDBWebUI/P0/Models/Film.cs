using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace P0.Models
{
    public class Film
    {
        [Range(1000, 9999)]
        public int FilmId { get; set; } // 4 digits, Unique
        [Required]
        [StringLength(100)]
        public string FilmName { get; set; } // max 100 chars
        [Required]
        [StringLength(100)]
        public string DirectorName { get; set; } // max 100 chars
        [Range(1800, 9999)]
        public int ProductionYear { get; set; } // 4 digits
        [Required]
        [StringLength(20)]
        public string Genre { get; set; } // max 20 chars

        public Film(int id, string name, string directorName, int productionYear, string genre)
        {
            FilmId = id;
            FilmName = name;
            DirectorName = directorName;
            ProductionYear = productionYear;
            Genre = genre;
        }

        public override string ToString()
        {
            return $"{FilmId.ToString()}/{FilmName}/{DirectorName}/{ProductionYear.ToString()}/{Genre}";
        }

        public static Film Parse(string s)
        {
            var sp = s.Split('/').ToArray();
            return new Film(int.Parse(sp[0]), sp[1], sp[2], int.Parse(sp[3]), sp[4]);
        }
    }
}
