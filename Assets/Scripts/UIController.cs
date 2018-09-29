using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{

	// UI fields references
	[SerializeField] Text playerStatus;					// Player status text field
	[SerializeField] Text powerUpDuration;			// PowerUp countdown text field
	[SerializeField] Text powerUpStatus;				// PowerUp values text field
		
	float activePowerUpDuration;						    // Remaining time of currently active bonus

	// Method that updates player status text field based on player status
	public void SetPlayerStatus(string newStatus)
	{
		playerStatus.text = newStatus;
	}

	// Method lauched by PowerUp once it's picked up. It's starting PowerUP UI configuration process.
	public void SetUIText(float newBonusDuration, float newSpeedBonus, float newJumpBonus)
	{
		ResetPowerUpBonuses ();										        	// Clear previous bonus if any was active
		SetPowerUpDuration (newBonusDuration);							// Activate bonus duration countdown panel
		SetPowerUpStatus (newSpeedBonus, newJumpBonus);			// Setup bonus values text panel
	}

	// Method that activates bonus duration countdown panel
	void SetPowerUpDuration( float newBonusDuration)
	{
		activePowerUpDuration = newBonusDuration;
		SetPowerUpDurationText(activePowerUpDuration);
		InvokeRepeating ("ReducePowerUpCounter", 1.0f, 1.0f);			// Update remaining bonus time each second
	}

	// Method that updates powerUp bonus text panels
	void SetPowerUpStatus( float newSpeedBonus, float newJumpBonus)
	{
    string speedBonusText = (newSpeedBonus * 100 - 100).ToString ();
    string jumpBonusText = (newJumpBonus * 100 - 100).ToString ();

    powerUpStatus.text = "Speed bonus: " + speedBonusText + "%\nJumpBonus: "+ jumpBonusText + "%";
	}

	// Bonus duration countdown method
	void ReducePowerUpCounter()
	{
		if (--activePowerUpDuration <= 0)
			ResetPowerUpBonuses ();
		else
			SetPowerUpDurationText(activePowerUpDuration);
	}

	// Method that clears any UI bonus activity back to idle state
	void ResetPowerUpBonuses()
	{
		CancelInvoke ("ReducePowerUpCounter");
		powerUpDuration.text = "";
		powerUpStatus.text = "";
	}

	// Method that returns proper bonus duration text based on remaining time
	void SetPowerUpDurationText( float duration)
	{
		powerUpDuration.text = "PowerUp Time: "+duration.ToString();
	}
}

