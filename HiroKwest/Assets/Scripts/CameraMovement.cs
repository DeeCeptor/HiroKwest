using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraMovement : MonoBehaviour
{
    public Camera gameplay_camera;

    public float camera_speed = 15f;

    public float zoom_sensitivity = 6f;
    public float zoom_min = 3f;
    public float zoom_max = 10f;


    void Awake () {
        gameplay_camera = this.GetComponent<Camera>();
    }


    void Update ()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float Vertical = Input.GetAxis("Vertical");
        this.transform.position = new Vector3(
            this.transform.position.x + horizontal * Time.deltaTime * camera_speed,
            this.transform.position.y + Vertical * Time.deltaTime * camera_speed,
            this.transform.position.z);

        float zoom = Input.GetAxis("Mouse ScrollWheel") * -zoom_sensitivity;
        gameplay_camera.orthographicSize = Mathf.Clamp(gameplay_camera.orthographicSize + zoom, zoom_min, zoom_max);

        if (Input.GetButtonDown("Cancel"))
            SceneManager.LoadScene("Menu");
    }
}
