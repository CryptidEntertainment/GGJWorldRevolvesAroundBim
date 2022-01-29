using UnityEngine;
using UnityEngine.SceneManagement;

public class Door : MonoBehaviour {
    public string DestinationScene;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter(Collision collision) {
        if (this.DestinationScene != "") {
            SceneManager.LoadScene(this.DestinationScene, LoadSceneMode.Single);
        }
    }
}
