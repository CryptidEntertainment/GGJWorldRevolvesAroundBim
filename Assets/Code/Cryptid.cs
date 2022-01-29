using UnityEngine;

public class Cryptid : MonoBehaviour {
    public GameObject helpText;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("Enter");
        if (this.helpText) this.helpText.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collider) {
        if (this.helpText) this.helpText.SetActive(false);
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("Enter");
        if (this.helpText) this.helpText.SetActive(true);
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (this.helpText) this.helpText.SetActive(false);
    }
}
