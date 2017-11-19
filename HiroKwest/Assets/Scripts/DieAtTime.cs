using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//this script kills the object it is attached to when die_time seconds have elapsed
public class DieAtTime : MonoBehaviour
{
    public float die_after_x_seconds = 1;
    [HideInInspector]
    public float die_timer;


    void Start ()
    {
        die_timer = die_after_x_seconds + Time.time;
	}


    void Update ()
    {
		if(die_timer <= Time.time)
        {
            Destroy(gameObject);
        }
	}
}
