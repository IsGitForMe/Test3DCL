using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFind : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        gameObject.transform.localEulerAngles = gameObject.transform.localEulerAngles + new Vector3(0,1,0);
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Destroy(gameObject);
            if (gameObject.name == "AxeItem")
            {
                collision.gameObject.GetComponent<PlayerMovement>().playerSlots[1] = collision.gameObject.GetComponent<PlayerMovement>().playerAxe;
            }
            else if (gameObject.name == "BowItem")
            {
                collision.gameObject.GetComponent<PlayerMovement>().playerSlots[0] = collision.gameObject.GetComponent<PlayerMovement>().playerBow;
            }
        }
    }
}
