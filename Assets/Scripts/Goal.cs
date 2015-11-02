﻿using UnityEngine;
using System.Collections;

public class Goal : MonoBehaviour {
	// A static field accessible by code anywhere 
	static public bool        goalMet = false; 
	
	void OnTriggerEnter( Collider other ) { 
		// When the trigger is hit by something 
		// Check to see if it's a Projectile 
		if ( other.gameObject.tag == "Projectile" ) { 
			// If so, set goalMet to true 
			Goal.goalMet = true; 
			// Also set the alpha of the color to higher opacity 
			Color c = GetComponent<Renderer>().material.color; 
			c.a = 1; 
			//  renderer.material.color = c; 
			// The above protocol is now obsolete.  Rather than every GameObject
			// having a renderer property we use the parameterized GetComponent
			// method (inherited).
			GetComponent<Renderer>().material.color = c; 
			
		} 
	} 
} 