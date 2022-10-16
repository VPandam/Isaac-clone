using UnityEngine;

// public interface Iitem
// {
//     public void DoEffect();
// }
public class Item : MonoBehaviour
{
    public string effectDescription;
    public string _name;
    public Sprite _sprite;
    public readonly Vector2 itemSize = new Vector2(0.5f, 0.5f);
    [HideInInspector] public int id;


    public virtual void DoEffect()
    {
        Debug.Log(effectDescription);
    }

}
