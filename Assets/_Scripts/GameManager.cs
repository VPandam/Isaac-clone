using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public GameObject player;
    private GameObject playerInstance;

    [SerializeField] Transform lootGO;

    CanvasGroup blackScreenCG;
    [SerializeField] private GameObject blackScreenGO, pausePanel, firstSelectedButton;

    public bool pause;
    private void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        else
        {
            Destroy(this);
        }
        // RoomsController.instance.InstantiateAllRooms();
        // RoomsController.instance.ConenctDoors();
        playerInstance = Instantiate(player);
        blackScreenCG = blackScreenGO.GetComponent<CanvasGroup>();

    }
    private void Start()
    {
        Time.timeScale = 1;
        Application.targetFrameRate = 120;
        RoomsController._instance.CreateRooms();
    }
    IEnumerator FadeInFadeOut()
    {
        pause = true;
        blackScreenCG.alpha = 1;
        yield return new WaitForSeconds(0.2f);
        
        while (blackScreenCG.alpha > 0)
        {
            blackScreenCG.alpha -= Time.deltaTime / 0.2f;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        pause = false;
    }
    IEnumerator FadeIn(float time)
    {
        pause = true;
        blackScreenCG.alpha = 0;
        while (blackScreenCG.alpha < 1)
        {
            blackScreenCG.alpha += Time.deltaTime / 0.2f;
            yield return null;
        }
        
        yield return new WaitForSeconds(time);

        while (blackScreenCG.alpha > 0)
        {
            blackScreenCG.alpha -= Time.deltaTime / 0.2f;
            yield return null;
        }
        
        pause = false;
    }
    

    public void PauseMenu()
    {
        pause = true;
        pausePanel.SetActive(true);
        EventSystem eventSystem = EventSystem.current;
        eventSystem.SetSelectedGameObject(firstSelectedButton);
        Time.timeScale = 0;
    } 
    public void PauseTime()
    {
        pause = true;
        Time.timeScale = 0;
    }

    public void Resume()
    {
        pause = false;
        Time.timeScale = 1;
        pausePanel.SetActive(false);
    }

    public void Die()
    {
        SceneManagerScript sceneManager = GetComponent<SceneManagerScript>();
        sceneManager.LoadMainMenuScene();
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void LoadNewLevel()
    {
        ClearLoot();
        StartCoroutine(nameof(FadeIn), 2f );
        RoomsController._instance.LoadNewLevel();
        Invoke(nameof(ChangeRoom), 1);
    }

    void ClearLoot()
    {
        foreach (Transform trans in lootGO)
        {
            Destroy(trans.gameObject);
        }
    }

    void ChangeRoom()
    {
        playerInstance.GetComponent<PlayerController>().ChangeRoom(PlayerManager.sharedInstance.currentRoom);
    }
    
}