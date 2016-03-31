using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {


    public int PlayerNumber = 1;
    Transform enemy;

    Rigidbody2D rig2d;
    Animator anim;

    float horizontal;
    float vertical;
    public float maxSpeed = 25;
    Vector3 movement;
    bool crouch;

    public float JumpForce = 20;
    public float JumpDuration = .1f;
    float jmpDuration;
    float jmpForce;
    bool jumpKey;
    bool falling;
    bool onGround;

	// Use this for initialization
	void Start () {

        rig2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
	}
    void Update()
    {

        UpdateAnimator();

    }
    // Update is called once per frame
    void FixedUpdate () {
        horizontal = Input.GetAxis("Horizontal" + PlayerNumber.ToString());
        vertical = Input.GetAxis("Vertical"+PlayerNumber.ToString());

        Vector3 movement = new Vector3(horizontal, 0, 0);

        crouch = (vertical < 0.1f);

        if (vertical > 0.1f)
        {
            if (!jumpKey) {
                jmpDuration += Time.deltaTime;
                jmpForce += Time.deltaTime;

                if (jmpDuration < JumpDuration)
                {
                    rig2d.velocity = new Vector2(rig2d.velocity.x, jmpForce);
                }
                else {
                    jumpKey = true;

                }
            }
        }

        if (!onGround && vertical < 0.1f)
        {
            falling = true;
        }
        if (!crouch)
            rig2d.AddForce(movement * maxSpeed);
        else
            rig2d.velocity = Vector3.zero;
    }

 

    void UpdateAnimator() {

        anim.SetBool("Crouch", crouch);
        anim.SetBool("OnGround", this.onGround);
        anim.SetBool("Falling",this.falling);
        anim.SetFloat("Movement", Mathf.Abs(horizontal));
    }

    void OnCollisionEnter2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            onGround = true;

            jumpKey = false;
            jmpDuration = 0;
            jmpForce = JumpForce;
            falling = false;
        }

    }
    void OnCollisionExit2D(Collision2D col)
    {
        if (col.collider.tag == "Ground")
        {
            onGround = false;
        }

    }
}
