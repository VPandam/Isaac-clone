using System.Collections;
using Pathfinding;
using UnityEngine;

public class RelocateAStarPath : MonoBehaviour
{
        
    public static RelocateAStarPath instance;
    GameObject _aStar;
    private AstarData data;
    private bool scanning;

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
        // This holds all graph data
        data = AstarPath.active.data;
    }
    public void Relocate(Vector2 positionToMove)
    {
        data.gridGraph.center = positionToMove;
        data.gridGraph.Scan();
    }

    public IEnumerator GridScan()
    {
        if (!scanning)
        {
            scanning = true;
            yield return new WaitForEndOfFrame();
            Debug.Log("Scan");
            data.gridGraph.Scan();
            scanning = false;
        }
    }
}