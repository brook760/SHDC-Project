using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    [Header("Controls")]
    public float PlayerSpeed = 5.0f;
    public float crouchSpeed = 2.0f;
    public float SprintSpeed = 7.0f;
    public float JumpHeight = 0.8f;
    public float GravityMultiplier = 2;
    public float RotationSpeed = 5f;
    public float CrouchColliderHeight =1.35f;
    
    [Header("Health")]
    public int currentHealth;
    [Range(0, 200)] public int maxHealth = 100;
    public bool regenerateHealth;
    public float healthPerSecond;
    Healthbar health;
    public AudioSource Source;
    public AudioClip defeat; public AudioClip Win;

    [Header("Animation Smoothing")]
    [Range(0, 1)] public float speedDampTime = 0.1f;
    [Range(0, 1)] public float VelocityDampTime = 0.9f;
    [Range(0, 1)] public float RotationDampTime = 0.2f;
    [Range(0, 1)] public float airControl = 0.5f;

    public StateMachine MovementSM;
    public StandingState standing;
    public JumpingState jumping;
    public CrouchingState crouching;
    public LandingState landing;
    public SprintState sprinting;
    public SprintJumpState sprintJumping;
    public CombatState combat;
    public AttackState attacking;

    [HideInInspector] public float gravityValue = -9.8f;
    [HideInInspector] public float normalColliderHieght;
    [HideInInspector] public CharacterController controller;
    [HideInInspector] public PlayerInput playerInput;
    [HideInInspector] public Transform Camtransform;
    [HideInInspector] public Animator animator;
    public Vector3 playerVelocity;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        playerInput = GetComponent<PlayerInput>();
        Camtransform = Camera.main.transform;

        health = GetComponentInChildren<Healthbar>();
        health.SetHealth(currentHealth);

        MovementSM = new StateMachine();
        standing = new StandingState(this, MovementSM);
        jumping = new JumpingState(this, MovementSM);
        crouching = new CrouchingState(this, MovementSM);
        landing = new LandingState(this, MovementSM);
        sprinting = new SprintState(this, MovementSM);
        sprintJumping = new SprintJumpState(this, MovementSM);
        combat = new CombatState(this, MovementSM);
        attacking = new AttackState(this, MovementSM);

        MovementSM.Initialize(standing);

        normalColliderHieght = controller.height;
        gravityValue *= GravityMultiplier;
    }
    void Update()
    {
        MovementSM.CurrentStae.HandleInput();
        MovementSM.CurrentStae.LogicUpdate();

        if (currentHealth < maxHealth && regenerateHealth)
        {
            currentHealth += (int)(healthPerSecond * Time.deltaTime);
            health.SetHealth(currentHealth);
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            health.SetHealth(currentHealth);
        }
        else if (currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
            health.SetHealth(currentHealth);
        }
        if (currentHealth == 0)
        {
            Die();
        }
    }
    private void FixedUpdate()
    {
        MovementSM.CurrentStae.PhysicsUpdate();
    }
    public void Die()
    {
        Source.PlayOneShot(defeat, 1);
        playerInput.enabled = false;
        controller.enabled = false;
        enabled = false;
        animator.SetTrigger("death");
    }
    public void ToggleRegen()
    {
        regenerateHealth = !regenerateHealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        health.SetHealth(currentHealth);
    }
    public void GainHealth(int hp)
    {
        currentHealth += hp;
        health.SetHealth(currentHealth);
    }
}
