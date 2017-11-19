using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Don't move if you can't reach the heroes
// Surround the hero with the lowest HP they can reach
public class Goblin : Monster
{


    public override void CanReachHeroes(List<Tile> valid_tiles)
    {
        base.CanReachHeroes(valid_tiles);

        // Remove some of the valid tiles so we only want to surround the hero with the lowest health (if we have the luxury of choice)
        int lowest_HP = 999;
        Unit lowest_health_hero = null;
        foreach (KeyValuePair<Unit, List<Tile>> pair in heroes_within_range_this_turn)
        {
            if (pair.Key.Health <= lowest_HP)
            {
                lowest_HP = pair.Key.Health;
                lowest_health_hero = pair.Key;
            }
        }
        valid_tiles = heroes_within_range_this_turn[lowest_health_hero];

        Queue<Tile> shortest_path = StaticSettings.GetLongestPathToPossibleTiles(valid_tiles, this.destination_tile);

        if (shortest_path == null)
            Debug.LogError("ShortestPath in Skeletons is null");

        SetPath(shortest_path);
        Debug.Log(this.name + " found a move");
    }


    public override void CantReachHeroes()
    {
        base.CantReachHeroes();

        EndTurn();
    }


    public override Unit DecideTarget()
    {
        return StaticSettings.GetClosestWeakestUnitTo(HeroManager.hero_manager.faction_units, this.destination_tile);
    }





    public override void Update()
    {
        base.Update();
    }
}
