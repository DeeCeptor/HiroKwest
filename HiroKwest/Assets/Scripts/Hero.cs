using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The player has 4 heroes at their command, and only 1 hero can be selected at a time
public class Hero : Unit
{
    public int Perception = 1;  // How far away they can sense traps & hidden features



    void Start ()
    {
        unit_card_parent = GraphicsManager.graphics_manager.hero_unit_card_holder;
        UpdateIndividualCard = true;
        base.Start();
	}


    public override void UnitSelected()
    {
        // Show on the UI that this hero is selected
        base.UnitSelected();
    }

    

    void Update ()
    {
        /*
        // User right clicked, move the hero
        if (Input.GetMouseButtonDown(1))
        {
            // Find out where we clicked
            Vector2 world_point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Tile destination = StaticSettings.GetNearestTile(world_point);


            if (destination != null)
            {
                // Path to that spot
                current_path = new Queue<Tile>(Pathing.OrthographicAStarPath(current_tile, destination));
            }
        }
        */
        base.Update();
    }
}
