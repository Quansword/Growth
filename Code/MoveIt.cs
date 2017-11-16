using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code attached to triggers that allow CRT to move to his next AI phase when they come in contact with the player

public class MoveIt : MonoBehaviour
{
    public CRTMovement mine;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            mine.wait = false;
            gameObject.SetActive(false); // Deactivates the trigger after use
        }
    }
}
