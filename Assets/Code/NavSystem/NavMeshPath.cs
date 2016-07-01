using UnityEngine;
using Grid;
using System.Collections.Generic;
using Helper;
using Priority_Queue;

public class NavMeshPath
{
    public RectangleMap NavMap { get; private set; }
    public HashSet<Hex> BlockedHex { get; private set; }

    private int MAX_PATH_DISTANCE = 10000;
    private int HEX_NUM_SIDES = 6;
    private int BLOCKED_HEX_COST = 100;
    private int UNBLOCKED_HEX_COST = 1;

    public NavMeshPath(float mapWidth, float mapHeight, float hexDiminishFactor, float worldToGridRatio, List<NavMeshObstacle> obstacles, List<NavMeshActor> actors)
    {
        Init(mapWidth, mapHeight, hexDiminishFactor, worldToGridRatio, obstacles, actors);
    }

    public void Init(float mapWidth, float mapHeight, float hexDiminishFactor, float worldToGridRatio, List<NavMeshObstacle> obstacles, List<NavMeshActor> actors)
    {
        NavMap = new RectangleMap(mapWidth * hexDiminishFactor, mapHeight * hexDiminishFactor, hexDiminishFactor, worldToGridRatio);
        BlockedHex = new HashSet<Hex>();
        foreach (NavMeshObstacle obstacle in obstacles)
        {
            AddObstacles(obstacle);
        }

        foreach (NavMeshActor actor in actors)
        {
            AddActor(actor);
        }
    }

    public void AddObstacles(NavMeshObstacle obstacle)
    {
        Vector2 obstacleCenter = new Vector2(obstacle.gameObject.transform.position.x, obstacle.gameObject.transform.position.y);
        Vector3 obstacleVolume = obstacle.gameObject.GetComponent<Renderer>().bounds.size;

        BlockedHex.UnionWith(GridHelper.BoxToHexList(obstacleCenter, obstacleVolume, NavMap));
    }

    public void AddActor(NavMeshActor actor)
    {
        actor.SetNavMesh(this);
    }

    public List<Hex> GetPath(Vector2 start, Vector2 destination)
    {
        List<Hex> hexPath = new List<Hex>();

        Hex startHex = GridHelper.Vector2ToHex(start, NavMap.MapLayout);
        Hex destinationHex = GridHelper.Vector2ToHex(destination, NavMap.MapLayout);
        HexNode startNode = new HexNode(startHex);
        PriorityQueue<HexNode> frontier = new PriorityQueue<HexNode>(MAX_PATH_DISTANCE);
       
        frontier.Enqueue(startNode, 0);
        Dictionary<Hex, Hex> came_from = new Dictionary<Hex, Hex>();
        Dictionary<Hex, int> cost_so_far = new Dictionary<Hex, int>();

        came_from[startHex] = startHex;
        cost_so_far[startHex] = 0;
        bool pathFound = false;
        while (frontier.Count != 0)
        {
            Hex current = frontier.Dequeue().ContainedHex;

            if (Hex.Equals(current, destinationHex))
            {
                pathFound = true;
                break;
            }

            for (int i = 0; i < HEX_NUM_SIDES; i++)
            {
                Hex next = Hex.Neighbor(current, i);

                int new_cost = cost_so_far[current] + (BlockedHex.Contains(next) ? BLOCKED_HEX_COST : UNBLOCKED_HEX_COST);

                if ((!cost_so_far.ContainsKey(next)) || (new_cost < cost_so_far[next]))
                {
                    HexNode nextNode = new HexNode(next);
                    cost_so_far[next] = new_cost;
                    int priority = new_cost + Hex.Distance(destinationHex, next);
                    frontier.Enqueue(nextNode, priority);

                    came_from[next] = current;
                }
            }
        }

        if (pathFound)
        {
            Hex current = destinationHex;
            while (!Hex.Equals(current , startHex))
            {
                hexPath.Add(current);
                current = came_from[current];
            }
        }

        return hexPath;
    }


}
