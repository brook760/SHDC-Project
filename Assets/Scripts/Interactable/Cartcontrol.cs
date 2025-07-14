using UnityEngine;
using Cinemachine;

public class Cartcontrol : MonoBehaviour
{
    public CinemachineDollyCart cart;
    public SittingSystem sit;
    public float speed;
    public float move;

    [SerializeField] private CapsuleCollider Front;
    [SerializeField] private CapsuleCollider Rear;

    public void Start()
    {
        cart = GetComponent<CinemachineDollyCart>();

    }

    private void FixedUpdate()
    {
        move = Input.GetAxis("Vertical");
        if (sit.isStanding)
        {
            HandleMoter(move);
            cart.m_Speed = speed * move;
        }
        else cart.m_Speed = 0;

    }
    private void HandleMoter(float movement)
    {
        Front.transform.Rotate(new Vector3(speed * movement, 0, 0));
        Rear.transform.Rotate(new Vector3(speed * movement, 0, 0));
    }
}
