using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbPush : MonoBehaviour
{
    public float pushForce = 30;
    public Vector2 pushVector = Vector2.zero;
    // Start is called before the first frame update

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag=="Player")
        {
            Debug.Log("Push!");
            pushVector.x = other.gameObject.transform.position.x - this.gameObject.transform.position.x;
            pushVector.y = other.gameObject.transform.position.y - this.gameObject.transform.position.y;
            other.gameObject.GetComponent<Rigidbody2D>().AddForce(pushVector * pushForce * Random.Range(0.66f,2), ForceMode2D.Impulse);
        }
    }
}
