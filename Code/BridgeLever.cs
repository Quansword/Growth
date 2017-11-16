using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code used for the bridge, water, and gate. Can be modified for use with any system where one object needs to move to another after a lever is pulled

public class BridgeLever : MonoBehaviour
{
	[HideInInspector]
	public bool extend;
	[HideInInspector]
	public bool extended;
	[HideInInspector]
	public bool leverPull;

	private GameObject lever;
	private GameObject bridge;
	private GameObject endLocation;
	public GameObject wall = null;

	private bool wallExt;
    private bool playedAudio = false;

	private AudioSource audioSource;

	void Start()
	{
		extend = false;
		extended = false;
		lever = transform.GetChild(0).gameObject;
		bridge = transform.GetChild(1).gameObject;
		endLocation = transform.GetChild(2).gameObject;
		wallExt = false;
		leverPull = false;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		if (extend)
		{
			float step = 10 * Time.deltaTime;
			bridge.transform.position = Vector2.MoveTowards(bridge.transform.position, endLocation.transform.position, step);

			if (audioSource != null && bridge.transform.position != endLocation.transform.position && !playedAudio) // If a sound is to be played as the "bridge" is extending
			{
				audioSource.Play();
				playedAudio = true;
			}

			if (!leverPull) // Modifies the lever's position and rotation if it is being pulled for the first time
			{
				lever.transform.localPosition = new Vector3(lever.transform.localPosition.x + 0.31f, lever.transform.localPosition.y - 1.29f, lever.transform.localPosition.z);
				lever.transform.localEulerAngles = new Vector3(lever.transform.localEulerAngles.x, lever.transform.localEulerAngles.y, lever.transform.localEulerAngles.z + 23.964f);
				leverPull = true;
			}

			if (Vector2.Distance(bridge.transform.position, endLocation.transform.position) < 0.01f) // When the "bridge" reaches its destination
			{
				extend = false;
				extended = true;
				return;
			}

			if (!wallExt && wall != null) // If an invisible wall is attached to the system, activate it
			{
				wallExt = true;
				wall.SetActive(true);
			}
		}
	}
}
