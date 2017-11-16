using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The code class that contains all of CRT's AI and movement behavior
// Written by Khadeidre Dean and edited/optimized by Sean Boland

public class CRTMovement : MonoBehaviour
{
	private float walkingSpeed;

	public bool wait;

	public GameObject gb;
	public GameObject arm1, arm2, arm3, arm4;
	public GameObject finish;

	public BridgeLever bridge;

	public GameObject phasepoint0;
	public GameObject phasepoint1;
	public GameObject phasepoint2;
	public GameObject phasepoint3;
	public GameObject fireWalk;
	public GameObject almostThere;
	public GameObject upPoint;
	public GameObject theEnd;

	public bool phase0;
	public bool phase1;
	public bool phase2;
	public bool phase3;
	public bool phase4;
	public bool phase5;
	public bool phase6;
	public bool phase7;

	public Sprite armOut, armDown, sideways;

	void Start()
	{
		walkingSpeed = 15f;

		wait = false;

		phase0 = true;
		phase1 = false;
		phase2 = false;
		phase3 = false;
		phase4 = false;
		phase5 = false;
		phase6 = false;
		phase7 = false;

		arm1.SetActive(false);
		arm2.SetActive(false);
		arm3.SetActive(false);
		arm4.SetActive(false);
		finish.SetActive(false);
	}

	private void Update()
	{
		if (phase0) // Beginning of game
		{
			this.GetComponent<SpriteRenderer>().sprite = sideways;
			this.transform.position = Vector3.MoveTowards(this.transform.position, phasepoint0.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, phasepoint0.transform.position);
			if (dist < 1)
			{
				phase0 = false;
				phase1 = true;
			}
		}
		else if (phase1 == true) // First stop. Jump onto ridge, wait for player
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 20);
			this.transform.position = Vector3.MoveTowards(this.transform.position, phasepoint1.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, phasepoint1.transform.position);
			if (dist < 2)
			{
				phase1 = false;
				phase2 = true;
				wait = true;

				this.GetComponent<SpriteRenderer>().flipX = true;
				this.GetComponent<SpriteRenderer>().sprite = armOut;
				arm1.SetActive(true);
			}
		}
		else if (phase2 == true && wait != true) // Waiting at top of ridge for bridge to extend
		{
			this.GetComponent<SpriteRenderer>().flipX = false;
			this.GetComponent<SpriteRenderer>().sprite = armDown;
			arm1.SetActive(false);

			this.GetComponent<SpriteRenderer>().sprite = sideways;
			this.transform.position = Vector3.MoveTowards(this.transform.position, phasepoint2.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, phasepoint2.transform.position);
			if (dist < 7.5)
			{
				phase2 = false;
				phase3 = true;

				this.GetComponent<SpriteRenderer>().sprite = armDown;
			}
		}
		else if (phase3 == true && bridge.extended) // After bridge has extended, and CRT is moving to the ladder. Bottom of ladder
		{
			this.GetComponent<SpriteRenderer>().sprite = sideways;
			this.transform.position = Vector3.MoveTowards(this.transform.position, phasepoint3.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, phasepoint3.transform.position);
			if (dist < 6.5)
			{
				phase3 = false;
				phase4 = true;
				wait = true;
				this.GetComponent<SpriteRenderer>().sprite = armDown;
			}
		}
		else if (phase4 == true && wait != true) // Top of ladder. Moves to side and extends arm to give GB a way up
		{
			this.transform.position = Vector3.MoveTowards(this.transform.position, upPoint.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, upPoint.transform.position);
			if (dist < 1)
			{
				phase4 = false;
				phase5 = true;
				wait = true;

				this.GetComponent<SpriteRenderer>().flipX = true;
				this.GetComponent<SpriteRenderer>().sprite = armOut;
				arm2.SetActive(true);
			}
		}
		else if (phase5 == true && wait != true) // To fire pit. Waiting for GB to get on arm to be carried across
		{
			arm2.SetActive(false);
			this.GetComponent<SpriteRenderer>().flipX = false;
			this.GetComponent<SpriteRenderer>().sprite = sideways;
			this.transform.position = Vector3.MoveTowards(this.transform.position, almostThere.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, almostThere.transform.position);
			if (dist < 1)
			{
				phase5 = false;
				phase6 = true;
				wait = true;

				this.GetComponent<SpriteRenderer>().sprite = armOut;
				arm3.SetActive(true);
			}
		}
		else if (phase6 == true && wait != true) // From fire pit to gate switch. Waiting at gate switch to help player.
		{
			if (!arm3.activeSelf)
			{
				this.GetComponent<SpriteRenderer>().sprite = sideways;
			}

			this.transform.position = Vector3.MoveTowards(this.transform.position, fireWalk.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, fireWalk.transform.position);
			if (dist < 1)
			{
				phase6 = false;
				phase7 = true;
				wait = true;

				this.GetComponent<SpriteRenderer>().sprite = armOut;
				arm4.SetActive(true);
			}
		}
		else if (phase7 == true && wait != true) // Final walk to the sapling
		{
			arm4.SetActive(false);
			this.GetComponent<SpriteRenderer>().sprite = sideways;
			this.transform.position = Vector3.MoveTowards(this.transform.position, theEnd.transform.position, walkingSpeed * Time.deltaTime);

			float dist = Vector2.Distance(this.transform.position, theEnd.transform.position);
			if (dist < 2)
			{
				phase7 = false;
				this.GetComponent<SpriteRenderer>().sprite = armDown;
				finish.SetActive(true);
			}
		}
	}
}