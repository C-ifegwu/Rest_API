using UnityEngine;
using TMPro; // Crucial! This allows us to control TextMeshPro components via code.

public class UIManager : MonoBehaviour
{
    [Header("Main UI Elements")]
    [SerializeField] private TextMeshProUGUI headerText;
    [SerializeField] private TextMeshProUGUI statsText;
    [SerializeField] private TextMeshProUGUI positionText;
    [SerializeField] private TextMeshProUGUI metadataText;

    [Header("Inventory Settings")]
    [SerializeField] private Transform inventoryContainer; // The InventoryContainer panel
    [SerializeField] private GameObject inventoryItemPrefab; // The prefab from Phase 3

    /// <summary>
    /// Takes the fully deserialized data class and maps it directly to the UI elements.
    /// </summary>
    public void PopulateUI(RootResponse data)
    {
        // 1. Double check that we actually have data to prevent crashes
        if (data == null || data.record == null)
        {
            Debug.LogError("UI Error: Received null or incomplete data!");
            return;
        }

        // 2. Map the top-level record data to our main text fields
        headerText.text = data.record.playerName;
        statsText.text = $"Level: {data.record.level}  |  Health: {data.record.health}%";
        
        // Formatted cleanly with commas using structural data
        positionText.text = $"Position: X: {data.record.position.x}, Y: {data.record.position.y}, Z: {data.record.position.z}";

        // 3. Clear any existing inventory items inside the container 
        // (This prevents items from stacking up if you download data multiple times!)
        foreach (Transform child in inventoryContainer)
        {
            Destroy(child.gameObject);
        }

        // 4. Loop through the inventory list and spawn a prefab for each item
        foreach (InventoryItem item in data.record.inventory)
        {
            // Instantiate creates a clone of our prefab inside the container layout group
            GameObject spawnedItem = Instantiate(inventoryItemPrefab, inventoryContainer);

            // Find the child text components hidden inside the spawned prefab layout
            TextMeshProUGUI[] itemTexts = spawnedItem.GetComponentsInChildren<TextMeshProUGUI>();

            // Assign the text properties based on our nested class values
            // (Assuming Order: 0 = Name, 1 = Qty, 2 = Weight based on how we built the prefab)
            if (itemTexts.Length >= 3)
            {
                itemTexts[0].text = item.itemName;
                itemTexts[1].text = $"Qty: {item.quantity}";
                itemTexts[2].text = $"Weight: {item.weight}kg";
            }
        }

        // 5. Populate the metadata tracking info at the bottom
        metadataText.text = $"Data ID: {data.metadata.id}\nCreated: {data.metadata.createdAt}";
    }
}