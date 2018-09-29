using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Moving platform class responsible or "moving" objects standing on the platorm
public class MovingPlatform : MonoBehaviour{

	// Make a child from any object that is a player of an enemy
	void OnCollisionEnter(Collision collision)
	{
    if (PlayerOrEnemy(collision))
		{
			if (transform.position.y < collision.transform.position.y) 
			{
				collision.transform.parent = this.transform;
			}
		}
	}

	// Remove player or enemy parent objects once they left the platform
	void OnCollisionExit(Collision collision)
	{
    if (PlayerOrEnemy(collision))
		{
			collision.transform.parent = null;
		}
	}


  bool PlayerOrEnemy( Collision collision)
  {
    return (collision.transform.tag == "Player" || collision.transform.tag == "Enemy");
  }

}
