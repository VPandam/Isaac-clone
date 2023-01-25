using System.Collections;
using Pathfinding;
using UnityEngine;

public class Chaser : Enemy
{
    AIDestinationSetter _destinationSetter;
    [SerializeField]private LayerMask chaserLayer;
    private bool waiting;
    float separateRadius = .5f;
    protected override void Start()
    {
        base.Start();
        _destinationSetter = GetComponent<AIDestinationSetter>();
    }
    
    protected override void FixedUpdate()
    {
        base.FixedUpdate();
        AvoidChasersOnSamePosition();
        switch (_currentState)
        {
            case EnemyState.follow:
                _destinationSetter.target = player.transform;
                break;
        }
    }
    
    //Detect other chaser enemies and stop moving if they are to e to us.
    //This is used to avoid having too much enemies in the same position.
    void AvoidChasersOnSamePosition()
    {
        if(!stopped || !waiting){
        
            // Overlap circle to detect other chasers
            var chasers = Physics2D.OverlapCircleAll(transform.position, separateRadius, chaserLayer);
        
            foreach (var chaser in chasers)
            {
                Chaser chaserOverlapped = chaser.GetComponent<Chaser>();
                //Make sure it is a fellow enemy
                //Check if the other enemies are stopped or waiting, when there is a chaser close to us we only want one to stop moving.
                if (chaserOverlapped != null && chaser.transform != transform && !chaserOverlapped.stopped && !chaserOverlapped.waiting)
                {
                    Debug.Log("stopped");
                    stopped = true;
                    //Stops for .1 seconds and keeps moving if there are no other enemies close.
                    StartCoroutine(WaitForSeconds(.1f));
                }
                else if(!waiting)
                {
                    stopped = false;
                }
            }
        }
    }
    IEnumerator WaitForSeconds(float seconds)
    {
        waiting = true;
        yield return new WaitForSeconds(seconds);
        waiting = false;
    }
    
}
        






