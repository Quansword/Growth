using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Part of the ladder code. This code is executed when CRT reaches the top of the ladder

public class TopofLadder : MonoBehaviour
{
	public Transform whatToMoveTowards;
	public float climbSpeed;
	public GameObject bottomOfLadder;

	private GameObject crt;
	public bool isActive;

	void OnDrawGizmos()
	{
		//Wire for start position
		Gizmos.color = Color.green;
		Gizmos.DrawWireCube(whatToMoveTowards.position, whatToMoveTowards.localScale);
	}

	void Update()
	{
		if (isActive)
		{
			crt.transform.position = Vector3.MoveTowards(crt.transform.position, whatToMoveTowards.transform.position, climbSpeed * Time.deltaTime);
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		// Making it so CRT doesn't acidentally move away while on the ladder
		if (other.CompareTag("crt"))
		{
			bottomOfLadder.SetActive(false);
			other.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
			other.GetComponent<Rigidbody2D>().velocity = new Vector2();
			crt = other.gameObject;
			isActive = true;
		}
	}
}
