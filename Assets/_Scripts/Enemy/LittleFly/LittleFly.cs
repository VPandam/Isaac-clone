using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleFly : FlyingEnemy
{
   
   //Animator params
   const string HIT = "Hit";
   protected override void FixedUpdate()
   {
      base.FixedUpdate();
      if (isKnockback) return;
      Move();
   }

   void Move()
   {
      Vector2 playerDirection = player.transform.position - transform.position;
      _rb.MovePosition(_rb.position + playerDirection.normalized * speed * Time.fixedDeltaTime);
   }
   protected override IEnumerator BlinkColorDamage()
   {
      _animator.SetBool(HIT, true);
      yield return new WaitForSeconds(0.07f);
      _animator.SetBool(HIT, false);
   }
}
