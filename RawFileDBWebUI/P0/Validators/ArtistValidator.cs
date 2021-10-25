using System;
using System.Collections.Generic;
using System.Text;

namespace P0.Validators
{
    public static class ArtistValidator
    {
        public static bool HasValidFormat(this Models.Artist artist) =>
            artist.ArtistId > 999 &&
            artist.ArtistId.ToString().Length == 4 &&
            artist.ArtistName.Length <= 100 &&
            artist.Age > 0 &&
            artist.Age < 1000;
    }
}
