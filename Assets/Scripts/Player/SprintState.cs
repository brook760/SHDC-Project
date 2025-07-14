using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SprintState : State
{
    float gravityValue;
    Vector3 currentVelocity;

    bool grounded;
    bool sprint;
    float playerSpeed;
    bool sprintJump;
    Vector3 cVelocity;
    public SprintState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        sprint = false;
        sprintJump = false;
        input = Vector2.zero;
        currentVelocity = Vector3.zero;
        GravityVelocity.y = 0;

        playerSpeed = character.SprintSpeed;
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }
    public override void HandleInput()
    {
        base.HandleInput();
        input = MoveAction.ReadValue<Vector2>();
        Velocity = new Vector3(input.x, 0, input.y);

        Velocity = Velocity.x * character.Camtransform.right.normalized +
            Velocity.z * character.Camtransform.forward.normalized;
        Velocity.y = 0;

        if (SprintAction.triggered || input.sqrMagnitude == 0f)
        {
            sprint = false;
        }
        else
        {
            sprint = true;
        }
        if(JumpAction.triggered)
        {
            sprintJump = true;
        }
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        if (sprint)
        {
            character.animator.SetFloat("speed", input.magnitude + 0.5f, character.speedDampTime, Time.deltaTime);
        }
        else
        {
            stateMachine.ChangeState(character.standing);
        }
        if (sprintJump)
        {
            stateMachine.ChangeState(character.sprintJumping);
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
        currentVelocity = Vector3.SmoothDamp(currentVelocity, Velocity,ref cVelocity, character.VelocityDampTime);
       
        character.controller.Move(playerSpeed * Time.deltaTime * currentVelocity + GravityVelocity * Time.deltaTime);

        if (Velocity.magnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                Quaternion.LookRotation(Velocity), character.RotationDampTime);
        }

    }
}
