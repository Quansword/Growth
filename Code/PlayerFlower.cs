using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// The code class written to handle the collision behanvior of the player's flower
// Mostly used to pass information to Player.cs

public class PlayerFlower : MonoBehaviour
{
	[HideInInspector]
	public bool colliding;
	[HideInInspector]
	public bool handle;
	[HideInInspector]
	public bool lever;
	[HideInInspector]
	public bool arm;
	[HideInInspector]
	public BridgeLever leverScript = null;

	public Sprite fStill;
	public Sprite fRight;
	public Sprite fLeft;

	void Start()
	{
		colliding = false;
		handle = false;
		lever = false;
		arm = false;
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		colliding = true;

		if (collision.gameObject.tag == "Handle")
		{
			handle = true;
		}
		else if (collision.gameObject.tag == "Lever")
		{
			if (!collision.gameObject.GetComponentInParent<BridgeLever>().leverPull)
			{
				lever = true;
				leverScript = collision.transform.parent.GetComponent<BridgeLever>();
			}
		}
		else if (collision.gameObject.tag == "Arm")
		{
			arm = true;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		colliding = false; if (collision.gameObject.tag == "Handle")
		{
			handle = false;
		}
		else if (collision.gameObject.tag == "Lever")
		{
			if (!collision.gameObject.GetComponentInParent<BridgeLever>().leverPull)
			{
				lever = false;
				leverScript = null;
			}
		}
		else if (collision.gameObject.tag == "Arm")
		{
			arm = false;
		}
	}
}
