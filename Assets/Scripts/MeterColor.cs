using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MeterColor : MonoBehaviour
{
    [SerializeField] Image speedometerImage;
    [SerializeField] Sprite red;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite green;
    [SerializeField] Sprite blue;
    [SerializeField] Pivot pivot;
    private float timeLeft = 0.5f;
    void Update()
    {
        if (GameManagerScript.Instance.player.launched)
        {
            if (pivot.angle >= 35)
            {
                if (timeLeft > 0.25f)
                {
                    //red
                    speedometerImage.sprite = red;
                }
                else if (timeLeft <= 0f)
                {
                    timeLeft = 0.5f;
                }
                else {
                    //blue
                    speedometerImage.sprite = blue;
                }
                timeLeft -= Time.deltaTime;
            }
            else if (pivot.angle >= -35)
            {
                //yellow
                speedometerImage.sprite = yellow;
            }
            else
            {
                //green
                speedometerImage.sprite = green;
            }
        }
    }
}
