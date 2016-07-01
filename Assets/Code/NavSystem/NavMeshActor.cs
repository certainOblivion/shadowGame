using UnityEngine;
using System.Collections.Generic;
using Grid;
using Helper;

public class NavMeshActor : MonoBehaviour
{
    public float Velocity = 20;
    List<Hex> Path;
    NavMeshPath mNavMesh;
    Vector2 Position;
    // Use this for initialization
    void Start()
    {
        Path = null;
        Position = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position;
        if (Path != null && Path.Count > 0)
        {
            Vector2 direction = GridHelper.HexToVector2(Path[Path.Count - 1], mNavMesh.NavMap.MapLayout) - Position;
            direction.Normalize();

            Position += direction * Velocity * Time.deltaTime;

            if (GridHelper.Vector2ToHex(Position, mNavMesh.NavMap.MapLayout).Equals(Path[Path.Count - 1]))
            {
                Path.RemoveAt(Path.Count - 1);
            }
        }

        transform.position = Position;
    }

    public void SetPath(List<Hex> path)
    {
        Path = path;
    }

    public void SetNavMesh(NavMeshPath navMesh)
    {
        mNavMesh = navMesh;
    }
}
