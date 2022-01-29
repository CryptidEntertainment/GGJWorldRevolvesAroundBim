using UnityEngine;
using UnityEngine.SceneManagement;

public class WizardlySpikes : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (FindObjectOfType<PlayerController>()) FindObjectOfType<PlayerController>().Die();
    }
}
