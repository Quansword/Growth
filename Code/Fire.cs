using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
	public GameObject LM;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			//collision.gameObject.SetActive(false);
            LM.GetComponent<LevelLoader>().LoadLevel("GameOver"); // Goes the the game over screen if the player comes in contact with the fire
		}
		else if (collision.CompareTag("Water"))
		{
			//Destorys self, maybe add some steam or effect
			Destroy(gameObject);
		}
	}
}
