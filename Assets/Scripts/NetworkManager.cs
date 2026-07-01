using System.Collections;
using UnityEngine;
using UnityEngine.Networking; // Required to use UnityWebRequest

public class NetworkManager : MonoBehaviour
{
    // The URL provided in your assignment
    private readonly string apiUrl = "https://api.jsonbin.io/v3/b/6686a992e41b4d34e40d06fa";

    // This runs the very moment the game starts
    void Start()
    {
        // We start the Coroutine to fetch data in the background
        StartCoroutine(GetJsonData());
    }

    // IEnumerator is the required return type for Coroutines in Unity
    private IEnumerator GetJsonData()
    {
        // 1. Create the web request
        using (UnityWebRequest webRequest = UnityWebRequest.Get(apiUrl))
        {
            // 2. Send the request and wait for a response without freezing the game
            yield return webRequest.SendWebRequest();

            // 3. Check for errors (Network error like no internet, or HTTP error like 404 Not Found)
            if (webRequest.result == UnityWebRequest.Result.ConnectionError || 
                webRequest.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError("Error fetching data: " + webRequest.error);
            }
            else
            {
                // 4. If successful, grab the raw text from the downloaded data
                string jsonResponse = webRequest.downloadHandler.text;
                Debug.Log("Successfully downloaded JSON! Raw text: " + jsonResponse);

                // 5. Deserialize! This is where the magic happens. 
                // We tell Unity's JsonUtility to map the raw text to our RootResponse class.
                RootResponse parsedData = JsonUtility.FromJson<RootResponse>(jsonResponse);

                // 6. Test that it worked by printing a specific piece of data to the console
                if (parsedData != null && parsedData.record != null)
                {
                    Debug.Log("Success! The Player's Name is: " + parsedData.record.playerName);
                    Debug.Log("Success! The Player's Health is: " + parsedData.record.health);
                }
            }
        }
    }
}