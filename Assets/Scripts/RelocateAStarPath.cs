using Pathfinding;
using UnityEngine;

public class RelocateAStarPath : MonoBehaviour
{
        
    public static RelocateAStarPath instance;
    GameObject _aStar;

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    public void Relocate(Vector2 positionToMove)
    {
        // This holds all graph data
        AstarData data = AstarPath.active.data;
        data.gridGraph.center = positionToMove;
        data.gridGraph.Scan();
    }
}