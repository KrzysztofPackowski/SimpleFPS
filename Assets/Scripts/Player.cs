using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Player class providing basic player functionality
public class Player : MonoBehaviour
{

	// Player body rotation method
	public void Rotate(Vector3 bodyRotationVector)
	{
		transform.eulerAngles += bodyRotationVector;

	}

	// Player head rotation method
	public void TurnHead(Transform head, Vector3 headRotationVector)
	{
		head.eulerAngles += headRotationVector;
	}

	// Player movement method
	public void Move(Rigidbody rb, Vector3 movementVector)
	{
		rb.MovePosition (movementVector);
	}

	// Player jumping method
	public void Jump (Rigidbody rb, float jumpSpeed)
	{
		rb.velocity = new Vector3 (0, jumpSpeed, 0);
	}

	// Player iring method
	public void Fire(Transform bulletPrefab, Vector3 startingPosition, Quaternion barrelRotation)
	{
		Instantiate (bulletPrefab, startingPosition, barrelRotation);
	}

	// Player ground colision check method
	public bool CheckIfGrounded(float rayDistance, LayerMask collisionMask)
	{
		// Cast a ray from player position downwards to check if the player is on the ground or not
    return (Physics.Raycast (transform.position, transform.TransformDirection (Vector3.down), rayDistance, collisionMask));
	}

}
