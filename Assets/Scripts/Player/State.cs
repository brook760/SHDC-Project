using UnityEngine;
using UnityEngine.InputSystem;
public class State
{
    public Character character;
    public StateMachine stateMachine;

    protected Vector3 GravityVelocity;
    protected Vector3 Velocity;
    protected Vector2 input;

    public InputAction MoveAction;
    public InputAction LookAction;
    public InputAction JumpAction;
    public InputAction CrouchAction;
    public InputAction SprintAction;
    public InputAction drawWeaponAction;
    public InputAction AttackAction;

    public State(Character _character,StateMachine _stateMachine)
    {
        character = _character;
        stateMachine = _stateMachine;

        MoveAction = character.playerInput.actions["Move"];
        LookAction = character.playerInput.actions["Look"];
        JumpAction = character.playerInput.actions["Jump"];
        CrouchAction = character.playerInput.actions["Crouch"];
        SprintAction = character.playerInput.actions["Sprint"];
        drawWeaponAction = character.playerInput.actions["DrawWeapon"];
        AttackAction = character.playerInput.actions["Attack"];
    }
    public virtual void Enter()
    {
    }
    public virtual void HandleInput()
    {
    }
    public virtual void LogicUpdate()
    {

    }
    public virtual void PhysicsUpdate()
    {

    }
    public virtual void Exit()
    {

    }
}
