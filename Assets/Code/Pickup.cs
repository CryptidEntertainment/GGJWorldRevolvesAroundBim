using UnityEngine;

public class Pickup : MonoBehaviour {
    public string DestinationScene;

    private void OnTriggerEnter2D(Collider2D collider) {
        Debug.Log("pickedup a pickup");
    }
}
