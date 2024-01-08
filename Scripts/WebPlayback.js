var accessToken

function setAccessToken(token) {
    accessToken = token;
}

// Creates a new Spotify player and handles all the events
window.onSpotifyWebPlaybackSDKReady = () => {
    const token = accessToken;
    const player = new Spotify.Player({
        name: 'Spotify Anarchy Web Edition Web Playback',
        getOAuthToken: cb => { cb(token); },
        volume: 0.5
    })
    // Error handling
    player.addListener('initialization_error', ({ message }) => { console.error(message); });
    player.addListener('authentication_error', ({ message }) => { console.error(message); });
    player.addListener('account_error', ({ message }) => { console.error(message); });
    player.addListener('playback_error', ({ message }) => { console.error(message); });

    // Playback status updates
    player.connect();

    // Pause / Resume on button click
    document.getElementById('togglePlay').onclick = function () {
        player.togglePlay();
    };
}
