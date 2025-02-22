using UnityEngine;

public class DivineLighController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10.0f;
    [SerializeField]
    float rotationLimit = 20.0f;

    [SerializeField]
    float rayLenght = 100.0f;

    private float initialLenght = 0;

    private int rotateDirection = 1;
    private int inputCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        initialLenght = transform.localScale.y;
    }

    // Update is called once per frame
    void Update()
    {
        RotateLight(Time.deltaTime * rotationSpeed);

       

        if (Input.GetKeyDown(KeyCode.R))
        {
            inputCount++;
            FireLightRay();
        }
    }

    void RotateLight(float rotation)
    {
        float currentRotation = transform.rotation.eulerAngles.z;

        // Convert to range (-180, 180)
        if (currentRotation > 180)
        {
            currentRotation -= 360;
        }

        if (currentRotation > rotationLimit)
        {
            rotateDirection = -1;
        }
        else if (currentRotation <= -rotationLimit)
        {
            rotateDirection = 1;
        }

        transform.Rotate(new Vector3(0, 0, rotateDirection * rotation));
    }

    void FireLightRay()
    {
        if (inputCount > 1)
        {
            inputCount = 0;
            transform.localScale = new Vector3(transform.localScale.x, initialLenght, transform.localScale.z);
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, rayLenght, transform.localScale.z);

        }
    }
}
