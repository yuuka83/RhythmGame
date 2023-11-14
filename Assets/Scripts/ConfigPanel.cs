using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfigPanel : MonoBehaviour
{
	[SerializeField]
	AudioSource audioSource;
	
	public void OnClickClose()
	{
		this.gameObject.SetActive(false);
	}
	public void OnSoundValueChange(float newSliderValue)
	{
		Debug.Log(newSliderValue);
		audioSource.volume = newSliderValue;
	}
	public void OnClickShowPanel()
	{
		this.gameObject.SetActive(true);
	}

}
