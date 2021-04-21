using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class PlayerController : MonoBehaviour
{
    //Required components to be inherited
    private CharacterController characterController;
    private GameWorldScript gameLogic;

    //Images on canvas for blink/fade
    [SerializeField]
    private Image flashFade;
    [SerializeField]
    private Image blinkFade;

    //GameObject references to hands and Teleport Spawn hand points
    [SerializeField]
    private GameObject leftHand;
    [SerializeField]
    private GameObject leftTeleSpawnPoint;
    [SerializeField]
    private GameObject rightHand;
    [SerializeField]
    private GameObject rightTeleSpawnPoint;
    [SerializeField]
    private GameObject teleportBall;

    //Input Action Variables for various control actions
    public SteamVR_Action_Vector2 playerMovementHMD;
    public SteamVR_Action_Vector2 playerMovementInde;
    public SteamVR_Action_Boolean playerRotationLeft;
    public SteamVR_Action_Boolean playerRotationRight;
    public SteamVR_Action_Boolean teleportTrigger;

    //Ammo Pools
    public int GreenAmmoPool { get; set; } = 300;
    public int RedAmmoPool { get; set; } = 300;
    public int BlueAmmoPool { get; set; } = 300;

    //Property Strings/Booleans for movement options
    public Vector3 TeleportPosition { get; set; }
    public string TeleportTypeVarient { get; set; }
    public bool TeleportBallWorldSpace { get; set; } = false;
    public bool HeadMountedMovementDependency { get; set; }

    //Gravity value and general speed of player
    private Vector3 gravity = new Vector3(0, 9.18f, 0);
    [SerializeField]
    private float speed;

    // Start is called before the first frame
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        characterController.enabled = true;
        gameLogic = GameObject.FindObjectOfType<GameWorldScript>();

        flashFade.canvasRenderer.SetAlpha(0.0f);
        blinkFade.canvasRenderer.SetAlpha(0.0f);

        HeadMountedMovementDependency = gameLogic.HMDDependency;
        TeleportTypeVarient = gameLogic.TeleportType;
    }

    // Update is called once per frame
    void Update()
    {
        if(HeadMountedMovementDependency == true)
        {
            PlayerMovementHMD();
        }
        else
        {
            SteamVR_Input_Sources sourceRotate = rightHand.gameObject.GetComponent<Hand>().handType;
            PlayerMovementIndependent(sourceRotate);
        }

        SteamVR_Input_Sources sourceLeft = leftHand.gameObject.GetComponent<Hand>().handType;
        TeleportingLeft(sourceLeft);
        SteamVR_Input_Sources sourceRight = rightHand.gameObject.GetComponent<Hand>().handType;
        TeleportingRight(sourceRight);

    }

    ///Movement System for the player.
    ///Can be dependent on head rotation or independent rotation
    #region Player Movement System
    /// <summary>
    /// Player Movement with forward dependent on position of Headset.
    /// Both Analogue sticks control movement.
    /// </summary>
    void PlayerMovementHMD()
    {
            Vector3 direction = Player.instance.hmdTransform.TransformDirection(new Vector3(playerMovementHMD.axis.x, 0, playerMovementHMD.axis.y));
            characterController.Move(speed * Time.deltaTime * Vector3.ProjectOnPlane(direction, Vector3.up) - (gravity * Time.deltaTime));
    }

    /// <summary>
    /// Player Movement with forward independent on position of Headset.
    /// Left Analogue stick moves, Right Analogue stick rotates.
    /// </summary>
    /// <param name="rotate"></param>
    void PlayerMovementIndependent(SteamVR_Input_Sources rotate)
    {
        if (playerRotationLeft[rotate].stateDown)
        {
            characterController.enabled = false;
            characterController.transform.Rotate(0, -45, 0);
            characterController.enabled = true;
        }
        if (playerRotationRight[rotate].stateDown)
        {
            characterController.enabled = false;
            characterController.transform.Rotate(0, 45, 0);
            characterController.enabled = true;
        }

        characterController.Move(speed * Time.deltaTime * new Vector3(playerMovementInde.axis.x, 0, playerMovementInde.axis.y) - (gravity * Time.deltaTime));   
    }
    #endregion

    ///Teleporting System for the player. Can be thrown from either hand
    ///Duplicate method for each hand and position
    #region Teleporting System Hands
    void TeleportingLeft(SteamVR_Input_Sources source)
    {
        if (leftHand.gameObject.GetComponent<Hand>().AttachedObjects.Count == 0 && TeleportBallWorldSpace == false)
        {
            if (teleportTrigger[source].stateDown)
            {
                TeleportBallWorldSpace = true;
                Quaternion projectileRotation;
                projectileRotation = leftTeleSpawnPoint.transform.rotation;

                _ = Instantiate(teleportBall, leftTeleSpawnPoint.transform.position, projectileRotation);
            } 
        }
        else
        {
            Debug.LogError("Can't Teleport: Either have object in hand or already thrown ball");
        }
    }

    void TeleportingRight(SteamVR_Input_Sources source)
    {
        if (rightHand.gameObject.GetComponent<Hand>().AttachedObjects.Count == 0 && TeleportBallWorldSpace == false)
        {
            if (teleportTrigger[source].stateDown)
            {
                TeleportBallWorldSpace = true;
                Quaternion projectileRotation;
                projectileRotation = rightTeleSpawnPoint.transform.rotation;

                _ = Instantiate(teleportBall, rightTeleSpawnPoint.transform.position, projectileRotation);
            }
        }
        else
        {
            Debug.LogError("Can't Teleport: Either have object in hand or already thrown ball");
        }
    }
    #endregion

    /// <summary>
    /// Switch method for teleporting the player. Uses a coroutine to delay the teleportation to blink/fade
    /// </summary>
    #region Teleporting System
    public void TeleportSnap()
    {
        characterController.enabled = false;

        switch (TeleportTypeVarient)
        {
            case "Flash":
                flashFade.CrossFadeAlpha(1, 0.25f, false);
                StartCoroutine(DelayTeleport(0.25f, flashFade));
                break;

            case "Blink":
                blinkFade.CrossFadeAlpha(1, 0.25f, false);
                StartCoroutine(DelayTeleport(0.25f, blinkFade));
                break;

            case "No Visual":
                characterController.transform.position = TeleportPosition;
                break;

            default:
                Debug.LogWarning("Warning: teleport type not identified. Default: No Visual");
                characterController.transform.position = TeleportPosition;
                break;
        }

        characterController.enabled = true;
    }

    IEnumerator DelayTeleport(float delay,Image flashType)
    {
        yield return new WaitForSeconds(delay);
        characterController.transform.position = TeleportPosition;
        flashType.CrossFadeAlpha(0, 0.25f, false);
    }
    #endregion
}
