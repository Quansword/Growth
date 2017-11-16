using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBlock : MonoBehaviour
{
	private bool destination = false;

	private void Start()
	{
		GetComponent<Rigidbody2D>().sharedMaterial.friction = 0.1f;
	}

	private void Update()
	{
		// Changes the block to kinematic once it reaches its destination, and makes it posible for GB to climb
		if (destination && GetComponent<Rigidbody2D>().velocity.x <= 0.0001f && GetComponent<Rigidbody2D>().velocity.y <= 0.0001f)
		{
			GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);
			GetComponent<Rigidbody2D>().freezeRotation = true;
			GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			GetComponent<Rigidbody2D>().sharedMaterial.friction = 0.1f;
			GetComponent<Rigidbody2D>().useFullKinematicContacts = true;
			tag = "Slope";

			return;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		// Quickly makes it hard to move the block in case it tries to slide off an inclide before settling
		if (collision.gameObject.tag == "Destination" || collision.gameObject.tag == "SeeSaw")
		{
			GetComponent<Rigidbody2D>().sharedMaterial.friction = 1000;
			destination = true;
		}
	}
}
