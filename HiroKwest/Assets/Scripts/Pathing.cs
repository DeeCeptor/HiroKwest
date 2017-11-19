using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathing : MonoBehaviour
{

	void Start ()
    {
		
	}


    void Update ()
    {
		
	}


    public static Dictionary<Tile, int> BoundedDijkstra(Tile start, int max_distance)
    {
        StaticSettings.ResetAllPathingInfo();

        Dictionary<Tile, int> tiles_we_can_get_to = new Dictionary<Tile, int>();

        // Already evaluated nodes
        List<Tile> closedSet = new List<Tile>();
        // Currently discovered (adjacent) nodes that are not evaluated yet
        // Starts off with start node in it
        List<Tile> openSet = new List<Tile>();
        openSet.Add(start);

        start.fScore = 0;
        start.gScore = 0;

        // Keep evaluating until openSet is empty
        while (openSet.Count > 0)
        {
            // Set current to Tile in openSet with lowest fScore
            Tile current = GetLowestFScore(openSet);

            openSet.Remove(current);
            closedSet.Add(current);

            if (current.fScore >= max_distance)
                continue;

            AddToDictionaryIfNotOccupied(current, tiles_we_can_get_to);

            foreach (Tile neighbour in current.orthogonal_neighbours)
            {
                if (closedSet.Contains(neighbour))
                    continue;

                // Check if this is a new adjacent valid neighbour
                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);

                // Each square is exactly 1 distance away from eachother. This is fine unless we add terrain that affects movement
                float tentative_f_score = current.fScore + 1f; // Vector2.Distance(current.transform.position, neighbour.transform.position);

                if (tentative_f_score >= neighbour.fScore)
                    // Current path is not the best path, leave it for now
                    continue;

                // This path is the best until now. Record it!
                neighbour.cameFrom = current;
                neighbour.fScore = tentative_f_score;

                AddToDictionaryIfNotOccupied(neighbour, tiles_we_can_get_to);
            }
        }

        return tiles_we_can_get_to;
    }
    public static void AddToDictionaryIfNotOccupied(Tile t, Dictionary<Tile, int> dict)
    {
        if (t.tile_occupied)
            return;

        if (dict.ContainsKey(t))
        {
            dict.Remove(t);
        }
        dict.Add(t, (int)t.fScore);
    }


    public static Queue<Tile> OrthographicAStarPath(Tile start, Tile goal, int max_length = 99999)
    {
        StaticSettings.ResetAllPathingInfo();

        // Already evaluated nodes
        List<Tile> closedSet = new List<Tile>();
        // Currently discovered (adjacent) nodes that are not evaluated yet
        // Starts off with start node in it
        List<Tile> openSet = new List<Tile>();
        openSet.Add(start);

        start.fScore = HeuristicCostEstimate(start, goal);
        start.gScore = 0;

        // Keep evaluating until openSet is empty
        while (openSet.Count > 0)
        {
            // Set current to Tile in openSet with lowest fScore
            Tile current = GetLowestFScore(openSet);

            if (current == goal)
                return ReconstructPath(current, max_length);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbour in current.orthogonal_neighbours)
            {
                if (closedSet.Contains(neighbour))
                    continue;

                // Check if this is a new adjacent valid neighbour
                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);

                // Each square is exactly 1 distance away from eachother. This is fine unless we add terrain that affects movement
                float tentative_g_score = current.gScore + 1f; // Vector2.Distance(current.transform.position, neighbour.transform.position);

                if (tentative_g_score >= neighbour.gScore)
                    // Current path is not the best path, leave it for now
                    continue;

                // This path is the best until now. Record it!
                neighbour.cameFrom = current;
                neighbour.gScore = tentative_g_score;
                neighbour.fScore = neighbour.gScore + HeuristicCostEstimate(neighbour, goal);
            }
        }

        return null;
    }
    public static float HeuristicCostEstimate(Tile start, Tile goal)
    {
        return StaticSettings.OrthographicDistance(start, goal);
    }

    public static Queue<Tile> ReconstructPath(Tile current, int max_length = 9999)
    {
        List<Tile> total_path = new List<Tile>();
        total_path.Add(current);

        while (current.cameFrom != null)
        {
            current = current.cameFrom;
            total_path.Add(current);
        }

        total_path.Reverse();
        
        // Chop off any steps after max length
        if (total_path.Count > max_length)
            total_path.RemoveRange(max_length + 1, total_path.Count - (max_length + 1));
            
        return new Queue<Tile>(total_path);
    }

    public static int DistanceBetween(Tile from, Tile to)
    {
        Queue<Tile> path = OrthographicAStarPath(from, to);
        if (path == null)
        {
            Debug.LogError("No possible path from " + from.transform.position + " to " + to.transform.position, to.transform);
            return StaticSettings.ERR_NO_POSSIBLE_PATH;
        }
        else
            return path.Count;
    }


    public static Tile GetLowestFScore(List<Tile> from_list)
    {
        float lowest_fScore = 999f;
        Tile lowest_scoring_tile = null;
        foreach (Tile t in from_list)
        {
            if (t.fScore < lowest_fScore || lowest_scoring_tile == null)
            {
                lowest_fScore = t.fScore;
                lowest_scoring_tile = t;
            }
        }

        if (lowest_scoring_tile == null)
            Debug.LogError("No tiles in list passed into GetLowestFScore " + from_list.Count);
        return lowest_scoring_tile;
    }
}
