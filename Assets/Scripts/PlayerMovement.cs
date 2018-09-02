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
    private GameObject playerHitBarText;
    [SerializeField]
    public GameObject playerAxe;
    [SerializeField]
    public GameObject playerBow;
    [SerializeField]
    public GameObject playerHook;
    //Slots are filled by game progress
    public GameObject[] playerSlots = new GameObject[slotsCount];
    [SerializeField]
    private GameObject playerArm;
    #endregion
    #region PlayerStats
    [SerializeField]
    public int MaxHitPoints = 10;
    public int realHitPoints;
    static public int slotsCount = 5;
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
    float groundCheckRadius;
    private Rigidbody rb;
    private int selectedSlot = 0;
    public bool canMove = true;
    [SerializeField]
    GameObject Bomb;
    GameObject bomb;
    // Use this for initialization
    void Start()
    {
        //PlayerWithAxe.SetActive(false);
        playerAxe.SetActive(false);
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
        if (!canMove)
        {
            return;
        }
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
    //private void ArrowShotHandler()
    //{
    //    bool IsShot = Input.GetKeyDown(KeyCode.Mouse0);
    //    if (IsShot&&arrowQivCount > 0)
    //    {
    //        int tickUpdate = Environment.TickCount;
    //        if (tickUpdate - tickArrowBegin > arrowReload * 1000)
    //        {
    //            GameObject arr = Arrows[_arrowPoolPosition];
    //            GetComponent<AudioSource>().Play();
    //            ArrowPoolHandler(arr);
    //            tickArrowBegin = tickUpdate;
    //            arrowQivCount--;
    //            Debug.Log(arrowQivCount);
    //        }
    //    }
    //}
    //private void WeaponSwapHandler()
    //{
    //    bool wantSwap = Input.GetKeyDown(KeyCode.Alpha2);
    //    if (wantSwap)
    //    {
    //        playerBow.SetActive(false);
    //        playerAxe.SetActive(true);
    //        //gameObject.SetActive(false);
    //        //PlayerWithAxe.SetActive(true);
    //        //PlayerWithAxe.transform.position = gameObject.transform.position;
    //        //PlayerWithAxe.transform.rotation = gameObject.transform.rotation;
    //        //PlayerWithAxe.GetComponent<PlayerMovementAxe>().playerHead.transform.localEulerAngles = playerHead.transform.localEulerAngles;
    //    }
    //}
    //private void AxeStrike()
    //{
    //    bool WantStrike = Input.GetKeyDown(KeyCode.Mouse0);
    //    if (WantStrike)
    //    {
    //        if (playerAxe.GetComponent<AxeMove>().strikeCheck == false)
    //        {
    //            playerAxe.GetComponent<AxeMove>().strikeCheck = true;

    //        }
    //    }
    //}
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
    private void PlayerDeath()
    {

    }
    bool isLiftChild = true;
    private void IsLifted()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.6f, 1 << LayerMask.NameToLayer("Lift")))
        {
            gameObject.transform.SetParent(hit.transform);
            isLiftChild = true;
        }
        else if (isLiftChild)
        {
            gameObject.transform.SetParent(null);
            isLiftChild = false;
        }
        //Collider[] col = Physics.OverlapSphere(transform.position,1.2f,1 << LayerMask.NameToLayer("Lift"));
        //if (col.Length != 0)
        //{
        //    gameObject.transform.SetParent(col[0].transform);
        //}
        //else
        //{
        //    gameObject.transform.SetParent(null);
        //}
    }
    private void SlotSelector(int slot)
    {
        if (playerSlots[selectedSlot] != null)
        {
            playerSlots[selectedSlot].SetActive(false);
        }
        if (playerSlots[slot] == null)
        {
            return;
        }
        playerSlots[slot].SetActive(true);
        //playerSlots[0].GetComponent<Interact>().SlotSelect();
        //playerSlots[0].transform.SetParent(gameObject.transform);
        selectedSlot = slot;
    }
    private void SlotSwapHandler()
    {
        bool slot1 = Input.GetKeyDown(KeyCode.Alpha1);
        bool slot2 = Input.GetKeyDown(KeyCode.Alpha2);
        bool slot3 = Input.GetKeyDown(KeyCode.Alpha3);
        bool slot4 = Input.GetKeyDown(KeyCode.Alpha4);
        bool slot5 = Input.GetKeyDown(KeyCode.Alpha5);

        if (slot1)
        {
            SlotSelector(0);
        }
        else if (slot2)
        {
            SlotSelector(1);
        }
        else if (slot3)
        {
            SlotSelector(2);
        }
        else if (slot4)
        {
            SlotSelector(3);
        }
        else if (slot5)
        {
            SlotSelector(4);
        }
    }
    private void SelectedPrimaryAction()
    {

    }
    private void GrappingHookHandler()
    {
        bool wantHook = Input.GetKeyDown(KeyCode.Q);
        if (wantHook)
        {
            playerHook.SetActive(true);
            //Debug.Log("Hook");
            if (playerHook.GetComponent<GrapAndHook>().IsHookGrap)
            {
                return;
            }
            playerHook.GetComponent<GrapAndHook>().IsHookShot = true;
        }
    }
    Vector3 destination;
    private void FShot()
    {
        bool IsShot = Input.GetKeyDown(KeyCode.F);
        if (!IsShot)
        {
            return;
        }
        bomb = Instantiate(Bomb,playerHead.transform.position + playerHead.transform.forward,playerHead.transform.rotation);
        destination = playerHead.transform.forward;

    }
    private void BombFly()
    {
        if (bomb != null)
        {
            if (!Physics.Raycast(bomb.transform.position,bomb.transform.forward,0.5f,1 << LayerMask.NameToLayer("Ground")))
            {
                bomb.transform.position = bomb.transform.position + destination / 1f;
            }
            else
            {
                //bomb = null;
            }
        }
    }
    void Update()
    {
        GrappingHookHandler();
        SelectedPrimaryAction();
        SlotSwapHandler();
        GroundMotion();
        VisionHorizontalMove();
        VerticalJump();
        CameraVision();
        InteractHandler();
        PlayerDeath();
        IsLifted();
        FShot();
        BombFly();
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