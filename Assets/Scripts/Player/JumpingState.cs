using UnityEngine;

public class JumpingState : State
{
    bool grounded;

    float gravityValue;
    float jumpHeight;
    float playerspeed;

    Vector3 airVeclocity;
    public JumpingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        base.Enter();

        grounded = false;
        gravityValue = character.gravityValue;
        jumpHeight = character.JumpHeight;
        playerspeed = character.PlayerSpeed;
        GravityVelocity.y = 0;

        character.animator.SetFloat("speed", 0);
        character.animator.SetTrigger("jump");
        Jump();
    }
    public override void HandleInput()
    {
        base.HandleInput();

        input = MoveAction.ReadValue<Vector2>();
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if(grounded)
        {
            stateMachine.ChangeState(character.landing);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        if (!grounded)
        {
            Velocity = character.playerVelocity;
            airVeclocity = new(input.x, 0, input.y);

            Velocity = Velocity.x * character.Camtransform.right.normalized +
                Velocity.z* character.Camtransform.forward.normalized;
            Velocity.y = 0;

            airVeclocity = airVeclocity.x *character.Camtransform.right.normalized +
                airVeclocity.z*character.Camtransform.forward.normalized;
            airVeclocity.y = 0;

            character.controller.Move(GravityVelocity * Time.deltaTime + playerspeed * Time.deltaTime * (airVeclocity * character.airControl +
                Velocity * (1 - character.airControl)));
        }
        GravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
    }
    public override void Exit() { base.Exit();}
    void Jump()
    {
        GravityVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
    }
}
