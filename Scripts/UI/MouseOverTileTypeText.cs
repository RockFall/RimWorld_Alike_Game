using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class MouseOverTileTypeText : MonoBehaviour
{
    // Every frame, this script checks to see which tile is under the mouse
    // Then updates the GetComponent<Text>.text parameter of the object it is attached to.

    Text myText;
    MouseController mouseController;

    // Start is called before the first frame update
    void Start()
    {
        myText = GetComponent<Text>();

        if (myText == null) {
            Debug.LogError("MouseOverTileTypeText: No 'Text' UI component on this object.");
            this.enabled = false;
            return;
        }

        mouseController = GameObject.FindObjectOfType<MouseController>();
        if (mouseController == null) {
            Debug.LogError("How do we not have an instance of mouse controller?");
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Tile tile = mouseController.GetTileUnderMouse();

        if (tile.Type == TileType.Empty) {
            myText.text = "Vacuum";
        } else if (tile.Type == TileType.Floor) {
            myText.text = "Normal Floor";
        }
    }
}
