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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player"&&gameDriver.flip==false)
        {
            gameDriver.flip = true;
            GameObject.Destroy(this.gameObject);
        }
    }
}
