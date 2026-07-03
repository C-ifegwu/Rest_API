# Rest_API (Unity)

A Unity project that fetches player data from a REST endpoint and displays it in UI.

The app demonstrates:

- `UnityWebRequest` API calls
- JSON deserialization with `JsonUtility`
- Event-driven communication between systems
- Dynamic UI population (including sorted inventory items)

## Tech Stack

- Unity `6000.4.6f1`
- C# scripts in `Assets/Scripts`
- TextMesh Pro (`com.unity.ugui`, TMP components used in UI)

## Project Structure

- `Assets/Scripts/NetworkManager.cs`: Fetches JSON from the API and raises a global event when data is ready.
- `Assets/Scripts/UIManager.cs`: Subscribes to fetched data and updates text/inventory UI.
- `Assets/Scripts/Data/PlayerData.cs`: Serializable data models used for JSON parsing.
- `Assets/Scenes/SampleScene.unity`: Default scene listed in build settings.

## Data Source

The current API URL is defined in `NetworkManager`:

`https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa`

Expected response shape:

- `record`
  - `playerName`, `level`, `health`
  - `position` (`x`, `y`, `z`)
  - `inventory[]` (`itemName`, `quantity`, `weight`)
- `metadata`
  - `id`, `private`, `createdAt`, `name`

## Runtime Flow

1. `NetworkManager.Start()` calls `FetchData()`.
2. `FetchData()` starts the coroutine `GetJsonData()`.
3. `GetJsonData()` sends a GET request with `UnityWebRequest`.
4. On success, JSON is deserialized into `RootResponse`.
5. `NetworkManager.OnDataFetched` event is invoked.
6. `UIManager` (subscribed in `OnEnable`) receives data and updates:
   - Header and stats text
   - Position text
   - Inventory UI (sorted by weight using LINQ)
   - Metadata text

## How to Run

1. Open the project in Unity Hub with Unity version `6000.4.6f1`.
2. Open `Assets/Scenes/SampleScene.unity`.
3. Press Play in the Unity Editor.

The scene should fetch data on startup and populate the UI automatically.

## Inspector Setup Checklist

For `UIManager`:

- Assign `Header Text` (`TextMeshProUGUI`)
- Assign `Stats Text` (`TextMeshProUGUI`)
- Assign `Position Text` (`TextMeshProUGUI`)
- Assign `Metadata Text` (`TextMeshProUGUI`)
- Assign `Inventory Container` (`Transform` with layout group)
- Assign `Inventory Item Prefab` (prefab containing at least 3 TMP text fields)

Optional refresh:

- Hook a UI Button `OnClick` to `NetworkManager.FetchData()`.

## Error Handling

- Network/protocol failures are logged with `Debug.LogError`.
- `UIManager` validates null/incomplete payloads before UI mapping.

## Notes

- This project uses Unity's built-in `JsonUtility`, which expects matching field names.
- In `Metadata`, the JSON key `private` maps to C# field `@private`.
