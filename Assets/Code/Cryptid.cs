using UnityEngine;

public class Cryptid : MonoBehaviour {
    public GameObject helpText;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (this.helpText) this.helpText.SetActive(true);
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (this.helpText) this.helpText.SetActive(false);
    }
}
