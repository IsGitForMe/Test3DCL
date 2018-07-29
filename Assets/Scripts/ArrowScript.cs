using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField]
    public int arrowDamage;
	// Use this for initialization
	void Start ()
    {
		
	}	
	// Update is called once per frame
	void Update ()
    {
		
	}
    public void SetArrowDamage(int arrowInputDamage)
    {
        arrowDamage = arrowInputDamage;
    }
    void OnCollisionEnter(Collision collision)
    {
        //Debug.Log("strike");
        if (collision.gameObject.tag == "Ground")
        {
            if (gameObject.activeSelf)
            {
                Debug.Log("Ground strike");
                gameObject.GetComponent<Rigidbody>().Sleep();
            }
            gameObject.SetActive(false);
        }
        else if (collision.gameObject.tag == "Enemy")
        {
            if (gameObject.activeSelf)
            {
                collision.gameObject.GetComponent<EnemyMove>().HitPointsChange(arrowDamage);
            }
            gameObject.SetActive(false);
        }
    }
}
