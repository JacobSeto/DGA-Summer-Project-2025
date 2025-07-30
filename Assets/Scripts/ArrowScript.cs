using UnityEngine;
using System.Collections.Generic;

public class ArrowScript : MonoBehaviour
{
    [HideInInspector] PlayerController playerController;

    [SerializeField] float spacing = 0.5f;

    [SerializeField] float maxArrows;

    [SerializeField] private Transform[] arrows;

    private Transform arrowTransform;

    private float arrowDragDistance = 40f;


    private float[] lastArrowFill;
    private float lastArrowCount = 0;
    //private bool maxPlayed = false;
    private LayerMask bounceLayers;

    [Header("Bounce Trajectory")]
    [SerializeField] LineRenderer bounceLineRenderer;
    [SerializeField] float maxBounceDistance = 10f;
    [SerializeField] float bounceLineWidth = 0.1f;
    [SerializeField] Color bounceLineColor = Color.white;
    [SerializeField] Material bounceLineMaterial;

    void Awake()
    {
        arrowTransform = this.transform;
        lastArrowFill = new float[(int)maxArrows];
        playerController = GetComponentInParent<PlayerController>();
        SetLineRenderer();
    }

    void SetLineRenderer()
    {
        if (bounceLineRenderer == null)
        {
            // Create a new GameObject for the line renderer
            GameObject lineObj = new GameObject("BounceTrajectory");
            lineObj.transform.SetParent(this.transform);
            bounceLineRenderer = lineObj.AddComponent<LineRenderer>();
        }
        
        bounceLineRenderer.material = bounceLineMaterial;
        bounceLineRenderer.startWidth = bounceLineWidth;
        bounceLineRenderer.endWidth = bounceLineWidth;
        bounceLineRenderer.useWorldSpace = true;
        bounceLineRenderer.sortingOrder = 10;
        
        Gradient gradient = new Gradient();
        GradientColorKey[] colorKeys = new GradientColorKey[2];
        GradientAlphaKey[] alphaKeys = new GradientAlphaKey[2];
        
        colorKeys[0].color = bounceLineColor;
        colorKeys[0].time = 0f;
        alphaKeys[0].alpha = 1f;
        alphaKeys[0].time = 0f;
        
        colorKeys[1].color = bounceLineColor;
        colorKeys[1].time = 1f;
        alphaKeys[1].alpha = 0.3f;
        alphaKeys[1].time = 1f;
        
        gradient.SetKeys(colorKeys, alphaKeys);
        bounceLineRenderer.colorGradient = gradient;
    }

    void UpdateBounceTrajectory(Vector3 startPos, Vector3 direction, bool showTrajectory)
    {
        if (!showTrajectory)
        {
            bounceLineRenderer.positionCount = 0;
            return;
        }
        
        Vector3 directionNormalized = direction.normalized;
        startPos = startPos + directionNormalized * 0.1f;
        

        RaycastHit2D bounceHit = Physics2D.Raycast(startPos, directionNormalized, maxBounceDistance, bounceLayers);
        
        Vector3 endPos;
        if (bounceHit.collider != null)
        {
            endPos = bounceHit.point;
        }
        else
        {
            endPos = startPos + directionNormalized * maxBounceDistance;
        }
        
        bounceLineRenderer.positionCount = 2;
        bounceLineRenderer.SetPosition(0, startPos);
        bounceLineRenderer.SetPosition(1, endPos);
    }

    void Update()
    {
        if (playerController == null)
            return;

        bounceLayers = playerController.bounceLayers; // idk where else to set it cause its set in runtime for playercontroller

        Vector3 playerPos = playerController.OriginalPlayerPos;

        Vector3 drawOrigin = playerController.playerRb.transform.position;

        Vector3 playerMousePos = Input.mousePosition;

        Vector3 originalMousePos = playerController.OriginalMousePos;

        float drag = Vector3.Distance(originalMousePos, playerMousePos);
        int arrowCount = (int)Mathf.Clamp(Mathf.FloorToInt(drag / 30f), 1f, maxArrows);
        
        
        Vector3 direction = (playerMousePos - originalMousePos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!playerController.IsStretching)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].gameObject.SetActive(false);
            }
            UpdateBounceTrajectory(Vector3.zero, Vector3.zero, false);
            return;
        }
        
        float rayDistance = spacing * (maxArrows + 2f);
        RaycastHit2D hit = Physics2D.Raycast(drawOrigin, -direction, rayDistance, bounceLayers);
        float maxDist = hit.collider != null ? hit.distance : float.MaxValue;

       
        bool clamped = false;
        for (int i = 0; i < arrowCount; i++)
        {

            float arrowDistance = spacing * (i + 1.5f);

            if (arrowDistance < maxDist)
            {
                arrows[i].gameObject.SetActive(true);
                arrows[i].localPosition = direction * -spacing * (i + 1.5f);
                arrows[i].rotation = Quaternion.Euler(0, 0, angle);
                float totalFillProgress = drag / arrowDragDistance;
                float arrowFillAmount = Mathf.Clamp01(totalFillProgress - i);
                
                if (lastArrowFill[i] != 1 && arrowFillAmount == 1)
                {
                    AudioManager.Instance.PlayFillArrow();
                }
                else if ((lastArrowFill[i] > 0 && arrowFillAmount <= 0) || lastArrowCount > arrowCount)
                {
                    AudioManager.Instance.PlayFillArrow();
                    lastArrowCount = arrowCount;
                }
                lastArrowFill[i] = arrowFillAmount;
                arrows[i].GetComponent<ArrowUIScript>().SetFillAmount(arrowFillAmount);
            }
            else
            {
                arrows[i].gameObject.SetActive(false);
                clamped = true;
            }
        }

        if (hit.collider != null)
        {
            UpdateBounceTrajectory(hit.point, Vector3.Reflect(-direction, hit.normal), clamped);
        }
        else
        {
            UpdateBounceTrajectory(Vector3.zero, Vector3.zero, false);
        }

        lastArrowCount = arrowCount;

        for (int i = arrowCount; i < arrows.Length; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }

        
    }
}
