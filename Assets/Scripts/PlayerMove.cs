using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerMove : MonoBehaviour
{


    #region variables
    [Header("Player Refrence")]
    public LayerMask GroundLayers;
    public CharacterController controller;
    public Animator animator;
    private Transform cam;
    private GameObject _Camera;

    [Header("Player Stats")]
    public float MoveSpeed = 2.0f;
    public float SprintSpeed = 5.5f;
    public float JumpHeight = 1.2f;
    public float Gravity = -15.0f;
    private readonly float JumpTimeout = 0.50f;
    private readonly float FallTimeout = 0.15f;
    private readonly float GroundedOffset = -0.14f;
    private readonly float GroundedRadius = 0.28f;
    private bool _hasAnimator;

    [Header("player Health")]
    public int currentHealth;
    public int maxHealth = 100;
    public Healthbar health;
    public GameObject UI;

    [Header("attack")]
    public float bulletSpeed;
    public float BulletUpward;
    public Transform Attackpoint;
    public GameObject bullet;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    //player
    private float speed = 3f;
    private float _animationBlend;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    private float _verticalVelocity;
    private readonly float _terminalVelocity = 53.0f;
    private readonly float RotationSmoothTime = 0.12f;

    private float _jumpTimeoutDelta;
    private float _fallTimeoutDelta;
    private readonly float interactRange;
    [Header("bools")]
    public bool Grounded = true;
    public bool inVehicle;
    private bool sprint;
    private bool attack;
    private readonly float SpeedChangeRate = 10.0f;
    #endregion

    private void Awake()
    {
        if(_Camera == null)
        {
            _Camera = GameObject.FindGameObjectWithTag("MainCamera");
        }
    }
    private void Start()
    {
        UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<PlayerInput>();
        _hasAnimator = TryGetComponent(out animator);
        cam = Camera.main.transform;
        currentHealth = maxHealth;
        health.SetMaxHealth(currentHealth, maxHealth);
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        if (inVehicle)
        {

            animator.SetTrigger("Car");
            animator.SetBool("Driving", true);
        }
        // reset our timeouts on start
        _jumpTimeoutDelta = JumpTimeout;
        _fallTimeoutDelta = FallTimeout;

    }
    void Update()
    {
        //CarDrive();
        if (inVehicle) 
        {
            UI.SetActive(false);
            return; 
        }
        UI.SetActive(true);
        health.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Die();
        }
        GroundedCheck();
        JumpAndGravity();
       // DragObject();
        Movement();
        CheckAttack();
    }
    #region health
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
    }
    public void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    #endregion

    #region Movements input manager
    private void GroundedCheck()
    {
        // set sphere position, with offset
        Vector3 spherePosition = new(transform.position.x, transform.position.y - GroundedOffset,
            transform.position.z);
        Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers,
            QueryTriggerInteraction.Ignore);

        // update animator if using character
        if (_hasAnimator)
        {
            animator.SetBool("Grounded", Grounded);
        }
    }
    private void JumpAndGravity()
    {
        if (Grounded)
        {
            _fallTimeoutDelta = FallTimeout;
            if (_hasAnimator)
            {
                animator.SetBool("Jump", false);
                animator.SetBool("FreeFall", false);
            }
            if (_verticalVelocity < 0.0f)
            {
                _verticalVelocity = -2f;
            }

            // Jump
            if (jumpAction.triggered && _jumpTimeoutDelta <= 0.0f)
            {
                // the square root of H * -2 * G = how much velocity needed to reach desired height
                _verticalVelocity = Mathf.Sqrt(JumpHeight * -9f * Gravity);

                if (_hasAnimator){ animator.SetBool("Jump", true); }
            }

            // jump timeout
            if (_jumpTimeoutDelta >= 0.0f)
            {
                _jumpTimeoutDelta -= Time.deltaTime;
            }
        }
        else
        {
            _jumpTimeoutDelta = JumpTimeout;

            // fall timeout
            if (_fallTimeoutDelta >= 0.0f)
            {
                _fallTimeoutDelta -= Time.deltaTime;
            }
            else
            {
                if (_hasAnimator)
                {
                    animator.SetBool("FreeFall", true);
                }
            }
        }

        // apply gravity over time if under terminal (multiply by delta time twice to linearly speed up over time)
        if (_verticalVelocity < _terminalVelocity)
        {
            _verticalVelocity += Gravity * Time.deltaTime;
        }
    }
    public void OnSprint(InputValue value)
    {
        SprintInput(value.isPressed);
    }
    public void SprintInput(bool newSprintState)
    {
        sprint = newSprintState;
    }
    public void Movement()
    {
        //set speed for sprint,walk
        speed = sprint ? SprintSpeed : MoveSpeed;
        //input values
        Vector2 input = moveAction.ReadValue<Vector2>();
        if (input == Vector2.zero) speed =0.0f;
        #region closed

        _animationBlend = Mathf.Lerp(_animationBlend, speed, Time.deltaTime * SpeedChangeRate);
        if (_animationBlend < 0.01f) _animationBlend = 0f;

        // normalise input direction
        Vector3 inputDirection = new Vector3(input.x, 0.0f, input.y).normalized;

        // note: Vector2's != operator uses approximation so is not floating point error prone, and is cheaper than magnitude
        // if there is a move input rotate player when the player is moving
        if (input != Vector2.zero)
        {
            _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                              cam.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }


        Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;
        #endregion

        /* Vector3 move = new Vector3(input.x, 0, input.y);
        move = move.x * transform.right.normalized + move.z * transform.forward.normalized;
         move.y = 0;

        animator.SetFloat("Speed", move.magnitude * speed);

        
        //rotate
        if (input.y > 0f)
        {
        Quaternion targetrotation = Quaternion.Euler(0, cam.eulerAngles.y, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetrotation,turnSmoothTime* Time.deltaTime);
        } */

        controller.Move(targetDirection * (Time.deltaTime * speed) + 
            new Vector3(0f, _verticalVelocity, 0f) * Time.deltaTime);
       
        if (_hasAnimator)
        {
            animator.SetFloat("Speed",_animationBlend);
            animator.SetFloat("MotionSpeed", input.magnitude);
        }
    }
    #endregion
    
    #region Car Enter/Exit
   /* public void CarDrive()
    {
        if(DriveAction.triggered && !inVehicle && GetCar_() != null)
        {
            virtualCamera.Priority += priorityBoostAmount;
            animator.SetTrigger("Car");
            animator.SetBool("Driving", true);
            controller.enabled = false;
            transform.parent = seat.transform;
            transform.position = seat.transform.position;
            transform.rotation = seat.transform.rotation;
            carController.onspot = true;
            inVehicle = true;
        }
        else if (DriveAction.triggered && inVehicle)
        {
            virtualCamera.Priority -= priorityBoostAmount;
            animator.SetBool("Driving", false);
            transform.parent = null;
            transform.position = exit.transform.position;
            controller.enabled = true;
            carController.onspot = false;
            inVehicle = false;
        }
    }*/
    public CarController GetCar_()
    {
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
            if (collider.TryGetComponent(out CarController Car))
                return Car;
        }
        return null;
    }
    #endregion

    #region Attack/intract
    public void CheckAttack()
    {
        if (attack)
        {
            GameObject projectile = Instantiate(bullet, Attackpoint.position, cam.rotation);
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();
            Vector3 forceDiretion = cam.transform.forward;
            if (Physics.Raycast(cam.position, cam.forward, out RaycastHit hit, 500f))
            {
                forceDiretion = (hit.point - Attackpoint.position).normalized;
            }
            Vector3 forcetoAdd = (forceDiretion * bulletSpeed) + (transform.up * BulletUpward);
            projectileRb.AddForce(forcetoAdd, ForceMode.Impulse);
            animator.SetTrigger("Attack");
            attack = false;
        }
    }
    public void OnAttack(InputValue value)
    {
        AttackInput(value.isPressed);
    }
    public void AttackInput(bool newSprintState)
    {
        attack = newSprintState;
    }
    #endregion
    public AutoOpen Gate()
    {
        float interactRange = 1f;
        Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
        foreach (Collider collider in colliderArray)
        {
             if (collider.TryGetComponent(out AutoOpen gate))
                return gate;
        }
        return null;
    }
}
