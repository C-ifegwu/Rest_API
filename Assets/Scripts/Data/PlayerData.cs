using System;
using System.Collections.Generic;

// I use [Serializable] so Unity's JsonUtility can read and write to these classes.

[Serializable]
public class InventoryItem
{
    public string itemName;
    public int quantity;
    public float weight;
}

[Serializable]
public class Position
{
    public float x;
    public float y;
    public float z;
}

[Serializable]
public class Record
{
    public string playerName;
    public int level;
    public float health;
    public Position position;
    
    // A List is used here because the JSON uses square brackets [...] for inventory, meaning an array/list of items.
    public List<InventoryItem> inventory; 
}

[Serializable]
public class Metadata
{
    public string id;
    
    // "private" is a reserved keyword in C#. By adding the @ symbol before it, 
    // we trick C# into accepting it as a variable name so it perfectly matches the JSON key!
    public bool @private; 
    
    public string createdAt;
    public string name;
}

[Serializable]
public class RootResponse
{
    // When we download the JSON, we will tell Unity to convert it into this specific class.
    public Record record;
    public Metadata metadata;
}