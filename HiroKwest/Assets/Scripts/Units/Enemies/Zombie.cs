using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Moves to attack the closest hero
// Moves towards the closest hero if cannot attack
public class Zombie : Monster
{


    public override void CanReachHeroes(List<Tile> valid_tiles)
    {
        base.CanReachHeroes(valid_tiles);

        Queue<Tile> shortest_path = StaticSettings.GetShortestPathToPossibleTiles(valid_tiles, this.destination_tile);

        if (shortest_path == null)
            Debug.LogError("ShortestPath in Zombie is null");
        
        SetPath(shortest_path);
        Debug.Log(this.name + " found a move");
    }


    public override void CantReachHeroes()
    {
        base.CantReachHeroes();

        // Get closest hero
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
