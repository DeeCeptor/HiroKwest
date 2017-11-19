using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves to surround the closest hero
// Moves towards the closest hero if they can't attack
public class Skeletons : Monster
{


    public override void CanReachHeroes(List<Tile> valid_tiles)
    {
        base.CanReachHeroes(valid_tiles);

        // Error comes from when there are multiple heroes adjacent to the same tile
        // Attack closest valid hero
        List<Unit> closest_heroes = StaticSettings.GetClosestUnitsTo(HeroManager.hero_manager.faction_units, this.destination_tile);
        Unit closest_hero = null;
        foreach (Unit u in closest_heroes)
        {
            if (heroes_within_range_this_turn.ContainsKey(u))
            {
                closest_hero = u;
                break;
            }
        }
        if (!heroes_within_range_this_turn.ContainsKey(closest_hero))
        {
            Debug.LogError(closest_hero.name + " not in heroeswithinrange " + heroes_within_range_this_turn.Keys.ToString() , this.gameObject);
            foreach (Unit k in heroes_within_range_this_turn.Keys)
            {
                Debug.LogError(k.name);
            }
        }
        valid_tiles = heroes_within_range_this_turn[closest_hero];

        // Need to change it to use only tiles to closest hero
        Queue<Tile> shortest_path = StaticSettings.GetLongestPathToPossibleTiles(valid_tiles, this.destination_tile);

        if (shortest_path == null)
            Debug.LogError("ShortestPath in Skeletons is null");

        SetPath(shortest_path);
        Debug.Log(this.name + " found a move");
    }


    public override void CantReachHeroes()
    {
        base.CantReachHeroes();

        // Get closest hero (u is null for some reason)
        Unit u = StaticSettings.GetClosestUnitsTo(HeroManager.hero_manager.faction_units, this.destination_tile)[0];

        // Move to closest possible tile of the closest hero
        Tile dest = StaticSettings.ClosestAdjacentTileTo(this.destination_tile, u.destination_tile);
        SetPath(Pathing.OrthographicAStarPath(this.destination_tile, dest, Speed));

        EndTurn();
    }


    public override Unit DecideTarget()
    {
        return StaticSettings.GetClosestUnitsTo(HeroManager.hero_manager.faction_units, this.destination_tile)[0];
    }





    public override void Update()
    {
        base.Update();
    }
}
