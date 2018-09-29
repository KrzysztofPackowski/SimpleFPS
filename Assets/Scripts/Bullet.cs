using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

	// Bullet configuration values
	const float bulletSpeed = 10.0f;		    	// Bullet flight speed
  const float bulletDamage = 50.0f;					// Bullet damage
  const float bulletRange = 10.0f;					// Bullet range (how long beore it will disapper if it finds no obstacle)


	void Start()
	{
		Invoke("SelfDestroy", bulletRange);	     // Start self destruction method in "bulletRange" time
	}


	void Update()
	{
		transform.Translate(Vector3.forward * bulletSpeed * Time.deltaTime);	// Make bullet fly forward each frame
	}


	// Bullet collision message
	void OnCollisionEnter( Collision other)
	{
		if (other.transform.tag == "Enemy")					// If bullet hits the enemy, apply damage to this enemy
		{
			Enemy script = other.transform.GetComponent<Enemy> ();		
			script.ApplyDamage (bulletDamage);
		}
		Destroy(this.gameObject);										// Destroy the bulletat the end no matter what was hit
	}

	void SelfDestroy()
	{
		Destroy(this.gameObject);										// Destroy the bullet 
	}
}
