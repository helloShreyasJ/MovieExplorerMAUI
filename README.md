# MovieExplorer — .NET MAUI Movie Explorer
A cross-platform application for browsing, searching, and managing movie collections using .NET MAUI and the TMDB API.

## Features
- **Browse Movies** from a [JSON listing](https://github.com/DonH-ITS/jsonfiles/blob/main/moviesemoji.json)
- **Search & filter** movies instantly in the UI
- **Add/Remove Favorites**
- **Movie Details** fetched from TMDB:
  - Popularity score
  - Original language
  - Overview/description
  - Poster URL
- **Poster Caching** on first app launch
- **Dynamic Dark/Light Theme**

## TMDB API Key
- The app uses the TMDB API to search and fetch movie details. Add your API key from in-app settings.
- Get your first key [here](https://www.themoviedb.org/settings/api)

## Architecture
MovieExplorer follows the **MVVM (Model–View–ViewModel)** design pattern for clean separation of UI and logic.
- **Models** define movie data
- **ViewModels** handle UI state, filtering, API enrichment, poster caching, and commands
- **Views (XAML pages)** bind reactively using `INotifyPropertyChanged` and `ObservableCollection`
- **Services** manage API calls and local persistence (Favorites, Posters, Settings)

## Prerequisites
- .NET 9.0 SDK
- Visual Studio 2022 (17.8+) or JetBrains Rider (MacOS)


## How do I use it?
1. Clone the repository
   `git clone https://github.com/helloShreyasJ/MovieExplorerMAUI.git`
2. Open `MovieExplorer.sln` in Visual Studio or Rider.
3. Build & Run Select your target framework (Android, iOS, or Windows) and run the application.

## License
MovieExplorer is under the [MIT](https://github.com/helloShreyasJ/MovieExplorerMAUI/blob/main/LICENSE.rtf) License
