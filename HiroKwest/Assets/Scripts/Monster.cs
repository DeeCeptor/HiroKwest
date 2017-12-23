using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Controlled by the AI
public class Monster : Unit
{
    List<Tile> tiles_adjacent_to_heroes = new List<Tile>();
    public Dictionary<Unit, List<Tile>> heroes_within_range_this_turn = new Dictionary<Unit, List<Tile>>();


    public override void Start()
    {
        unit_card_parent = GraphicsManager.graphics_manager.enemy_unit_card_holder;

        //FirstTimeSetupMonster();

        base.Start();
    }


    public void FirstTimeSetupMonster()
    {
        gameObject.SetActive(true);
        transform.parent = null;

        GameState.game_state.Enemy_Faction.AddUnitToFaction(this);
    }


    public virtual void PerformAITurn()
    {
        if (Actions_Remaining != Actions_per_Turn)
            return;

        // Are we already adjacent to a hero we want to attack?
        AIAttackIfPossible();
        if (Actions_Remaining < Actions_per_Turn)
            return;

        Debug.Log ("Performing " + this.name);
        GetTilesAdjacentToHeroes();

        // Are any of these hero-adjacent tiles tiles that we can move to?
        List<Tile> reachable_hero_adjacent_tiles = new List<Tile>();
        /*
        foreach (Tile t in tiles_adjacent_to_heroes)
        {
            if (this.tiles_we_can_move_to.ContainsKey(t))
                reachable_hero_adjacent_tiles.Add(t);
        }*/
        // Cull out any tiles we can't reach this turn
        Dictionary<Unit, List<Tile>> new_reachable_tiles_per_hero = new Dictionary<Unit, List<Tile>>();
        foreach (KeyValuePair<Unit, List<Tile>> pair in heroes_within_range_this_turn)
        {
            List<Tile> new_tile_list = new List<Tile>();
            foreach (Tile t in pair.Value)
            {
                if (this.tiles_we_can_move_to.ContainsKey(t))
                {
                    reachable_hero_adjacent_tiles.Add(t);
                    new_tile_list.Add(t);
                }
            }
            if (new_tile_list.Count > 0)
                new_reachable_tiles_per_hero[pair.Key] = new_tile_list;
        }
        heroes_within_range_this_turn = new_reachable_tiles_per_hero;

        if (reachable_hero_adjacent_tiles.Count > 0)
        {
            CanReachHeroes(reachable_hero_adjacent_tiles);
        }
        else
        {
            // Can't reach any heroes
            CantReachHeroes();
        }
    }


    public override void SetPath(Queue<Tile> trail)
    {
        base.SetPath(trail);
        if (owner == null)
        {
            Debug.LogError("owner null setpath", this.gameObject);
        }
        this.owner.UnitMoved();
        this.moved_once = true;
        ActionPerformed(1);
    }


    public void GetTilesAdjacentToHeroes()
    {
        tiles_adjacent_to_heroes.Clear();

        // Collect all non-occupied tiles adjacent to the heroes
        heroes_within_range_this_turn.Clear();
        foreach (Unit u in HeroManager.hero_manager.faction_units)
        {
            foreach (Tile neighbour in u.destination_tile.orthogonal_neighbours)
            {
                if (!neighbour.tile_occupied)
                {
                    //Debug.LogError("GetTilesAdjacentToHeroes " + u.name);
                    // Record which tiles are adjacent to which heroes
                    if (!heroes_within_range_this_turn.ContainsKey(u))
                        heroes_within_range_this_turn.Add(u, new List<Tile>());
                    heroes_within_range_this_turn[u].Add(neighbour);

                    if (!tiles_adjacent_to_heroes.Contains(neighbour))
                        tiles_adjacent_to_heroes.Add(neighbour);
                }
            }
        }

        // Check if any of our tiles we can move to is adjacent to a hero


        // Get distance to each hero

    }


    // Must decide if we can attack, and if there are multiple targets, which hero
    public virtual void AIAttackIfPossible(bool end_turn_regardless = false)
    {
        Unit target = DecideTarget();
        if (AttackIfPossible(target))
        {
            ActionPerformed(99);
            return;
        }
        if (end_turn_regardless)
            ActionPerformed(99);
    }


    public virtual Unit DecideTarget()
    {
        return null;
    }


    // WE can't reach the hero, must decide what to do
    public virtual void CantReachHeroes()
    {
       
    }
    // We can reach THESE tiles adjacent to the heroes, now we have to decide which tile to go to
    public virtual void CanReachHeroes(List<Tile> valid_tiles)
    {

    }


	public override void Update ()
    {
        base.Update();

        // Attack if we're done moving
        if (Active && moved_once && next_tile == null)
        {
            AIAttackIfPossible(true);
        }
    }


    
}
