
# SpotifyAnarchyWebEdition
A very simple Spotify Client, created with the ASP.NET framework, using the official Spotify API
<small>This is my learning project for the ASP.NET-framework. I would be very happy about suggestions, criticism, praise and alternative solutions!</small>

## Using this client, you are able to...
 - ...view and browse the newest releases
 - ...login to your Spotify account and see your playlists and devices, on which you are logged in
 - ...follow and unfollow playlists
 - ...search songs, albums, playlists, e.g. by metadata
 - ...play a 30 seconds long demo of some selected songs

## Features to implement soon
 - [x] Search through the Spotify catalogue
 - [ ] Search for all content types
 - [ ] Follow/Unfollow Artists and Users
 - [ ] Add a simple API for retrieving user data by providing an access token
 - [ ] Outsource the program logic to a class library
 - [ ] UI/UX optimizations and redesign
 - [ ] Refactor models and classes as seperate files (many of them are currently stored together)

## F.A.Q.

 - **Q: Why is this project called as ,,Anarchy Edition"?**
 **A:** This is a Spotify client, how **I** want it. With less tracking and no personalized suggestions
 
 - **Q: What's the meaning of these japanese letters on the start page, and why are they there?**
  **A:** Oh, you noticed the "スポティファイ・アナーキー"? The meaning is directly below these letters. 
Literally translated, it's "supotifai anaakii" and means "Spotify Anarchy". I have included this so that the project at least stands out a little from others.

## Before you can run this application
 **On Spotify's side...**
 - ...make sure, that you have a [Spotify developer](https://developer.spotify.com/) account with an registered app
 - - As URI, you have to enter -   `https://localhost:44394/oauth` in the [Spotify for Developers Dashboard](https://developer.spotify.com/dashboard/) app settings
 
**On your app's side...**
 - ...you have to enter your custom Client credentials into the `/Models/DefaultValues.cs` file
- - Notice the `basicAuthorization` variable! This is a base64 string, which contains the value "`yourClientId`:`yourClientSecret`" and is used by [Spotify's Authorization flow](https://developer.spotify.com/documentation/web-api/concepts/authorization).
