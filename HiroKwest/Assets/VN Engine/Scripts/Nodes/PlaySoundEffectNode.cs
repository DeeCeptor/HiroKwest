using UnityEngine;
using System.Collections;
using UnityEngine.UI;

// Plays a sound effect. This does not use the music or voice audio sources.
public class PlaySoundEffectNode : Node
{
    //public AudioClip sound_clip;


    public override void Run_Node()
    {
        gameObject.GetComponent<AudioSource>().Play();
        //AudioManager.audio_manager.Play_Sound_Effect(sound_clip);
        Finish_Node();
    }


    public override void Finish_Node()
    {
        StopAllCoroutines();

        base.Finish_Node();
    }
}
