﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AutomaticVerticalSize : MonoBehaviour
{
    public float childHeight = 20f;
    
    
    // Start is called before the first frame update
    void Start()
    {
        AdjustSize();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AdjustSize() {
        Vector2 size = this.GetComponent<RectTransform>().sizeDelta;
        size.y = this.transform.childCount * childHeight;
        this.GetComponent<RectTransform>().sizeDelta = size;
    }
}
