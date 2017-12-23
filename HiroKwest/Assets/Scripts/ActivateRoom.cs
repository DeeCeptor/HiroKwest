using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateRoom : MonoBehaviour
{
    public GameObject Room_Parent;


    private void Awake()
    {
        if (Room_Parent != null)
            Room_Parent.SetActive(false);

        Monster[] monsters = this.GetComponentsInChildren<Monster>(true);
        foreach (Monster m in monsters)
        {
            m.gameObject.SetActive(false);
        }
    }
    void Start () {
		
	}


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
        // Make tiles appear
        Room_Parent.gameObject.SetActive(true);
        Tile[] tiles = Room_Parent.GetComponentsInChildren<Tile>();
        foreach (Tile t in tiles)
        {
            t.AnimateTileEntering();
        }
        

        // Rebuild graph
        


        // Activate all monsters
        Monster[] monsters = this.GetComponentsInChildren<Monster>(true);
        foreach (Monster m in monsters)
        {
            m.FirstTimeSetupMonster();
        }

        Destroy(this.gameObject);
    }
}
