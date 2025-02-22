using UnityEngine;
using UnityEngine.UIElements;

public class DivineLightNotWorking : MonoBehaviour
{
    [SerializeField]
    float rotationSpeed = 10.0f;
    [SerializeField]
    float rotationLimit = 20.0f;

    int rotateDirection = 1;
    float width = 0;
    
    Vector2 leftRayPos;
    Vector2 rightRayPos;

    private void Start()
    {
        width = GetComponent<Collider2D>().bounds.size.x;
       
    }

    void Update()
    {
       

        RotateLight(Time.deltaTime * rotationSpeed);
        leftRayPos = new Vector2(transform.position.x - (width / 2), transform.position.y);
        rightRayPos = new Vector2(transform.position.x + (width / 2), transform.position.y);
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
        
        Vector2 leftLightAngle = transform.rotation * leftRayPos;
        Vector2 rightLightAngle = transform.rotation * rightRayPos;
        // Dont forget to change the layer name to the correct one
        LayerMask playerLayer = LayerMask.GetMask("SamplePlayer");
        RaycastHit2D leftHit = Physics2D.Raycast(leftRayPos, leftLightAngle, 20.0f,playerLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rightRayPos, rightLightAngle, 20.0f,playerLayer);
        Debug.DrawLine(leftRayPos, leftLightAngle * 20.0f, Color.yellow,1.0f);
        Debug.DrawLine(rightRayPos, rightLightAngle * 20.0f, Color.yellow,1.0f);

       
        if (leftHit || rightHit)
        {
            print("PLayer HIt");
        }
        else
        {
           print("ENTRA AQUI");
           CheckObjectsBetween(leftHit.point, rightHit.point);
        }
    }

    void CheckObjectsBetween(Vector2 pointA, Vector2 pointB)
    {
        LayerMask playerLayer = LayerMask.GetMask("SamplePlayer");
        Collider2D[] colliders = Physics2D.OverlapAreaAll(pointA, new Vector2(pointB.x,pointB.y + 20),playerLayer);
        DrawDebugBox(pointA, new Vector2(pointB.x, pointB.y + 20), Color.red);
        foreach (var collider in colliders)
        {
          
           Debug.Log("Object found between points: " + collider.name);
            
        }
    }

    void DrawDebugBox(Vector2 pointA, Vector2 pointB, Color color)
    {
        // Calculate corner points for the box
        Vector2 topLeft = new Vector2(pointA.x, pointB.y);
        Vector2 topRight = pointB;
        Vector2 bottomLeft = pointA;
        Vector2 bottomRight = new Vector2(pointB.x, pointA.y);

        // Draw lines to form a box
        Debug.DrawLine(topLeft, topRight, color, 1.0f);
        Debug.DrawLine(topRight, bottomRight, color, 1.0f);
        Debug.DrawLine(bottomRight, bottomLeft, color, 1.0f);
        Debug.DrawLine(bottomLeft, topLeft, color, 1.0f);
    }

}
