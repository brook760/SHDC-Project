using UnityEngine;

public class CombatState : State
{
    float gravityValue;
    Vector3 currentVelocity;

    bool grounded;
    bool sheatWeapon;
    float playerSpeed;
    bool attack;

    Vector3 cVelocity;
    public CombatState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }

    public override void Enter()
    {
        base.Enter();
        sheatWeapon = false;
        attack = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        GravityVelocity.y = 0;

        Velocity = character.playerVelocity;
        playerSpeed = character.PlayerSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        if (drawWeaponAction.triggered)
        {
            sheatWeapon = true;
        }
        if (AttackAction.triggered)
        {
            attack = true;
        }
        input = MoveAction.ReadValue<Vector2>();
        Velocity = new(input.x, 0, input.y);

        Velocity = Velocity.x * character.Camtransform.right.normalized +
            Velocity.z * character.Camtransform.forward.normalized;
        Velocity.y = 0f;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        character.animator.SetFloat("speed", input.magnitude, character.speedDampTime, Time.deltaTime);

        if (sheatWeapon)
        {
            character.animator.SetTrigger("sheathSword");
            stateMachine.ChangeState(character.standing);
        }
        if (attack)
        {
            character.animator.SetTrigger("attack");
            stateMachine.ChangeState(character.attacking);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        GravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;

        if (grounded && GravityVelocity.y < 0)
        {
            GravityVelocity.y = 0;
        }

        currentVelocity = Vector3.SmoothDamp(currentVelocity, Velocity, ref cVelocity, character.VelocityDampTime);
        character.controller.Move(playerSpeed * Time.deltaTime * currentVelocity + GravityVelocity * Time.deltaTime);

        if (Velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                Quaternion.LookRotation(Velocity), character.RotationDampTime);
        }
    }
    public override void Exit()
    {
        base.Exit();

        GravityVelocity.y = 0;
        character.playerVelocity = new Vector3(input.x, 0, input.y);
        if (Velocity.sqrMagnitude > 0)
        {
            character.transform.rotation = Quaternion.LookRotation(Velocity);
        }

    }
}
