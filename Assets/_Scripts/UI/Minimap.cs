using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour
{
    public bool isMinimapOpened;
    public static Minimap _sharedInstance;
    [SerializeField]private Camera minimapCamera;

    private void Awake()
    {
        if (_sharedInstance == null) _sharedInstance = this;
        else Destroy(this);
    }

    public void OpenCloseMinimap()
    {
        var rectTransform = gameObject.transform as RectTransform; 

        if (isMinimapOpened)
        {
            isMinimapOpened = false;
            if(rectTransform) rectTransform.sizeDelta = new Vector2(300, 300);
            minimapCamera.orthographicSize = 45;

        }
        else
        {
            isMinimapOpened = true;
            if(rectTransform) rectTransform.sizeDelta = new Vector2(500, 500);
            minimapCamera.orthographicSize = 100;

        }
    }
}
