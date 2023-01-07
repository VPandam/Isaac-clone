using UnityEngine;


    public class Sword : Item
    {
        [SerializeField] private GameObject tearPrefab;
        

        override public void DoEffect()
        {
            base.DoEffect();
            playerManager.attackDamage += 2;
            playerManager.currentTear = tearPrefab;
        }
    }


