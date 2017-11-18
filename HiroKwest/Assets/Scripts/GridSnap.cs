using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

// https://www.youtube.com/watch?v=ukYbRmmlaTM
[ExecuteInEditMode]
public class GridSnap : MonoBehaviour
{
    public bool remove_on_difficulty_higher_than = false;
    Vector3 offset = new Vector3(-1, 1, 0);

    public float cell_size = 1f;
    private float x, y, z;

    // Camera distance
    Vector3 minScreenBounds;
    Vector3 maxScreenBounds;
    string distance = "";
    Vector3 prev_position;

    void Start()
    {
        x = 0f;
        y = 0f;
        z = 0f;
    }

    #if (UNITY_EDITOR)
    void Update()
    {
        if (!Application.isPlaying)
        {
            x = Mathf.Round(transform.position.x / cell_size) * cell_size;
            y = Mathf.Round(transform.position.y / cell_size) * cell_size;
            z = transform.position.z;
            transform.position = new Vector3(x, y, z);

            if (minScreenBounds == Vector3.zero)
            {
                maxScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
                minScreenBounds = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 0));
            }
        }
    }
    #endif
}
