using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	Renderer r;					              // Enemy object renderer ( used to change material color depending on remaining hit points )
	const float maximumHp = 255.0f;		// Enemy max hp value ( used for color calculation )
	float currentHp;                  // Current enemy hit points value


	// Enemy initialization
	void Start () 
	{
    currentHp = maximumHp;					  // maximising current hp value
		r = GetComponent<Renderer> ();		// Assining rendered reerence
	}

	// Enemy apply damage method
	public void ApplyDamage(float damage)
	{
    currentHp -= damage;               // reduce current hp based on received damage
		if (CheckIfAlive ())               // Check if playeris still alive
		{
      r.material.color = new Color (1.0f - currentHp/maximumHp , currentHp/maximumHp, 0.0f);	// if alive update material color based on remaining hp
      return;
		}
		else
			KillSelf ();                      // If enemy hp <=0 kill this enemy
	}

	// Method checking if hp went down to 0 or below
	bool CheckIfAlive()
	{
    return currentHp > 0;
	}

	// Method removing "dead" enemy object
	void KillSelf()
	{
		Destroy (this.gameObject);
	}


}
