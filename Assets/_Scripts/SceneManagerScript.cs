using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    [SerializeField] private GameObject defaultButton;
    private bool firstVerticalInputIsBeenMade;
    public InputActions controls;

    private void Awake()
    {
        controls = new InputActions();
        controls.Enable();
        
        //Set the navigation to the default start button
        controls.Player.Move.performed += (ctxt) =>
        { 
            float yValue = ctxt.ReadValue<Vector2>().y;
            if ((yValue > .2 || yValue < -.2) && !firstVerticalInputIsBeenMade)
            {
                EventSystem.current.SetSelectedGameObject(defaultButton);
                firstVerticalInputIsBeenMade = true;
            }
        };
    }

    private void Update()
    {
        
    }

    public void LoadGameScene()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);

    }
    
    public void ExitGame()
    {
        Application.Quit();
    }
}
