using UnityEngine;

public class MonkeyController : MonoBehaviour
{
    [SerializeField] GameObject monkey;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void destroyMonkey()
    {
        monkey.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            destroyMonkey();
            GameManagerScript.Instance.goInAir();
        }
    }
}
