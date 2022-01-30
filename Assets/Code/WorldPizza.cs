using UnityEngine;

public class WorldPizza : MonoBehaviour {

    //Script should be on the empty that parents the rest of the stage, with its object tagged with WorldFlip so the game driver can find it OnAwake at the start of the stage.


    public Rigidbody bim;
    public float FlipDuration = 0.3f;

    public GameObject fgRed;
    public GameObject fgBlue;

    private float flipTime = 0;
    private bool isFlipping = false;
    private GameDriver gameDriver;
    public bool startWithFlip = true;
    public bool unlimitedFlips = false;
    void Start() {
        gameDriver = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameDriver>();
        gameDriver.connectWorldFlipper(this.gameObject);//attempt to give this object over to the driver to actually use it.
    }

    //is called by the gamedriver to let us know that it's time for the room to flip. Immediately go back and tell the driver that we can no longer flip so we can't hold the button to make it go forever. That is a thing otherwise.
    public void flipRoom()
    {
        if(!unlimitedFlips)gameDriver.flip = false;
        this.flipTime = 0;
        this.isFlipping = true;
        if (this.bim) this.bim.isKinematic = true;
        this.fgRed.SetActive(!this.fgRed.activeInHierarchy);
        this.fgBlue.SetActive(!this.fgBlue.activeInHierarchy);
    }

    // Update is called once per frame. Spin the room.
    void Update() {
        if (this.isFlipping) {
            this.flipTime = Mathf.Min(this.flipTime + Time.deltaTime / FlipDuration, 1f);
            this.gameObject.transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * 180 / FlipDuration), Space.Self);
            if (this.flipTime == 1f) {
                this.isFlipping = false;
                Transform transform = this.gameObject.transform;
                transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Round(transform.rotation.eulerAngles.z / 180f) * 180f);
                if (this.bim) this.bim.isKinematic = false;
            }
        }
    }

    void Spin() {

    }
}
