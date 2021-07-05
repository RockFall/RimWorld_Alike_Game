using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory{

    public string objectType;
    public int    maxStackSize = 50;

    protected int _stackSize = 1;
    public int stackSize {
        get { return _stackSize; }
        set { if (_stackSize != value) {
                _stackSize = value;
                if (cbInventoryChanged != null) {
                    cbInventoryChanged(this);
                }
            } 
        }
    }

    public Tile      tile;
    public Character character;

    // The function we callback any time our inventory's data changes.
    public Action<Inventory> cbInventoryChanged;

    // Empty constructor for loading and saving reasons.
    public Inventory() {

    }

    // Normal constructor.
    public Inventory(string objectType, int maxStackSize, int stackSize) {
        this.objectType   = objectType;
        this.maxStackSize = maxStackSize;
        this.stackSize    = stackSize;
    }

    // Copy constructor.
    protected Inventory (Inventory other) {
        objectType   = other.objectType;
        maxStackSize = other.maxStackSize;
        stackSize    = other.stackSize;
    }
    virtual public Inventory Clone() {
        return new Inventory(this);

    }


    /// <summary>
    /// Register a function to be called back when our inventory type changes.
    /// </summary>
    public void RegisterInventoryChangedCallback(Action<Inventory> callback) {
        cbInventoryChanged += callback;
    }
    /// <summary>
    /// Unregister a callback.
    /// </summary>
    public void UnregisterInventoryChangedCallback(Action<Inventory> callback) {
        cbInventoryChanged -= callback;
    }
}
