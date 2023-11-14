using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditPanel : MonoBehaviour
{
	public void OnClickClose()
	{
		this.gameObject.SetActive(false);
	}
	public void OnClickConfig()
	{
		this.gameObject.SetActive(true);
	}
}
