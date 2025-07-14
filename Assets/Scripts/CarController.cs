using System;
using UnityEngine;

public class CarController : MonoBehaviour
{
    #region variables
    //private AudioSource source;
   // private float speed;

  //  [Header("Audio")]
   // public float maxSpeed = 100;
   // public float pitch = 0.05f;


    private float horizontalInput;
    private float verticalInput;
    private float steerAngle;
    private float currentBreakForce;
    private bool isbreaking;
    public bool inVechicle = false,outside;


    public GameObject player;
    public GameObject seat;
    public GameObject exit;


    [Header("UI")]
    public GameObject EnterText;
    public GameObject ExitText;
    //private Transform cam;

    [Header("Car controls")]
    public bool onspot = false;
    [SerializeField] private float motorforce;
    [SerializeField] private float breakforce;
    [SerializeField] private float maxSteeringAngle;

    [SerializeField] private WheelCollider leftFront;
    [SerializeField] private WheelCollider rightFront;
    [SerializeField] private WheelCollider leftRear;
    [SerializeField] private WheelCollider rightRear;
    
    [SerializeField] private Transform _leftFront;
    [SerializeField] private Transform _rightFront;
    [SerializeField] private Transform _leftRear;
    [SerializeField] private Transform _rightRear;
    #endregion

    private void Start()
    {
        //source = GetComponent<AudioSource>();
        //cam = Camera.main.transform;
        //rb = GetComponent<Rigidbody>();
        player = GameObject.FindWithTag("Player");
        seat = GameObject.Find("seat");
        exit = GameObject.Find ("Enter/Exit");
    }
    #region trigger/exit-car
    private void OnTriggerExit(Collider other)
    {
        //if (other.gameObject.CompareTag("Player"))
        //   source.Stop();
        if (other.CompareTag("Player"))
        {
            EnterText.SetActive(false);
            outside = false;
        }

    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.gameObject;
            EnterText.SetActive(true);
            outside = true;
        }
    }
    public void EnterCar() 
    {
        // if (other.gameObject.CompareTag("Player"))
        //    source.Play();

        if (Input.GetKey(KeyCode.E))
        {
            EnterText.SetActive(false);
            ExitText.SetActive(true);
            player.transform.parent = seat.transform;
            player.GetComponent<CapsuleCollider>().enabled = false;
            player.GetComponent<AimBehaviourBasic>().enabled = false;
            player.GetComponent<Rigidbody>().isKinematic = true;
            player.GetComponent<Animator>().SetBool("Sit", true);
            player.GetComponent<MoveBehaviour>().enabled = false;
            player.GetComponent<BasicBehaviour>().enabled = false;
            Destroy(player.GetComponent<Rigidbody>());
            player.transform.SetPositionAndRotation(seat.transform.position, seat.transform.rotation);
            player.GetComponentInChildren<ThirdPersonOrbitCamBasic>().player = transform;
            inVechicle = true;
            outside = false;
        }
    }
    public void ExitCar()
    {
        if (Input.GetKey(KeyCode.Q))
        {
            player.AddComponent<Rigidbody>();
            player.GetComponent<Rigidbody>().freezeRotation = true;
            EnterText.SetActive(true);
            ExitText.SetActive(false);
            player.GetComponent<CapsuleCollider>().enabled = true;
            player.GetComponent<AimBehaviourBasic>().enabled = true;
            player.GetComponent<BasicBehaviour>().rBody = player.GetComponent<Rigidbody>();
            player.GetComponent<MoveBehaviour>().enabled = true;
            player.GetComponent<BasicBehaviour>().enabled = true;
            player.GetComponent<Rigidbody>().useGravity = true;
            player.GetComponent<Animator>().SetBool("Sit", false);
            player.GetComponentInChildren<ThirdPersonOrbitCamBasic>().player = player.transform;
            player.transform.parent = null;
            player.transform.position = exit.transform.position;
            inVechicle = false;
        }
    }
    #endregion

    private void Update()
    {
        if (outside == true)
        {
            EnterCar();
        }
        if(inVechicle)
        {
            ExitCar();
            GetInput();
            HandleMoter();
            HandleSteering();
            UpdateWheel();
            ApplyBreaking();
        }




       // speed = (rb.velocity.magnitude*3.6f)/maxSpeed;
       // if (speed < pitch)
       //     source.pitch = pitch;
       // else
       //     source.pitch = speed;
    }
    #region carscript
    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
        isbreaking = Input.GetKey(KeyCode.Space);
    }
    private void HandleMoter()
    {
        leftFront.motorTorque = verticalInput * motorforce;
        rightFront.motorTorque = verticalInput * motorforce;
        leftRear.motorTorque = verticalInput * motorforce;
        rightRear.motorTorque = verticalInput * motorforce;
        currentBreakForce = isbreaking ? breakforce : 0;
        if (isbreaking)
        {
            ApplyBreaking();
        }
    }
    private void ApplyBreaking()
    {
        leftFront.brakeTorque = currentBreakForce;
        rightFront.brakeTorque = currentBreakForce;
        leftRear.brakeTorque = currentBreakForce;
        rightRear.brakeTorque = currentBreakForce;
    }
    private void HandleSteering()
    {
        steerAngle = maxSteeringAngle * horizontalInput;
        leftFront.steerAngle = steerAngle;
        rightFront.steerAngle = steerAngle;
    }
    private void UpdateWheel()
    {
        UpdateSingleWheel(leftFront, _leftFront);
        UpdateSingleWheel(rightFront, _rightFront);
        UpdateSingleWheel(leftRear, _leftRear);
        UpdateSingleWheel(rightRear, _rightRear);
    }

    private void UpdateSingleWheel(WheelCollider wheelCollider, Transform transform)
    {
        wheelCollider.GetWorldPose(out Vector3 pos, out Quaternion rot);
        transform.SetPositionAndRotation(pos, rot);
    }
    #endregion
}
