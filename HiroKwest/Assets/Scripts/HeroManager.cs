using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroManager : Faction
{
    public static HeroManager hero_manager;

    public HeroManager(string name, bool is_human_controlled, List<Unit> units) : base(name, is_human_controlled, units)
    {
        hero_manager = this;
    }

    public override void FactionUpdate()
    {
        base.FactionUpdate();

        if (GameState.game_state.current_players_turn.player_controlled)
        {
            if (Input.GetMouseButtonDown(0))
            {
                LeftMouseClicked();
            }
            if (Input.GetMouseButtonDown(1))
            {
                RightMouseClicked();
            }
        }

        if (UnitManager.unit_manager.selected_unit == null)
            UnitManager.unit_manager.selected_unit_indicator.SetActive(false);
        else
        {
            UnitManager.unit_manager.selected_unit_indicator.SetActive(true);
            UnitManager.unit_manager.selected_unit_indicator.transform.position = UnitManager.unit_manager.selected_unit.transform.position;
        }
    }

    public void LeftMouseClicked()
    {
        // Check if they clicked on a valid unit
        RaycastHit2D ray = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.up, 0.1f, UnitManager.unit_manager.Unit_Layermask.value);
        if (ray.transform != null)
            UnitManager.unit_manager.SelectUnit(ray.transform.GetComponent<Unit>());
        else
            UnitManager.unit_manager.DeselectAll();
    }
    public void RightMouseClicked()
    {
        // Move the selected unit if possible
        if (UnitManager.unit_manager.selected_unit != null && GameState.game_state.current_players_turn.player_controlled)
        {
            // Find the tile they right clicked on
            Tile t = StaticSettings.GetNearestTile(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            if (t != null)
            {
                // Do they want to move or attack?
                // Is that tile occupied by an enemy?
                if (t.unit_on_tile != null && UnitManager.unit_manager.selected_unit.owner.IsEnemy(t.unit_on_tile))
                    UnitManager.unit_manager.selected_unit.AttackIfPossible(t.unit_on_tile);
                else
                    // Tile is empty, move to it
                    UnitManager.unit_manager.selected_unit.MoveIfValid(t);
            }
        }
    }
}
