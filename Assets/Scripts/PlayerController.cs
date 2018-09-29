using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Player controller class coordinating user input and player status and functions
public class PlayerController : MonoBehaviour
{

	[SerializeField] LayerMask collisionMask;	// Collision mask reference

	Player player;                        // Player class reference
	Rigidbody rb;							            // Player Rigidbody
	Camera playerHead;					        	// Main Camera beeing plaer head
	[SerializeField]  UIController ui;		// Global UI controller

	const float walkingSpeed = 3.0f;	    // Base walking speed
	const float runningSpeed = 6.0f;	    // Base running speed
	const float JumpSpeed = 5.0f;	        // Jump speed ( height )
	const float headRotationSpeed = 2.0f;	// Head ( camera ) rotation speed
	const float rayDistance = 1.5f;	      // Collision check ray distance

	string playerStatus;					        // Current player status (standing/walking/running/jumping etc.)


	float horizontalMovementInput;		  	// Horizontal movement axis input (let, right, A, D)
	float verticalMovementInput;		    	// Vertical movement axis input (forward, backwards, W, S)
	float horizontalRotationInput;	  		// Mouse horizontal movement axis ( camera rotation left and right )
	float verticalRotationInput;		    	// Mouse vertical movement axis ( camera rotation up and down )

	bool isGrounded;					            // Is the player "standing" on the ground flag ( collision check against the ground below the player object )
	bool isFiring;						          	// Is player currently firing ? ( set by CheckInput() )

	float currentMovementSpeed = 0;			  // Current player movement speed (including input, running speed and bonus values)
	float currentSpeedBonus = 1.0f;		  	// Current movement speed multiplier ( modified by powerUps )
	float currentJumpBonus = 1.0f;		  	// Current jump speed multiplier ( modified by powerUps )


	[SerializeField] Transform gun;			  // Player gun reference ( bullets instantiation position )
	[SerializeField] Transform bullet;		// Bullet prefab reference


	//Mouse look smoothing
	//Mouse movement input is going to be averaged in order to improve camera behavior
	const int smoothingSteps = 8;				          // number of input steps to be taken into calculation
	List<float> rotArrayX = new List<float> ();		// list of X axis inputs
	List<float> rotArrayY = new List<float> ();		// list of Y axis inputs
	float totalMouseInputX = 0.0f;						    // sum of X axis inputs
	float totalMouseInputY = 0.0f;						    // sum of Y axis inputs


	void Start ()
	{
		//player = GetComponent<Player> ();			    	   // Assigning reference to Player class implementation
		player = GetComponent<PlayerAdvanced> ();		     // Assigning reference to Player class extension allowing additional abilities implementation
		rb = GetComponent<Rigidbody> ();				         // Assigning reference to Player Rigidbody
		playerHead = GetComponentInChildren<Camera> ();  // Assigning reference to main camera playing role of player head/eyes
	}
	

	void Update()
	{
		isGrounded = player.CheckIfGrounded (rayDistance, collisionMask);							// Check if player is standing on the ground
		CheckInput ();																			                        	// Check user input

		player.Rotate (new Vector3 (0, horizontalRotationInput, 0));								  // Rotate player body based on user mouse input
		player.TurnHead (playerHead.transform, new Vector3 (-verticalRotationInput, 0.0f, 0.0f));	// Rotate player head based on user mouse input

		if (isFiring)																				
			player.Fire (bullet, gun.position, playerHead.transform.rotation);					// Fire a bullet based on user mouse input

		ui.SetPlayerStatus (playerStatus);															              // Update player status UI text
	}

	void FixedUpdate () 
	{

		if (CheckIfCanMove () || !isGrounded)
			player.Move (rb, CalculateMovementVector (1.0f));											      // Move player based on user keyboard input

		if (Input.GetButton ("Jump") && isGrounded)
			player.Jump (rb, JumpSpeed * currentJumpBonus);												      // Make player jump based on user keyboardinput
	}

	void CheckInput()
	{

    bool isRunning = Input.GetKey (KeyCode.LeftShift);                            // Check if user is holding let shift button
    currentMovementSpeed = currentSpeedBonus * (isRunning ? runningSpeed:walkingSpeed);
    playerStatus = isRunning ? "Running":"Walking";

		horizontalMovementInput = Input.GetAxisRaw ("Horizontal") * currentMovementSpeed;			// Get horizontal movement user input
		verticalMovementInput = Input.GetAxisRaw ("Vertical") * currentMovementSpeed;			  	// Get vertical movement user input

		if ( (horizontalMovementInput==0) && (verticalMovementInput==0))
			playerStatus = "Standing";																                    // If movement user input is 0, change player status accordingly

		if (!isGrounded)
			playerStatus = "Jumping";																                      // If player is not on the ground and he cannot fall it means he is jumping

		horizontalRotationInput = Input.GetAxis ("Mouse X") * headRotationSpeed;				// Get horizontal rotation user input
		verticalRotationInput = Input.GetAxis ("Mouse Y") * headRotationSpeed;					// Get vertical rotation user input
		SmoothMouse ();																				                          // Smooth mouse input to have better camera movement feedback

    isFiring = Input.GetButtonDown ("Fire1");														            // Set isFiring status based on user input
	}


	bool CheckIfCanMove()
	{
		// Check if there is a floor in front of player ( taking movement vector into consideration )

		if (Physics.Raycast (CalculateMovementVector (5.0f), Vector3.down, rayDistance, collisionMask))		// Cast a ray from player next step position downwards to check if there is a loor or not
			return true;
		else
			return false;
	}

	// Calculating movement vector multiplied by distance ( used for moving player as well as for collision check which requires further distance)
	Vector3 CalculateMovementVector(float DistanceMultiplier)
	{
		//Debug.DrawRay (transform.position + rb.transform.forward * Time.deltaTime * verticalMovementInput * DistanceMultiplier + rb.transform.right * horizontalMovementInput * Time.deltaTime * DistanceMultiplier, Vector3.down, Color.red);
		return transform.position + rb.transform.forward * Time.deltaTime * verticalMovementInput * DistanceMultiplier + rb.transform.right * horizontalMovementInput * Time.deltaTime * DistanceMultiplier;
	}


	void SmoothMouse()
	{
		// Mouse input smoothing function that calculates the average value from recent several inputs

		// X axis
    rotArrayX.Add (verticalRotationInput);															  // Add X axis input to the list 
		totalMouseInputX += verticalRotationInput;														// add this value to the sum of recent values (to avoid going through the list each step )

		if (rotArrayX.Count >= smoothingSteps)														  	//Remove latest input if limit was crossed
		{
			totalMouseInputX -= rotArrayX[0];
			rotArrayX.RemoveAt (0);
		}

		verticalRotationInput = totalMouseInputX / rotArrayX.Count;						// Calculate average value


		// Y axis
		rotArrayY.Add (horizontalRotationInput);													  	// Add Y axis input to the list 
		totalMouseInputY += horizontalRotationInput;													// add this value to the sum of recent values (to avoid going through the list each step )

		if (rotArrayY.Count >= smoothingSteps)													  		//Remove latest input if limit was crossed
		{
			totalMouseInputY -= rotArrayY[0];
			rotArrayY.RemoveAt (0);
		}

		horizontalRotationInput = totalMouseInputY / rotArrayY.Count;					// Calculate average value
	}

	public void IncreasePower( float bonusDuration, float speedBonus, float jumpBonus)
	{
		CancelInvoke ("ResetBonus");								// Cancel any bonus duration countdown to be sure that bonus will not be reseted by some older power up countdown
		currentSpeedBonus = speedBonus;							// Assign speed bonus value
		currentJumpBonus = jumpBonus;								// Assign jump bonus value
		Invoke ("ResetBonus", bonusDuration);				// Start bonus cancelation countdown
	}

	void ResetBonus()
	{
		currentSpeedBonus = 1.0f;
		currentJumpBonus = 1.0f;
	}

}
