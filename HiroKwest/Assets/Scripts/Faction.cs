using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Faction
{
    public string faction_name;
    public bool player_controlled = true;
    public List<Unit> faction_units = new List<Unit>();
    public List<Unit> units_with_actions_remaining = new List<Unit>();
    public bool done_turn = false;

	public Faction(string name, bool is_human_controlled, List<Unit> units)
    {
        faction_name = name;
        player_controlled = is_human_controlled;
        faction_units = units;

        foreach (Unit u in faction_units)
        {
            u.owner = this;
        }
    }


    public bool IsEnemy(Unit u)
    {
        return u.owner != this;
    }


    public virtual void AddUnitToFaction(Unit u)
    {
        u.owner = this;
        faction_units.Add(u);
    }


    public void UnitMoved()
    {
        foreach (Unit u in units_with_actions_remaining)
        {
            if (!u.moved_once)
                u.GetTilesWeCanMoveTo(u.Speed + u.Running_Speed);
            else
                u.GetTilesWeCanMoveTo(u.Running_Speed);
        }
    }


    // Removes nodes from the cnnected graph so enemies properly block movement
    public void MakeUnitsBlock()
    {
        // Reconnect the whole graph
        StaticSettings.ReconstructFullGraph();

        // Selective remove parts of it occupied by enemies
        foreach (Unit u in faction_units)
        {
            u.destination_tile.RemoveSelfFromGraph();
        }
    }


    public virtual void StartTurn()
    {
        ActivateUnits();
    }


    public virtual void FactionUpdate()
    {

    }


    // Called when this factions turn starts
    public void ActivateUnits()
    {
        done_turn = false;
        units_with_actions_remaining = new List<Unit>(faction_units);

        foreach (Unit u in units_with_actions_remaining)
        {
            u.StartTurn();
        }
    }
    // Called when the Factions turn is over
    public void DeactivateUnits()
    {
        done_turn = true;

        foreach (Unit u in faction_units)
        {
            u.EndTurn();
        }
    }
}
