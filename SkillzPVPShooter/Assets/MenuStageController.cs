using UnityEngine;
using System.Collections;

public class MenuStageController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		Skillz.skillzInitForGameIdAndEnvironment("989", Skillz.SkillzEnvironment.SkillzSandbox);
		Skillz.launchSkillz(Skillz.SkillzOrientation.SkillzLandscape);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
