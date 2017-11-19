using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StaticSettings
{
    public const int ERR_NO_POSSIBLE_PATH = 999;

    public static Color transparent_blue = new Color(0, 0, 1, 0.2f);
    public static Color transparent_yellow = new Color(1f, 0.9f, 0, 0.2f);
    public static Color transparent = new Color(0, 0, 0, 0);


    public static Vector2 TopLeft = new Vector2(-1, 1);
    public static Vector2 TopRight = new Vector2(1, 1);
    public static Vector2 BottomLeft = new Vector2(-1, -1);
    public static Vector2 BottomRight = new Vector2(1, -1);

    public static List<Tile> all_Tiles = new List<Tile>();
    public static Dictionary<Vector2, Tile> tile_coordinates = new Dictionary<Vector2, Tile>();

    public static float OrthographicDistance(Tile from, Tile to)
    {
        return Mathf.Abs(from.transform.position.x - from.transform.position.x) +
            Mathf.Abs(from.transform.position.y - from.transform.position.y);
    }

    public static Tile GetTile(int x, int y)
    {
        return GetTile(new Vector2(x, y));
    }
    public static Tile GetTile(Vector2 position)
    {
        return tile_coordinates[position];
    }
    public static Tile GetNearestTile(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        if (ValidTilePosition(position))
            return GetTile(position);
        else
            return null;
    }
    public static bool ValidTilePosition(Vector2 position)
    {
        position.x = Mathf.Round(position.x);
        position.y = Mathf.Round(position.y);

        return tile_coordinates.ContainsKey(position);
    }


    public static void ReconstructFullGraph()
    {
        foreach (Tile t in all_Tiles)
        {
            t.RaycastNeighbours();
        }
    }


    public static void UnhighlightAllTiles()
    {
        foreach (Tile t in all_Tiles)
        {
            t.UnhighlightThisTile();
        }
    }


    public static void ResetAllPathingInfo()
    {
        foreach(Tile t in all_Tiles)
        {
            t.ResetPathingInfo();
        }
    }



    public static List<Unit> GetClosestUnitsTo(List<Unit> units, Tile to)
    {
        int closest_distance = 999;
        List<Unit> closest_unit = null;
        foreach (Unit u in units)
        {
            int dist = Pathing.DistanceBetween(u.destination_tile, to);

            if (dist < closest_distance && dist != StaticSettings.ERR_NO_POSSIBLE_PATH)
            {
                closest_distance = dist;
                closest_unit = new List<Unit> { u } ;
            }
            else if (dist == closest_distance && dist != StaticSettings.ERR_NO_POSSIBLE_PATH)
            {
                closest_unit.Add(u);
            }
        }
        return closest_unit;
    }
    // Weakest as in lowest health
    public static Unit GetClosestWeakestUnitTo(List<Unit> units, Tile to)
    {
        int closest_distance = 999;
        int lowest_HP = 999;
        Unit closest_unit = null;
        foreach (Unit u in units)
        {
            int dist = Pathing.DistanceBetween(u.destination_tile, to);

            if (dist <= closest_distance && u.Health <= lowest_HP && dist != StaticSettings.ERR_NO_POSSIBLE_PATH)
            {
                closest_distance = dist;
                closest_unit = u;
            }
        }
        return closest_unit;
    }
    // Strongest as in most Health
    public static Unit GetClosestStrongestUnitTo(List<Unit> units, Tile to)
    {
        int closest_distance = 999;
        int highest_HP = 0;
        Unit closest_unit = null;
        foreach (Unit u in units)
        {
            int dist = Pathing.DistanceBetween(u.destination_tile, to);

            if (dist <= closest_distance && u.Health >= highest_HP && dist != StaticSettings.ERR_NO_POSSIBLE_PATH)
            {
                closest_distance = dist;
                closest_unit = u;
            }
        }
        return closest_unit;
    }
     

    public static Tile ClosestAdjacentTileTo(Tile from, Tile tile_we_want_to_be_adjacent_to)
    {
        int closest_distance = 999;
        Tile closest = null;
        foreach (Tile t in tile_we_want_to_be_adjacent_to.orthogonal_neighbours)
        {
            int dist = Pathing.DistanceBetween(tile_we_want_to_be_adjacent_to, from);
            if (dist <= closest_distance && dist != StaticSettings.ERR_NO_POSSIBLE_PATH)
            {
                closest_distance = dist;
                closest = t;
            }
        }
        Debug.Log(closest.transform.position, closest.gameObject);
        return closest;
    }


    public static Queue<Tile> GetShortestPathToPossibleTiles(List<Tile> valid_tiles, Tile from)
    {
        int shortest_path_distance = 999;
        Queue<Tile> shortest_path = null;
        foreach (Tile t in valid_tiles)
        {
            Queue<Tile> path = Pathing.OrthographicAStarPath(from, t);

            // Check if this was the shortest path to the closest tile
            if (path != null && path.Count <= shortest_path_distance)
            {
                shortest_path_distance = path.Count;
                shortest_path = path;
            }
        }
        return shortest_path;
    }
    public static Queue<Tile> GetLongestPathToPossibleTiles(List<Tile> valid_tiles, Tile from)
    {
        int longest_path_distance = -1;
        Queue<Tile> longest_path = null;
        foreach (Tile t in valid_tiles)
        {
            Queue<Tile> path = Pathing.OrthographicAStarPath(from, t);

            // Check if this was the shortest path to the closest tile
            if (path != null && path.Count >= longest_path_distance)
            {
                longest_path_distance = path.Count;
                longest_path = path;
            }
        }
        return longest_path;
    }
}
