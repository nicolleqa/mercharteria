using SpotifyAPI.Web;
using Microsoft.Extensions.Options;
using mercharteria.Config;
using System.Threading.Tasks;

namespace mercharteria.Services
{
    public class SpotifyService
    {
        private readonly SpotifyClient _spotifyClient;

        public SpotifyService(IOptions<SpotifyConfig> config)
        {
            try
            {
                var credentials = new ClientCredentialsRequest(
                    config.Value.ClientId,
                    config.Value.ClientSecret
                );
                
                var token = new OAuthClient().RequestToken(credentials).GetAwaiter().GetResult();
                _spotifyClient = new SpotifyClient(token.AccessToken);
            }
            catch (Exception ex)
            {
                throw new Exception("Error initializing Spotify client", ex);
            }
        }

        public async Task<string> GetPlaylistEmbedUrl(string playlistId)
        {
            if (string.IsNullOrEmpty(playlistId))
            {
                throw new ArgumentNullException(nameof(playlistId));
            }
            
            return $"https://open.spotify.com/embed/playlist/{playlistId}";
        }
    }
}