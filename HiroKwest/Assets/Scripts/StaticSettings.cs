using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public static class StaticSettings
{
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


    public static void ResetAllPathingInfo()
    {
        foreach(Tile t in all_Tiles)
        {
            t.ResetPathingInfo();
        }
    }
}
