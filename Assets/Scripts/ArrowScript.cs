using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [SerializeField] PlayerController playerController;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float minSpeed = 0f;

    [SerializeField] float maxSpeed = 10f;

    [SerializeField] float minStretch = 0.2f;

    [SerializeField] float maxStretch = 1f;

    private Transform arrowTransform;

    void Awake()
    {
        arrowTransform = this.transform;

        //should eventually be given to by game manager right?
        if (playerController == null)
        {
            Debug.LogError("PlayerController reference is not assigned for UI trajectory");
        }
    }

    void Update()
    {
        if (playerController == null)
            return;

        if (playerController.IsStretching)
        {
            spriteRenderer.enabled = true;
            Debug.Log("Stretching");
        }
        else
        {
            spriteRenderer.enabled = false;
            return;
        }

        Vector3 playerPos = playerController.transform.position;

        Vector3 playerMousePos = Input.mousePosition;

        Vector3 originalMousePos = playerController.OriginalMousePos;

        float magnitude = playerController.getDragDistance();

        Vector3 direction = -(playerMousePos - originalMousePos).normalized;

        // Calculate what the arrow will look like
        Vector3 targetScreenPos = originalMousePos + direction * 200f; // 100 is arbitrary to get a point "forward"
        Vector3 targetWorldPos = Camera.main.ScreenToWorldPoint(new Vector3(targetScreenPos.x, targetScreenPos.y, -Camera.main.transform.position.z));

        Vector2 directionWorld = (targetWorldPos - playerPos).normalized;

        float angleDegrees = Mathf.Atan2(directionWorld.y, directionWorld.x) * Mathf.Rad2Deg;

        float dragDistancePixels = (playerMousePos - originalMousePos).magnitude;

        float t = Mathf.InverseLerp(0, 300f, dragDistancePixels);
        float stretch = Mathf.Lerp(minStretch, maxStretch, t);

        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);

        arrowTransform.localScale = new Vector3(stretch, 0.3f, 0.3f);

        arrowTransform.position = playerPos + Vector3.Scale(new Vector3(4, 4, 4), directionWorld);

        Debug.Log($"arrowTransform at: {arrowTransform.position}");
    }
}
