using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GraphicsManager : MonoBehaviour {

    public static GraphicsManager graphics_manager;

    public Canvas world_canvas;
    public GameObject hero_unit_card_holder;
    public GameObject enemy_unit_card_holder;
    public AudioSource effects_audio;


    public void GetHitAnimation(int dmg, Vector2 position)
    {
        if (dmg > 1)
            Instantiate<GameObject>(Resources.Load("BigHit") as GameObject, position, Quaternion.identity);
        else
            Instantiate<GameObject>(Resources.Load("SmallHit") as GameObject, position, Quaternion.identity);

        // Spawn damage number
        GameObject obj = Instantiate<GameObject>(Resources.Load("DamageNumber") as GameObject, position, Quaternion.identity, world_canvas.transform);
        obj.GetComponent<Text>().text = "" + dmg;
    }


    public void PlaySound(AudioClip sound)
    {
        effects_audio.clip = sound;
        effects_audio.Play();
    }


    private void Awake()
    {
        graphics_manager = this;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
