using UnityEngine;

public class Cryptid : MonoBehaviour {
    public GameObject helpText;
    public GameObject[] texts;

    private float textTime = 0;
    private float textStayDuration = 3;

    private void Update() {
        if (this.helpText && this.helpText.activeInHierarchy) {
            this.textTime = Mathf.Min(this.textTime + Time.deltaTime / textStayDuration, this.texts.Length - 1);
            this.texts[(int)Mathf.Max(0, this.textTime - 1)].SetActive(false);
            this.texts[(int)this.textTime].SetActive(true);
        }
    }

    void OnTriggerEnter2D(Collider2D collider) {
        if (collider.tag == "Player") {
            if (this.helpText) this.helpText.SetActive(true);
            this.textTime = 0;
            foreach (GameObject text in this.texts) {
                if (text) text.SetActive(false);
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider) {
        if (collider.tag == "Player") {
            if (this.helpText) this.helpText.SetActive(false);
            foreach (GameObject text in this.texts) {
                if (text) text.SetActive(false);
            }
        }
    }
}
