using UnityEngine;

public class ElephantController : MonoBehaviour
{
    public int hp = 3;

    public void DecreaseHP()
    {
        hp -= 1;
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
