using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRoom : MonoBehaviour {


    private void Awake()
    {
        Monster[] monsters = this.GetComponentsInChildren<Monster>(true);
        foreach (Monster m in monsters)
        {
            m.gameObject.SetActive(false);
        }
    }
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Hero entered " + collision.gameObject.tag);
        if (collision.gameObject.CompareTag("Hero"))
        {
            Activate_Room();
        }
    }
    public void Activate_Room()
    {
        // Activate all monsters here
        Monster[] monsters = this.GetComponentsInChildren<Monster>(true);
        foreach (Monster m in monsters)
        {
            m.gameObject.SetActive(true);
            m.transform.parent = null;
            GameState.game_state.Enemy_Faction.AddUnitToFaction(m);
        }

        Destroy(this.gameObject);
    }
}
