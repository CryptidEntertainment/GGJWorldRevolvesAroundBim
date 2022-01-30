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
	public bool doubleJumpOnFlip = false;
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
	private float flipAnimLength = 0.0f; //essentially the same thing for the flip animation
	private float flipAnimStart = 0.0f;
	public Animator anim;
	
	//Gamedriver and State
	private GameDriver gameDriver;
	public PlayerState state = PlayerState.Controllable;


	//audio
	private PlayerAudio plAud;


	//button buffer
	private bool selectIsDown = false;

	private int ignoreExit = 0;
    


    // Use this for initialization
    void Awake()
    {
		//GameObject.DontDestroyOnLoad (gameObject);

		if (startPoint != null) {
			this.transform.position = startPoint.position;
		}

		gameDriver = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameDriver> ();
		plAud = gameObject.GetComponent<PlayerAudio>();
		if (preJumpAnimation) preJumpAnimLength = preJumpAnimation.length;
		if (flipAnimation) flipAnimLength = flipAnimation.length;
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
			if (phys.velocity.x < 0)
			{
				if (Input.GetAxis("Horizontal") < 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0), ForceMode2D.Force);
					if (canJump)
					{
						anim.SetInteger("animState", 1);//walk
						plAud.playBimWalk();
					}
					else
                    {
						plAud.stopPlaying();
                    }
				}
				else if (Input.GetAxis("Horizontal") > 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed + phys.velocity.x*phys.mass*-5f, 0), ForceMode2D.Force);
					if (canJump)
					{
						anim.SetInteger("animState", 1);//walk
						plAud.playBimWalk();
					}
					else
					{
						plAud.stopPlaying();
					}
				}
				else if (Input.GetAxis("Horizontal")==0)
				{
					phys.AddForce(new Vector2(phys.velocity.x*phys.mass*-20f,0), ForceMode2D.Force);
					anim.SetInteger("animState", 0);
					plAud.stopPlaying();
				}
			}
			else if (phys.velocity.x > 0)
			{
				if (Input.GetAxis("Horizontal") < 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed + phys.velocity.x * phys.mass * -5f, 0), ForceMode2D.Force);
					if (canJump)
					{
						anim.SetInteger("animState", 1);//walk
						plAud.playBimWalk();
					}
					else
					{
						plAud.stopPlaying();
					}
				}
				else if (Input.GetAxis("Horizontal") > 0)
				{
					phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed, 0), ForceMode2D.Force);
					if (canJump)
					{
						anim.SetInteger("animState", 1);//walk
						plAud.playBimWalk();
					}
					else
					{
						plAud.stopPlaying();
					}
				}
				else if (Input.GetAxis("Horizontal") == 0)
				{
					phys.AddForce(new Vector2(phys.velocity.x * phys.mass * -20f, 0), ForceMode2D.Force);
					anim.SetInteger("animState", 0);//idle
					plAud.stopPlaying();
				}
			}
			else if (phys.velocity.x == 0 && Input.GetAxis("Horizontal")!=0)
			{
				phys.AddForce(new Vector2(Input.GetAxis("Horizontal") * moveSpeed*20, 0), ForceMode2D.Force);
				if (canJump)
				{
					anim.SetInteger("animState", 1);//walk
					plAud.playBimWalk();
				}
				else
				{
					plAud.stopPlaying();
				}
			}
			else if (phys.velocity.x==0 && Input.GetAxis("Horizontal")==0)
			{
				if (canJump)
				{
					anim.SetInteger("animState", 0);//idle
					plAud.stopPlaying();
				}
			}

			phys.velocity = Vector3.ClampMagnitude(phys.velocity, moveSpeed);
			

			//Now for the jumping part of the code!
			if (Input.GetAxis("Vertical") > 0)
			{
				if (canJump && jumpCD==0)
				{
					if (preJump == false)
					{
						plAud.stopPlaying();
						preJump = true;
						preJumpAnimStart = Time.time+Time.deltaTime;
						jumpCD = 30;
						anim.SetInteger("animState", 2);//jump
					}
				}
			}
			if (preJump == true)
			{
				//this does all the prejump stuff to play the animation before leaving the ground.
				if (Time.time >= preJumpAnimStart + preJumpAnimLength)
				{
					preJump = false;
					canJump = false;
					anim.SetInteger("animState", 3);//in-air
					phys.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
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
		}
		if (Input.GetAxis("Flip") != 0 && gameDriver.flip == true)
		{
			
			Debug.Log("Should Flip");
			flipAnimStart = Time.time + Time.deltaTime;
			anim.SetInteger("animState", 5);
			plAud.playBimFlip();
			phys.gravityScale = 0;
			state = PlayerState.Flipping;
			GetComponent<Collider2D>().enabled = false;
			GetComponent<Rigidbody2D>().simulated = false;
			foreach(Collider2D coll in this.gameObject.GetComponentsInChildren<Collider2D>())
            {
				coll.enabled = false;
            }
			gameDriver.doFlip();
		}
		if (phys.velocity.x > 0) GetComponent<SpriteRenderer>().flipX = false;
		if (phys.velocity.x < 0) GetComponent<SpriteRenderer>().flipX = true;
		if (jumpCD > 0) jumpCD--;
		if (state == PlayerState.Flipping)
        {
			phys.velocity = Vector2.zero;
			anim.SetInteger("animState", 5);
			//Debug.Log("Currently flipping. | " + anim.GetInteger("animState"));
			if (Time.time >= flipAnimStart + flipAnimLength)
			{
				state = PlayerState.Controllable;
				phys.gravityScale = 1.0f;
				GetComponent<Collider2D>().enabled = true;
				GetComponent<Rigidbody2D>().simulated = true;
				foreach (Collider2D coll in this.gameObject.GetComponentsInChildren<Collider2D>())
				{
					coll.enabled = true;
				}
			}
			if(doubleJumpOnFlip)canJump = true;
		}
	}

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

	//presently unused trigger that could be used for something or other.
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


	//we've hit the something. Make sure it's the ground and that we're above it so we can jump again.
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
                    isAbove = true;
                    canJump = true;
					plAud.playBimLand();
					anim.SetInteger("animState", 4);//land

                }
            }
        }
    }

	//we've left the ground somehow, make sure we know we're up in the air now.
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (!GetComponent<CapsuleCollider2D>().IsTouchingLayers(64))//bit check specifically for unity physics layer 6
        {
            canJump = false;
			anim.SetInteger("animState", 3);//in-air
			plAud.stopPlaying();
        }
    }
}
