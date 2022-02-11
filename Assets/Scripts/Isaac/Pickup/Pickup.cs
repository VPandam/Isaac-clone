using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : ScriptableObject
{
    public string _name;
    public Sprite _sprite;


    public virtual void DoEffect()
    {
        Debug.Log("SO Effect");
    }
}
