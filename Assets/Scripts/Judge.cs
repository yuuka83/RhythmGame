using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Judge : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 startPos = (Vector2)transform.position + Vector2.right;
        Vector2 endPos = (Vector2)transform.position - Vector2.right;
        Debug.DrawLine(startPos, endPos, Color.red);
        if (Input.GetKeyDown(KeyCode.Q))
		{
            RaycastHit2D hit2D = Physics2D.Linecast(startPos, endPos);
			if (hit2D)
			{
                Debug.Log("ぶつかった");
                Destroy(hit2D.collider.gameObject);
			}
		}
    }
}
