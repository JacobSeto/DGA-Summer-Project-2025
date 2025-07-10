using UnityEngine;

public class PlayerTestLaunch : MonoBehaviour
{
    [SerializeField] private Rigidbody2D testPlayerBody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(testPlayerBody.linearVelocity);
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            testPlayerBody.linearVelocity = new Vector2(0f, 5f);
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            testPlayerBody.linearVelocity = new Vector2(0f, -5f);
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            testPlayerBody.linearVelocity = new Vector2(5f, 0f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            testPlayerBody.linearVelocity = new Vector2(-5f, 0f);
        }
    }
}
