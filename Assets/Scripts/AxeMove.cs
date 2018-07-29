using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeMove : MonoBehaviour {

    [SerializeField]
    public int axeDamage = 4;
    private Vector3 _strikeStart;
    private Vector3 _strikeStartRotation;
    private Vector3 _strikeEnd;
    private Vector3 _strikeEndRotation;
    private bool forwardMoveCheck = true;
    private bool downMoveCheck = false;
    public bool strikeCheck = false;
	void Start ()
    {
        _strikeStart = transform.localPosition;
        _strikeStartRotation = transform.localEulerAngles;
	}
    private void AxeMoveStart()
    {
        if (transform.localEulerAngles.z >= 0)
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(0, 0, 0.8f);
            transform.localPosition = transform.localPosition + new Vector3(-0.03f, 0.03f, 0);
        }
        else
        {
            forwardMoveCheck = false;
            downMoveCheck = true;
        }
    }
    private void AxeMoveMid()
    {
        if (transform.localEulerAngles.x < 87)
        {
            transform.localEulerAngles = transform.localEulerAngles + new Vector3(5f, 0, 0);
            transform.localPosition = transform.localPosition + new Vector3(0, -0.04f, 0.04f);
        }
        else
        {
            RaycastHit hit;
            Debug.DrawRay(transform.parent.parent.transform.position, transform.up, Color.red, 10);
            if (Physics.Raycast(transform.parent.parent.transform.position, transform.up, out hit, 2, 1<< LayerMask.NameToLayer("Enemy")))
            {
                Debug.Log("Strike");
                hit.collider.gameObject.GetComponent<EnemyMove>().HitPointsChange(axeDamage);
                //hit.collider.gameObject.SetActive(false);
            }
            transform.localEulerAngles = _strikeStartRotation;
            transform.localPosition = _strikeStart;
            downMoveCheck = false;
        }
    }
    private void MovementHandler()
    {
        if (forwardMoveCheck)
        {
            AxeMoveStart();
        }
        else if (downMoveCheck)
        {
            AxeMoveMid();
        }
        else
        {
            strikeCheck = false;
        }
    }
	void Update ()
    {
        if (strikeCheck)
        {
            MovementHandler();
        }
        else
        {
            forwardMoveCheck = true;
        }
    }
}
