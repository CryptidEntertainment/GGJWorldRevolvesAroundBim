using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{
    public AudioSource introBG = null;
    public AudioSource loopBG = null;

    void Start()
    {
        //introBG.Play(); //intro autoplays, no need to call it.
        loopBG.PlayDelayed(introBG.clip.length*0.995f); //play the looping audio, with the tiniest bit of overlap to try and make up for Unity not actually preloading the audio so it starts exactly when it's supposed to.
    }

}
