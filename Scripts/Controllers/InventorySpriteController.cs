using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySpriteController : MonoBehaviour
{
    public GameObject inventoryUIPrefab;
    public GameObject inventoryLight;

    // Dictionarys storing the INVENTORY/GAMEOBJECT and the SPRITE_NAME/SPRITE
    Dictionary<Inventory, GameObject> inventoryGameObjectMap;
    Dictionary<string, Sprite> inventorySprites;

    // The world and tile data.
    World world { get { return WorldController.Instance.world; } }


    // Start is called before the first frame update
    void Start()
    {
        // Load all sprites from "Resources/" and store them in the sprite dictionary
        LoadSprites();

        // Instantiate Dictionary that tracks which GameObject is rendering which Character/ UI.
        inventoryGameObjectMap = new Dictionary<Inventory, GameObject>();

        // Register our Callbacks
        world.RegisterInventoryCreated(OnInventoryCreated);

        // Check pre-existing inventory, which won't do the callback.
        foreach (List<Inventory> InvList in world.inventoryManager.inventories.Values) {
            foreach (Inventory inv in InvList) {
                OnInventoryCreated(inv);
            }
        }
        
    }

    // Load All Sprites from Resources
    void LoadSprites() {
        inventorySprites = new Dictionary<string, Sprite>();
        Sprite[] sprites = Resources.LoadAll<Sprite>("Sprites/Inventory/");

        foreach (Sprite s in sprites) {
            inventorySprites[s.name] = s;
        }
    }

    public void OnInventoryCreated(Inventory inv) {
        // Creates a new GameObject and adds it to our scene.
        GameObject inv_go = new GameObject();

        // Add Inventory/GameObject pair to dictionary.
        inventoryGameObjectMap.Add(inv, inv_go);

        inv_go.name = inv.objectType;
        inv_go.transform.position = new Vector3(inv.tile.X, inv.tile.Y);
        inv_go.transform.SetParent(this.transform, true);

        SpriteRenderer inv_sr = inv_go.AddComponent<SpriteRenderer>();
        inv_sr.sprite = inventorySprites[inv.objectType];
        inv_sr.sortingLayerName = "Inventory";

        GameObject inv_lightGO = Instantiate(inventoryLight, inv_go.transform);

        if (inv.maxStackSize > 1) {
            // This is a stackable object, so let's add a InventoryUI component
            GameObject ui_go = Instantiate(inventoryUIPrefab);
            ui_go.transform.SetParent(inv_go.transform);
            ui_go.transform.localPosition = Vector3.zero;
            ui_go.GetComponentInChildren<Text>().text = inv.stackSize.ToString();
        }

        // Register our callback so that our GameObject gets updated whenever it's info changes
        inv.RegisterInventoryChangedCallback(OnInventoryChanged);
    }

    
    void OnInventoryChanged(Inventory inv) {
        // Make sure the furniture's graphics are correct.

        if (inventoryGameObjectMap.ContainsKey(inv) == false) {
            Debug.LogError("OnCharacterChanged -- trying to change visuals for inventory not in our map");
            return;
        }

        GameObject inv_go = inventoryGameObjectMap[inv];
        // TODO: char_go.GetComponent<SpriteRenderer>().sprite = GetSpriteForInventory(inv);

        if (inv.stackSize > 0) {
            Text amountUI = inv_go.GetComponentInChildren<Text>();
            if (amountUI != null) {
                amountUI.text = inv.stackSize.ToString();
            }
        }
        else {
            // This stack has gone to zero, remove the sprite
            Destroy(inv_go);
            inventoryGameObjectMap.Remove(inv);
            inv.UnregisterInventoryChangedCallback(OnInventoryChanged);
        }
    }
}
