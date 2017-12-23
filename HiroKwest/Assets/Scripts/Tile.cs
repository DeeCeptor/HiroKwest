using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public List<Tile> orthogonal_neighbours = new List<Tile>();
    public List<Tile> diagonal_neighbours = new List<Tile>();
    public List<Tile> all_neighbours = new List<Tile>();

    [HideInInspector]
    public Tile cameFrom;       // Most efficient neighbour node to reach this node
    public float gScore = 999999f;        // Cost of getting from start to this node
    public float fScore = 999999f;    // Total cost of getting from start node to goal by using THIS node

    SpriteRenderer sprite;
    Collider2D collider;
    float neighbour_distance = 1f;  // Should be 1, no reason to ever change it
    int tile_mask;

    public Unit unit_on_tile;
    public bool tile_occupied;


    public void ResetPathingInfo()
    {
        gScore = 999999f;
        fScore = 999999f;
        cameFrom = null;
    }

    void Awake()
    {
        collider = this.GetComponent<Collider2D>();
        sprite = this.GetComponent<SpriteRenderer>();
        tile_mask = LayerMask.NameToLayer("Tile");
        StaticSettings.all_Tiles.Add(this);
        StaticSettings.tile_coordinates.Add(this.transform.position, this);
    }
    void Start ()
    {
        tile_mask = UnitManager.unit_manager.Tile_Layermask.value;
        RaycastNeighbours();
	}

    public void AnimateTileEntering()
    {
        // Randomly choose a direction to fly in from
        Animator anim = this.transform.GetComponentInChildren<Animator>();
        anim.Play("Entrance1");// + Random.Range(0, 2));
    }


    public void UnhighlightThisTile()
    {
        this.sprite.color = StaticSettings.transparent;
    }


    public void RemoveSelfFromGraph()
    {
        // Find all neighbours, remove references to myself
        foreach (Tile neighbour in all_neighbours)
        {
            neighbour.orthogonal_neighbours.Remove(this);
            neighbour.diagonal_neighbours.Remove(this);
            neighbour.all_neighbours.Remove(this);
        }
    }


    // Creates the connected graph via raycasting to find adjacencies
    public void RaycastNeighbours()
    {
        orthogonal_neighbours.Clear();
        diagonal_neighbours.Clear();
        all_neighbours.Clear();

        // Populate orthogonal neighbours
        CheckAndAddNeighbour(Vector2.left, orthogonal_neighbours);
        CheckAndAddNeighbour(Vector2.right, orthogonal_neighbours);
        CheckAndAddNeighbour(Vector2.up, orthogonal_neighbours);
        CheckAndAddNeighbour(Vector2.down, orthogonal_neighbours);

        // Populate diagonal neighbours
        CheckAndAddNeighbour(StaticSettings.TopLeft, diagonal_neighbours);
        CheckAndAddNeighbour(StaticSettings.TopRight, diagonal_neighbours);
        CheckAndAddNeighbour(StaticSettings.BottomLeft, diagonal_neighbours);
        CheckAndAddNeighbour(StaticSettings.BottomRight, diagonal_neighbours);

        // Combine orthogonal & diagonal neighbours to get ALL neighbours
        all_neighbours.AddRange(orthogonal_neighbours);
        all_neighbours.AddRange(diagonal_neighbours);
    }
    public void CheckAndAddNeighbour(Vector2 direction, List<Tile> list_to_add_to)
    {
        // Disable your own collider so you can't hit yourself with the ray
        collider.enabled = false;
        RaycastHit2D ray = Physics2D.Raycast(this.transform.position, direction, 1.0f, tile_mask);
        if (ray.transform != null && ray.transform.CompareTag("Tile"))
        {
            list_to_add_to.Add(ray.transform.GetComponent<Tile>());
            Debug.DrawRay(this.transform.position, direction, Color.red, 15f);
        }
        collider.enabled = true;
    }


    void Update ()
    {
		
	}
}
