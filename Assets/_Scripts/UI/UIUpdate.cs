using System.Collections.Generic;
using TMPro;
using UnityEngine;


    public class UIUpdate : MonoBehaviour
    {
        public GameObject heart, halfHeart, emptyHeart;
        public GameObject hpContainer;
        public GameObject bHeart, halfBHeart;
        [SerializeField] private TextMeshProUGUI bombsText, keysText, coinsText;

        [SerializeField] List<GameObject> hpList = new List<GameObject>();
        [SerializeField] List<GameObject> blueHpList = new List<GameObject>();
        PlayerManager playerManager;
        // Start is called before the first frame update
        void Start()
        {
            playerManager = PlayerManager.sharedInstance;
            UpdateUI();
            PlayerManager.sharedInstance.onUIChangeCallback += UpdateUI;
        }

        public void UpdateUI()
        {
            PlayerManager playerManager = PlayerManager.sharedInstance;
            if (heart && hpContainer && this.playerManager)
            {
                //Clear all hearts of the lists
                ClearLists();

                //Add a heart to the red health UI for each 2 health
                for (int x = 0; x < this.playerManager.currentHealth / 2; x++)
                {
                    GameObject heartAdd = Instantiate(heart, hpContainer.transform);
                    hpList.Add(heartAdd);
                }

                //If our current health is odd, add a half heart at the end of our red hp UI
                if (this.playerManager.currentHealth % 2 == 1)
                {
                    GameObject halfHeartAdd = Instantiate(halfHeart, hpContainer.transform);
                    hpList.Insert(0, halfHeartAdd);
      
                }
                
                if (playerManager.currentHealthContainers * 2 > playerManager.currentHealth)
                {
                    int emptyHeartAmount = ((playerManager.currentHealthContainers * 2) - playerManager.currentHealth) /2;
                    for (int i = 0; i < emptyHeartAmount; i++)
                    {
                        GameObject emptyHeartAdd = Instantiate(emptyHeart, hpContainer.transform);
                        hpList.Insert(0,emptyHeartAdd);
                    }
                }
                

                //Add a heart to the blue health UI for each 2 blue health
                for (int x = 0; x < this.playerManager.currentBlueHealth / 2; x++)
                {
                    GameObject bHeartAdd = Instantiate(bHeart, hpContainer.transform);
                    blueHpList.Add(bHeartAdd);
                }
                
                //If our current blue health is odd, add a half heart at the end of our blue hp UI
                if (this.playerManager.currentBlueHealth % 2 == 1)
                {
                    GameObject bHalfHeartAdd = Instantiate(halfBHeart, hpContainer.transform);
                    hpList.Insert(0, bHalfHeartAdd);
                }
            }

            bombsText.text = playerManager.currentBombs.ToString();
            keysText.text = playerManager.currentKeys.ToString();
            coinsText.text = playerManager.currentCoins.ToString();

        }
        void ClearLists()
        {
            foreach (GameObject heartList in hpList)
            {
                Destroy(heartList);
            }
            hpList.Clear();

            foreach (GameObject bHeartList in blueHpList)
            {
                Destroy(bHeartList);
            }
            blueHpList.Clear();
        }
    }


