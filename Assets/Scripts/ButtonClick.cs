using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonClick : MonoBehaviour {
    [SerializeField]
    GameObject lift;
	// Use this for initialization
	void Start ()
    {
		
	}
    public void Click()
    {
        lift.GetComponent<LiftMovement>().Switch();
    }
    // Update is called once per frame
    void Update ()
    {
		
	}
}
