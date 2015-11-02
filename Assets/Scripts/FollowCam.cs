
/**
 * Author: Jeremy Gibson, with mods by Matt Evett
 * 2014,5
 */

using UnityEngine;
using System.Collections;

public class FollowCam : MonoBehaviour {

	static public FollowCam  S; // a FollowCam Singleton 
	
	// fields set in the Unity Inspector pane 
	public float 			easing = 0.05f; // Eases the camera's movement/interpolation
	public Vector2			minXY; // Default is [0,0].  Min value for camera's position
	// Following variable name acts to "separate" the dynamically set variables
	public bool              _____________________________; 
	
	// fields set dynamically 
	public GameObject        poi; // The point of interest 
	public float             camZ; // The desired Z pos of the camera 
	
	void Awake() { 
		S = this; 
		camZ = this.transform.position.z; 
	} 
	
	void FixedUpdate () { 
		Vector3 destination; 

		// If there is no poi, return to P:[0,0,0], to center the camera
		//  on the slingshot again
		if (poi == null) { 
			destination = Vector3.zero; //(N.B.: The slingshot is at the origin)
		} else { 
			// Get the position of the poi 
			destination = poi.transform.position; 
			// If poi is a Projectile, check to see if it's at rest 
			if (poi.tag == "Projectile") { 
				// if it is sleeping (that is, not moving) 
				if ( poi.GetComponent<Rigidbody>().IsSleeping() ) { 
					// return to default view 
					poi = null; 
					// in the next update 
					return; 
				} 
			}
		}

		// Limit the X & Y against minimum values 
		destination.x = Mathf.Max( minXY.x, destination.x ); 
		destination.y = Mathf.Max( minXY.y, destination.y ); 
		// Interpolate from current Camera position toward the destination
		//   3rd arg determines the "weighting" between the 1st and 2nd args
		destination = Vector3.Lerp (transform.position,destination, easing);
		// Maintain destination.z (camera stays at same distance from XY-plane)
		destination.z = camZ; 
		// Set the camera to the destination 
		transform.position = destination; 
		// Set the orthographicSize of the Camera to keep Ground in view.
		// Because the cameraY can never move below 0, by expanding the size of the
		// view volume we can guarantee that the 
		this.GetComponent<Camera>().orthographicSize = destination.y + 10; 
	} 
}
