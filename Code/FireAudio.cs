using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code used to play the fire audio when the player is close enough, and scale the volume with the player's distance from the source

public class FireAudio : MonoBehaviour
{
	public GameObject player;
	public bool playingAudio = false;
	private AudioSource audioSource;
	public float distVolume = 0.01f;

	void Start()
	{
		player = GameObject.Find("Player");
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		float dist = Vector3.Distance(transform.position, player.transform.position);

		if (playingAudio)
		{
			audioSource.volume = 1 - (dist * distVolume) - 0.3f;
			if (audioSource.volume >= 0.7)
			{
				audioSource.volume = 0.7f;
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playingAudio = true;
			audioSource.Play();
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			playingAudio = false;
			audioSource.Stop();
		}
	}
}
