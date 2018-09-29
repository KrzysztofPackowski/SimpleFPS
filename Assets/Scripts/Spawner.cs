using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
	// PowerUp coniguration values can be different with each spawner
	[SerializeField] float resetTime = 10.0f;					    // PowerUp respawn timer 
	[SerializeField] float bonusDuration = 10.0f;				  // Number of seconds beore powerup bonus will be removed
	[SerializeField] float speedBonusValue = 1.5f;				// Running speed bonus value
	[SerializeField] float jumpBonusValue = 1.5f;			  	// Jump speed bonus value

	[SerializeField] Transform powerUpPrefab;					    // PowerUp symbol prefab
	Transform powerUp;											              // PowerUp Transform reference

	Vector3 offset = new Vector3(0,1.2f,0);						    // PowerUp symbol height offset ( how high is icon floating over the spawner )

	// Initial powerUp instantiation
	void Start ()
	{
		if (!powerUp)
		{
			CreatePowerUp ();
		}
	}

	// Method that will start respawn counter oncepowerup is picked up
	public void ResetPowerUp()
	{
		Invoke ("CreatePowerUp", resetTime);
	}

	// PowerUp respawn and configuration method
	void CreatePowerUp()
	{
		powerUp = Instantiate (powerUpPrefab, transform.position + offset, Quaternion.identity);
		PowerUp script = powerUp.GetComponent<PowerUp> ();
		script.SetBonus (bonusDuration,	speedBonusValue, jumpBonusValue);	
		script.SetSpawner (this.transform);
	}
	
}
