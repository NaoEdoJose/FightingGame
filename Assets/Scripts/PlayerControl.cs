using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{

    public int PlayerNum = 1;
    public float maxSpeed = 25;

    Transform enemy;

    Rigidbody2D rb2d;
    Animator anim;

    float horizontal;
    float vertical;

    Vector3 movement;
    bool crouch;

    public float JumpForce = 20;
    public float jumpDuration = 0.1f;
    float jmpDuration;
    float jmpForce;
    bool jumpKey;
    bool falling;
    bool onGround;

    public float attackRate = 0.3f;
    bool[] attack = new bool[2];
    float[] attacktimer = new float[2];
    int[] timesPressed = new int[2];


    // Use this for initialization
    void Start()
    {

        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();

		jmpForce = JumpForce;

		GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");

		foreach (GameObject pl in players) {
		
			if(pl.transform != this.transform){

				enemy = pl.transform;
			}

		}




    }

    void UpdateAnimator()
    {

        anim.SetBool("Crouch", crouch);
        anim.SetBool("OnGround", this.onGround);
        anim.SetBool("Falling", this.falling);
        anim.SetFloat("Movement", Mathf.Abs(horizontal));
        anim.SetBool("Attack1", attack[0]);
        anim.SetBool("Attack2", attack[1]);
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            onGround = true;

            jumpKey = false;
            jmpDuration = 0;
            jmpForce = JumpForce;
            falling = false;
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.collider.tag == "Ground")
        {
            onGround = false;

        }
    }

    void Update()
    {
        AttackInput();
		ScaleCheck ();
		UpdateAnimator();
        OnGroundCheck();

    }
    void FixedUpdate()
    {

        horizontal = Input.GetAxis("Horizontal" + PlayerNum.ToString());
        vertical = Input.GetAxis("Vertical" + PlayerNum.ToString());

        Vector3 movement = new Vector3(horizontal, 0, 0);

        crouch = (vertical < -0.01f);

        if (vertical > 0.1f)
        {
            if (!jumpKey)
            {
                jmpDuration += Time.deltaTime;
                jmpForce += Time.deltaTime;

                if (jmpDuration < jumpDuration)
                {

                    rb2d.velocity = new Vector2(rb2d.velocity.x, jmpForce);
                }
                else
                {
                    jumpKey = true;
                }
            }
        }
        
        if (attack[0] && !jumpKey || attack[1] && !jumpKey)
        {
            movement = Vector3.zero;
        }

        if (!onGround && vertical < 0.1f)
        {

            falling = true;
        }


        if (!crouch)
            rb2d.AddForce(movement * maxSpeed);
        else
            rb2d.velocity = Vector3.zero;
    }

    void OnGroundCheck()
    {
        if (!onGround)
        {
            rb2d.gravityScale = 4;
        }
        else
        {
            rb2d.gravityScale = 1;
        }
    }

	void ScaleCheck(){
	 	
		if (transform.position.x < enemy.position.x)
			transform.localScale = new Vector3 (-1, 1, 1);
		else
			transform.localScale = Vector3.one;
	
	}

    void AttackInput()
    {
        //attack1 region
     #region
        if (Input.GetButtonDown("Attack1" + PlayerNum.ToString()))
        {

            attack[0] = true;
            attacktimer[0] = 0;
            timesPressed[0]++;
        }

        if (attack[0])
        {
            attacktimer[0] += Time.deltaTime;
            if (attacktimer[0] > attackRate || timesPressed[0] >= 4)
            {

                attacktimer[0] = 0;
                attack[0] = false;
                timesPressed[0] = 0;
            }
        }
        #endregion //attack1

        //attack2 region
    #region
        if (Input.GetButtonDown("Attack2" + PlayerNum.ToString()))
        {

            attack[1] = true;
            attacktimer[1] = 0;
            timesPressed[1]++;
        }

        if (attack[0])
        {
            attacktimer[1] += Time.deltaTime;
            if (attacktimer[1] > attackRate || timesPressed[1] >= 4)
            {

                attacktimer[1] = 0;
                attack[1] = false;
                timesPressed[1] = 0;
            }
        }
        #endregion

    }
}