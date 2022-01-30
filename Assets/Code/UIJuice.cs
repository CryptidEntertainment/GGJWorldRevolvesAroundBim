using UnityEngine;

public class UIJuice : MonoBehaviour {
    private float t = 0;

    private void Awake() {
        this.t = 0;
    }

    private void Update() {
        this.t += Time.deltaTime;

        if (FindObjectOfType<GameDriver>().canFlip) {
            RectTransform transform = this.GetComponent<RectTransform>();
            float scale = Mathf.Max(1, Mathf.Sin(this.t * 2) / 2 + 0.6f);
            transform.localScale = new Vector3(scale, scale, 1);
        } else {
            transform.localScale = new Vector3(0, 0, 1);
        }
    }
}
