using UnityEngine;

public class InsectController : MonoBehaviour
{
    public bool removed = false;

    [SerializeField] private Rigidbody2D insect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (removed = true)
        {
            Destroy(rb);
        }

        
    }
}
