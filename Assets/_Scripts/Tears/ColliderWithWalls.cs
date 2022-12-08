using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderWithWalls : MonoBehaviour
{
    private Tear _tear;

    private void Start()
    {
        _tear = GetComponentInParent<Tear>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Wall") || col.CompareTag("Door"))
        {
            if (_tear.tearCollidesSound)
                _tear.playerAudioSource.PlayOneShot(_tear.tearCollidesSound, 0.2f);
            Destroy(_tear.gameObject);
        }
    }
}
