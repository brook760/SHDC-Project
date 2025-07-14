using UnityEngine;

public class Daimond : MonoBehaviour
{
    [Header("Float")]
    public bool isFloating = false;
    private float floatTimer;
    public float distance;
    private bool goingUp = true;
    public float floatRate;

    [Header("Rotate")]
    public bool isRotating = false;
    public Vector3 rotationAngle;
    public float rotationSpeed;

    private void Update()
    {
        if (isRotating)
        {
            transform.Rotate(rotationSpeed * Time.deltaTime * rotationAngle);
        }

        if (isFloating)
        {
            floatTimer += Time.deltaTime;
            Vector3 moveDir = new(0.0f, 0.0f, distance);
            transform.Translate(moveDir);

            if (goingUp && floatTimer >= floatRate)
            {
                goingUp = false;
                floatTimer = 0;
                distance = -distance;
            }

            else if (!goingUp && floatTimer >= floatRate)
            {
                goingUp = true;
                floatTimer = 0;
                distance = +distance;
            }
        } 
    }
}
