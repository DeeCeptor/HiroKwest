using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FadeOutTextAfter : MonoBehaviour
{
    public float wait_x_seconds;
    public float fade_out_rate = 1.0f;
    Text t;

    public bool randomly_move_upwards;
    public float movement_speed = 2.0f;
    Vector2 random_dir;

    void Start ()
    {
        t = this.GetComponent<Text>();

        if (randomly_move_upwards)
            random_dir = new Vector2(Random.Range(-0.6f, 0.6f), 1);
	}


    void Update ()
    {
        wait_x_seconds -= Time.deltaTime;

        this.transform.position += (Vector3) ((Vector2) random_dir * Time.deltaTime * movement_speed);

        if (wait_x_seconds <= 0)
        {
            // Fading
            Color c = t.color;
            c.a -= Time.deltaTime * fade_out_rate;
            t.color = c;

            if (c.a <= 0)
                Destroy(this);
        }
	}
}
