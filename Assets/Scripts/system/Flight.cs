using System;
using UnityEngine;
using Cinemachine;

public class Flight : MonoBehaviour
{
    [Header("Varaibles")]
    public float MoveSpeed;
    public float MaxFloatHeight = 10;
    public float minFloatHeight;

    public Camera FreeLookCamera;
    public float currentHeight;
    private Animator anim;

    private float xRotation;

    [Header("Camera")]
    public CinemachineFreeLook freeLook;
    public float zoomSpeed=5;
    public float StartFOV;
    public float zoomedOutFOV;

    public float OffsetStart;
    public float OffsetEnd;

    private float TargetFOV;
    private float TargetOffset;
    private bool isZoomingOut;
    private void Start()
    {
        currentHeight = transform.position.y;
        anim = GetComponent<Animator>();

        TargetFOV = StartFOV;
        TargetOffset = OffsetStart;

        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Update()
    {
        isZoomingOut = Input.GetKey(KeyCode.W);
        TargetFOV = isZoomingOut ? zoomedOutFOV : StartFOV;
        TargetOffset = isZoomingOut? OffsetEnd: OffsetStart;

        float newFov = Mathf.Lerp(freeLook.m_Orbits[1].m_Radius,TargetFOV,Time.deltaTime);
        float newOffset =
            Mathf.Lerp(freeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>().m_TrackedObjectOffset.x,
            TargetOffset,Time.deltaTime*zoomSpeed);

        CinemachineComposer Composer = freeLook.GetRig(1).GetCinemachineComponent<CinemachineComposer>();
        Composer.m_TrackedObjectOffset.x = newOffset;
        freeLook.m_Orbits[1].m_Radius =newFov;

        xRotation = FreeLookCamera.transform.rotation.eulerAngles.x;

        if (Input.GetKey(KeyCode.W))
        {
            MoveCharacter();
        }
        else
            DiableMovement();

        //currentHeight = Mathf.Clamp(transform.position.y,currentHeight,MaxFloatHeight);
        transform.position = new Vector3(transform.position.x,currentHeight,transform.position.z);

    }

    private void MoveCharacter()
    {
        Vector3 CameraForward = 
            new(FreeLookCamera.transform.forward.x,0,FreeLookCamera.transform.forward.z);
        transform.rotation = Quaternion.LookRotation(CameraForward);
        transform.Rotate(new Vector3(xRotation, 0, 0), Space.Self);

        anim.SetBool("isFlying", true);

        Vector3 forward = FreeLookCamera.transform.forward;
        Vector3 FlyDirection = forward.normalized;

        currentHeight += FlyDirection.y * MoveSpeed * Time.deltaTime;
        currentHeight = Mathf.Clamp(currentHeight, minFloatHeight, MaxFloatHeight);

        transform.position += MoveSpeed * Time.deltaTime * FlyDirection;
        transform.position = 
            new Vector3(transform.position.x, currentHeight, transform.position.z);
    }
    private void DiableMovement()
    {
        anim.SetBool("isFlying", false);
        transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
    }

}
