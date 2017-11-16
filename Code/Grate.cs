using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code for grates that disables their collisions when they hit the ground

public class Grate : MonoBehaviour
{
	private AudioSource audioSource;

	private void Start()
	{
		audioSource = GetComponent<AudioSource>();
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (GetComponent<Rigidbody2D>().bodyType == RigidbodyType2D.Dynamic && GetComponent<Rigidbody2D>().velocity.y == 0)
		{
			audioSource.Play();
			GetComponent<Rigidbody2D>().simulated = false;
			return;
		}
	}
}
