using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcSoundTank : MonoBehaviour {

	[Range(-1f, 1f)]
	public float offset;

	public float cutoffOn = 600;
	public float cutoffOff = 200;
	public bool engineOn;

	private float cutoffEase = 0.2f;
	private float cutoffValue = 200f;


	System.Random rand = new System.Random();
	AudioLowPassFilter lowPassFilter;

	void Awake() {
		lowPassFilter = GetComponent<AudioLowPassFilter>();
		Update();
	}

	void OnAudioFilterRead(float[] data, int channels) {
		for (int i = 0; i < data.Length; i++) {
			data[i] = (float)(rand.NextDouble() * 2.0 - 1.0 + offset);
		}
	}

	void Update() {
		if (engineOn) {
			cutoffValue = (1.0f - cutoffEase) * cutoffValue + cutoffEase * cutoffOn;
		} else {
			cutoffValue = (1.0f - cutoffEase) * cutoffValue + cutoffEase * cutoffOff;
		}

		lowPassFilter.cutoffFrequency = cutoffValue;
	}

	public void Reset() {
		engineOn = false;
		cutoffValue = cutoffOff;
	}

}
