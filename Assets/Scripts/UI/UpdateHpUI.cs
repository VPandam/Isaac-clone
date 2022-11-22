using System.Collections.Generic;
using UnityEngine;


    public class UpdateHpUI : MonoBehaviour
    {
        public GameObject heart, halfHeart;
        public GameObject hpContainer;
        public GameObject bHeart, halfBHeart;

        [SerializeField] List<GameObject> hpList = new List<GameObject>();
        [SerializeField] List<GameObject> blueHpList = new List<GameObject>();
        PlayerManager playerStats;
        // Start is called before the first frame update
        void Start()
        {
            playerStats = PlayerManager.sharedInstance;
            UpdateHPUI(0);
            PlayerManager.sharedInstance.OnHpChangeCallback += UpdateHPUI;
        }

        public void UpdateHPUI(int hpChange)
        {
            if (heart && hpContainer && playerStats)
            {
                //Clear all hearts of the lists
                ClearLists();

                for (int x = 0; x < playerStats.currentHealth / 2; x++)
                {
                    GameObject heartAdd = Instantiate(heart, hpContainer.transform);
                    hpList.Add(heartAdd);
                }

                if (playerStats.currentHealth % 2 == 1)
                {
                    GameObject halfHeartAdd = Instantiate(halfHeart, hpContainer.transform);
                    hpList.Insert(0, halfHeartAdd);
                }

                for (int x = 0; x < playerStats.currentBlueHealth / 2; x++)
                {
                    GameObject bHeartAdd = Instantiate(bHeart, hpContainer.transform);
                    blueHpList.Add(bHeartAdd);
                }
                if (playerStats.currentBlueHealth % 2 == 1)
                {
                    GameObject bHalfHeartAdd = Instantiate(halfBHeart, hpContainer.transform);
                    hpList.Insert(0, bHalfHeartAdd);
                }

            }
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


