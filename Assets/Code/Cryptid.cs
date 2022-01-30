using UnityEngine;

public class Cryptid : MonoBehaviour {
    public GameObject helpText;
    public GameObject[] texts;

    private int textIndex= 0;
    private bool inRange = false;

    private void Awake() {
        this.helpText.SetActive(false);
        foreach (GameObject text in this.texts) {
            text.SetActive(false);
        }
    }

    private void Update() {
        if (Input.GetButtonDown("Interact")) {
            if (this.inRange) {
                if (this.helpText.activeInHierarchy) {
                    this.texts[this.textIndex++].SetActive(false);
                    if (this.textIndex == this.texts.Length) {
                        this.helpText.SetActive(false);
                    } else {
                        this.texts[this.textIndex].SetActive(true);
                    }
                } else {
                    this.helpText.SetActive(true);
                    this.textIndex = 0;
                    this.texts[0].SetActive(true);
                }
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.gameObject.tag == "Player") {
            this.inRange = true;
            this.helpText.SetActive(true);
            this.textIndex = 0;
            this.texts[0].SetActive(true);
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        this.inRange = false;
        this.helpText.SetActive(false);
        foreach (GameObject text in this.texts) {
            text.SetActive(false);
        }
    }
}
