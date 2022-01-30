using UnityEngine;

public class MainMenuControls : MonoBehaviour {
    public void Play() {
        StartCoroutine(FindObjectOfType<SceneFader>().FadeAndLoadScene(SceneFader.FadeDirection.In, "Scenes/Levels/Level 1"));
    }

    public void Quit() {
        Application.Quit();
    }
}
