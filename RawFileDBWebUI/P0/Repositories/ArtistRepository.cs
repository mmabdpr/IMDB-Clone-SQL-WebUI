using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using P0.Models;
using P0.Settings;
using P0.Validators;

namespace P0.Repositories
{
    public class ArtistRepository
    {
        private string _filePath => FileSettings.ArtistFilePath;

        public ArtistRepository()
        {
        }

        public IEnumerable<(int Index, Artist Artist)> GetAll() =>
            ReadFromArtistFile();

        public IEnumerable<Artist> GetArtistsByFilm(string filmName) => 
            ReadFromArtistFile().Where(a => a.Artist.ArtistFilms.Contains(filmName)).Select(a => a.Artist).ToList();

        public void Add(Artist artist) // throw error if have same artistId, check films exist
        {
            if (!artist.HasValidFormat())
                throw new InvalidDataException("Artist data is not in valid format");

            if (FindById(artist.ArtistId) != null)
                throw new InvalidOperationException("There is a artist with this Id");

            var filmRepo = new FilmRepository();
            foreach (var film in artist.ArtistFilms)
                if (!filmRepo.FindByName(film, true).Any())
                    throw new InvalidDataException($"Can't find any film with name {film}\nDouble check spelling and character cases");

            var index = Helpers.DBIndexHelper.GetLastIndex(_filePath) + 1;
            WriteToArtistFile(ReadFromArtistFile().Append((index, artist)).ToList());

            UpdateIndexFiles(index, artist);
        }

        private void UpdateIndexFiles(int index, Artist artist)
        {
            UpdateIdIndexFile(index, artist);
            UpdateNamesIndexFile(index, artist);
        }

        private void UpdateNamesIndexFile(int index, Artist artist)
        {
            var lines = ReadFromNameIndexFile();
            var n = lines.Select(l => l.Name).ToList().BinarySearch(artist.ArtistName);
            lines.Insert(n < 0 ? ~n : n, (artist.ArtistName, index));
            WriteToNameIndexFile(lines);
        }

        private void UpdateIdIndexFile(int index, Artist artist)
        {
            var lines = ReadFromIdIndexFile();
            var n = lines.Select(l => l.Id).ToList().BinarySearch(artist.ArtistId);
            lines.Insert(~n, (artist.ArtistId, index));
            WriteToIdIndexFile(lines);
        }

        public Artist FindById(int artistId)
        {
            // Using index file to find artist (proof of concept)
            var index = GetArtistIndexById(artistId);
            if (index == null)
                return null;

            var artists = ReadFromArtistFile();
            var artistNumber = artists.Select(a => a.Index).ToList().BinarySearch(index.Value);
            var artist = artists[artistNumber].Artist;
            return artist;
        }

        private int? GetArtistIndexById(int artistId)
        {
            var indexLines = ReadFromIdIndexFile();
            var n = indexLines.Select(x => x.Id).ToList().BinarySearch(artistId);
            return n < 0 ? (int?)null : indexLines[n].Index;
        }

        private List<(int Id, int Index)> ReadFromIdIndexFile() =>
            File.ReadAllLines(FileSettings.ArtistIdIndexFilePath)
                .Select(l => (int.Parse(l.Split(',')[0]), int.Parse(l.Split(',')[1]))).ToList();

        private void WriteToIdIndexFile(List<(int Id, int Index)> lines) =>
            File.WriteAllLines(FileSettings.ArtistIdIndexFilePath,
                lines.OrderBy(l => l.Id).Select(l => $"{l.Id},{l.Index}").ToArray());
        
        private List<(string Name, int Index)> ReadFromNameIndexFile() =>
            File.ReadAllLines(FileSettings.ArtistNameIndexFilePath)
                .Select(l => (l.Split(',')[0], int.Parse(l.Split(',')[1]))).ToList();

        private void WriteToNameIndexFile(List<(string Name, int Index)> lines) =>
            File.WriteAllLines(FileSettings.ArtistNameIndexFilePath,
                lines.OrderBy(l => l.Name).Select(l => $"{l.Name},{l.Index}").ToArray());

        public void RemoveArtistsFilm(string filmName) => WriteToArtistFile(
            ReadFromArtistFile().Select(a =>
            {
                a.Artist.ArtistFilms = a.Artist.ArtistFilms.Where(f => f != filmName).ToList();
                return a;
            }).ToList());

        public void UpdateArtistsFilm(string oldFilmName, string newFilmName) => WriteToArtistFile(
            ReadFromArtistFile().Select(a =>
                {
                    a.Artist.ArtistFilms = a.Artist.ArtistFilms.Select(f => f == oldFilmName ? newFilmName : f).ToList();
                    return a;
                }).ToList());

        public void RemoveById(int artistId)
        {
            var artist = FindById(artistId);
            if (artist == null)
                throw new InvalidOperationException("Artist with this Id couldn't be found");

            WriteToIdIndexFile(ReadFromIdIndexFile().Where(l => l.Id != artistId).ToList());
            WriteToNameIndexFile(ReadFromNameIndexFile().Where(l => l.Name != artist.ArtistName).ToList());
            WriteToArtistFile(ReadFromArtistFile().Where(a => a.Artist.ArtistId != artistId).ToList());
        }

        public void Update(int oldArtistId, Artist newArtist)
        {
            if (!newArtist.HasValidFormat())
                throw new InvalidDataException("Artist data is not in valid format");

            if (oldArtistId == newArtist.ArtistId)
            {
                var oldArtist = FindById(oldArtistId);
                WriteToNameIndexFile(
                    ReadFromNameIndexFile().Select(l => l.Name == oldArtist.ArtistName ? (newArtist.ArtistName, l.Index) : l).ToList());
                WriteToArtistFile(
                    ReadFromArtistFile().Select(a => a.Artist.ArtistId == newArtist.ArtistId ? (a.Index, newArtist) : a).ToList());
            }
            else if (FindById(newArtist.ArtistId) != null)
                throw new InvalidOperationException("There is an artist with this ID");
            else
            {
                RemoveById(oldArtistId);
                Add(newArtist);
            }
        }

        private List<(int Index, Artist Artist)> ReadFromArtistFile() =>
            File.ReadAllLines(_filePath).Select(l => ParseLine(l)).ToList();

        private void WriteToArtistFile(List<(int Index, Artist Artist)> artists) =>
            File.WriteAllLines(_filePath, artists.OrderBy(a => a.Index).Select(a => $"{a.Index}-{a.Artist}").ToArray());

        private (int Index, Artist Artist) ParseLine(string line) => (int.Parse(line.Split('-').First()),
            Artist.Parse(string.Join('-', line.Split('-').Skip(1))));
    }
}
