using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Chest : MonoBehaviour
{
    //Animation trigger name
    const string TRIGGER_CHEST = "OpenChest";

    protected List<GameObject> chestItemsList;

    private Animator _animator;
    private bool isOpened;


    public virtual void Start()
    {
        _animator = GetComponent<Animator>();
        chestItemsList = ItemManager.instance.chestLoot;
    }

    public virtual void OpenChest()
    {
        float lootDistance = 1.2f;
        _animator.SetTrigger(TRIGGER_CHEST);

        //We can get a random amount of items between 1 and 3.s
        int lootAmount = 4; Random.Range(1, 4);

        for (int i = 0; i < lootAmount; i++)
        { 
            int randomIndex = Random.Range(0, chestItemsList.Count);
            var randomPosition = (Vector2)transform.position + (Resources.sharedInstance.RandomVector2() * lootDistance);
            
            //Since the loot will move in parabola, we dont want our loot to end close to 1 or -1 x.
            if (randomPosition.x < 0.3 && randomPosition.x > -0.3)
            {
                if (randomPosition.x >= 0) randomPosition.x = 0.3f;
                else randomPosition.x = -0.3f;
            }

            var loot = Instantiate(chestItemsList[randomIndex], transform.position, 
                Quaternion.identity);
            StartCoroutine(MoveChestLoot(loot, randomPosition));
        }
        isOpened = true;
    }
      IEnumerator MoveChestLoot(GameObject loot, Vector2 end)
      {
          float animation = 0;
          float animationTime = .8f;
          var lootCollider = loot.GetComponent<Collider2D>();
          lootCollider.enabled = false;
          Debug.Log("Transform position : " + transform.position + " RandomPosition = " + end);

          while (Vector3.Distance(loot.transform.position, end) >= .1f)// object has reached goal } )
          { 
             animation += Time.deltaTime;
             // animation = animation % animationTime;
             loot.transform.position = Resources.sharedInstance.Parabola(transform.position, end, 1.5f, animation / animationTime);
            yield return null;
          }
          lootCollider.enabled = true;
      }
   
    private void OnCollisionEnter2D(Collision2D col)
    {
        Debug.Log("ChestCollision");
        if(col.gameObject.CompareTag("Player") && !isOpened) OpenChest();
    }
}
