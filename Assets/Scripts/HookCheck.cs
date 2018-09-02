using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookCheck : MonoBehaviour {
 //   public bool hookCheck = false;
 //   public bool playerCheck = false;
	//// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        //hookCheck = Physics.Raycast(transform.position,transform.forward,0.5f,1 << LayerMask.NameToLayer("Ground"));
        Debug.DrawRay(transform.position, transform.forward,Color.blue, 100);
    }

    public bool GroundGrap()
    {
        return Physics.Raycast(transform.position, transform.forward, 0.5f, 1 << LayerMask.NameToLayer("Ground"));
    }
}
