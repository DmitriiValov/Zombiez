using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Status : MonoBehaviour
{

	public GameObject[] deadbody;
	public AudioClip[] hitsound;
	public int hp = 100;
	private Vector3 velositydamage;
	
	public void ApplyDamage (int damage, Vector3 velosity)
	{
		if (hp <= 0) {
			return;
		}
		hp -= damage;
		velositydamage = velosity;
		if (hp <= 0) {
			Dead (Random.Range (0, deadbody.Length));
		}
	}
	
	public void ApplyDamage (int damage, Vector3 velosity, int deadIndex)
	{
		if (hp <= 0) {
			return;
		}
		hp -= damage;
		velositydamage = velosity;
		if (hp <= 0) {
			Dead (deadIndex);
		}
	}
	
	
	// ** Important! for Ragdoll replacement
	private AS_ActionCamera actioncam;
	private AS_RagdollReplace ragdollReplace;
	
	public void Dead (int index)
	{

		if (deadbody.Length > 0 && index >= 0 && index < deadbody.Length) {
			// this Object has removed by Dead and replaced with Ragdoll. the ObjectLookAt will null and ActionCamera will stop following and looking.
			// so we have to update ObjectLookAt to this Ragdoll replacement. then ActionCamera to continue fucusing on it.
			GameObject deadReplace = (GameObject)Instantiate (deadbody [index], this.transform.position, this.transform.rotation);

			ragdollReplace = deadReplace.GetComponent<AS_RagdollReplace> ();
			actioncam = (AS_ActionCamera)FindObjectOfType (typeof(AS_ActionCamera));
			// Focus on dead object replaced
			if (actioncam)
				actioncam.ObjectLookAt = deadReplace.gameObject;
			// copy all of transforms to dead object replaced
			CopyTransformsRecurse (this.transform, deadReplace);
			// destroy dead object replaced after 5 sec
			Destroy (deadReplace, 5);
			// destry this game object.
			Destroy (this.gameObject);
		
		}
	}
	
	// Copy all transforms to Ragdoll object
	public void CopyTransformsRecurse (Transform src, GameObject dst)
	{
		
	
		dst.transform.position = src.position;
		dst.transform.rotation = src.rotation;

		// Have to looking for root of Ragdoll and update it to ObjectLookAt in ActionCamera 
		if (actioncam) {
			if (actioncam.ObjectLookAtRoot == this.gameObject) {
			
				if (ragdollReplace != null) {
					actioncam.ObjectLookAt = ragdollReplace.LootAtObject.gameObject;
					if (ragdollReplace.rigidbody){
						ragdollReplace.rigidbody.AddForce(velositydamage,ForceMode.Impulse);
					}
				}
			}	
		}

		foreach (Transform child in dst.transform) {
			var curSrc = src.Find (child.name);
			if (curSrc) {
				CopyTransformsRecurse (curSrc, child.gameObject);
			}
		}
	}

}
