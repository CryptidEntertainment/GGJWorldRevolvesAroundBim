using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerState{
	Controllable,
	Flipping,
	Dying
}

public class PlayerController : MonoBehaviour
{

	public Transform startPoint;
	[Space(6)]

	[Header("Movement")]
	public float jumpPower = 1;
    public float moveSpeed = 10;
    public float accelTime = 0.1f;
    public bool preJump = false;
    public bool canJump = false;
	public bool landing = false;
    public float jumpCD = 0f;
	public Rigidbody2D phys;
	public bool canMove = true;
	[Space(8)]

	[Header("Animation")]
	public float deathTime = 1.0f;
	public float deathTimer = 0.0f;
	public AnimationClip preJumpAnimation = null;
	private float preJumpAnimLength = 0.0f; //set this to the linked preJumpAnimation's duration when the script starts
	private float preJumpAnimStart = 0.0f; //will mark the start point for the timer to go off after the duration of the jump animation time
	public AnimationClip flipAnimation = null;
	private float flipAnimLength = 0.0f;
	private float flipAnimStart = 0.0f;
	public Animator anim;
	
	//Gamedriver and State
	private GameDriver gameDriver;
	public PlayerState state = PlayerState.Controllable;




	//button buffer
	private bool selectIsDown = false;

	//animation
	/*private Vector3 transitionStartPos;
	private float transTimer;
	private float lerpRate = 1f;
	private Vector3 transitionEndPos;
	private bool doneMoving = true;
	private float transitionTravelDis;
	private float scaleFactor = 1f;
	private float deathTimer = 0f;*/

	private int ignoreExit = 0;
    


    // Use this for initialization
    void Awake()
    {
		//GameObject.DontDestroyOnLoad (gameObject);

		if (startPoint != null) {
			this.transform.position = startPoint.position;
		}

		gameDriver = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameDriver> ();
		Debug.Log(gameDriver);
		if (preJumpAnimation) preJumpAnimLength = preJumpAnimation.length;
		if (flipAnimation) flipAnimLength = flipAnimation.length;
        //phys = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

	// Update is called once per screen frame.
	void Update(){
		switch (state) {
		case PlayerState.Controllable:
			if (Input.GetAxis ("Interact") > 0) {
				if (!selectIsDown) {
					Debug.Log ("KeyPressed");
					selectIsDown = true;
				}
			} else {
				selectIsDown = false;
			}
			break;

		case PlayerState.Dying:
			if (deathTimer > 0) {
				gameObject.GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, Color.white, deathTimer%0.5f);
				deathTimer -= Time.deltaTime;
			} else {
				gameDriver.ReloadLevel ();
			}
			break;

		}
	}

    // FixedUpdate is called once per physics(??) frame; much more well regulated intervals than Update
    void FixedUpdate()
    {
		if (state == PlayerState.Controllable)
        {
			Debug.Log("Hit Flip");
			if (Input.GetAxis("Flip") != 0 && gameDriver.flip == true)
            {
				Debug.Log("Should Flip");
				gameDriver.flip = false;
				anim.SetInteger("animState", 5);
				phys.gravityScale = 0;
				GetComponent<Collider2D>().enabled = false;
			}
            if(phys.velocity.x>0)GetComponent<SpriteRenderer>().flipX = false;
            if (phys.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
            if (jumpCD > 0) jumpCD--;
			


			if (phys.velocity.x < 0)
			{
				if (Input.GetAxis("Horizontal") < 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0), ForceMode2D.Force);
					if (canJump) anim.SetInteger("animState", 1);//walk
				}
				else if (Input.GetAxis("Horizontal") > 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed + phys.velocity.x*phys.mass*-5f, 0), ForceMode2D.Force);
					if (canJump) anim.SetInteger("animState", 1);//walk
				}
				else if (Input.GetAxis("Horizontal")==0)
				{
					phys.AddForce(new Vector2(phys.velocity.x*phys.mass*-20f,0), ForceMode2D.Force);
					anim.SetInteger("animState", 0);
				}
			}
			else if (phys.velocity.x > 0)
			{
				if (Input.GetAxis("Horizontal") < 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed + phys.velocity.x * phys.mass * -5f, 0), ForceMode2D.Force);
					if (canJump) anim.SetInteger("animState", 1);//walk
				}
				else if (Input.GetAxis("Horizontal") > 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0), ForceMode2D.Force);
					if (canJump) anim.SetInteger("animState", 1);//walk
				}
				else if (Input.GetAxis("Horizontal") == 0)
				{
					phys.AddForce(new Vector2(phys.velocity.x * phys.mass * -20f, 0), ForceMode2D.Force);
					anim.SetInteger("animState", 0);//idle
				}
			}
			else if (phys.velocity.x == 0 && Input.GetAxis("Horizontal")!=0)
			{
				phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed*20, 0), ForceMode2D.Force);
				if (canJump) anim.SetInteger("animState", 1);//walk
			}
			else if (phys.velocity.x==0 && Input.GetAxis("Horizontal")==0)
			{
				if (canJump) anim.SetInteger("animState", 0);//idle
			}

			phys.velocity = Vector3.ClampMagnitude(phys.velocity, moveSpeed);
			//phys.velocity = new Vector2(Mathf.Lerp(phys.velocity.x, Input.GetAxis("Horizontal"), accelTime) * moveSpeed, phys.velocity.y);
			

			//Now for the jumping part of the code!
			if (Input.GetAxis("Vertical") > 0)
			{
				if (canJump && jumpCD==0)
				{
					if (preJump == false)
					{
						preJump = true;
						preJumpAnimStart = Time.time+Time.deltaTime;
						jumpCD = 30;
						//Debug.Log("Jump Pressed, jumping prep");
						anim.SetInteger("animState", 2);//jump
						//Debug.Log("Anim State: " + anim.GetInteger("animState"));
						//Debug.Log(preJumpAnimStart);
						//Do the prejump animation
					}
					else
                    {
						//phys.AddForce(Vector2.up * jumpCD); //This is how to do the jump force, need to find a place to put it - or an alternate way??
					}
				}
			}
			if (preJump == true)
			{
				//need to mess with prejump stuff
				if (Time.time >= preJumpAnimStart + preJumpAnimLength)
				{
					preJump = false;
					canJump = false;
					anim.SetInteger("animState", 3);//in-air
					phys.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
				}
                else
                {
					Debug.Log("Time: " +Time.time + " | preJumpAnimStart: " + preJumpAnimStart + " | preJumpAnimLength: " + preJumpAnimLength);
                }

			}
			else if (Input.GetAxis("Vertical") <= 0)
			{
				jumpCD = 0;
			}
		}
		if(phys.velocity.y<0)
		{
			phys.AddForce(new Vector2(0, -18f * phys.mass));
			//Debug.Log("Falling, trigger falling anim");
			//anim.SetInteger("animState", 4);//no change in this case, stays in the in-air animation
			//Do the falling animation
		}

		if(state == PlayerState.Flipping)
        {
			if (Time.time >= flipAnimStart + flipAnimLength)
			{
				state = PlayerState.Controllable;
				phys.gravityScale = 1.0f;
				GetComponent<Collider2D>().enabled = true;
			}
		}
	}

	/*public void SetSwitch(EmiterSwitch targetSwitch){
		Debug.Log ("Over switch");
		overSwitch = true;
        
		eSwitch = targetSwitch;
	}*/

	/*public void LeaveSwitch(){
		Debug.Log ("Leaveswitch");
		overSwitch = false;
		eSwitch = null;
	}*/

	/*public void SetInSignal(bool isIn){
		if (inWifiRange && isIn) {
			ignoreExit++;
		} else {
			inWifiRange = isIn;
			if (!inTransitionRange) {
				if (isIn) {
					this.gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
                    GetComponent<Rigidbody2D>().gravityScale = 0.1f;
                } else {
					this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
                    GetComponent<Rigidbody2D>().gravityScale = 1;
                }
			}
		}
	}*/

	/*public void SetInTransitionRange(bool isIn, TransitionDish targetDish){
		inTransitionRange = isIn;
		if (isIn) {
			dish = targetDish;
		} else if (inWifiRange) {
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
		} else {
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}
	}*/

	public void Die(){
		phys.velocity = Vector2.zero;
		phys.gravityScale = 0;
		deathTimer = deathTime;
		state = PlayerState.Dying;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "DeathBox") {
			gameDriver.ReloadLevel ();
		}
	}

	void OnTriggerExit2D(Collider2D other){
		/*if (other.gameObject.tag == "Signal") {
			Debug.Log ("Exited Signal");
			ignoreExit--;
			if (ignoreExit <= 0) {
				SetInSignal (false);
			}
		} else if(other.gameObject.tag == "EmiterSwitch"){
			if(!gameObject.GetComponent<Collider2D> ().IsTouchingLayers (11)){//bit check for unity physics layers 0, 1, 3
				LeaveSwitch();
			}
		}*/
	}


    private void OnCollisionEnter2D(Collision2D collision)

    {
        if(collision.gameObject.layer==6)
        {
            bool isAbove = false;
            ContactPoint2D[] points = collision.contacts;
            foreach(ContactPoint2D point in points)
            {
                if(point.point.y<(transform.position.y-GetComponent<SpriteRenderer>().bounds.extents.y*0.5f))
                {
                    //Debug.Log("The point: " + point.point + " Is below: " + (transform.position.y - GetComponent<SpriteRenderer>().bounds.extents.y * 0.9f));
					isAbove = true;
                    canJump = true;
					anim.SetInteger("animState", 4);//land
                    //phys.velocity = Vector2.zero;
                    //anim.SetInteger("animState", 0);
                    //jumpCD = 0;
                }
            }
            //canJump = true;
            //jumpCD = 0;
            //Do the landing animation
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        //Debug.Log("Collision Exit");
        if (!GetComponent<CapsuleCollider2D>().IsTouchingLayers(64))//bit check specifically for unity physics layer 6
        {
            //Debug.Log("Not touching ground, can't jump.");
            canJump = false;
			anim.SetInteger("animState", 3);//in-air
            //anim.SetInteger("animState", 4);//descend
        }
    }
}
