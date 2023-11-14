using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*設定*/
public class Config : MonoBehaviour
{
	public static Config instance;
	private void Awake()
	{
		if(instance == null)
		{
			instance = this;
		}
	}
	public int speed;
}
