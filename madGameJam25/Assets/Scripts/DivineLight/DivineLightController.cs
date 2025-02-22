using UnityEngine;

public class DivineLightController : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10.0f;
    [SerializeField]
    float rotationLimit = 20.0f;

    int rotateDirection = 1;
    

    void Update()
    {
        RotateLight(Time.deltaTime * rotationSpeed);
     
        if (Input.GetKeyDown(KeyCode.R))
        {
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
       
        if (currentRotation > rotationLimit )
        {
            rotateDirection = -1;
        }
        else if ( currentRotation <= -rotationLimit)
        {
            rotateDirection = 1;
        }

        transform.Rotate(new Vector3(0, 0,rotateDirection * rotation));
    }

    void FireLightRay()
    {
        Vector2 lightAngle = transform.rotation * transform.position;
        // Dont forget to change the layer name to the correct one
        LayerMask playerLayer = LayerMask.GetMask("SamplePlayer");
        RaycastHit2D hit = Physics2D.Raycast(transform.position, lightAngle, 20.0f,playerLayer);
        Debug.DrawLine(transform.position, lightAngle * 20.0f, Color.yellow,1.0f);

        if (hit)
        {
            print("PLayer HIt");
        }
    }
}
