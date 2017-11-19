using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : Faction
{
    // Enemies move in order of appearance, with those earliest in the faction unit list deciding the order
    public List<string> Enemy_Unit_Order = new List<string>();


    public EnemyManager(string name, bool is_human_controlled, List<Unit> units) : base(name, is_human_controlled, units)
    {
        // Sort what enemies we get into the proper Enemy unit order
    }


    public override void StartTurn()
    {
        base.StartTurn();

        // Organize monsters into groups that need ordering
    }


    public override void FactionUpdate()
    {
        base.FactionUpdate();

        while (units_with_actions_remaining.Count > 0)
        {
            Debug.Log(this.units_with_actions_remaining[0].name + "'s turn");
            ((Monster)units_with_actions_remaining[0]).PerformAITurn();
            break;
        }
    }
}
