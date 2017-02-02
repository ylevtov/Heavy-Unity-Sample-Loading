using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System;
using System.IO;
using Parameter = Hv_simple_sample_load_AudioLib.Parameter;

public class HeavySampleLoader : MonoBehaviour
{
	public Hv_simple_sample_load_AudioLib heavyWrapper;

	private string sampleName = "";

	private AudioClip currentClip;
	private float[] audioBuffer;

	public List<AudioClip> sampleList;
	void Awake() {

	}

	// Use this for initialization
	void Start () {

		loadNextSample ();

		Debug.Log ("HEAVY SAMPLE RATE : " + AudioSettings.outputSampleRate);

	}

	private void loadNextSample() {

		if (sampleList.Count > 0) {
			currentClip = sampleList [0];
			sampleList.RemoveAt (0);

			sampleName = currentClip.name;

			if (!currentClip) {
				Debug.LogError ("Error loading " + sampleName + " - not found");
				this.loadNextSample ();
				return;
			}

			currentClip.LoadAudioData ();
			extractSampleDataToHeavy ();
		} else {
			
		}
	}


	private void extractSampleDataToHeavy() {

		audioBuffer = new float[currentClip.samples * currentClip.channels];
		currentClip.GetData (audioBuffer, 0);

		if (currentClip.samples > 0) {
			heavyWrapper.FillTableWithFloatBuffer (sampleName, audioBuffer);
			Debug.Log ("Loaded sample: " + sampleName);
		} else {
			Debug.Log ("Just tried calling FillTableWithFloatBuffer with " + currentClip.samples + " samples for " + sampleName);
		}

		loadNextSample ();

	}


	// Update is called once per frame
	void Update () {

	}

}