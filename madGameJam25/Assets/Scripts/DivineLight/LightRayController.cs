using UnityEngine;

public class LightRayController : MonoBehaviour
{
    [SerializeField] private float maxRayDistance = 10f;
    [SerializeField] private float rayWidth = 0.5f; // Thickness of the light beam
    [SerializeField] private LayerMask obstacleLayer;
    [SerializeField] private LineRenderer lineRenderer;

    private Vector2 rayDirection = Vector2.down; // Light moves downward

    void Update()
    {
        CastLightRay();
    }

    void CastLightRay()
    {
        // Perform a BoxCast instead of a Raycast
        RaycastHit2D hit = Physics2D.BoxCast(transform.position, new Vector2(rayWidth, 0.1f), 0f, rayDirection, maxRayDistance, obstacleLayer);

        Vector2 endPosition = hit.collider != null ? hit.point : (Vector2)transform.position + rayDirection * maxRayDistance;

        if (hit.collider != null)
        {
            Debug.Log("Hit: " + hit.collider.gameObject.name);
        }

        DrawRay(endPosition);
    }

    void DrawRay(Vector2 endPosition)
    {
        if (lineRenderer != null)
        {
            lineRenderer.SetPosition(0, transform.position); // Start position
            lineRenderer.SetPosition(1, endPosition); // End position
        }
    }
}
