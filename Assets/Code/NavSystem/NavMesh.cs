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
    LinkedList<Vector2> Path;
#if UNITY_EDITOR
    List<Hex> TestedHex;
    List<Hex> HexInPath;
#endif
    // Use this for initialization
    void Start () {

        mFloorMesh = GetComponent<MeshRenderer>();
        mNavObstacles = new List<NavMeshObstacle>(FindObjectsOfType<NavMeshObstacle>());
        mNavMeshActors = new List<NavMeshActor>(FindObjectsOfType<NavMeshActor>());
        mNavMesh = new NavMeshPath(mFloorMesh.bounds.size.x * HexScale, mFloorMesh.bounds.size.y * HexScale, HexScale, WorldToGridRatio, mNavObstacles, mNavMeshActors);
        Path = new LinkedList<Vector2>();

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
#if UNITY_EDITOR
            Path = mNavMesh.GetPath(mNavMeshActors[0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), ref HexInPath, ref TestedHex);
#else
            Path = mNavMesh.GetPath(mNavMeshActors[0].transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
#endif
            mNavMeshActors[0].SetPath(Path);          
        }

#if UNITY_EDITOR
        if (TestedHex != null)
        {
            foreach (Hex hex in TestedHex)
            {
                GridHelper.DrawHex(mNavMesh.NavMap.MapLayout, hex, Color.blue);
            } 
        }

        if (HexInPath != null)
        {
            foreach (Hex hex in HexInPath)
            {
                GridHelper.DrawHex(mNavMesh.NavMap.MapLayout, hex, Color.red);
            } 
        }
#endif
    }
}
