using UnityEngine;
using System.Collections.Generic;
using Grid;
using Helper;

public class NavMesh : MonoBehaviour {

    public float HexScale;
    public float WorldToGridRatio;
    private MeshRenderer mFloorMesh;
    private List<NavMeshObstacle> mNavObstacles;
    private List<NavMeshActor> mNavMeshActors;
    NavMeshPath mNavMesh;
    List<Hex> path;
    // Use this for initialization
    void Start () {

        mFloorMesh = GetComponent<MeshRenderer>();
        mNavObstacles = new List<NavMeshObstacle>(FindObjectsOfType<NavMeshObstacle>());
        mNavMeshActors = new List<NavMeshActor>(FindObjectsOfType<NavMeshActor>());
        mNavMesh = new NavMeshPath(mFloorMesh.bounds.size.x * HexScale, mFloorMesh.bounds.size.y * HexScale, HexScale, WorldToGridRatio, mNavObstacles, mNavMeshActors);
        path = new List<Hex>();
    }

    // Update is called once per frame
    void Update ()
    {
        if (mNavMeshActors.Count == 0)
        {
            return;
        }
        if (Input.GetMouseButton(0))
        {
            path = mNavMesh.GetPath(mNavMeshActors[0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            mNavMeshActors[0].SetPath(path);          
        }

        foreach (Hex hex in path)
        {
            GridHelper.DrawHex(mNavMesh.NavMap.MapLayout, hex, Color.red);
        }

    }


}
