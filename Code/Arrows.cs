using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// As of yet unused code that would use on-screen arrows to show the player possible inputs when they have idled for long enough

public class Arrows : MonoBehaviour
{
	private void Awake()
	{
		GameObject player = GameObject.Find("Player");
		player.GetComponent<Player>();
	}

	// Use this for initialization
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{

	}
}
