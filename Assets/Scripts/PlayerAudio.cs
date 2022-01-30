using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class PlayerAudio : MonoBehaviour
{
    public AudioSource aud;
    public AudioClip bimWalk;
    public AudioClip bimLand;
    public AudioClip bimFlip;
    public AudioClip bimPickup;

    // Start is called before the first frame update
    void Start()
    {
        aud = gameObject.GetComponent<AudioSource>();
    }

    public void stopPlaying()
    {
        aud.clip=null;
    }

    public void playBimWalk()
    {
        if (aud.clip != bimWalk && !aud.isPlaying)
        {
            aud.Stop();
            aud.clip = bimWalk;
            aud.loop = true;
            aud.Play();
        }
    }

    public void playBimLand()
    {
        aud.PlayOneShot(bimLand);
    }

    public void playBimFlip()
    {
        aud.Stop();
        aud.clip = bimFlip;
        aud.loop = false;
        aud.Play();
    }

    public void playPickupAudio()
    {
        aud.PlayOneShot(bimPickup);
    }
}
