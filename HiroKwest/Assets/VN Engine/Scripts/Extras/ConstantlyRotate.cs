using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConstantlyRotate : MonoBehaviour 
{
    public Vector3 rotation_speed = new Vector3(0, 0, 130.0f);

	void Update () 
	{
        this.transform.Rotate(rotation_speed * Time.deltaTime);
	}
}
