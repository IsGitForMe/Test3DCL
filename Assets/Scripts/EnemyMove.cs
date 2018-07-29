using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour {
    [SerializeField]
    float DetectionRadius;
    //private bool found = false;
    [SerializeField]
    private int MaxHitPoints = 4;
    int realHitPoints = 0;
    GameObject player;
	// Use this for initialization
	void Start ()
    {
        realHitPoints = MaxHitPoints;
        Debug.Log(realHitPoints+" realHitPoints");
	}
    bool FindPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, DetectionRadius,1 << LayerMask.NameToLayer("Player"));
        if (players.Length > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void HitPointsChange(int damage)
    {
        realHitPoints = realHitPoints - damage;
        Debug.Log(realHitPoints+" hit change");
        Debug.Log("damage hit "+damage);
        Death(realHitPoints);
    }
    public void Death(int realHitPoints)
    {
        if (realHitPoints <= 0)
        {
            //if (explosion != null)
            //{
            //    Instantiate(explosion, gameObject.transform.position, gameObject.transform.rotation);
            //    Debug.Log("Death");
            //}
            //GameObject qiv = Instantiate(Quiver, transform.position, Quaternion.identity);
            //qiv.tag = "Quiver";
            //Debug.Log("LR " + qiv.transform.localEulerAngles);
            //var locRotation = qiv.transform.localEulerAngles;
            //locRotation.z = 180;
            //qiv.transform.localEulerAngles = locRotation;
            //Destroy(gameObject);
            gameObject.SetActive(false);
        }
    }
    void MoveToPlayer()
    {
        Collider[] players = Physics.OverlapSphere(transform.position, DetectionRadius, 1 << LayerMask.NameToLayer("Player"));
        if (FindPlayer())
        {
            Vector3 delta = transform.position - players[0].gameObject.transform.position;
            delta.Normalize();
            transform.position = transform.position - delta * 1 * Time.deltaTime;
        }
        //found = false; came to it
    }
	// Update is called once per frame
	void Update ()
    {
        MoveToPlayer();
    }
}
//int a = Random.Range(1,10);
//if (a >= 5)
//{
//    transform.position = transform.position + new Vector3(0.07f, 0, 0);
//}
//else
//{
//    transform.position = transform.position + new Vector3(-0.07f, 0, 0);
//}
//int b = Random.Range(1, 100);
//if (b >= 50)
//{
//    transform.position = transform.position + new Vector3(0, 0, 0.07f);
//}
//else
//{
//    transform.position = transform.position + new Vector3(0, 0, -0.07f);
//}
//if ((a > 6)&&(b > 61))
//{
//    transform.position = transform.position + new Vector3(0, 0.2f, 0);
//}