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
    public AudioClip bimDeath1;
    public AudioClip bimDeath2;
    public AudioClip bimDeath3;

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
        /*aud.Stop();
        aud.clip = bimFlip;
        aud.loop = false;
        aud.Play();*/
        aud.PlayOneShot(bimFlip);
    }

    public void playPickupAudio()
    {
        aud.PlayOneShot(bimPickup);
    }

    public void playBimDeath()
    {
        switch (Mathf.FloorToInt(Random.Range(1,3.999f)))
        {
            case 1:
                aud.PlayOneShot(bimDeath1, 0.7f);
                return;
            case 2:
                aud.PlayOneShot(bimDeath2, 0.7f);
                return;
            case 3:
                aud.PlayOneShot(bimDeath3, 0.7f);
                return;
        }
    }
}
