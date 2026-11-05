# CongressCompanion

[![Project status](https://img.shields.io/badge/status-legacy%20project-D97706?style=for-the-badge)](#project-status)
[![C sharp](https://img.shields.io/badge/C%23-239120?style=for-the-badge)](#technology)
[![Xamarin.Forms](https://img.shields.io/badge/Xamarin.Forms-3.2-3498DB?style=for-the-badge)](#technology)
[![.NET Standard](https://img.shields.io/badge/.NET_Standard-2.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](#technology)
[![Platforms](https://img.shields.io/badge/platforms-Android%20%7C%20iOS%20%7C%20UWP-0078D4?style=for-the-badge)](#repository-layout)

> Your representatives, organized around where you live.

CongressCompanion is a civic contact app built to help people find and contact their United States federal, state, and local officials. Enter an address or ZIP code, browse officials by level of government, and open a profile with the contact information available for that person.

The project uses a shared C# and Xamarin.Forms interface with Android, iOS, and Windows UWP host projects. It includes address validation, representative profiles, phone and email shortcuts, saved-location settings, and three visual themes.

**Current availability:** The original representative lookup depends on Google's Civic Information Representatives API, which was [retired in April 2025](https://groups.google.com/g/google-civicinfo-api/c/9fwFn-dhktA). The checked-in app needs a replacement data integration before live representative lookup can work.

## Table of contents

- [Why CongressCompanion?](#why-congresscompanion)
- [App experience](#app-experience)
- [Data integration](#data-integration)
- [Technology](#technology)
- [Repository layout](#repository-layout)
- [Local development](#local-development)
- [Configuration and saved data](#configuration-and-saved-data)
- [Project status](#project-status)
- [License](#license)

## Why CongressCompanion?

Finding the right public official often means moving between separate government websites. CongressCompanion brings the discovery and contact steps into one location-based interface.

The app is organized around a few focused ideas:

- Start with a place the user recognizes: a street address or ZIP code.
- Separate federal, state, and local officials into clear lists.
- Keep the person's office, party, photo, and available contact details together.
- Make phone calls, email, and official websites accessible from the profile.
- Let users reuse a location and choose a comfortable visual theme.

The name reflects the project's civic focus; the interface also includes state and local offices beyond Congress.

## App experience

### Address or ZIP-code lookup

The location page accepts either a ZIP code or a street address with city and state. Custom entry controls validate the input before requesting representative data. The interface shows a loading indicator while the lookup runs.

A saved location can be reused on later launches. Settings includes an edit flow for changing that location.

### Officials by government level

The main side menu provides **Federal**, **State**, **Local**, and **Settings** pages.

The data layer associates officials with their offices and divisions, then groups country-level offices under Federal, state offices under State, and county or place offices under Local. List cards show the official's name and office title. Entries labeled `vacant` display a vacant-seat message when selected.

### Representative profiles

A profile displays the official's name, office, party, photo, phone number, and website when that information is available.

| Action | Behavior |
| --- | --- |
| Phone button | Open the device's `tel:` handler with the listed number |
| Website button | Open the official's website |
| Email button | Open the device's `mailto:` handler with the listed email address |

The profile hides unavailable photo, phone, or website content and disables contact buttons when the corresponding information is missing. Contact actions use the device's registered handlers.

### Themes and settings

Three themes are defined in the shared app: **Default**, **Light**, and **Dark**. Theme changes update the interface colors and are saved for later launches.

Settings also contains the Save Location switch, an Edit action for the stored location, and a Reset User Data action with an in-app confirmation.

## Data integration

The original client calls the Google Civic Information API directly. Its historical request shape is:

```text
GET https://www.googleapis.com/civicinfo/v2/representatives?address={address}&key={apiKey}
```

This endpoint is documented here to explain the source. Google's Civic Information API team announced an April 30, 2025 shutdown and subsequently [confirmed that the Representatives API had been turned down](https://groups.google.com/g/google-civicinfo-api/c/9fwFn-dhktA). A new Google API key alone will not restore this lookup.

The current processing flow is:

1. `AppManager.GetRepData()` requests the location's representative information.
2. Newtonsoft.Json deserializes the response into the models in `ClassObjects/JsonClasses.cs`.
3. Official records become `Representative` objects.
4. Office `officialIndices` connect those people to office names, government levels, and division IDs.
5. The grouped results feed the list and profile pages.

A replacement integration must supply or adapt the office and official data used by this flow, including contact details and government-level classification. The repository includes the client integration and models; it does not include an application-owned backend, offline official dataset, or replacement provider.

## Technology

| Layer | Technology | Responsibility |
| --- | --- | --- |
| Shared application | C# and .NET Standard 2.0 | Shared pages, representative models, navigation, and application state |
| Interface | Xamarin.Forms 3.2.0.839982 | XAML layouts, lists, profiles, settings, and platform-independent UI |
| Android host | Xamarin.Android | Android entry point, manifest, icons, and platform resources |
| iOS host | Xamarin.iOS | iOS entry point, launch assets, and platform configuration |
| Windows host | Universal Windows Platform | UWP application host targeting Windows SDK 10.0.18362.0 |
| JSON handling | Newtonsoft.Json 11.0.2 | Deserialize the original provider response |
| Contact controls | Forms.Controls.FlexButton 0.8.0 | Icon buttons for phone, website, email, and settings actions |
| App utilities | Repository-local `AE-Xamarin` code | Input validation, theme management, state, and property persistence |
| Original data service | Google Civic Information Representatives API | Historical address-to-official lookup; now retired |

## Repository layout

```text
CongressCompanion/
├── README.md
└── XamarinApp/
    ├── CongressCompanion.sln
    └── CongressCompanion/
        ├── CongressCompanion/              # Shared .NET Standard application
        │   ├── AE-Xamarin/
        │   │   ├── Controls/               # Validated address, city, ZIP, and other inputs
        │   │   └── Managers/               # App state, data loading, themes, and saved settings
        │   ├── ClassObjects/               # Representative and provider-response models
        │   ├── MasterPageViews/            # Side menu and navigation shell
        │   ├── App.xaml.cs                 # Startup and saved-state loading
        │   ├── LocationPage.xaml           # Address and ZIP-code entry
        │   ├── RepsListPage.xaml           # Federal, state, and local official lists
        │   ├── RepProfilePage.xaml         # Profile and contact actions
        │   └── SettingsPage.xaml           # Theme and saved-location controls
        ├── CongressCompanion.Android/      # Android host
        ├── CongressCompanion.iOS/          # iOS host
        └── CongressCompanion.UWP/          # Windows UWP host
```

## Local development

### Prerequisites

Reconstructing the app requires a compatible legacy Xamarin development environment:

- Visual Studio/MSBuild with the Xamarin tooling needed by the selected host project.
- NuGet restore support for the shared and platform-specific package references.
- Matching Android SDKs for the selected Android build configuration.
- Windows SDK 10.0.18362.0 for the checked-in UWP target.
- A compatible Mac, Xamarin.iOS tooling, and signing configuration for an iOS device build.
- A replacement data integration or a locally stubbed response to exercise representative lookup and populated profiles.

The solution records Visual Studio 15-era tooling. Its classic Xamarin and UWP hosts require their platform build targets; a current .NET SDK by itself does not provide those targets.

### Open and restore

From PowerShell:

```powershell
git clone https://github.com/Amitai5/CongressCompanion.git
cd CongressCompanion
```

Open `XamarinApp/CongressCompanion.sln` in a compatible Visual Studio installation and restore its NuGet packages. Select the Android, iOS, or UWP startup project for the environment you have configured.

From a compatible Visual Studio Developer PowerShell, the solution restore command is:

```powershell
msbuild XamarinApp/CongressCompanion.sln /t:Restore
```

### Review the data path before running

Start with `XamarinApp/CongressCompanion/CongressCompanion/AE-Xamarin/Managers/AppManager.cs`. The `APICallFormat`, `GetRepData()`, and `LoadRepresentatives()` members define the original request and response-processing flow.

Replace or stub the retired provider integration, then adapt `ClassObjects/JsonClasses.cs` and the representative mapping as needed. Use non-sensitive development locations when reviewing the flow.

Build and launch the selected platform host through Visual Studio. These instructions describe the checked-in project structure; current platform builds, device behavior, and a replacement lookup service have not been verified.

## Configuration and saved data

### Provider settings

The current `GetRepData()` implementation embeds a provider key in source. Do not reuse that key. Configure credentials for any replacement integration outside tracked source and follow that provider's client-application credential requirements.

The entered address or ZIP code is supplied to the configured data provider. The original implementation performs this request directly from the client.

### Local preferences

The app uses Xamarin.Forms `Application.Current.Properties` for saved values:

| Key | Stored value |
| --- | --- |
| `ShouldSaveLocation` | Whether the app should automatically reuse the stored location |
| `LocationInfo` | The entered address or ZIP code |
| `SavedThemeName` | The selected theme name |

The default saved-location setting is enabled. Both `ShouldSaveLocation` and `LocationInfo` are marked for persistence by the reflection-based save helper. Turning off Save Location controls automatic reuse; it does not remove `LocationInfo` from saved properties in the current implementation. Reset User Data restores the tagged defaults and the default theme.

## Project status

CongressCompanion is a **legacy Xamarin.Forms project** with implemented location, list, profile, contact, theme, and settings flows.

The main limitations are:

- Live representative lookup requires replacing the retired Google Representatives endpoint.
- The host projects depend on historical Xamarin and platform tooling.
- Network failures are not comprehensively handled; the current request can throw before the location page displays its failure message.
- Saved-location behavior needs to be understood as described above, particularly when disabling location reuse.
- No automated test project, continuous-integration workflow, or bundled sample response is included.

The README is based on the checked-in source and the provider's retirement notice. It does not establish current official coverage, contact-data freshness, or working app-store distributions.

## License

No repository-wide `LICENSE` file is included.
