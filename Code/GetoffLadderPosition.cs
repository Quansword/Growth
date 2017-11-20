using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Part of the ladder code. This code is executed when CRT is getting off the ladder

public class GetoffLadderPosition : MonoBehaviour
{
	public GameObject topOfLadder;

	// CRT gettinbg off the ladder changes him back to normal and disables the ladder components now that they're not needed
	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("crt"))
		{
			other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			other.GetComponent<Rigidbody2D>().velocity = new Vector2();
			other.GetComponent<CRTMovement>().wait = false;
			topOfLadder.SetActive(false);
			gameObject.SetActive(false);
		}
	}
}
