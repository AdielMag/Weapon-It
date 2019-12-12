using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Singelton
    static public PlayerController instance;
    private void Awake()
    {
        instance = this;
    }
    #endregion

    public float movementSpeed = 3;
    public int movementRotationForce = 15;
    public float yOffset; // Offset weapon Y location.
    public float gameWidth;
    public float targetIndicatorYMultiplier = 20; // Multiply input.y precentage.


    public WeaponController WeaponCon { get; private set; }
    GameManager gMan;
    Animator anim;
    InputHandler inputH;

    void Start()
    {
        gMan = GameManager.instance;
        WeaponCon = GetComponent<WeaponController>();
        anim = GetComponent<Animator>();
        inputH = InputHandler.instance;

        // Set the look from the start 
        // (if not will lerp from Vector3.zero and will look weird)
        targetLookAt = transform.position
                + Vector3.forward * 20
                + posDelta.y * Vector3.up * movementRotationForce
                + posDelta.x * Vector3.right * movementRotationForce;

        SetPlayerItems();
    }

    void Update()
    {
        MovePlayer();
        RotatePlayer();

        HandleAnimations();
    }

    void HandleAnimations()
    {
        anim.SetFloat("Horizontal", posDelta.x * 8);
        anim.SetBool("Aim",WeaponCon.TargetDetected);
    }

    Vector2 targetPos;
    void MovePlayer()
    {
        // Use input precentage to set width location.
        targetPos.x = Mathf.Lerp(-gameWidth, gameWidth, inputH.inputPrecentage.x);

        // Multiply precentage by the multiplier and add offset.
        targetPos.y = yOffset;

        // Lerp position
        transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * movementSpeed);
    }

    Vector3 lerpTargetVar, targetLookAt;
    float angleDiffFromForward;
    void RotatePlayer()
    {
        CalculatePosDelta();

        // Check the angle of the forward to the current rot
        angleDiffFromForward = Vector3.Angle(transform.forward, Vector3.forward);

        // Check if there's a target on sight
        if (WeaponCon.TargetDetected)
        {
            lerpTargetVar = WeaponCon.targetFuturePos();
        }
        else
        {
            // Set target for lerp with fixed forward and wth posDelta variables
            lerpTargetVar = transform.position
                + Vector3.forward * 20
                + posDelta.y * Vector3.up * movementRotationForce
                + posDelta.x * Vector3.right * movementRotationForce;
        }

        targetLookAt = Vector3.Lerp(targetLookAt, lerpTargetVar, Time.deltaTime * 10);
        targetLookAt.y = 0;     // Dont want the player yo rotate in the Y axis
        transform.LookAt(targetLookAt);
    }

    Vector2 lastPos, currentPos,posDelta;
    // Calculates the diffrence from this pos to last frame pos.
    void CalculatePosDelta()                
    {
        currentPos = transform.position;

        posDelta = currentPos - lastPos;

        lastPos = currentPos;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "Target")
            gMan.LevelCon.LostLevel();
    }

    // Player items handler!
    [Header("Store items parents")]
    public Transform weaponsItemsParent;
    public Transform charactersItemsParent;

    // Set player character
    void SetPlayerItems()
    {
        StartCoroutine (SetPlayerCharacter());
    }

    IEnumerator SetPlayerCharacter()
    {        
        // Get the equipped character
        GameObject playerCharacterPrefab =
            charactersItemsParent.GetChild(gMan.DataManager.storeData.EquippedCharacter).gameObject;

        Transform currentCharacter = Instantiate(playerCharacterPrefab, transform).transform;
        currentCharacter.gameObject.SetActive(true); // Set it active

        // Set pos and rot to 0 
        // Not doing it inside the instantiate because i want to change rot before pos 
        // (else it will change the pos)
        currentCharacter.rotation = Quaternion.Euler(Vector3.zero);
        currentCharacter.localPosition = Vector3.zero;

        // Used to let the animator sync with the bones or whatever... - stupid unity bug
        yield return new WaitForEndOfFrame();

        anim.Rebind();

        SetPlayerWeapon();
    }
    void SetPlayerWeapon()
    {
        // Get character hand transform for instantiating the weapons.
        Transform characterRightHand = anim.GetBoneTransform(HumanBodyBones.RightHand);

        // Get equipped weapon
        GameObject playerWeaponPrefab =
            weaponsItemsParent.GetChild(gMan.DataManager.storeData.EquippedWeapon).gameObject;

        GameObject currentWeapon = Instantiate(playerWeaponPrefab, characterRightHand);
        currentWeapon.SetActive(true); // Set it active

        WeaponCon.CurrentWeapon = currentWeapon.GetComponent<Weapon>();

        WeaponCon.Init();
    }
}
