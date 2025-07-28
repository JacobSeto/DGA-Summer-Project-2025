using UnityEngine;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector] PlayerController playerController;

    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] float minStretch = 0.2f;

    [SerializeField] float maxStretch = 1f;

    private Transform arrowTransform;

    void Awake()
    {
        arrowTransform = this.transform;

        playerController = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        if (playerController == null)
            return;

        if (playerController.IsStretching)
        {
            spriteRenderer.enabled = true;
        }
        else
        {
            spriteRenderer.enabled = false;
            return;
        }

        Vector3 playerPos = playerController.OriginalPlayerPos;
        Vector3 drawOrigin = playerController.playerRb.transform.position;
        Vector3 playerMousePos = Input.mousePosition;
        Vector3 originalMousePos = playerController.OriginalMousePos;
        
        Vector3 direction = -(playerMousePos - originalMousePos).normalized;
        float angleDegrees = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        float dragDistancePixels = (playerMousePos - originalMousePos).magnitude;
        float t = Mathf.InverseLerp(0, 300f, dragDistancePixels);
        float stretch = Mathf.Lerp(minStretch, maxStretch, t);


        arrowTransform.rotation = Quaternion.Euler(0f, 0f, angleDegrees);
        arrowTransform.localScale = new Vector3(stretch, 0.3f, 0.3f);
        float displacementStretch = Mathf.Lerp(1f, 6f, stretch);

        arrowTransform.position = drawOrigin + Vector3.Scale(
            new Vector3(displacementStretch, displacementStretch, displacementStretch),
            direction);
    }
}
