using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementAxe : MonoBehaviour
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
    private GameObject playerAxe;
    [SerializeField]
    private GameObject playerHook;
    #endregion    
    [SerializeField]
    GameObject PlayerWithBow;
    [SerializeField]
    float groundCheckRadius;
    private Rigidbody rb;
    bool isLiftChild = true;
    // Use this for initialization
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerHook.SetActive(false);
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
                //Debug.DrawRay(transform.position, transform.right + transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.right + transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
                {
                    return;
                }
            }
            else if (moveHorizontal == 0)
            {
                //Debug.Log("Forward");
                //Debug.DrawRay(transform.position, transform.forward, Color.black, 1);
                if (Physics.Raycast(transform.position, transform.forward, 1, 1 << LayerMask.NameToLayer("Ground")))
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
    #region JumpProperties
    private bool GroundCheck()
    {
        return Physics.Raycast(transform.position, -transform.up, groundCheckRadius, 1 << LayerMask.NameToLayer("Ground"));
    }
    private bool LiftCheck()
    {
        return Physics.Raycast(transform.position, -transform.up, groundCheckRadius, 1 << LayerMask.NameToLayer("Lift"));
    }
    private void VerticalJump()
    {
        bool IsJumped = Input.GetKeyDown(KeyCode.Space);
        if (IsJumped && (GroundCheck() || LiftCheck()))
        {
            //Debug.Log("Jump");
            rb.AddForce(new Vector3(0, 100 * jumpSpeed, 0));
        }
    }
    #endregion
    private void CameraVision()
    {
        float MouseAxisX = Input.GetAxis("Mouse Y");
        playerHead.transform.localEulerAngles = playerHead.transform.localEulerAngles + new Vector3(-MouseAxisX, 0, 0);
    }
    private void WeaponSwapHandler()
    {
        bool WantSwap = Input.GetKeyDown(KeyCode.Alpha1);
        if (WantSwap)
        {
            gameObject.SetActive(false);
            PlayerWithBow.SetActive(true);
            PlayerWithBow.transform.position = gameObject.transform.position;
            PlayerWithBow.transform.rotation = gameObject.transform.rotation;
            PlayerWithBow.GetComponent<PlayerMovement>().playerHead.transform.localEulerAngles = playerHead.transform.localEulerAngles;
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
    private void AxeStrike()
    {
        bool WantStrike = Input.GetKeyDown(KeyCode.Mouse0);
        if (WantStrike)
        {
            if (playerAxe.GetComponent<AxeMove>().strikeCheck == false)
            {
                playerAxe.GetComponent<AxeMove>().strikeCheck = true;

            }
        }
    }
    private void IsLifted()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position,-transform.up,out hit,0.6f,1 << LayerMask.NameToLayer("Lift")))
        {
            gameObject.transform.SetParent(hit.transform);
            isLiftChild = true;
        }
        else if (isLiftChild)
        {
            gameObject.transform.SetParent(null);
            isLiftChild = false;
        }
        //Collider[] col = Physics.OverlapSphere(transform.position, 1.2f, 1 << LayerMask.NameToLayer("Lift"));
        //if (col.Length != 0)
        //{
        //    gameObject.transform.SetParent(col[0].transform);
        //}
        //else
        //{
        //    gameObject.transform.SetParent(null);
        //}
    }
    private void GrappingHookHandler()
    {
        bool wantHook = Input.GetKeyDown(KeyCode.Q);
        if (wantHook)
        {
            playerHook.SetActive(true);
            if (Physics.Raycast(playerHead.transform.position, playerHead.transform.forward, 20, 1 << LayerMask.NameToLayer("Ground")))
            {
                playerHook.GetComponentInChildren<GameObject>(GameObject.Find("Hook"));
                GameObject.Find("Hook");
            }
        }
    }
    void Update()
    {
        WeaponSwapHandler();
        GroundMotion();
        VisionHorizontalMove();
        VerticalJump();
        CameraVision();
        AxeStrike();
        InteractHandler();
        IsLifted();
        GrappingHookHandler();
    }
}
