using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowMouseClick : MonoBehaviour
{
    public Queue<Tile> current_path = new Queue<Tile>();
    Tile next_tile;
    public Tile current_tile;
    Tile previous_tile;
    float follow_speed = 4.0f;
    float distance_between_next_tile;


    void Start ()
    {
        current_tile = StaticSettings.GetNearestTile(this.transform.position);
	}


    void Update ()
    {
		if (Input.GetMouseButtonDown(0))
        {
            // Find out where we clicked
            Vector2 world_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile destination = StaticSettings.GetNearestTile(world_point);


            if (destination != null)
            {
                // Path to that spot
                current_path = new Queue<Tile>( Pathing.OrthographicAStarPath(current_tile, destination) );
                Debug.Log(current_path.Count);
            }
        }

        // Follow a path if we got one
        if (next_tile == null && current_path.Count > 0)
        {
            Debug.Log("asd");
            next_tile = current_path.Dequeue();
            distance_between_next_tile = 0;
        }
        if (next_tile != null)
        {
            // Move towards the next tile
            distance_between_next_tile += Time.deltaTime * follow_speed;
            this.transform.position =
                Vector2.Lerp(current_tile.transform.position, next_tile.transform.position, distance_between_next_tile);

            if (distance_between_next_tile >= 1)
            {
                current_tile = next_tile;
                next_tile = null;
            }
        }
	}
}
