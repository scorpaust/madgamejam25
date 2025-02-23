using UnityEngine;

public class DivineLighController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 10.0f;
    [SerializeField] private float rotationLimit = 20.0f;
    [SerializeField] private float increaseSpeed = 10.0f;

    private float initialLength;
    private int rotateDirection = 1;
    private int inputCount = 0;
    private bool isScaling = false;

    void Start()
    {
        initialLength = transform.localScale.y;
    }

    void Update()
    {
        RotateLight(Time.deltaTime * rotationSpeed);

        if (Input.GetKeyDown(KeyCode.R))
        {
            inputCount++;
            if (inputCount > 1)
            {
                ResetLightRay();
            }
            else
            {
                isScaling = true; // Start scaling
            }
        }

        if (isScaling)
        {
            ScaleLightRay();
        }
    }

    void RotateLight(float rotation)
    {
        float currentRotation = transform.rotation.eulerAngles.z;

        if (currentRotation > 180) currentRotation -= 360;

        if (currentRotation > rotationLimit) rotateDirection = -1;
        else if (currentRotation <= -rotationLimit) rotateDirection = 1;

        transform.Rotate(new Vector3(0, 0, rotateDirection * rotation));
    }

    void ScaleLightRay()
    {
        transform.localScale += new Vector3(0, increaseSpeed * Time.deltaTime, 0);
    }

    void ResetLightRay()
    {
        inputCount = 0;
        transform.localScale = new Vector3(transform.localScale.x, initialLength, transform.localScale.z);
        isScaling = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        isScaling = false; // Stop scaling when hitting an object
    }
}
