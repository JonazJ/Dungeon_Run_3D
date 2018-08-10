using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour {

    public AudioSource Volume;
    public Slider volSlider;

	// Update is called once per frame
	void Update () {
        Volume.volume = volSlider.value;
	}
}
