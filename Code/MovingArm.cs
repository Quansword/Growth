using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Used to move the platform that acts as CRT's arm when CRT is moving across the fire pit

public class MovingArm : MonoBehaviour
{
	private Transform arm;

	public GameObject player;
	public GameObject CRT;

	private bool onBoard;

	void Start()
	{
		arm = transform.GetChild(0);
		onBoard = false;
	}

	void Update()
	{
		// When the player gets on CRT's arm
		if (!onBoard && arm.gameObject.GetComponent<BoxCollider2D>().IsTouching(player.GetComponent<Collider2D>()) && arm.localPosition.x < 25)
		{
			onBoard = true;

			if (CRT.GetComponent<CRTMovement>().wait)
			{
				CRT.GetComponent<CRTMovement>().wait = false;
			}
		}

		// Moving the arm to CRT's position when CRT is walking over the fire pit
		if (onBoard)
		{
			float oldX = arm.position.x;
			Vector2 newPos = new Vector2(CRT.transform.position.x + 2, CRT.transform.position.y);

			float step = 30 * Time.deltaTime;
			arm.position = Vector2.MoveTowards(arm.position, newPos, step);

			// Adding the change in position to the player's position so they are not left behind
			player.transform.position = new Vector2(player.transform.position.x + (arm.position.x - oldX), player.transform.position.y);

			if (arm.localPosition.x >= 28) // The arm has moved over the firepit and should now be deactivated
			{
				onBoard = false;
				gameObject.SetActive(false);
				return;
			}
		}
	}
}
