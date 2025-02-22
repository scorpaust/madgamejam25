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
    float speed = 0;
    
    [SerializeField]
    float limit = 0;

    private Vector3 startPos;

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        switch (movementType)
        {   
            case Movement.HORIZONTAL:
                transform.position += new Vector3( speed * Time.deltaTime,0,0);
                CheckLimit();
                break;
            case Movement.VERTICAL:
                transform.position += new Vector3(0,speed * Time.deltaTime, 0);
                CheckLimit();
                break;
            case Movement.Rotate:
                transform.Rotate(new Vector3(0,0,speed * Time.deltaTime));
                break;
            default:
                break;
        }
    }

    void CheckLimit()
    {
        float distanceFromStart = Vector3.Distance(startPos, transform.position);

        if (distanceFromStart >= limit)
        {
            speed *= -1;
            transform.position = startPos + (transform.position - startPos).normalized * limit;
        }
    }
}
