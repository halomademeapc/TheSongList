using IF.Lastfm.Core.Api;
using System.Linq;
using System.Threading.Tasks;
using TheSongList.Models.Entities;
using TheSongList.Models.Lastfm;

namespace TheSongList.Services
{
    public class MetadataService
    {
        private LastfmClient client;

        public MetadataService(LastfmClient client)
        {
            this.client = client;
        }

        public async Task<SongInfo> GetTrackInfo(Song s)
        {
            var response = await client.Track.GetInfoAsync(s.Name, s.Artist.Name);
            return new SongInfo
            {
                Song = s,
                AlbumName = response?.Content?.AlbumName,
                Duration = response?.Content?.Duration,
                Image = response?.Content?.Images?.Largest != null ? response?.Content?.Images?.Largest.ToString() : string.Empty,
                Tags = response?.Content?.TopTags?.Select(t => t.Name)
            };
        }

        public async Task<ArtistInfo> GetArtistInfo(Artist a)
        {
            var response = await client.Artist.GetInfoAsync(a.Name);
            return new ArtistInfo
            {
                Artist = a,
                Summary = response?.Content?.Bio?.Summary,
                Biography = response?.Content?.Bio?.Content,
                YearFormed = response?.Content?.Bio?.YearFormed,
                Tags = response?.Content?.Tags?.Select(t => t.Name),
                Image = response?.Content?.MainImage?.Largest != null ? response.Content.MainImage.Large.ToString() : string.Empty
            };
        }
    }
}
