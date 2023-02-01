using UnityEngine;


    public class Sword : Item
    {
        [SerializeField] private GameObject tearPrefab;
        [SerializeField]private int attackIncrease;

        override public void DoEffect()
        {
            base.DoEffect();
            playerManager.attackDamage += attackIncrease;
            playerManager.currentTear = tearPrefab;
        }
    }


