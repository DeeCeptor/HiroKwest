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

        Unit closest_hero = StaticSettings.GetClosestUnitTo(HeroManager.hero_manager.faction_units, this.destination_tile);
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
        Unit u = StaticSettings.GetClosestUnitTo(HeroManager.hero_manager.faction_units, this.destination_tile);

        // Move to closest possible tile of the closest hero
        Tile dest = StaticSettings.ClosestAdjacentTileTo(this.destination_tile, u.destination_tile);
        SetPath(Pathing.OrthographicAStarPath(this.destination_tile, dest, Speed));

        EndTurn();
    }


    public override Unit DecideTarget()
    {
        return StaticSettings.GetClosestUnitTo(HeroManager.hero_manager.faction_units, this.destination_tile);
    }





    public override void Update()
    {
        base.Update();
    }
}
