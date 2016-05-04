using UnityEngine;
using System.Collections;
using UnityEngine.Audio;
using ArmadHeroes;

public class OptionsMenu : MonoBehaviour {

	public AudioMixer gameAudioMixer;

	// Use this for initialization

	void Awake()
	{
		DefaultVolumes(0,0); //I think 0 is the default volume as it's done in Db. 
	}

	// Update is called once per frame
	void Update () 
	{
		
	}

	public void DefaultVolumes(float musicLevel, float sfxLevel)
	{
		gameAudioMixer.SetFloat ("MusicVol", musicLevel);
		gameAudioMixer.SetFloat ("SFXVol", sfxLevel);

		//This may set the two channels to default. not entirely sure if that is it. If not the above lines just set it to the default value
		//gameAudioMixer.ClearFloat ("MusicVol");
		//gameAudioMixer.ClearFloat ("SFXVol");
	}

	public void SetMusicVolume (float musicLevel)
	{
		gameAudioMixer.SetFloat ("MusicVol", musicLevel);
	}

	public void SetSFXVolume(float sfxLevel)
	{
		gameAudioMixer.SetFloat ("SFXVol", sfxLevel);
	}

}
