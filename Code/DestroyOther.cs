using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOther : MonoBehaviour
{
	public GameObject fireAudio;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// Used to destroy the fire audio when the fire emitters are destroyed
		if (collision.CompareTag("Water"))
		{
			Destroy(fireAudio);
		}
	}
}
