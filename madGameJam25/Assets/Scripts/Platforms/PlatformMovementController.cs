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
    [SerializeField] 
    private Movement movementType;

    [SerializeField]
    float speed = 1.0f;

    [SerializeField]
    float timeToChangeDir = 2.0f;
    
    private Vector2 startPos;
    private Rigidbody2D rb;

    [SerializeField]
    Vector3 m_EulerAngleVelocity = new Vector3(0,0,100);


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPos = rb.position;
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
               // Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
               // rb.MoveRotation(rb.rotation * deltaRotation);
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
            print("WaitAndPrint " + Time.time);
        }
      
    }
}
