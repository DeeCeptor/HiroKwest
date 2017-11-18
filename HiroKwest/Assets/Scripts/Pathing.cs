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


    public static List<Tile> OrthographicAStarPath(Tile start, Tile goal)
    {
        StaticSettings.ResetAllPathingInfo();

        Debug.Log("Pathing from " + start.transform.position + " to " + goal.transform.position);

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
            if (current == null)
                Debug.Log("Current is null " + openSet.Count);

            if (current == goal)
                return ReconstructPath(current);

            openSet.Remove(current);
            closedSet.Add(current);

            foreach (Tile neighbour in current.orthogonal_neighbours)
            {
                if (closedSet.Contains(neighbour))
                    continue;

                // Check if this is a new adjacent valid neighbour
                if (!openSet.Contains(neighbour))
                    openSet.Add(neighbour);

                float tentative_g_score = current.gScore + 1f; // Vector2.Distance(current.transform.position, neighbour.transform.position);
                Debug.Log(tentative_g_score + " " + current.gScore + " neighbour: " + neighbour.gScore);
                Debug.Log("Current " + current.transform.position + " neighbour " + neighbour.transform.position);

                if (tentative_g_score >= neighbour.gScore)
                    // Current path is not the best path, leave it for now
                    continue;

                // This path is the best until now. Record it!
                Debug.Log("best path " + neighbour.transform.position);
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

    public static List<Tile> ReconstructPath(Tile current)
    {
        List<Tile> total_path = new List<Tile>();
        total_path.Add(current);

        Debug.Log(current.cameFrom);
        while (current.cameFrom != null)
        {
            Debug.Log(current.transform.position);
            current = current.cameFrom;
            total_path.Add(current);
        }
        total_path.Reverse();
        return total_path;
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
