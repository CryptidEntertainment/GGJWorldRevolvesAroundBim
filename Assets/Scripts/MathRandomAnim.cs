using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathRandomAnim : MonoBehaviour
{
    public float rate=0.5f;
    private float delay = 0.5f;
    private float timer;
    private int rndInt;
    public Animator anim;


    private void Start()
    {
        timer = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Time.time >= timer + delay)
        {
            rndInt = (int)Random.Range(0, 10000);
            timer = Time.time;
            if (rndInt<=2499)
            {
                delay = rate;
                anim.SetInteger("randAnim", 1);
            }
            else if (rndInt <= 4998)
            {
                delay = rate;
                anim.SetInteger("randAnim", 2);
            }
            else if (rndInt <= 7497)
            {
                delay = rate;
                anim.SetInteger("randAnim", 3);
            }
            else if (rndInt <= 9997)
            {
                delay = rate;
                anim.SetInteger("randAnim", 4);
            }
            else if (rndInt <= 10000)
            {
                delay = rate*500;
                anim.SetInteger("randAnim", 5);
            }
        }
    }
}
