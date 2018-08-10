using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChangeMusicVolume : MonoBehaviour {

    public Slider Volume;
    public AudioSource gameMusic;
	
	// Update is called once per frame
	void Update () {
        gameMusic.volume = Volume.value;
	}
}
