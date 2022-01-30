using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndMusic : MonoBehaviour
{
    private BGMusicPlayer mainMusic;
    // Start is called before the first frame update
    void Start()
    {
        mainMusic = GameObject.FindGameObjectWithTag("GameController").GetComponent<BGMusicPlayer>();
        mainMusic.stopMusic();
        this.gameObject.GetComponent<AudioSource>().Play();
    }
}
