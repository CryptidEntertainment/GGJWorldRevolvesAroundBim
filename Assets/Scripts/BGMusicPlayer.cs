using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicPlayer : MonoBehaviour
{
    public AudioSource introBG = null;
    public AudioSource loopBG = null;

    void Start()
    {
        //introBG.Play();
        loopBG.PlayDelayed(introBG.clip.length*0.995f);
    }

}
