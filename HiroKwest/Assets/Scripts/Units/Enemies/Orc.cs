using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Don't move if you can't reach the heroes
// Surround the hero with the highest HP they can reach
public class Orc : Monster
{


    public override void CanReachHeroes(List<Tile> valid_tiles)
    {
        base.CanReachHeroes(valid_tiles);

        // Remove some of the valid tiles so we only want to surround the hero with the lowest health (if we have the luxury of choice)
        int highest_HP = 0;
        Unit target = null;
        foreach (KeyValuePair<Unit, List<Tile>> pair in heroes_within_range_this_turn)
        {
            if (pair.Key.Health >= highest_HP)
            {
                highest_HP = pair.Key.Health;
                target = pair.Key;
            }
        }
        valid_tiles = heroes_within_range_this_turn[target];

        Queue<Tile> shortest_path = StaticSettings.GetShortestPathToPossibleTiles(valid_tiles, this.destination_tile);

        if (shortest_path == null)
            Debug.LogError("ShortestPath in Orc is null");

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
        return StaticSettings.GetClosestStrongestUnitTo(HeroManager.hero_manager.faction_units, this.destination_tile);
    }



    public override void Update()
    {
        base.Update();
    }
}
