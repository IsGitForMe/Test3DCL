using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour {
    [SerializeField]
    float ElevHight = 5;
    [SerializeField]
    bool IsLiftMoveUp = false;
    [SerializeField]
    int LiftSpeed = 1;
    [SerializeField]
    GameObject StartPosition;
    [SerializeField]
    GameObject EndPosition;
    [SerializeField]
    GameObject Platform;
    Vector3 startPosition;
    Vector3 endPosition;
    bool buttonClicked = false;
    bool liftOnMove = false;
    void Start ()
    {
        if (StartPosition != null && EndPosition != null)
        {
            startPosition = StartPosition.transform.position;
            endPosition = EndPosition.transform.position;
        }
        else
        {
            gameObject.SetActive(false);
        }
	}
	public void Switch()
    {
        if (!liftOnMove)
        {
            liftOnMove = true;
            buttonClicked = true;
            IsLiftMoveUp = !IsLiftMoveUp;
        }
    }
	void Update ()
    {
        if (IsLiftMoveUp&&buttonClicked)
        {
            if (Platform.transform.position.y < endPosition.y)
            {
                float y = LiftSpeed * Time.deltaTime;
                Platform.transform.position = Platform.transform.position + new Vector3(0, y, 0);
            }
            else
            {
                buttonClicked = false;
                liftOnMove = false;
            }
        }
        else if (buttonClicked)
        {
            if (Platform.transform.position.y > startPosition.y)
            {
                float y = LiftSpeed * Time.deltaTime;
                Platform.transform.position = Platform.transform.position + new Vector3(0, -y, 0);
            }
            else
            {
                liftOnMove = false;
                buttonClicked = false;
            }
            
        }
	}
}
//Обнести условиями Х-ы и  Z-ы для  равномерного перемещения между точками.