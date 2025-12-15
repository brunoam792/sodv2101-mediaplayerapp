using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MediaPlayerApp.MusicBrainz
{
    public class ArtistCredit
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    public class Release
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }
    }

    public class Recording
    {
        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("artist-credit")]
        public List<ArtistCredit> ArtistCredits { get; set; }

        // The 'releases' field contains the list of albums or releases
        [JsonPropertyName("releases")]
        public List<Release> Releases { get; set; }

        [JsonPropertyName("length")]
        public int LengthMilliseconds { get; set; }

        // Calculated property to get the main artist from the credit list
        public string PrimaryArtist
        {
            get => ArtistCredits?.Count > 0 ? ArtistCredits[0].Name : "Unknown Artist";
        }
    }

    public class MusicBrainzSearchResponse
    {
        [JsonPropertyName("count")]
        public int Count { get; set; }

        // The list of results is named 'recordings'
        [JsonPropertyName("recordings")]
        public List<Recording> Recordings { get; set; }
    }
}