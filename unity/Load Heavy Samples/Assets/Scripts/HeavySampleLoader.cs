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

	// Where we'll place our samples to load
	public List<AudioClip> sampleList;

	void Start () {
		
		// Start the process
		loadNextSample ();

		// Always useful to know...
		Debug.Log ("HEAVY SAMPLE RATE : " + AudioSettings.outputSampleRate);
	}

	private void loadNextSample() {		
		
		// If we have samples in our list
		if (sampleList.Count > 0) {
			
			// Take the first sample in the list
			currentClip = sampleList [0];

			// Now that we've got that sample stored as currentClip, remove it from sampleList
			sampleList.RemoveAt (0);

			sampleName = currentClip.name;

			if (!currentClip) {
				Debug.LogError ("Error loading " + sampleName + " - not found");
				this.loadNextSample ();
				return;
			}

			// Tell Unity to load the clip data from the device storage 
			currentClip.LoadAudioData ();

			// Send the sample data to Heavy
			extractSampleDataToHeavy ();
		} else {
			
			// No samples left - we must be done
			Debug.Log ("Done loading samples!");

			// Here is a good place to send a notification that all the 
			// samples are now loaded to other parts of your Scene
		}
	}


	private void extractSampleDataToHeavy() {
		
		// Create a float array for our samples
		audioBuffer = new float[currentClip.samples];

		// Fill audioBuffer with the samples from the first (and should be only) channel of the clip
		// Note: here might be a good place to put a warning if you're accidentally using a stereo sample
		// as this method assumes that you've done all of your deinterleaving already i.e. are only using mono samples
		currentClip.GetData (audioBuffer, 0);

		if (currentClip.samples > 0) {
			
			// Fill a table in the Heavy plugin (must already exist and have the right name)
			heavyWrapper.FillTableWithFloatBuffer (sampleName, audioBuffer);
			Debug.Log ("Loaded sample: " + sampleName);
		} else {
			Debug.Log ("Just tried calling FillTableWithFloatBuffer with " + currentClip.samples + " samples for " + sampleName);
		}

		// Start the process again
		loadNextSample ();

	}
}