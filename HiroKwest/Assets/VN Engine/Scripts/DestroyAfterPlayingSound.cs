using UnityEngine;
using System.Collections;

// Destroys the object after its audio source is done playing. Requires an audio source.
public class DestroyAfterPlayingSound : MonoBehaviour
{
    AudioSource source;


    void Start ()
    {
        source = this.GetComponent<AudioSource>();
	}


    void Update ()
    {
	    if (!source.isPlaying)
        {
            Debug.Log("Sound effect done playing");
            Destroy(this.gameObject);
        }
	}
}
