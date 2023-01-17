using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadAnimationEvent : MonoBehaviour
{
    private Animator headAnimator;

    private void Start()
    {
        headAnimator = GetComponent<Animator>();
    }
    
    public void OnEndShootRight()
    {
        headAnimator.SetBool("ShootingRight", false);
    }
    public void OnEndShootLeft()
    {
        headAnimator.SetBool("ShootingLeft", false);
    }
    public void OnEndShootUp()
    {
        headAnimator.SetBool("ShootingUp", false);
    }
    public void OnEndShootDown()
    {
        headAnimator.SetBool("ShootingDown", false);
    }
}
