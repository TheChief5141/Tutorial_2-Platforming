using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatController : MonoBehaviour {

	public float speed;
	public bool moveRight = true;

	// Use this for initialization
	void Update () 
    {
		// Use this for initialization
		if(moveRight) 
        {
            Debug.Log("Move Right");
			transform.Translate(2 * Time.deltaTime * speed, 0,0);
			transform.localScale= new Vector2 (0.4f,0.4f);
 		}
		else
        {
            Debug.Log("Move Left");
			transform.Translate(-2* Time.deltaTime * speed, 0,0);
			transform.localScale= new Vector2 (-0.4f,0.4f);
		}
	}
	void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Marker")
        {
            Debug.Log("Made Contact");

			if (moveRight)
            {
				moveRight = false;
			}
			else
            {
				moveRight = true;
			}	
		}
	}
}