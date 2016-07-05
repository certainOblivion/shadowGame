using UnityEngine;
using System.Collections.Generic;
using Grid;
using Helper;

public class NavMeshActor : MonoBehaviour
{
    public float Velocity = 20;
    LinkedList<Vector2> Path;
    NavMeshPath mNavMesh;
    Vector2 Position;
    Vector2 Direction;
    // Use this for initialization
    void Start()
    {
        Path = null;
        Position = transform.position;
        Direction = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        Position = transform.position;
        if (Path != null && Path.Count > 0)
        {
            Vector2 subDestination = Path.First.Value;
            if (Direction == Vector2.zero)
            {
                Direction = subDestination - Position;
                Direction.Normalize(); 
            }
            Vector2 moveDelta = Direction * Velocity * Time.deltaTime;
            if (moveDelta.SqrMagnitude() > (subDestination - Position).SqrMagnitude())
            {
                Position = subDestination;
                Direction = Vector2.zero;
                Path.RemoveFirst();
            }
            else
            {
                Position += moveDelta;
            }
        }

        transform.position = Position;
    }

    public void SetPath(LinkedList<Vector2> path)
    {
        Path = path;
    }

    public void SetNavMesh(NavMeshPath navMesh)
    {
        mNavMesh = navMesh;
    }
}
