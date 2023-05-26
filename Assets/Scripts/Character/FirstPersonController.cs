using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class FirstPersonController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private CharacterAnimatorController animatorController;

    #region Camera Variables
    [SerializeField] private Camera playerCamera;
    [SerializeField] private float fov = 60f;
    [SerializeField] private bool cameraCanMove = true;
    [SerializeField] private float mouseSensitivity = 2f;
    [SerializeField] private float maxLookAngle = 50f;
    #endregion

    #region Crosshair Variables
    [SerializeField] private bool lockCursor = true;
    [SerializeField] private bool crosshair = true;
    [SerializeField] private Sprite crosshairImage;
    [SerializeField] private Color crosshairColor = Color.white;
    private float yaw = 0.0f;
    private float pitch = 0.0f;
    private Image crosshairObject;
    #endregion

    #region Movement Variables
    [SerializeField] private bool playerCanMove = true;
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float maxVelocityChange = 10f;
    [SerializeField] private bool enableSprint = true;
    [SerializeField] private KeyCode sprintKey = KeyCode.LeftShift;
    [SerializeField] private float sprintSpeed = 7f;
    private bool isWalking = false;
    private bool isSprinting = false;
    #endregion

    #region Jump Variables
    public bool enableJump = true;
    public KeyCode jumpKey = KeyCode.Space;
    public float jumpPower = 5f;
    #endregion

    #region Crouch variables
    private bool isGrounded = false;
    public KeyCode crouchKey = KeyCode.LeftControl;
    public float crouchHeight = .75f;
    public float speedReduction = .5f;
    private bool isCrouched = false;
    private Vector3 originalScale;
    #endregion

    #region Melee Variables
    [SerializeField] private Transform originMelee;
    private float meleeRange = 3f;
    private float lastHitMelee = 0f;
    private float reloadTime = 2f;
    private int meleeDamage = 50;
    #endregion

    #region Other
    [SerializeField] public HealthSystem healthSystem;
    public UnityEvent OnDamageUpPowerUpPicked;
    public UnityEvent OnDamageDownPowerUpPicked;
    #endregion

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        crosshairObject = GetComponentInChildren<Image>();

        playerCamera.fieldOfView = fov;
        originalScale = transform.localScale;

        healthSystem.OnEntityDead += OnEntityDeadHandler;

    }

    void Start()
    {
        #region Crosshair
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (crosshair)
        {
            crosshairObject.sprite = crosshairImage;
            crosshairObject.color = crosshairColor;
        }
        else
        {
            //crosshairObject.gameObject.SetActive(false);
        }
        #endregion

        this.healthSystem.Init();
    }

    private void Update()
    {
        #region Camera

        if (cameraCanMove)
        {
            yaw = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * mouseSensitivity;
            
            pitch -= mouseSensitivity * Input.GetAxis("Mouse Y");
            
            pitch = Mathf.Clamp(pitch, -maxLookAngle, maxLookAngle);

            transform.localEulerAngles = new Vector3(0, yaw, 0);
            playerCamera.transform.localEulerAngles = new Vector3(pitch, 0, 0);
        }
        #endregion

        #region Jump

        if (enableJump && Input.GetKeyDown(jumpKey) && isGrounded)
        {
            Jump();
        }

        #endregion

        #region Crouch
        if (Input.GetKeyDown(crouchKey) )
        {
            Crouch();
        }
        CheckGround();
        #endregion

        #region Melee
        if (Input.GetMouseButtonDown(0) && Time.time > lastHitMelee + 1f)
        {
            HitMelee();
            lastHitMelee = Time.time;            
        }
        #endregion

    }

    void FixedUpdate()
    {
        #region Movement

        if (playerCanMove)
        {
            Vector3 targetVelocity = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));

            if (targetVelocity.y != 0 || targetVelocity.x != 0 || targetVelocity.z != 0)
            {
                isWalking = true;

                animatorController.Walk(isWalking);
            }
            else
            {
                isWalking = false;

                animatorController.Walk(isWalking);
            }

            if (enableSprint && Input.GetKey(sprintKey))
            {
                targetVelocity = transform.TransformDirection(targetVelocity) * sprintSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                if (velocityChange.x != 0 || velocityChange.z != 0)
                {
                    isSprinting = true;

                    animatorController.Sprint(isSprinting);

                    if (isCrouched)
                    {
                        Crouch();
                    }

                }
                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }

            else
            {
                isSprinting = false;

                animatorController.Sprint(isSprinting);

                targetVelocity = transform.TransformDirection(targetVelocity) * walkSpeed;

                Vector3 velocity = rb.velocity;
                Vector3 velocityChange = (targetVelocity - velocity);
                velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
                velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
                velocityChange.y = 0;

                rb.AddForce(velocityChange, ForceMode.VelocityChange);
            }
        }

        #endregion


    }

    private void CheckGround()
    {
        Vector3 origin = new Vector3(transform.position.x, transform.position.y - (transform.localScale.y * .5f), transform.position.z);
        Vector3 direction = transform.TransformDirection(Vector3.down);
        float distance = .75f;

        if (Physics.Raycast(origin, direction, out RaycastHit hit, distance))
        {
            Debug.DrawRay(origin, direction * distance, Color.red);
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    private void Jump()
    {
        if (isGrounded)
        {
            rb.AddForce(0f, jumpPower, 0f, ForceMode.Impulse);
            isGrounded = false;
        }

        if (isCrouched)
        {
            Crouch();
        }
    }

    private void Crouch()
    {
        if (isCrouched)
        {
            transform.localScale = new Vector3(originalScale.x, originalScale.y, originalScale.z);
            walkSpeed /= speedReduction;

            isCrouched = false;
        }

        else
        {
            transform.localScale = new Vector3(originalScale.x, crouchHeight, originalScale.z);
            walkSpeed *= speedReduction;

            isCrouched = true;
        }
    }

    private IEnumerator HitMeleeCoroutine()
    {
        animatorController.HitMelee(true);

        RaycastHit hit;
        if (Physics.Raycast(originMelee.position, originMelee.transform.forward, out hit, meleeRange))
        {
            Debug.Log("Pegue");
            if(hit.collider.CompareTag("Zombie"))
            {
                ZombieController zombie = hit.collider.GetComponent<ZombieController>();
                if (zombie != null)
                {
                    zombie.TakeDamage(meleeDamage);
                }
            }
        }

        yield return new WaitForSeconds(reloadTime);

        animatorController.HitMelee(false);
    }

    private void HitMelee()
    {
        StartCoroutine(HitMeleeCoroutine());
    }

    public void Hitted(float damage)
    {
        healthSystem.EntityHitted(damage);
    }

    public float GetHp()
    {
        return healthSystem.GetCurrentHealth();
    }

    public virtual void OnEntityDeadHandler()
    {
        this.animatorController.Dead(true);
        this.playerCanMove = false;
        this.cameraCanMove = false;
    }

    private void OnDestroy()
    {
        healthSystem.OnEntityDead -= OnEntityDeadHandler;
    }

    public void OnDamagePowerUpPicked ()
    {
        OnDamageUpPowerUpPicked?.Invoke();
    }

    public void OnDamagePowerDownPicked()
    {
        OnDamageDownPowerUpPicked?.Invoke();
    }

    public void RaiseMaxDamage(int amount)
    {
        Debug.Log("El daño del jugador aumentó en " + amount);
        this.meleeDamage += amount;
    }

    public void ReduceMaxDamage (int amount)
    {
        Debug.Log("El daño del jugador disminuyó en " + amount);
        this.meleeDamage -= amount;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(originMelee.position, originMelee.transform.forward * meleeRange);
    }
}
