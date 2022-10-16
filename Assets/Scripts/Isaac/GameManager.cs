using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager _instance;
    public GameObject player;

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
        Instantiate(player);

        blackScreenCG = blackScreenGO.GetComponent<CanvasGroup>();

    }
    private void Start()
    {
        Application.targetFrameRate = 120;
    }
    IEnumerator FadeInFadeOut()
    {

        pause = true;

        while (blackScreenCG.alpha < 1)
        {
            blackScreenCG.alpha += Time.deltaTime / 0.2f;
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        while (blackScreenCG.alpha > 0)
        {
            blackScreenCG.alpha -= Time.deltaTime / 0.2f;
            yield return null;
        }
        yield return new WaitForSeconds(0.1f);
        Debug.Log(pause);
        pause = false;



    }
}