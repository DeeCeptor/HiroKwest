using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager unit_manager;
    public Unit selected_unit;
    public GameObject selected_unit_indicator;

    public LayerMask Unit_Layermask;
    public LayerMask Tile_Layermask;

	void Awake ()
    {
        unit_manager = this;
	}


    public void RemoveUnitFromLists(Unit u)
    {
        u.owner.faction_units.Remove(u);
        u.owner.units_with_actions_remaining.Remove(u);
        u.destination_tile = null;
        GameState.game_state.current_players_turn.UnitMoved();

        // Check if the heroes are all dead
        if (HeroManager.hero_manager.faction_units.Count <= 0)
            GameState.game_state.Defeat();
    }


    // Recalculate potential movement now that this unit is gone
    public void UnitDied(Unit u)
    {
        Debug.Log(u.name + " died");
        RemoveUnitFromLists(u);

        u.owner.MakeUnitsBlock();

        foreach (Faction f in GameState.game_state.factions)
        {
            f.UnitMoved();
        }
    }


    public void DeselectAll()
    {
        if (selected_unit != null)
        {

        }

        StaticSettings.UnhighlightAllTiles();
        selected_unit = null;
    }
    public void SelectUnit(Unit unit)
    {
        // Do nothing if they click the same unit again
        if (unit == selected_unit)
            return;

        selected_unit = unit;
        unit.UnitSelected();

        // Highlight the tile they selected
    }
}
