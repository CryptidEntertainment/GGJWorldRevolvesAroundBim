using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldPizza : MonoBehaviour {
    public float FlipDuration = 0.5f;

    private float flipTime = 0;
    private bool isFlipping = false;

    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.flipTime = 0;
            this.isFlipping = true;
        }

        if (this.isFlipping) {
            this.flipTime = Mathf.Min(this.flipTime + Time.deltaTime / FlipDuration, 1f);
            this.gameObject.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 180 / FlipDuration), Space.Self);
            if (this.flipTime == 1f) {
                this.isFlipping = false;
                Transform transform = this.gameObject.transform;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Round(transform.rotation.eulerAngles.z / 180f) * 180f,);
            }
        }
    }

    void Spin() {

    }
}
