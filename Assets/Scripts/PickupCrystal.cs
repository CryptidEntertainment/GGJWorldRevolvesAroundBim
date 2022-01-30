using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupCrystal : MonoBehaviour
{

    private GameDriver gameDriver;

    // Start is called before the first frame update
    void Awake()
    {
        gameDriver = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDriver>();
    }

    //we've triggered something. Make sure it's the player and if it is, give another flip and die. Also call the audio component to play the pickup sound.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"&&gameDriver.flip==false)
        {
            other.gameObject.GetComponent<PlayerAudio>().playPickupAudio();
            gameDriver.flip = true;
            GameObject.Destroy(this.gameObject);
        }
    }
}
