using UnityEngine;
using UnityEngine.UI;

public class MeterColor : MonoBehaviour
{
    GameObject normal;
    GameObject current;
    GameObject red;
    GameObject yellow;
    GameObject green;
    PlayerController player;
    [SerializeField] Pivot pivot;
    void Start()
    {
        normal = gameObject.transform.GetChild(0).gameObject;
        red = gameObject.transform.GetChild(1).gameObject;
        red.SetActive(false);
        yellow = gameObject.transform.GetChild(2).gameObject;
        yellow.SetActive(false);
        green = gameObject.transform.GetChild(3).gameObject;
        green.SetActive(false);
        current = normal;
        player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
    }

    void Update()
    {
        if (player.launched)
        {
            if (pivot.angle >= 35)
            {
                //red highlight
                current.SetActive(false);
                red.SetActive(true);
                current = red;
            }
        else if (pivot.angle >= -35)
            {
                //yellow
                current.SetActive(false);
                yellow.SetActive(true);
                current = yellow;
            }
        else
            {
                //green
                current.SetActive(false);
                green.SetActive(true);
                current = green;
            }
        }
    }
}
