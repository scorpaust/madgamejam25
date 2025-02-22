using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.Controls;
enum Movement
{
    HORIZONTAL,
    VERTICAL,
    Rotate
};

public class PlatformMovementController : MonoBehaviour
{
    [Header("Movement variables")]

    [SerializeField] 
    private Movement movementType;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float timeToChangeDir = 2.0f;

    [Header("Rotation variables")]

    [SerializeField]
    Vector3 m_EulerAngleVelocity = new Vector3(0, 0, 100);

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        StartCoroutine(WaitAndPrint(timeToChangeDir));
        
    }

    void FixedUpdate()
    {
        switch (movementType)
        {   
            case Movement.HORIZONTAL:
                float posX = rb.position.x + speed * Time.fixedDeltaTime;
                rb.MovePosition(new Vector2(posX,rb.position.y));
                break;
            case Movement.VERTICAL:
                float posY = rb.position.y + speed * Time.fixedDeltaTime;
                rb.MovePosition(new Vector2(rb.position.x, posY));
                break;
            case Movement.Rotate:
                float deltaAngle = m_EulerAngleVelocity.z * Time.fixedDeltaTime; // Get the Z-axis rotation
                rb.MoveRotation(rb.rotation + deltaAngle); // Add the angle directly
                break;
            default:
                break;
        }
    }

    private IEnumerator WaitAndPrint(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            speed *= -1;
        }
      
    }
}
