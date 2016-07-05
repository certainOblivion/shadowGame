using UnityEngine;
using Grid;
using System.Collections.Generic;
using Helper;
using Priority_Queue;

public class NavMeshPath
{
    public RectangleMap NavMap { get; private set; }
    public HashSet<Hex> BlockedHex { get; private set; }
    public List<Rect> Obstacles { get;  private set; }
    public Dictionary<NavMeshActor, Vector2> ActorData {get;  private set;}

    private int HEX_NUM_SIDES = 6;
    private int UNBLOCKED_HEX_COST = 1;
    private int SPATIAL_HASH_CELL_SIZE = 5;

    private Dictionary<int, List<NavMeshObstacle>> SpacialBuckets;

    public NavMeshPath(float mapWidth, float mapHeight, float hexDiminishFactor, float worldToGridRatio, List<NavMeshObstacle> obstacles, List<NavMeshActor> actors)
    {
        InitHexGrid(mapWidth, mapHeight, hexDiminishFactor, worldToGridRatio, obstacles, actors);
        InitSpatialHashGrid(mapWidth, mapHeight);
    }

    void InitHexGrid(float mapWidth, float mapHeight, float hexDiminishFactor, float worldToGridRatio, List<NavMeshObstacle> obstacles, List<NavMeshActor> actors)
    {
        Obstacles = new List<Rect>();
        ActorData = new Dictionary<NavMeshActor, Vector2>();
        NavMap = new RectangleMap(mapWidth * hexDiminishFactor, mapHeight * hexDiminishFactor, hexDiminishFactor, worldToGridRatio);
        BlockedHex = new HashSet<Hex>();
        foreach (NavMeshObstacle obstacle in obstacles)
        {
            AddObstacle(obstacle);
        }

        foreach (NavMeshActor actor in actors)
        {
            AddActor(actor);
        }
    }

    void InitSpatialHashGrid(float mapWidth, float mapHeight)
    {
        int cols = (int)(mapWidth / SPATIAL_HASH_CELL_SIZE);
        int rows = (int)(mapHeight / SPATIAL_HASH_CELL_SIZE);
        SpacialBuckets = new Dictionary<int, List<NavMeshObstacle>> (cols * rows);

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                SpacialBuckets.Add((i* rows)+j, new List<NavMeshObstacle>());
            }
        }
    }



    public void AddObstacle(NavMeshObstacle obstacle)
    {
        Vector2 obstacleCenter = new Vector2(obstacle.gameObject.transform.position.x, obstacle.gameObject.transform.position.y);
        Vector3 obstacleSize = obstacle.gameObject.GetComponent<Renderer>().bounds.size;

        Rect obstacleRect = new Rect(obstacleCenter, obstacleSize);

        Obstacles.Add(obstacleRect);

        BlockedHex.UnionWith(GridHelper.BoxToHexList(obstacleCenter, obstacleSize, NavMap));
    }

    public void AddActor(NavMeshActor actor)
    {
        ActorData.Add(actor, actor.gameObject.GetComponent<Renderer>().bounds.size);
        actor.SetNavMesh(this);
    }

#if UNITY_EDITOR
    public LinkedList<Vector2> GetPath(Vector2 start, Vector2 destination,ref List<Hex> hexInPath, ref List<Hex> testedHex)
#else
    public List<Hex> GetPath(Vector2 start, Vector2 destination)
#endif
    {
        Hex startHex = GridHelper.Vector2ToHex(start, NavMap.MapLayout);
        Hex destinationHex = GridHelper.Vector2ToHex(destination, NavMap.MapLayout);

        Dictionary<Hex, Hex> came_from = new Dictionary<Hex, Hex>();
#if UNITY_EDITOR
        testedHex = new List<Hex>();
#endif
        if (!BlockedHex.Contains(destinationHex))
        {
            PriorityQueue<float, Hex> frontier = new PriorityQueue<float, Hex>();
            frontier.Enqueue(0, startHex);
            Dictionary<Hex, int> cost_so_far = new Dictionary<Hex, int>();

            came_from[startHex] = startHex;
            cost_so_far[startHex] = 0;
            while (!frontier.IsEmpty)
            {
                Hex current = frontier.Dequeue();
#if UNITY_EDITOR
                testedHex.Add(current);
#endif
                if (Hex.Equals(current, destinationHex))
                {
                    break;
                }

                for (int i = 0; i < HEX_NUM_SIDES; i++)
                {
                    Hex next = Hex.Neighbor(current, i);
                    if (BlockedHex.Contains(next))
                    {
                        continue;
                    }
                    int new_cost = cost_so_far[current] + UNBLOCKED_HEX_COST;

                    if ((!cost_so_far.ContainsKey(next)) || (new_cost < cost_so_far[next]))
                    {
                        cost_so_far[next] = new_cost;
                        float priority = new_cost + CalculateHeuristic(destinationHex, next);
                        frontier.Enqueue(priority, next);

                        came_from[next] = current;
                    }
                }
            } 
        }
#if UNITY_EDITOR
        return SmoothPath(came_from,startHex, destinationHex,ref hexInPath);
#else
        return SmoothPath(came_from,startHex, destinationHex);
#endif
    }
#if UNITY_EDITOR
    LinkedList<Vector2> SmoothPath( Dictionary<Hex, Hex> pathMap,Hex start, Hex destination, ref List<Hex> hexInPath)
#else
    LinkedList<Vector2> SmoothPath( Dictionary<Hex, Hex> pathMap,Hex start, Hex destination)
#endif
    {
        LinkedList<Vector2> path = new LinkedList<Vector2>();

#if UNITY_EDITOR
        hexInPath = new List<Hex>();
#endif

        Hex current = destination;
        Hex next = pathMap[current];
        Hex afterNext = pathMap[next];
        bool reachedStart = false;
        while (!afterNext.Equals(start))
        {
            while (IsVisible(current, afterNext) && !reachedStart)
            {
                next = afterNext;
                afterNext = pathMap[next];
                reachedStart = afterNext.Equals(start);
            }
            path.AddFirst(GridHelper.HexToVector2(current, NavMap.MapLayout));
#if UNITY_EDITOR
            hexInPath.Add(current);
#endif
            current = next;
        }

         return path;
    }

    float CalculateHeuristic(Hex a, Hex b)
    {
        return (GridHelper.AccurateDistance(a, b, NavMap.MapLayout) * 5)/* + (Hex.Distance(a, b) *.01f)*/;
    }

    bool IsVisible(Hex observer, Hex destination)
    {
        List<Hex> hexesInBetween = FractionalHex.HexLinedraw(observer, destination);

        foreach (Hex hex in hexesInBetween)
        {
            if (BlockedHex.Contains(hex))
            {
                return false;
            }
        }

        return true;
    }


}
