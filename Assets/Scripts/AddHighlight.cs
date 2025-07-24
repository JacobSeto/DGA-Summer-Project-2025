using UnityEngine;

public class AddHighlight : MonoBehaviour
{
    void Start()
    {
        gameObject.AddComponent<Outline>();
    }

}
