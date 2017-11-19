using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Both heroes & enemies are units, and can follow paths
public class Unit : FollowPath
{
    public int Health = 8;
    public int Damage = 1;
    public int Armour = 0;  // Temporary Health that regenerates each turn
    public int Current_Armour;
    public int Speed = 5;   // How far can we move each turn?
    public int Running_Speed = 3;    // Added to our speed, monsters have 0 running speed

    public bool moved_once = false;
    public bool Active;     // If they have actions remaining, they are Active
    public int Actions_Remaining;
    public int Actions_per_Turn = 2;
    public bool Not_Their_Turn;

    public Faction owner;
   

    void Start ()
    {
        base.Start();

    }


    void Update()
    {
        base.Update();
    }


    public virtual void StartTurn()
    {
        moved_once = false;
        Active = true;
        Not_Their_Turn = false;
        Actions_Remaining = Actions_per_Turn;
        Current_Armour = Armour;

        GetTilesWeCanMoveTo(Speed + Running_Speed);
    }
    public void EndTurn()
    {
        UnitManager.unit_manager.DeselectAll();
        Actions_Remaining = 0;
        StaticSettings.UnhighlightAllTiles();
        GameState.game_state.current_players_turn.units_with_actions_remaining.Remove(this);
        Active = false;
    }


    // Returns true if we successfully attacked the target
    public bool AttackIfPossible(Unit target)
    {
        if (target == null || (!Active || Actions_Remaining <= 0))
            return false;

        // Are we adjacent to target? Only valid if we're doing a melee attack
        if (target.destination_tile.orthogonal_neighbours.Contains(this.destination_tile))
        {
            Attack(target);
            return true;
        }
        else
            return false;
    }
    public void Attack(Unit target)
    {
        target.TakeHit(this.Damage);
        ActionPerformed(Actions_per_Turn);
    }

    public void TakeHit(int incoming_damage)
    {
        // Take into account armour
        int modified_damage = incoming_damage - Current_Armour;
        Current_Armour = Mathf.Max(0, Current_Armour - incoming_damage);

        this.Health -= modified_damage;
        if (Health <= 0)
        {
            Die();
        }
    }
    public void Die()
    {
        Debug.Log(this.name + " was killed!");
        UnitManager.unit_manager.RemoveUnitFromLists(this);
        Destroy(this.gameObject);
    }


    public void ActionPerformed(int action_cost)
    {
        Actions_Remaining -= action_cost;
        if (Actions_Remaining <= 0)
        {
            EndTurn();
        }
    }


    // User has clicked on or is hovering over this unit
    public virtual void UnitSelected()
    {
        Debug.Log(this.name + " selected");
        HighlightSpaceWeCanMoveTo();
    }


    public void MoveIfValid(Tile destination)
    {
        if (Active)
        {
            if (Actions_Remaining > 0 & tiles_we_can_move_to.ContainsKey(destination))
            {
                ActionPerformed(1);
                MoveTo(destination);

                // Can we move again?
                if (Actions_Remaining > 0)
                {
                    HighlightSpaceWeCanMoveTo();
                }
            }
            else if (Actions_Remaining > 1 && tiles_we_can_run_to.ContainsKey(destination))
            {
                // Sprinting takes 2 Action Points
                ActionPerformed(2);
                MoveTo(destination);
            }
        }
    }
    public void MoveTo(Tile destination)
    {
        this.SetPath(Pathing.OrthographicAStarPath(this.destination_tile, destination));
        moved_once = true;
        owner.UnitMoved();
    }


    // Gets a list of tiles that can be moved to
    public void GetTilesWeCanMoveTo(int distance)
    {
        tiles_we_can_move_to.Clear();
        tiles_we_can_run_to.Clear();

        if (distance <= 0)
            return;

        Dictionary<Tile, int> all_tiles_we_can_move_to = Pathing.BoundedDijkstra(destination_tile, distance);
        Debug.Log(all_tiles_we_can_move_to.Count + " tiles we could move to");
        // Sort through the tiles we can move to
        foreach (KeyValuePair<Tile, int> t in all_tiles_we_can_move_to)
        {
            if (t.Value <= Speed)
            {
                // We can move to this space normally
                tiles_we_can_move_to.Add(t.Key, t.Value);
            }
            else
            {
                // Gotta sprint/dash to get here, using our full turn
                tiles_we_can_run_to.Add(t.Key, t.Value);
            }
        }
    }


    public void HighlightSpaceWeCanMoveTo()
    {
        StaticSettings.UnhighlightAllTiles();

        foreach (KeyValuePair<Tile, int> t in tiles_we_can_move_to)
        {
            // We can move to this space normally
            t.Key.GetComponent<SpriteRenderer>().color = Color.blue;
        }
        foreach (KeyValuePair<Tile, int> t in tiles_we_can_run_to)
        {
            // We can move to these spaces if we run
            t.Key.GetComponent<SpriteRenderer>().color = Color.yellow;
        }
    }


    private void OnMouseDown()
    {
        UnitSelected();
    }
}
