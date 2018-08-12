using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapAndHook : MonoBehaviour {
    public bool IsHookShot = false;
    public bool IsHookGrap = false;
    [SerializeField]
    public GameObject Wire;
    [SerializeField]
    public GameObject Hook;
    [SerializeField]
    public GameObject Player;
    Vector3 startPositionWire;
    Vector3 startPositionHook;
    Vector3 startScaleWire;
    Vector3 startRotationHook;
    float hookSpeed = 0.2f;
    float wireMaxLength = 20;
    float playerPosInterpol = 0;
    Vector3? playerStartPos = null;
    // Use this for initialization
    void Start ()
    {
        startPositionHook = Hook.transform.localPosition;
        startPositionWire = Wire.transform.localPosition;
        startScaleWire = Wire.transform.localScale;
        startRotationHook = Hook.transform.localEulerAngles;
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (IsHookShot)
        {
            if (!Hook.GetComponent<HookCheck>().GroundGrap())
            {
                if (Wire.transform.localScale.y > wireMaxLength)
                {
                    
                    Wire.transform.localPosition = startPositionWire;
                    Wire.transform.localScale = startScaleWire;
                    Hook.transform.localPosition = startPositionHook;
                    IsHookShot = false;
                    return;
                }
                //Debug.Log("2 ");
                Wire.transform.localScale = Wire.transform.localScale + new Vector3(0, hookSpeed, 0);
                Wire.transform.localPosition = Wire.transform.localPosition + new Vector3(0, 0, hookSpeed);
                Hook.transform.localPosition = Hook.transform.localPosition + new Vector3(0, 0, hookSpeed * 2);
                //Debug.Log(Hook.transform.position);
            }
            else
            {
                //Debug.Log("1 "+ Hook.transform.position);
                IsHookShot = false;
                IsHookGrap = true;
            }
        }
        playerPosInterpol = playerPosInterpol + Time.deltaTime / 2;
        //Debug.Log(Time.deltaTime);
        if (playerStartPos == null)
        {
            playerStartPos = Player.transform.position;
        }
        if (IsHookGrap)
        {
            if (playerPosInterpol < 1)
            {
                Player.transform.position = Vector3.Lerp(playerStartPos.Value, Hook.transform.position, playerPosInterpol);
                Wire.transform.localPosition = startPositionWire;
                Wire.transform.localScale = startScaleWire;
                //Debug.Log("Player "+Player.transform.position+" Hook "+Hook.transform.position);
            }
            else
            {
                Hook.transform.SetParent(Player.GetComponent<PlayerMovement>().playerHook.transform);
                Wire.transform.localPosition = startPositionWire;
                Wire.transform.localScale = startScaleWire;
                Hook.transform.localPosition = startPositionHook;
                Hook.transform.localEulerAngles = startRotationHook;
                IsHookGrap = false;
                Player.GetComponent<PlayerMovement>().canMove = true;
                Player.GetComponent<PlayerMovement>().playerHook.SetActive(false);
                Player.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
                Player.GetComponent<Rigidbody>().useGravity = true;
                //Hook.GetComponent<HookCheck>().hookCheck = false;
                playerPosInterpol = 0;
                playerStartPos = null;
            }
        }

	}
}
//if (IsHookShot && Wire.transform.localScale.y < wireMaxLength && !Hook.GetComponent<HookCheck>().hookCheck)
//      {
//          Wire.transform.localScale = Wire.transform.localScale + new Vector3(0,hookSpeed,0);
//          Wire.transform.localPosition = Wire.transform.localPosition + new Vector3(0, 0, hookSpeed);
//          Hook.transform.localPosition += new Vector3(0,0,hookSpeed*2);
//      }
//      else if (IsHookShot)
//      {
//          IsHookShot = false;
//          IsHookGrap = true;
//      }
//if (IsHookGrap && !Hook.GetComponent<HookCheck>().playerCheck)
//{
//    Player.transform.position = Vector3.Lerp(Player.transform.position, Hook.transform.position, playerPosInterpol);
//    Wire.transform.localPosition = startPositionWire;
//    Wire.transform.localScale = startScaleWire;
//    //Debug.Log("Player "+Player.transform.position+" Hook "+Hook.transform.position);
//}
//else if (IsHookGrap)
//{
//    //Debug.Log("END");
//    Hook.transform.SetParent(Player.GetComponent<PlayerMovement>().playerHook.transform);
//    Wire.transform.localPosition = startPositionWire;
//    Wire.transform.localScale = startScaleWire;
//    Hook.transform.localPosition = startPositionHook;
//    IsHookGrap = false;
//    Player.GetComponent<PlayerMovement>().canMove = true;
//    Player.GetComponent<PlayerMovement>().playerHook.SetActive(false);
//    Player.GetComponent<BoxCollider>().size = new Vector3(1, 1, 1);
//    Player.GetComponent<Rigidbody>().useGravity = true;
//    Hook.GetComponent<HookCheck>().hookCheck = false;
//    playerPosInterpol = 0;
//}