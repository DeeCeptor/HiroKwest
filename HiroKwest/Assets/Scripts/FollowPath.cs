using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPath : MonoBehaviour
{
    [HideInInspector]
    private Queue<Tile> current_path = new Queue<Tile>();
    [HideInInspector]
    public Tile current_tile;
    public Tile destination_tile
    {
        get { return _destination_tile; }
        set
        {
            if (_destination_tile != null)
            {
                _destination_tile.tile_occupied = false;
                _destination_tile.unit_on_tile = null;
                prev_destination_tile = _destination_tile;
            }
            _destination_tile = value;

            if (value != null)
            {
                value.tile_occupied = true;
                value.unit_on_tile = (Unit)this;
            }
        }
    }
    Tile _destination_tile;
    [HideInInspector]
    public Tile prev_destination_tile;

    [HideInInspector]
    protected Tile next_tile;
    Tile previous_tile;
    float follow_speed = 7.0f;
    float distance_between_next_tile;

    // Contains tiles that we can move to, along with their DISTANCES to this unit
    public Dictionary<Tile, int> tiles_we_can_move_to = new Dictionary<Tile, int>();
    public Dictionary<Tile, int> tiles_we_can_run_to = new Dictionary<Tile, int>();


    public virtual void Awake ()
    {

    }
    public virtual void Start ()
    {
        current_tile = StaticSettings.GetNearestTile(this.transform.position);
        destination_tile = current_tile;
    }


    public virtual void SetPath(Queue<Tile> trail)
    {
        if (trail == null)
        {
            Debug.LogError("Impossible to reach destination", this.gameObject);
            return;
        }

        current_path = trail;
        destination_tile = current_path.ToArray()[current_path.Count - 1];
        Debug.Log(trail.Count);
    }


    public virtual void Update ()
    {
        // Follow a path if we got one
        if (next_tile == null && current_path.Count > 0)
        {
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
