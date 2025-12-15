using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

using MediaPlayerApp.MusicBrainz;

namespace MediaPlayerApp
{
    public static class MusicBrainzClient
    {
        private static readonly HttpClient client;
        private const string ApiBaseUrl = "http://musicbrainz.org/ws/2/";

        static MusicBrainzClient()
        {
            client = new HttpClient();
            // Set required User-Agent header for MusicBrainz API
            client.DefaultRequestHeaders.UserAgent.ParseAdd("MediaPlayerApp/1.0 (b.alvesmartins495@mybvc.ca)");
        }

        public static async Task<Recording> SearchRecording(string query)
        {
            try
            {
                // Correctly encode the search query for the URL
                string encodedQuery = Uri.EscapeDataString(query);
                string url = $"{ApiBaseUrl}recording?query={encodedQuery}&fmt=json&limit=1";

                // Send GET request to the MusicBrainz API
                HttpResponseMessage response = await client.GetAsync(url);
                // Throws an exception if the HTTP status code is an error (4xx or 5xx)
                response.EnsureSuccessStatusCode();

                string responseBody = await response.Content.ReadAsStringAsync();

                // Options for case-insensitive JSON deserialization (robustness)
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var searchResult = JsonSerializer.Deserialize<MusicBrainzSearchResponse>(responseBody, options);

                if (searchResult?.Recordings?.Count > 0)
                {
                    return searchResult.Recordings[0];
                }
            }
            catch (HttpRequestException e)
            {
                // Handle network or HTTP status errors
                Console.WriteLine($"[MusicBrainz Error] HttpRequestException: {e.Message}");
            }
            catch (JsonException e)
            {
                // Handle errors during JSON parsing
                Console.WriteLine($"[MusicBrainz Error] JsonException: Failed to parse response. {e.Message}");
            }
            catch (Exception e)
            {
                Console.WriteLine($"[MusicBrainz Error] An unexpected error occurred: {e.Message}");
            }

            return null;
        }

        /// <summary>
        /// Isolated test function for API calls.
        /// </summary>
        public static async Task TestClientAsync(string artist, string title)
        {
            // Known successful query format
            string query = $"artist:\"{artist}\" AND recording:\"{title}\"";

            Console.WriteLine($"\n--- MUSICBRAINZ TEST ---");
            Console.WriteLine($"Test Query: {query}");

            var result = await SearchRecording(query);

            if (result != null)
            {
                Console.WriteLine("SUCCESS! Metadata Received:");
                Console.WriteLine($"  Title: {result.Title}");
                // PrimaryArtist relies on ArtistCredits[0] being parsed correctly
                Console.WriteLine($"  Primary Artist: {result.PrimaryArtist}");
                Console.WriteLine($"  Album/Release: {(result.Releases?.Count > 0 ? result.Releases[0].Title : "N/A")}");
                Console.WriteLine($"  Duration (ms): {result.LengthMilliseconds}");
            }
            else
            {
                Console.WriteLine("FAILURE: Could not find metadata or an error occurred during request/parsing.");
            }
        }
    }
}