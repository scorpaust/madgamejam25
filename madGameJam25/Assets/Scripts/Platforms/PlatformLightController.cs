using UnityEngine;

public class PlatformLightController : MonoBehaviour
{

    public BoxCollider2D rect1;
    public BoxCollider2D rect2;

    void Update()
    {
        if (rect1.bounds.Intersects(rect2.bounds))
        {
            Bounds b1 = rect1.bounds;
            Bounds b2 = rect2.bounds;

            float xMin = Mathf.Max(b1.min.x, b2.min.x);
            float xMax = Mathf.Min(b1.max.x, b2.max.x);
            float yMin = Mathf.Max(b1.min.y, b2.min.y);
            float yMax = Mathf.Min(b1.max.y, b2.max.y);

            float width = xMax - xMin;
            float height = yMax - yMin;

            float area = width * height;

            Debug.Log("Intersection Area: " + area);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(Tags.LIGHTZONE)) 
        {
        
            print(collision.gameObject.name);
        
        }

        ContactPoint2D[] contacts = new ContactPoint2D[100];
        collision.GetContacts(contacts);
        print(contacts);
    }
    
}
