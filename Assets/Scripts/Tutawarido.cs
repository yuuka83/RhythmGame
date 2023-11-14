using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tutawarido : MonoBehaviour
{
	Image image;
	private void Start()
	{
		image = gameObject.GetComponent<Image>();
	}

	public void TutawaridoUp(float current,float max)
	{
		image.fillAmount = current / max;
	}
}
