using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDriver : MonoBehaviour {

	private static GameDriver _gameDriver;

	//public string[] levels;
	public int levelCount = 4;
	public int currentLevel = 0;
	public WorldPizza worldFlipper;

	private static bool resetPressed = false;
	private static float buttonBuffer = 0.5f;
	private static float bufferTimer = 0f;

	public Vector3 cameraPos = Vector3.zero;
	public float cameraSize = 0f;


	//info whether the player can flip the stage, used by Bim and any flip crystals.
	public bool canFlip; // private field for the flip
	public bool flip   // property
	{
		get { return canFlip; }   // get method
		set { canFlip = value; }  // set method
	}

	public void doFlip()
	{
		if (canFlip)
		{
			worldFlipper.flipRoom();
			canFlip = false;
		}
	}


	//Ok so I'm giving the driver the code for the world flipper to call it, and also set that it can flip because this will actually reliably run every reload while awake and start kind of just don't a lot of the time????
	public void connectWorldFlipper(GameObject flpp)
    {
		worldFlipper = flpp.GetComponent<WorldPizza>();
		//Debug.Log("Now I'm trying to put it in its own damn function because WTF.");
		//Debug.Log("Here's what we got: " + worldFlipper);
		canFlip = worldFlipper.startWithFlip;
	}

	void Awake()
	{
		//Debug.Log("Does it even run this??? AWAKE version.");
		if (!_gameDriver)
		{
			_gameDriver = this;
		}
		else
		{
			GameObject.Destroy(this.gameObject);
		}
		canFlip = true;
		//Debug.Log("I totally did the thing! | " + canFlip);
		if (currentLevel > 0)
		{
			//Debug.Log("Yes");
			Camera.main.transform.position = cameraPos;
			Camera.main.orthographicSize = cameraSize;
		}
	}

	// Use this for initialization
	void Start() {
		//Debug.Log("Does it even run this???");
		GameObject.DontDestroyOnLoad(this.gameObject);
		flip = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.Escape)){
			Application.Quit();
		}
		if (Input.GetAxis ("Reset") > 0) {
			if (!resetPressed) {
				resetPressed = true;
				bufferTimer = buttonBuffer;
				ReloadLevel ();
			}
		} else {
			bufferTimer -= Time.deltaTime;
			if (bufferTimer <= 0) {
				resetPressed = false;
			}
		}

	}
			
	public void ReloadLevel(){
		SceneManager.LoadScene (SceneManager.GetActiveScene().name);
	}

	public void GoToNextLevel(){
		currentLevel++;
		if (currentLevel > levelCount) {
			currentLevel = 1;
			//TODO:load final
		}
		if (currentLevel == 4) {
			SceneManager.LoadScene ("LevelX");
		} else {
			SceneManager.LoadScene ("Level" + currentLevel);
		}
	}

}
