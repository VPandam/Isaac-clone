using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public GameObject player;
    private GameObject playerInstance;

    CanvasGroup blackScreenCG;
    [SerializeField] GameObject blackScreenGO;

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
    

    public void Pause()
    {
        Debug.Log("Pause");
        pause = true;
    }

    public void Resume()
    {
        pause = false;
    }

    public void LoadNewLevel()
    {
        StartCoroutine(nameof(FadeIn), 2f );
        RoomsController._instance.LoadNewLevel();
        Invoke(nameof(ChangeRoom), 1);
    }

    void ChangeRoom()
    {
        playerInstance.GetComponent<PlayerController>().ChangeRoom(PlayerManager.sharedInstance.currentRoom);
    }
    
}