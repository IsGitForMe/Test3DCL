using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombMove : MonoBehaviour {
    
	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		if (true)
        {
            transform.position = transform.position + (transform.forward - transform.position) * 0.005f;
        }
	}
}
