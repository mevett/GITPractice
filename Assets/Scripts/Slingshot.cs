using UnityEngine;
using System.Collections;

public class Slingshot : MonoBehaviour {
	// Fields set in Unity's Inspector pane
	public GameObject prefabProjectile;
	public float 			velocityMult = 4f;
	//The name of the following variable will result in what looks like a "divider" line
	//  in the Inspector pane.   Basically a nifty trick to add clarity to Unity's programmer interface.
	public bool _______________________;
	// Fields set dynamically
	public GameObject        launchPoint; 
	public Vector3           launchPos; 
	public GameObject        projectile; 
	public bool              aimingMode; 
	static public Slingshot	 S;

	
	void Awake() { 
		//Set the Slingshot singleton so that it can be accessed in ProjectileLine
		S = this;

		Transform launchPointTrans = transform.Find("LaunchPoint"); 
		launchPoint = launchPointTrans.gameObject; 
		launchPoint.SetActive( false ); 
		launchPos = launchPointTrans.position;
	} 

	void OnMouseEnter() { 
		//print("Slingshot:OnMouseEnter()"); 
		launchPoint.SetActive(true);
	} 
	
	void OnMouseExit() { 
		//print("Slingshot:OnMouseExit()"); 
		launchPoint.SetActive(false);
	} 

	void OnMouseDown() { 
		// The player has pressed the mouse button while over Slingshot 
		aimingMode = true; 
		// Instantiate a Projectile 
		projectile = Instantiate( prefabProjectile ) as GameObject; 
		// Start it at the launchPoint 
		projectile.transform.position = launchPos; 
		// Set it to isKinematic for now.  This means that the object will not be moved by
		//   the physics simulation, but will remain otherwise involved in the simulation
		projectile.GetComponent<Rigidbody>().isKinematic = true; 
	} 

	void Update() { 
		// If Slingshot is not in aimingMode, don't run this code 
		if (!aimingMode) return; 

		// Get the current mouse position in 2D screen coordinates 
		Vector3 mousePos2D = Input.mousePosition; 
		// Convert the mouse position to 3D world coordinates 
		mousePos2D.z = -Camera.main.transform.position.z; 
		Vector3 mousePos3D = Camera.main.ScreenToWorldPoint( mousePos2D ); 
		// Find the delta from the launchPos to the mousePos3D 
		Vector3 mouseDelta = mousePos3D-launchPos; 
		// Limit mouseDelta to the radius of the Slingshot SphereCollider 
		float maxMagnitude = this.GetComponent<SphereCollider>().radius; 
		if (mouseDelta.magnitude > maxMagnitude) { 
			mouseDelta.Normalize(); //Convert to unit vector 
			mouseDelta *= maxMagnitude; 
		} 
		// Move the projectile to this new position 
		Vector3 projPos = launchPos + mouseDelta; 
		projectile.transform.position = projPos; 
		
		if ( Input.GetMouseButtonUp(0) ) { 
			// The mouse has been released 
			aimingMode = false; 
			Rigidbody shotsBody = projectile.GetComponent<Rigidbody>();
			shotsBody.isKinematic = false;  // We want physics simulation to handle it now
			shotsBody.velocity = -mouseDelta * velocityMult; 
			FollowCam.S.poi = projectile; // Set camera to follow the projectile
			projectile = null; 
		} 
	} 
}
