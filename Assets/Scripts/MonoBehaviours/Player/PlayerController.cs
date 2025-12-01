using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviourPunCallbacks
{    
    [Header("Movement Settings")]
    [SerializeField] private CharacterController characterController;
    [SerializeField] private ShootingAbility shootingAbility;
    [SerializeField] private float walkSpeed = 4f;
    [SerializeField] private float runSpeed = 8f;
    [SerializeField] private float accelerationTime = 0.5f;
    [SerializeField] private float decelerationTime = 0.3f;
    [SerializeField]
    private AnimationCurve accelerationCurveWalk =
        new AnimationCurve(new Keyframe(0f, 0f),new Keyframe(1f, 1f));  

    [Space(5)]
    [Header("Ground Check")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask ground;
    [SerializeField] private float groundDistance = 0.6f;
    private bool isGround;

    [Space(5)]
    [Header("Gravity")]
    [SerializeField] private float gravity = -9.8f;
    private float gravityDivisor = 2;
    private float velocityY = 0f;

    [Space(5)]
    [Header("Camera")]
    [SerializeField] private float smoothTime = 0.1f;
    private Transform thirdPersonCamera;

    private float smoothVelocity;
    private AnimationPlayerController animationPlayerController;    
    private Vector3 direction;
    private Vector3 lastMoveDirection;    
    private float speedWalkProgress;
    private float speedRunProgress;
    private float currentSpeed;
    private float horizontalSpeed;   
    private bool isMoving = true;
    private bool isDead = false;
    public bool IsMoving { set { isMoving = value; } }
    public Transform ThirdPersonCamera
    { set { thirdPersonCamera = value; } }    

    private void Start()
    {       

        animationPlayerController = GetComponent<AnimationPlayerController>();
    }

    private void FixedUpdate()
    {
        if (photonView.IsMine)
        {
            if (isMoving == true)
            {
                HandleGroundCheck();
                HandleMovementInput();
                SpeedProgress();
            }
            else
            {
                speedRunProgress = 0f;
                speedWalkProgress = 0f;
                currentSpeed = 0f;
                direction = new Vector3();
                lastMoveDirection = new Vector3();
            }
            ApplyMovement();
        }
    }
    private void Update()
    {     
        if (photonView.IsMine)
            UpdateAnimations();
    }

    public void PlayerIsDead()
    {
        isDead = true;
    }

    private void HandleGroundCheck()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, ground);

        if (isGround && velocityY < 0)        
            velocityY = gravity / gravityDivisor;        
        else        
            velocityY += gravity * Time.deltaTime;        
    }

    private void HandleMovementInput()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + thirdPersonCamera.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref smoothVelocity, smoothTime);
            lastMoveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private void SpeedProgress()
    {
        bool wantsToMove = direction.magnitude >= 0.1f;
        bool wantsToRun = wantsToMove && Input.GetKey(KeyCode.LeftShift);

        if (wantsToMove && wantsToRun == false)
        {
            speedWalkProgress = Mathf.Clamp01(speedWalkProgress + Time.deltaTime / accelerationTime);
        }
        else
        {
            speedWalkProgress = Mathf.Clamp01(speedWalkProgress - Time.deltaTime / decelerationTime);
        }
        
        if (wantsToMove && wantsToRun)
        {
            speedRunProgress = Mathf.Clamp01(speedRunProgress + Time.deltaTime / accelerationTime);
        }
        else
        {
            speedRunProgress = Mathf.Clamp01(speedRunProgress - Time.deltaTime / decelerationTime);
        }
        
        currentSpeed = walkSpeed * accelerationCurveWalk.Evaluate(speedWalkProgress) +
                      runSpeed * accelerationCurveWalk.Evaluate(speedRunProgress);      
    }

    private void ApplyMovement()
    {
        Vector3 movement = lastMoveDirection.normalized * currentSpeed * Time.deltaTime;
        movement.y = velocityY * Time.deltaTime;

        if (direction.magnitude < 0.1f && isGround == true)        
            movement += Vector3.down * Time.deltaTime;

        characterController.Move(movement);                    
    }

    private void UpdateAnimations()
    {
        if (isMoving)
            horizontalSpeed = new Vector3(characterController.velocity.x, 0f, characterController.velocity.z).magnitude;
        else        
            horizontalSpeed -= Time.deltaTime * 5;              

        
        animationPlayerController.SetHorizontalSpeed(horizontalSpeed);

        if (Input.GetKeyDown(KeyCode.E))        
            animationPlayerController.PlayWavingAnim();
        if (Input.GetKeyDown(KeyCode.Space))
            shootingAbility.ShootPress();

        if (isDead == true)
            animationPlayerController.PlayDeathAnim();                  
    }
}
