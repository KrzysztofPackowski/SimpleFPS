using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PowerUp class responsible for proper PowerUp bonus application and PowerUp system behaviour
public class PowerUp : MonoBehaviour
{
	UIController ui;			    		// Global UI controller

	// PowerUp values received rom PowerUp spawner
	float bonusDuration;				  // PowerUp bonus duration
	float speedBonusValue;				// PowerUp movement speed bonus
	float jumpBonusValue;				  // PowerUp jump speed bonus

	Transform spawner;					  // Corresponding spawner ( received from spawner )

	void Start()
	{
		ui = GameObject.Find ("UIController").GetComponent<UIController> ();			// Initialization - find global UI controller script
	}

	// Collision with player message
	void OnTriggerEnter ( Collider other)
	{
		if (other.transform.tag == "Player") 								              	// If player collides with PowerUp symbol
		{
			AddPowers(other.transform);										                  	// Apply bonus to player 
			ui.SetUIText (bonusDuration, speedBonusValue, jumpBonusValue);		// Ask UI controller to update bonus status
			ActivateSpawner ();												                      	// Ask PowerUp spawner to start respawn countdown
			Destroy(this.gameObject);											                    // Remove PowerUp symbol
		}
	}

	// Method that applies PowerUp bonuses to player
	void AddPowers(Transform player)
	{
		PlayerController script = player.GetComponent<PlayerController> ();
		script.IncreasePower (bonusDuration,speedBonusValue,jumpBonusValue);
	}

	// Method used by poweUp spawners to set up bonus values
	public void SetBonus(float newBonusDuration, float newSpeedBonus, float newJumpBonus)
	{
		bonusDuration = newBonusDuration;
		speedBonusValue = newSpeedBonus;
		jumpBonusValue = newJumpBonus;
	}

	// Assign corresponding spawner
	public void SetSpawner(Transform newSpawner)
	{
		this.spawner = newSpawner;
	}

	// Method that activates spawner respawn countdown
	void ActivateSpawner()
	{
		Spawner script = spawner.GetComponent<Spawner> ();
		script.ResetPowerUp ();
	}
}
