using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SingleNote : MonoBehaviour
{
	GameManager gameManager;
	float deathLineX = -11;
	private void Start()
	{
		gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

	}
	// Update is called once per frame
	void Update()
    {
        transform.Translate(- Config.instance.speed * Time.deltaTime, 0, 0);
		if(transform.position.x < deathLineX)
		{
			Destroy(this.gameObject);
		}
    }
}
