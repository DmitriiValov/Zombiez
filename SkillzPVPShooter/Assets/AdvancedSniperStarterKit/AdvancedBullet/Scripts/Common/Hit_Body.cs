using UnityEngine;
using System.Collections;

public class Hit_Body : AS_BulletHiter {
	private GameController gameController;

	void Start(){
		GameObject gameControllerObject = GameObject.Find ("GameController");
		gameController = gameControllerObject.GetComponent<GameController> ();
	}

	public override void OnHit (RaycastHit hit,AS_Bullet bullet)
	{

		if(this.RootObject.GetComponent<Status>()){
			this.RootObject.GetComponent<Status>().ApplyDamage(bullet.Damage,bullet.transform.forward * bullet.HitForce,Random.Range(0,2));

			// increment kills counter;
			if (gameController.IsGame()){
				gameController.ChangeKillsCounter(1);
			}
		}
		AddAudio(hit.point);
		base.OnHit (hit,bullet);
	}
}
