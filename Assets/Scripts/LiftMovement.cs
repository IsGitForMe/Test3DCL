using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiftMovement : MonoBehaviour {
    [SerializeField]
    float ElevHight = 5;
    [SerializeField]
    bool IsLiftMoveUp = false;
    [SerializeField]
    float LiftSpeed = 1;
    [SerializeField]
    GameObject StartPosition;
    [SerializeField]
    GameObject EndPosition;
    [SerializeField]
    GameObject Platform;
    Vector3 startPosition;
    Vector3 endPosition;
    Vector3 direction;
    bool buttonClicked = false;
    bool liftOnMove = false;
    void Start ()
    {
        if (StartPosition != null && EndPosition != null)
        {
            startPosition = StartPosition.transform.position;
            endPosition = EndPosition.transform.position;
            direction = endPosition - startPosition;
            Debug.Log(direction+" direction");
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
    private float t = 0;
	void Update ()
    {
        if (IsLiftMoveUp && buttonClicked)
        {
            if (Platform.transform.position != endPosition)
            {
                t = t + Time.deltaTime;
                Platform.transform.position = Vector3.Lerp(startPosition,endPosition, t / 2 * LiftSpeed);
            }
            else
            {
                buttonClicked = false;
                liftOnMove = false;
            }
        }
        else if (buttonClicked)
        {
            if (Platform.transform.position != startPosition)
            {
                t = t - Time.deltaTime;
                Platform.transform.position = Vector3.Lerp(startPosition, endPosition, t / 2 * LiftSpeed);
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