using UnityEngine;
using System.Collections;
[RequireComponent(typeof(AudioSource))]

public class AS_SoundOnHit : MonoBehaviour {

	private AudioSource audiosource;
	public AudioClip[] Sounds;
	void Start () {
		if(audio != null && Sounds!=null && Sounds.Length>0){
			audiosource = audio;	
			audiosource.pitch = Time.timeScale;
			if(audiosource.pitch<0.5f){
				audiosource.pitch = 0.5f;
			}
			audiosource.PlayOneShot(Sounds[Random.Range(0,Sounds.Length)]);
		}
	}
	void Update(){
		if(audiosource != null && Sounds!=null && Sounds.Length>0){
			audiosource.pitch = Time.timeScale;
			if(audiosource.pitch<0.5f){
				audiosource.pitch = 0.5f;
			}
		
		}
	}
	

}
