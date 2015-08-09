using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour {
	 
	public int PlayerNum = 1;
	public float maxSpeed = 25;

	Transform enemy;

	Rigidbody2D rb2d;
	Animator anim;
	
	float horizontal;
	float vertical;

	Vector3 movement;
	bool crouch;

	// Use this for initialization
	void Start () {

		rb2d = GetComponent<Rigidbody2D> ();
		anim = GetComponentInChildren<Animator> ();


	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	
		horizontal = Input.GetAxis("Horizontal" + PlayerNum.ToString());
		vertical = Input.GetAxis ("Vertical" + PlayerNum.ToString ());

		Vector3 movement = new Vector3 (horizontal, 0, 0);

		crouch = (vertical < -0.01f);

		if (vertical > 0.1f) {
		}

		if (!crouch)
			rb2d.AddForce (movement * maxSpeed);
		else
			rb2d.velocity = Vector3.zero;
	}
}
