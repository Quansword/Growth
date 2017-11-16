using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
	public GameObject player;
	[HideInInspector]
	public Vector3 m_DesiredPosition; // The point we want the camera rig to be at
	private Vector3 m_MoveVelocity; // Deals with the damping. The function will use them
	[HideInInspector]
	public float m_DampTime = 0.2f; // How long the camera takes to move to it's target location

	void Start()
	{
		GameObject.Find("Player");
	}

	private void LateUpdate()
	{
		if (m_DampTime == 0.2f && player.GetComponent<Player>().extended)
		{
			m_DampTime = 0.4f;
		}
		else if (m_DampTime == 0.4f && !player.GetComponent<Player>().extended)
		{
			m_DampTime = 0.2f;
		}
		MoveCamera(); // Moves the camera to the player
	}

	private void MoveCamera() // Moves the camera to track the player
	{
		FindAveragePosition();
		// Moves to the center position of the player gradually
		transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
	}

	private void FindAveragePosition() // Finds the center position of player
	{
		Vector3 averagePos = new Vector3();

		averagePos = player.transform.position;

		averagePos.y += 10;

		averagePos.z = -46.2f;

		m_DesiredPosition = averagePos;
	}
}
