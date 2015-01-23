using UnityEngine;
using System.Collections;

public class PlayButtonBehaviour : MonoBehaviour {

	void Awake(){
		Skillz.skillzInitForGameIdAndEnvironment("989", Skillz.SkillzEnvironment.SkillzSandbox);
	}

	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void PlayButtonClick(){
		Skillz.launchSkillz(Skillz.SkillzOrientation.SkillzLandscape);
		//Application.LoadLevel ("Demo");
	}

	public void HelpButtonClick(){

	}


}
