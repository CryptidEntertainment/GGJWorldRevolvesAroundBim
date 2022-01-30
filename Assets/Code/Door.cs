using UnityEngine;

public class Door : MonoBehaviour {
    public string DestinationScene;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D collider) {
        this.transform.Find("Frame").gameObject.SetActive(true);

        if (this.DestinationScene != "") {
            StartCoroutine(FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, this.DestinationScene));
        }
    }
}
