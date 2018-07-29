using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    #region MovementStats
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float jumpSpeed;
    #endregion
    #region PlayersParts
    [SerializeField]
    public GameObject playerHead;
    [SerializeField]
    private GameObject playerBow;
    [SerializeField]
    private GameObject playerHitBarText;
    #endregion
    #region PlayerStats
    [SerializeField]
    public int MaxHitPoints = 10;
    public int realHitPoints;
    #endregion
    #region ArrowStats
    GameObject[] Arrows;
    [SerializeField]
    int arrowDamage;
    float arrowShotPower = 3000;
    int arrowsPoolCount = 4;
    [SerializeField]
    GameObject ArrowObj;
    [SerializeField]
    int arrowQivCount;
    int _arrowPoolPosition = 0;
    [SerializeField]
    private uint arrowReload = 2;
    int tickArrowBegin = 0;
    //Transform LastPlace;
    #endregion
    [SerializeField]
    GameObject PlayerWithAxe;
    [SerializeField]
    float groundCheckRadius;
    private Rigidbody rb;
    // Use this for initialization
    void Start()
    {
        PlayerWithAxe.SetActive(false);
        tickArrowBegin = Environment.TickCount;
        rb = GetComponent<Rigidbody>();
        Arrows = new GameObject[arrowsPoolCount];
        for (int i = 0;i < Arrows.Length; i++)
        {
            Arrows[i] = Instantiate(ArrowObj,transform.position, Quaternion.identity);
            Arrows[i].SetActive(false);
        }
        realHitPoints = MaxHitPoints;
    }
    /// <summary>
    /// w,a,s,d ground motion
    /// </summary>
    private void GroundMotion()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        if (moveVertical > 0)
        {
            if (moveHorizontal > 0)
            {
                //Debug.Log("ForwRigth");
                //Debug.DrawRay(transform.position, transform.right+transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.right + transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else if (moveHorizontal == 0)
            {
                //Debug.Log("Forward");
                //Debug.DrawRay(transform.position, transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.forward, 1,1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else
            {
                //Debug.Log("ForwLeft");
                //Debug.DrawRay(transform.position, -transform.right + transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, -transform.right + transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
        }
        else if (moveVertical == 0)
        {
            if (moveHorizontal > 0)
            {
                //Debug.Log("Rigth");
                //Debug.DrawRay(transform.position, transform.right, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.right, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else if (moveHorizontal == 0)
            {
                //Debug.Log("Stand");
            }
            else
            {
                //Debug.Log("Left");
                //Debug.DrawRay(transform.position, -transform.right, Color.black, 1);
                if (Physics.Raycast(transform.position, -transform.right, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
        }
        else
        {
            if (moveHorizontal > 0)
            {
                //Debug.Log("BackwRigth");
                //Debug.DrawRay(transform.position, transform.right + -transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.right + -transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else if (moveHorizontal == 0)
            {
                //Debug.Log("Backward");
                //Debug.DrawRay(transform.position, -transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, -transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else
            {
                //Debug.Log("BackwLeft");
                //Debug.DrawRay(transform.position, -transform.right + -transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, -transform.right + -transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
        }
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        Vector3 movementHead = transform.TransformDirection(movement);
        gameObject.transform.position = gameObject.transform.position + movementHead * moveSpeed / 100;
    }
    private void VisionHorizontalMove()
    {
        float MouseAxisX = Input.GetAxis("Mouse X");
        gameObject.transform.localEulerAngles = gameObject.transform.localEulerAngles + new Vector3(0, MouseAxisX, 0);
        transform.localEulerAngles = new Vector3(0, transform.localEulerAngles.y, transform.localEulerAngles.z);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }
    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, -transform.up,groundCheckRadius, 1 << LayerMask.NameToLayer("Ground"));
    }
    private void VerticalJump()
    {
        bool IsGrounded = GroundCheck();
        bool IsJumped = Input.GetKeyDown(KeyCode.Space);
        if (IsJumped && IsGrounded)
        {
            //Debug.Log("Jump");
            rb.AddForce(new Vector3(0,100 * jumpSpeed,0));
        }
    }
    private void CameraVision()
    {
        float MouseAxisX = Input.GetAxis("Mouse Y");
        playerHead.transform.localEulerAngles = playerHead.transform.localEulerAngles + new Vector3(-MouseAxisX, 0,0);
    }
    private void ArrowPoolHandler(GameObject arr)
    {
        _arrowPoolPosition = ++_arrowPoolPosition % Arrows.Length;
        arr.SetActive(true);
        Rigidbody arrRb = arr.GetComponent<Rigidbody>();
        arrRb.Sleep();
        Vector3 delta = transform.position - playerBow.transform.position;
        delta.Normalize();
        arr.transform.position = transform.position - delta * 1.5f;
        arr.transform.rotation = playerBow.transform.rotation;
        arrRb.AddForce(-delta * arrowShotPower);
        arr.GetComponent<ArrowScript>().SetArrowDamage(arrowDamage);
    }
    private void ArrowShotHandler()
    {
        bool IsShot = Input.GetKeyDown(KeyCode.Mouse0);
        if (IsShot&&arrowQivCount > 0)
        {
            int tickUpdate = Environment.TickCount;
            if (tickUpdate - tickArrowBegin > arrowReload * 1000)
            {
                GameObject arr = Arrows[_arrowPoolPosition];
                GetComponent<AudioSource>().Play();
                ArrowPoolHandler(arr);
                tickArrowBegin = tickUpdate;
                arrowQivCount--;
                Debug.Log(arrowQivCount);
            }
        }
    }
    private void WeaponSwapHandler()
    {
        bool wantSwap = Input.GetKeyDown(KeyCode.Alpha2);
        if (wantSwap)
        {
            gameObject.SetActive(false);
            PlayerWithAxe.SetActive(true);
            PlayerWithAxe.transform.position = gameObject.transform.position;
            PlayerWithAxe.transform.rotation = gameObject.transform.rotation;
            PlayerWithAxe.GetComponent<PlayerMovementAxe>().playerHead.transform.localEulerAngles = playerHead.transform.localEulerAngles;
        }
    }
    private void InteractHandler()
    {
        bool IsAct = Input.GetKeyDown(KeyCode.E);
        if (IsAct)
        {
            RaycastHit hit;
            Debug.DrawRay(playerHead.transform.position, playerHead.transform.forward, Color.red, 4);
            if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, out hit, 2, 1 << LayerMask.NameToLayer("Button")))
            {
                //Debug.Log("button find");
                hit.transform.gameObject.GetComponent<ButtonClick>().Click();
            }
        }
    }
    public void HitPointChangeHandler(int damage)
    {
        realHitPoints = realHitPoints - damage;
    }
    public void PlayerDeath()
    {

    }
    void Update()
    {
        WeaponSwapHandler();
        ArrowShotHandler();
        GroundMotion();
        VisionHorizontalMove();
        VerticalJump();
        CameraVision();
        InteractHandler();
        PlayerDeath();
    }
}
//Debug.DrawRay(transform.position,)
//if (Physics.Raycast(transform.position,))
//Debug.Log(moveHorizontal+" "+moveVertical);
//rb.AddForce(movement * speed);
//Physics2D.Linecast(transform.position, gameObject.transform.position, 1 << LayerMask.NameToLayer("Ground"))
//[SerializeField]
//private Transform groundCheck;
//.FindGameObjectWithTag("Ground")
//Debug.Log(ground.Length);
//for (int i = 0;i < ground.Length; i++)
//{
//    Debug.Log(ground[i].transform.position);
//}
//return Physics.Linecast(transform.position,groundCheck.position,1 << LayerMask.NameToLayer("Ground"));
//Collider[] grounds = Physics.OverlapSphere(transform.position - new Vector3(0, 0.5f, 0), groundCheckRadius/*, LayerMask.NameToLayer("Ground")*/);
//        foreach(var col in grounds)
//        {
//            if (col.tag == "Ground")
//            {
//                Debug.Log(col+"Had Obj");
//            }
//        }
//if (ground.Length > 0)
//{
//    //Debug.Log("You can");
//    return true;
//}
//else
//{
//    //Debug.Log("You cant");
//    return false;
//Collider[] ground = Physics.OverlapSphere(transform.position - new Vector3(0,0.5f,0),groundCheckRadius/*,LayerMask.NameToLayer("Ground")*/);
//}
//Collider[] grounds = Physics.OverlapSphere(transform.position - new Vector3(0, 0.5f, 0), groundCheckRadius/*, LayerMask.NameToLayer("Ground")*/);
//foreach (var col in grounds)
//{
//    if (col.tag == "Ground")
//    {
//        return true;
//    }
//}
//return false;
//if (playerHead.transform.forward)