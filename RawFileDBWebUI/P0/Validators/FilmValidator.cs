using System;
using System.Collections.Generic;
using System.Text;

namespace P0.Validators
{
    public static class FilmValidator
    {
        public static bool HasValidFormat(this Models.Film film) =>
            film.FilmId > 999 &&
            film.FilmId.ToString().Length == 4 &&
            film.FilmName.Length <= 100 &&
            film.DirectorName.Length <= 100 &&
            film.ProductionYear > 999 &&
            film.ProductionYear.ToString().Length == 4 &&
            film.Genre.Length <= 100;
    }
}
