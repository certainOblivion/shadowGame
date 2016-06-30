using UnityEngine;
using System.Collections.Generic;
using Grid;
using Helper;

public class NavMesh : MonoBehaviour {

    public float HexScale;
    public float WorldToGridRatio;
    public bool EnableDebugDraw;
    private RectangleMap mNavMap;
    private MeshRenderer mFloorMesh;
    private List<NavMeshObstacle> mNavObstacles;
    private List<NavMeshActor> mNavMeshActors;
    // Use this for initialization
    void Start () {

        mFloorMesh = GetComponent<MeshRenderer>();

        mNavMap = new RectangleMap((int)mFloorMesh.bounds.size.y * HexScale , (int)mFloorMesh.bounds.size.x * HexScale, HexScale, WorldToGridRatio);

        mNavObstacles = new List<NavMeshObstacle>(FindObjectsOfType<NavMeshObstacle>());
        mNavMeshActors = new List<NavMeshActor>(FindObjectsOfType<NavMeshActor>());
    }

    // Update is called once per frame
    void Update () {

        if (EnableDebugDraw)
        {
            float quadHeight = Camera.main.orthographicSize * 2.0f;
            float quadWidth = quadHeight * Screen.width / Screen.height;

            DebugDrawHelper.DrawRectangle(mFloorMesh.transform.position, quadWidth, quadHeight, color:Color.red);

            foreach (Hex hex in mNavMap.Map)
            {
                GridHelper.DrawHex(mNavMap.MapLayout, hex, Color.yellow);
            }
            foreach (NavMeshObstacle obstacle in mNavObstacles)
            {
                Vector2 obstacleCenter = new Vector2(obstacle.gameObject.transform.position.x, obstacle.gameObject.transform.position.y);
                Vector3 obstacleVolume = obstacle.gameObject.GetComponent<Renderer>().bounds.size;
                Vector2 obstacleVolume2D = new Vector2(obstacleVolume.x, obstacleVolume.y);
                foreach (Hex hex in GridHelper.BoxToHexList(obstacleCenter, obstacleVolume2D, mNavMap))
                {
                    GridHelper.DrawHex(mNavMap.MapLayout, hex, Color.blue, true);
                }
            }
            foreach (NavMeshActor actor in mNavMeshActors)
            {
                Point position = GridHelper.Vector3ToPoint(actor.transform.position);
                
                Hex occupiedHex = FractionalHex.HexRound(Layout.PixelToHex(mNavMap.MapLayout, position));

                GridHelper.DrawHex(mNavMap.MapLayout, occupiedHex, Color.green, true);
            }
        }

        
    }


}
