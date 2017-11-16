using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Code class containing all behavior for the player character: GB

public class Player : MonoBehaviour
{
	private float speed;
	private float runForce;
	private float decelerationForce;
	private Rigidbody2D body;

	[HideInInspector]
	public bool extended;
	private bool movable;
    
	private bool flowerHit;
	private bool flowerHandle;
	private bool flowerArm;
	private float hitTime;

	private bool grateCollide;
	private GameObject collideObject;
	private AudioSource audioSource;

	public AudioClip buttonPress;
	public AudioClip grapple;

	public Sprite pStill;
	public Sprite pRight;
	public Sprite pLeft;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();

		speed = 20;
		runForce = 5000;
		decelerationForce = 4000;
		extended = false;
		movable = true;
		flowerHit = false;
		flowerHandle = false;
		flowerArm = false;
		hitTime = 0;
		grateCollide = false;
		audioSource = GetComponent<AudioSource>();
	}

	void Update()
	{
		Extend();
		GrateBreak();

		if (movable)
		{
			PlayerMovement();
		}
	}

	void PlayerMovement() // Handles all player movement
	{
		float velocityX = Input.GetAxisRaw("Horizontal");
		Vector2 movement;

		// Sprite changes
		if (velocityX > 0.5f && GetComponent<SpriteRenderer>().sprite != pRight)
		{
			GetComponent<SpriteRenderer>().sprite = pRight;
			transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = transform.GetChild(1).gameObject.GetComponent<PlayerFlower>().fRight;
		}
		else if (velocityX < -0.5f && GetComponent<SpriteRenderer>().sprite != pLeft)
		{
			GetComponent<SpriteRenderer>().sprite = pLeft;
			transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = transform.GetChild(1).gameObject.GetComponent<PlayerFlower>().fLeft;
		}
		else if (velocityX == 0 && GetComponent<SpriteRenderer>().sprite != pStill)
		{
			GetComponent<SpriteRenderer>().sprite = pStill;
			transform.GetChild(1).gameObject.GetComponent<SpriteRenderer>().sprite = transform.GetChild(1).gameObject.GetComponent<PlayerFlower>().fStill;
		}

		// Decelerating the player if they are not giving input
		if (velocityX == 0 && Mathf.Abs(body.velocity.x) > 0.5f)
		{
			movement = new Vector2(-(Mathf.Sign(body.velocity.x)) * decelerationForce * Time.deltaTime, 0);
			body.AddForce(movement);
		}
		else if (velocityX == 0 && Mathf.Abs(body.velocity.x) < 0.5f && Mathf.Abs(body.velocity.x) > 0) // Stopping the player
		{
			body.velocity = new Vector2(0, body.velocity.y);
		}
		else if (collideObject == null || (collideObject.tag != "SeeSaw" && collideObject.tag != "Slope")) // Movement code for normal terrain
		{
			if (velocityX != 0 && Mathf.Abs(body.velocity.x) < speed)
			{
				if ((body.velocity.x < 0 && velocityX > 0) || (body.velocity.x > 0 && velocityX < 0))
				{
					body.velocity = new Vector2(0, body.velocity.y);
				}

				movement = new Vector2(velocityX * runForce * Time.deltaTime, 0);
				body.AddForce(movement);
			}
		}
		else // Movement code for slopes and seesaws. Angle needs to be taken in to account to compensate with proper upward force
		{
			float angle = collideObject.transform.eulerAngles.z;

			if (angle > 180)
			{
				angle = -(360 - angle);
			}

			float vSpeed = speed * Mathf.Abs(angle) * 0.02f;

			if (velocityX != 0 && Mathf.Abs(body.velocity.x) < speed)
			{
				if ((body.velocity.x < 0 && velocityX > 0) || (body.velocity.x > 0 && velocityX < 0))
				{
					body.velocity = new Vector2(0, body.velocity.y);
				}

				if ((velocityX <= 0 && angle >= 0) || (velocityX > 0 && angle <= 0))
				{
					vSpeed = 0;
				}
				else
				{
					vSpeed = Mathf.Abs(velocityX) * vSpeed;
				}

				body.velocity = new Vector2(body.velocity.x, vSpeed);
				movement = new Vector2(velocityX * runForce * Time.deltaTime, 0);
				body.AddForce(movement);
			}
		}
	}

	void Extend() // Code for extending or retracting the flower
	{
		if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) // Extending
		{
			if (body.velocity.x != 0) // Stop movement
			{
				Vector2 movement;

				if (Mathf.Abs(body.velocity.x) > 0.5f)
				{
					movement = new Vector2(-(Mathf.Sign(body.velocity.x)) * decelerationForce * Time.deltaTime, 0);
					body.AddForce(movement);
				}
				else if (Mathf.Abs(body.velocity.x) < 0.5f && Mathf.Abs(body.velocity.x) > 0)
				{
					body.velocity = new Vector2(0, body.velocity.y);
				}
			}

			extended = true;
			movable = false;
			GameObject stem = transform.GetChild(0).gameObject;
			GameObject flower = transform.GetChild(1).gameObject;

			if (!flowerHit && flower.GetComponent<PlayerFlower>().colliding) // If the flower has collided with anything
			{
				flowerHit = true;
				hitTime = Time.fixedTime;
			}

			if (stem.transform.localScale.y < 11.25f && !flowerHit) // Extends the flower as long as it has not hit anything or reached its max height
			{
				float yScale = stem.transform.localScale.y + (10.75f * Time.deltaTime);
				float yPosition = stem.transform.localPosition.y + (13.63f * Time.deltaTime);
				float fPosition = flower.transform.localPosition.y + (27.15f * Time.deltaTime);

				if (yScale > 11.25f)
				{
					yScale = 11.25f;
				}
				if (yPosition > 15.5f)
				{
					yPosition = 15.5f;
				}
				if (fPosition > 30f)
				{
					fPosition = 30f;
				}

				stem.transform.localScale = new Vector3(0.1f, yScale, 0.1f);
				stem.transform.localPosition = new Vector3(0, yPosition, 0);
				flower.transform.localPosition = new Vector3(0, fPosition, 0);
			}
		}
		else if (extended) // Retracting the flower
		{
			GameObject stem = transform.GetChild(0).gameObject;
			GameObject flower = transform.GetChild(1).gameObject;

			if (stem.transform.localScale.y > 0.5f) // If the flower is longer than minimum length
			{
				if (flowerHit) // If the flower hit anything, find out what and handle it
				{
					flowerHit = false;
					hitTime = 0;
					flower.GetComponent<PlayerFlower>().colliding = false;

					if (flower.GetComponent<PlayerFlower>().handle)
					{
						if (!flowerHandle)
						{
							GetComponent<AudioSource>().clip = grapple;
							audioSource.Play();
						}
						flowerHandle = true;
						GetComponent<Rigidbody2D>().gravityScale = 0;
					}
					else if (flower.GetComponent<PlayerFlower>().lever)
					{
						GetComponent<AudioSource>().clip = buttonPress;
						GetComponent<AudioSource>().volume = 1;
						audioSource.Play();
						GetComponent<AudioSource>().volume = 0.5f;
						flower.GetComponent<PlayerFlower>().lever = false;
						flower.GetComponent<PlayerFlower>().leverScript.extend = true;
					}
					else if (flower.GetComponent<PlayerFlower>().arm)
					{
						if (!flowerArm)
						{
							GetComponent<AudioSource>().clip = grapple;
							audioSource.Play();
						}
						flowerArm = true;
						GetComponent<Rigidbody2D>().gravityScale = 0;
					}
				}

				// Retract the flower
				float yScale = stem.transform.localScale.y - (32.25f * Time.deltaTime);
				float yPosition = stem.transform.localPosition.y - (40.89f * Time.deltaTime);
				float fPositionChange = (81.45f * Time.deltaTime);
				float fPosition = flower.transform.localPosition.y - fPositionChange;

				if (yScale <= 4.5f && !movable && !flowerHandle)
				{
					movable = true;
				}

				if (yScale < 0.5f)
				{
					yScale = 0.5f;
				}
				if (yPosition < 1.87f)
				{
					yPosition = 1.87f;
				}
				if (fPosition < 2.85f)
				{
					fPosition = 2.85f;
				}

				stem.transform.localScale = new Vector3(0.1f, yScale, 0.1f);
				stem.transform.localPosition = new Vector3(0, yPosition, 0);
				flower.transform.localPosition = new Vector3(0, fPosition, 0);

				if (flowerHandle)
				{
					transform.position = new Vector3(transform.position.x, transform.position.y + fPositionChange);
				}
				else if (flowerArm)
				{
					flower.GetComponent<BoxCollider2D>().isTrigger = false;
					transform.position = new Vector2(transform.position.x, transform.position.y + fPositionChange);
				}

				// Reset variables once the flower has retracted
				if (yScale == 0.5f && yPosition == 1.87f && fPosition == 2.85f)
				{
					extended = false;

					if (flowerHandle)
					{
						flowerHandle = false;
						movable = true;
						GetComponent<Rigidbody2D>().gravityScale = 15;
						flower.GetComponent<PlayerFlower>().handle = false;
					}
					else if (flowerArm)
					{
						body.velocity = new Vector2(0, 50);
						flowerArm = false;
						movable = true;
						GetComponent<Rigidbody2D>().gravityScale = 15;
						flower.GetComponent<PlayerFlower>().arm = false;
					}
				}
			}
			else // Reset variables if nothing is going on
			{
				extended = false;
				movable = true;

				if (flowerHit)
				{
					flowerHit = false;
					hitTime = 0;
					flower.GetComponent<PlayerFlower>().colliding = false;
				}
			}
		}
	}

	// Code made to push grates out from under the player when they push them down
	void GrateBreak()
	{
		if (flowerHit && grateCollide && hitTime != 0 && hitTime + 0.5 < Time.fixedTime)
		{
			collideObject.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
			collideObject.GetComponent<Rigidbody2D>().mass = 10;
			collideObject.GetComponent<Rigidbody2D>().gravityScale = 30;
			collideObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -200), ForceMode2D.Impulse);
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0, -10), ForceMode2D.Impulse);
			//collideObject.GetComponent<Rigidbody2D>().AddTorque(100, ForceMode2D.Impulse);

			grateCollide = false;
			collideObject = null;
		}
	}

	private void OnCollisionStay2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Grate" && !grateCollide)
		{
			grateCollide = true;
			collideObject = collision.gameObject;
		}
		else if (collision.gameObject.tag == "SeeSaw" || collision.gameObject.tag == "Slope")
		{
			collideObject = collision.gameObject;
		}
	}

	private void OnCollisionExit2D(Collision2D collision)
	{
		if (collision.gameObject.tag == "Grate")
		{
			grateCollide = false;
			collideObject = null;
		}
		else if (collision.gameObject.tag == "SeeSaw" || collision.gameObject.tag == "Slope")
		{
			collideObject = null;
		}
	}
}
