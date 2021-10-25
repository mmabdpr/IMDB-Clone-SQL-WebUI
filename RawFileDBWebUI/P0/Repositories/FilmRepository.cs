using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using P0.Models;
using P0.Settings;
using P0.Validators;

namespace P0.Repositories
{
    public class FilmRepository
    {
        private string _filePath => FileSettings.FilmFilePath;

        public FilmRepository()
        {
        }

        public IEnumerable<(int Index, Film Film)> GetAll()
        {
            return File.ReadAllLines(_filePath).Select(l => ParseLine(l));
        }

        public void Add(Film film) // throw error if have same filmId
        {
            if (!film.HasValidFormat())
                throw new InvalidDataException("Film data is not in valid format");

            if (FindById(film.FilmId) != null)
                throw new InvalidOperationException("There is a film with this Id");

            File.AppendAllLines(_filePath, new[] {$"{(Helpers.DBIndexHelper.GetLastIndex(_filePath) + 1).ToString()}-{film}"});
        }

        public IEnumerable<Film> FindByName(string filmName, bool exactMatch = false)
        {
            if (string.IsNullOrWhiteSpace(filmName))
                throw new InvalidDataException("Film name can't be empty");

            var lines = File.ReadAllLines(_filePath).Select(l => ParseLine(l).Film);
            var result = (exactMatch ? lines.Where(l => l.FilmName == filmName)
                : lines.Where(l => l.FilmName.Contains(filmName, StringComparison.InvariantCultureIgnoreCase))).ToList();
            return result;
        }

        public Film FindById(int filmId) =>
            File.ReadAllLines(_filePath).Select(l => ParseLine(l).Film)
                .FirstOrDefault(l => l.FilmId == filmId);

        public void RemoveById(int filmId, bool updateArtists = true)
        { // remove related artists film
            if (updateArtists)
            {
                var film = FindById(filmId);
                var artistRepo = new ArtistRepository();
                artistRepo.RemoveArtistsFilm(film.FilmName);
            }

            File.WriteAllLines(_filePath,
                File.ReadAllLines(_filePath).Where(l => ParseLine(l).Film.FilmId != filmId));
        }

        public void Update(int oldFilmId, Film newFilm)
        {
            if (!newFilm.HasValidFormat())
                throw new InvalidDataException("Film data is not in valid format");

            if (oldFilmId == newFilm.FilmId) 
            {
                var film = FindById(oldFilmId);
                var artistRepo = new ArtistRepository();
                artistRepo.UpdateArtistsFilm(film.FilmName, newFilm.FilmName);

                File.WriteAllLines(_filePath,
                        File.ReadAllLines(_filePath).Select(l =>
                            ParseLine(l).Film.FilmId == newFilm.FilmId
                                ? $"{ParseLine(l).Index.ToString()}-{newFilm}"
                                : l));
            }
            else if (FindById(newFilm.FilmId) != null)
                throw new InvalidOperationException("There is an film with this ID");
            else
            {
                // update artists if FilmName has changes
                var oldFilm = FindById(oldFilmId);
                var artistRepo = new ArtistRepository();
                if (oldFilm.FilmName != newFilm.FilmName)
                    artistRepo.UpdateArtistsFilm(oldFilm.FilmName, newFilm.FilmName);

                RemoveById(oldFilmId, false);
                Add(newFilm);
            }
        }

        private (int Index, Film Film) ParseLine(string line) => (int.Parse(line.Split('-').First()),
            Film.Parse(string.Join('-', line.Split('-').Skip(1))));
    }
}