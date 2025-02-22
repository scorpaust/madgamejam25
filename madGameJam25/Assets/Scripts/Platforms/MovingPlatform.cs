using Unity.VisualScripting;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField]
    private float speed = 5f;
    [SerializeField]
    private int startingPoint = 0;
    [SerializeField]
    private Transform[] points;

    private int index = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = points[startingPoint].position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector2.Distance(transform.position, points[index].position) < 0.02f){

            index++;
            if (index == points.Length) { 
                
                index = 0;
            
            }
        }

        transform.position = Vector2.MoveTowards(transform.position,points[index].position, speed * Time.deltaTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.position.y > transform.position.y && collision.gameObject.CompareTag(Tags.PLAYER))
        {
            collision.gameObject.GetComponent<Rigidbody2D>().gravityScale = 0;
            collision.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.PLAYER))
        {
            collision.transform.SetParent(null);
        }
    }
}



