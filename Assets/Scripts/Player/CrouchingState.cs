using UnityEngine;

public class CrouchingState : State
{
    float playerSpeed;
    bool belowCeiling;
    bool crouchHold;

    bool grounded;
    float gravityValue;
    Vector3 currentVelocity;
    public CrouchingState(Character _character, StateMachine _stateMachine) : base(_character, _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;
    }
    public override void Enter()
    {
        base.Enter();
        character.animator.SetTrigger("crouch");
        belowCeiling = false;
        crouchHold = false;
        GravityVelocity.y = 0;

        playerSpeed = character.crouchSpeed;
        character.controller.height = character.CrouchColliderHeight;
        character.controller.center = new Vector3(0, character.CrouchColliderHeight / 2f, 0);
        grounded = character.controller.isGrounded;
        gravityValue = character.gravityValue;
    }
    public override void Exit()
    {
        base.Exit();
        character.controller.height = character.normalColliderHieght;
        character.controller.center = new Vector3(0, character.normalColliderHieght / 2f, 0);
        GravityVelocity.y = 0;
        character.playerVelocity = new Vector3(input.x, 0, input.y);
        character.animator.SetTrigger("move");
    }
    public override void HandleInput()
    {
        base.HandleInput();
        if(CrouchAction.triggered && !belowCeiling)
        {
            crouchHold = true;
        }
        input = MoveAction.ReadValue<Vector2>();
        Velocity = new(input.x,0,input.y);

        Velocity = Velocity.x *character.Camtransform.right.normalized + 
            Velocity.z *character.Camtransform.forward.normalized;
        Velocity.y = 0;
    }
    public override void LogicUpdate()
    {
        base.LogicUpdate();
        character.animator.SetFloat("speed",input.magnitude,character.speedDampTime,Time.deltaTime);
        if (crouchHold)
        {
            stateMachine.ChangeState(character.standing);
        }
    }
    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
        belowCeiling = CheckCollisionOverlap(character.transform.position + Vector3.up * character.normalColliderHieght);
        GravityVelocity.y += gravityValue * Time.deltaTime;
        grounded = character.controller.isGrounded;
        if(grounded && GravityVelocity.y < 0) 
        {
            GravityVelocity.y = 0;
        }
        currentVelocity = Vector3.Lerp(currentVelocity, Velocity, character.VelocityDampTime);

        character.controller.Move(playerSpeed * Time.deltaTime * currentVelocity + GravityVelocity * Time.deltaTime);
       
        if(Velocity.magnitude > 0)
        {
            character.transform.rotation = Quaternion.Slerp(character.transform.rotation,
                Quaternion.LookRotation(Velocity),character.RotationDampTime);
        }
    }
    public bool CheckCollisionOverlap(Vector3 targetPostion)
    {
        int layermask = 1 <<LayerMask.NameToLayer("Player");
        layermask = ~layermask;
        RaycastHit hit;

        Vector3 direction = targetPostion - character.transform.position;
        if(Physics.Raycast(character.transform.position,direction, out hit,character.normalColliderHieght,layermask)) 
        {
            Debug.DrawRay(character.transform.position, direction * hit.distance, Color.yellow);
            return true;
        }
        else
        {
            Debug.DrawRay(character.transform.position, direction *character.normalColliderHieght, Color.white);
            return false;
        }

    }
}
