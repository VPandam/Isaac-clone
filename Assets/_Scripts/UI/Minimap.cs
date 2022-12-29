using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Minimap : MonoBehaviour
{
    public bool isMinimapOpened;
    public static Minimap _sharedInstance;
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Sprite marco500, marco300;
    [SerializeField] GameObject marco;

    private void Awake()
    {
        if (_sharedInstance == null) _sharedInstance = this;
        else Destroy(this);
    }



    public void OpenCloseMinimap()
    {
        var rectTransform = gameObject.transform as RectTransform; 
        var rectTransformMarco = marco.transform as RectTransform; 

        if (isMinimapOpened)
        {
            isMinimapOpened = false;
            if(rectTransform) rectTransform.sizeDelta = new Vector2(300, 300);
            if(rectTransformMarco) rectTransformMarco.sizeDelta = new Vector2(300, 300);
            marco.GetComponent<Image>().sprite = marco300;
            minimapCamera.orthographicSize = 45;

        }
        else
        {
            isMinimapOpened = true;
            if(rectTransform) rectTransform.sizeDelta = new Vector2(500, 500);
            if (rectTransformMarco) rectTransformMarco.sizeDelta = new Vector2(500, 500);
            marco.GetComponent<Image>().sprite = marco500;
            minimapCamera.orthographicSize = 100;

        }
    }
}
