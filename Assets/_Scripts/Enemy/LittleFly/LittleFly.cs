using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleFly : FlyingEnemy
{
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
}
