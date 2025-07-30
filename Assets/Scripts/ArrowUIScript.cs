using UnityEngine;
using System.Collections;

public class ArrowUIScript : MonoBehaviour
{

    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private float fillDuration = 0.5f;
    private float currentFill = 0f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetFillAmount(float amount)
    {
        // Clamp to [0,1]
        float clamped = Mathf.Clamp01(amount);
        spriteRenderer.material.SetFloat("_FillAmount", clamped);
    }


    public void StartFill()
    {
        currentFill = 0f;
        StartCoroutine(FillCoroutine());
    }

    private IEnumerator FillCoroutine()
    {
        while (currentFill < 1f)
        {
            currentFill += Time.deltaTime / fillDuration;
            spriteRenderer.material.SetFloat("_FillAmount", currentFill);
            yield return null;
        }
    }

}
