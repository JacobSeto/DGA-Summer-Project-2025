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

    private int lastArrowCount = 0;

    private bool maxPlayed = false;

    void Awake()
    {
        arrowTransform = this.transform;

        playerController = GetComponentInParent<PlayerController>();
        for (int i = 0; i < arrows.Length; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerController == null)
            return;

        Vector3 playerPos = playerController.OriginalPlayerPos;

        Vector3 drawOrigin = playerController.playerRb.transform.position;

        Vector3 playerMousePos = Input.mousePosition;

        Vector3 originalMousePos = playerController.OriginalMousePos;

        float drag = Vector3.Distance(originalMousePos, playerMousePos);
        int arrowCount = (int)Mathf.Clamp(Mathf.FloorToInt(drag / 30f), 1f, maxArrows);
        if(arrowCount != lastArrowCount)
        {

        }
        lastArrowCount = arrowCount;
        Vector3 direction = (playerMousePos - originalMousePos).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        if (!playerController.IsStretching)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                arrows[i].gameObject.SetActive(false);
            }
            AudioManager.Instance.StopMaxPull();
            return;
        }

        for (int i = 0; i < arrowCount; i++)
        {
            arrows[i].gameObject.SetActive(true);


            arrows[i].localPosition = direction * -spacing * (i + 1.5f);
            arrows[i].rotation = Quaternion.Euler(0, 0, angle);
            float totalFillProgress = drag / arrowDragDistance;
            float arrowFillAmount = Mathf.Clamp01(totalFillProgress - i);
            arrows[i].GetComponent<ArrowUIScript>().SetFillAmount(arrowFillAmount);
        }

        if (arrowCount == maxArrows && !maxPlayed)
        {
            AudioManager.Instance.StopPull();
            AudioManager.Instance.PlayMaxPull();
            maxPlayed = true;
        }
        else
        {
            AudioManager.Instance.StopMaxPull();
            maxPlayed = false;
        }

        for (int i = arrowCount; i < arrows.Length; i++)
        {
            arrows[i].gameObject.SetActive(false);
        }

        
    }
}
