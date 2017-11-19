using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        this.transform.position = new Vector3(
            this.transform.position.x + horizontal * Time.deltaTime * 3f,
            this.transform.position.y + Vertical * Time.deltaTime * 3f,
            this.transform.position.z);

        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene("Menu");
    }
}
