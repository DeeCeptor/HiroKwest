using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GameState : MonoBehaviour
{
    public static GameState game_state;

    public int round_number;
    public bool paused;

    Queue<Faction> factions_ready_to_go_this_turn = new Queue<Faction>();
    public List<Faction> factions = new List<Faction>();
    public Faction current_players_turn;

    public Faction Hero_Faction;
    public Faction Enemy_Faction;

    public GameObject victory_if_destroyed;

    public bool waiting = false;
    public bool game_over = false;


    private void Awake()
    {
        game_state = this;
    }
    void Start()
    {
        Faction player = new HeroManager("Heroes", true, new List<Unit>(GameObject.FindObjectsOfType<Hero>()));
        factions.Add(player);
        Hero_Faction = player;

        Faction enemies = new EnemyManager("Monsters", false, new List<Unit>());//new List<Unit>(GameObject.FindObjectsOfType<Monster>()));
        factions.Add(enemies);
        Enemy_Faction = enemies;

        StartRound();
    }


    public void StartRound()
    {
        StartCoroutine(StartAfterTimeRound());
    }
    public IEnumerator StartAfterTimeRound()
    {
        waiting = true;
        yield return new WaitForSeconds(0.4f);
        waiting = false;
        round_number++;
        factions_ready_to_go_this_turn = new Queue<Faction>(factions);
        StartFactionTurn();
    }
    public void StartFactionTurn()
    {
        if (factions_ready_to_go_this_turn.Count <= 0)
        {
            EndRound();
            return;
        }
        current_players_turn = factions_ready_to_go_this_turn.Dequeue();

        // Change connected graph by making ALL enemies of ALL factions block movement (by removing self from connected graph
        foreach (Faction f in factions)
        {
            if (f != current_players_turn)
            {
                f.MakeUnitsBlock();
            }
        }

        current_players_turn.StartTurn();
    }
    public void EndCurrentFactionTurn()
    {
        current_players_turn.DeactivateUnits();

        StartFactionTurn();
    }
    public void EndRound()
    {
        StartRound();
    }

    void Update()
    {
        if (!waiting && (Input.GetButtonDown("Submit") || current_players_turn.units_with_actions_remaining.Count <= 0))
        {
            EndCurrentFactionTurn();
        }
        if (current_players_turn != null)
            current_players_turn.FactionUpdate();

        if (victory_if_destroyed == null)
            Victory();
    }


    public void Defeat()
    {
        if (game_over)
            return;

        Debug.Log("DEFEAT");
    }
    public void Victory()
    {
        if (game_over)
            return;

        Debug.Log("VICTORY");
    }


    private void OnGUI()
    {
        if (!waiting)
        {
            GUI.Label(new Rect(0, 0, 200, 100), current_players_turn.faction_name + ", round " + round_number);
        }
    }
}
