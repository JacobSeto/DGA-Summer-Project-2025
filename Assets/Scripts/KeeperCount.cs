using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeeperCount : MonoBehaviour
{
    [Header("Zookeeper Counter UI")]
    [SerializeField] private Image keeperIcon;
    [SerializeField] private TMP_Text countText;
    
    [Header("Animation Settings")]
    [SerializeField] private Sprite normalIcon;
    [SerializeField] private Sprite ouchIcon;
    [SerializeField] private float flickDistance = 100f;
    [SerializeField] private float fallDistance = 500f;
    [SerializeField] private float animationDuration = 1f;
    [SerializeField] private float rotationSpeed = 360f; 
    [SerializeField] private AnimationCurve flickCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    [SerializeField] private AnimationCurve fallCurve = new AnimationCurve(new Keyframe(0, 0), new Keyframe(1, 1));
    
    private Vector3 originalIconPosition;
    private int currentCount = -1;
    private bool isAnimating = false;
    private bool initialized = false;
    private Image animatingIcon; 
    
    void Start()
    {
        if (keeperIcon != null)
        {
            originalIconPosition = keeperIcon.transform.localPosition;
        }
        StartCoroutine(InitializeCount());
    }
    
    IEnumerator InitializeCount()
    {
        yield return null;
        if (GameManagerScript.Instance != null)
        {
            currentCount = GameManagerScript.Instance.numZookeepers();
            UpdateDisplay();
            initialized = true;
        }
    }
    
    void Update()
    {
        if (GameManagerScript.Instance != null && initialized)
        {
            int newCount = GameManagerScript.Instance.numZookeepers();
            if (newCount != currentCount && !isAnimating)
            {
                StartCoroutine(AnimateDecrement(newCount));
            }
        }
    }
    
    void UpdateDisplay()
    {
        if (keeperIcon == null || countText == null) return;
        
        if (currentCount > 0)
        {
            keeperIcon.gameObject.SetActive(true);
            countText.gameObject.SetActive(true);
            countText.text = "x" + currentCount.ToString();
            keeperIcon.sprite = normalIcon;
        }
        else
        {
            keeperIcon.gameObject.SetActive(false);
            countText.gameObject.SetActive(false);
        }
    }
    
    IEnumerator AnimateDecrement(int newCount)
    {
        if (isAnimating || keeperIcon == null) yield break;
        
        isAnimating = true;
        
        GameObject animObj = new GameObject("AnimatingKeeper");
        animObj.transform.SetParent(keeperIcon.transform.parent);
        animObj.transform.localPosition = originalIconPosition;
        animObj.transform.localScale = keeperIcon.transform.localScale;
        
        animatingIcon = animObj.AddComponent<Image>();
        animatingIcon.sprite = ouchIcon != null ? ouchIcon : normalIcon;
        animatingIcon.color = keeperIcon.color;
        
        float randomRotationDirection = Random.Range(0, 2) == 0 ? -1f : 1f; 
        float totalRotation = 0f;
        
        currentCount = newCount;
        UpdateDisplay();
        
        Vector3 flickTarget = originalIconPosition + Vector3.left * flickDistance + Vector3.up * flickDistance;
        float elapsedTime = 0f;

        while (elapsedTime < animationDuration * 0.3f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (animationDuration * 0.3f);
            float curveValue = flickCurve.Evaluate(t);
            
            animatingIcon.transform.localPosition = Vector3.Lerp(originalIconPosition, flickTarget, curveValue);
            yield return null;
        }
        
        Vector3 fallTarget = flickTarget + Vector3.down * fallDistance;
        elapsedTime = 0f;
        
        while (elapsedTime < animationDuration * 0.7f)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / (animationDuration * 0.7f);
            float curveValue = fallCurve.Evaluate(t);
            
            animatingIcon.transform.localPosition = Vector3.Lerp(flickTarget, fallTarget, curveValue);
            
            float rotationThisFrame = rotationSpeed * Time.deltaTime * randomRotationDirection;
            totalRotation += rotationThisFrame;
            animatingIcon.transform.localRotation = Quaternion.Euler(0, 0, totalRotation);
            
            Color iconColor = animatingIcon.color;
            iconColor.a = Mathf.Lerp(1f, 0f, curveValue);
            animatingIcon.color = iconColor;
            
            yield return null;
        }
        
        if (animatingIcon != null)
        {
            Destroy(animatingIcon.gameObject);
        }
        
        isAnimating = false;
    }
}