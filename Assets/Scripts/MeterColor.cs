using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MeterColor : MonoBehaviour
{
    [SerializeField] Image speedometerImage;
    [SerializeField] Sprite red;
    [SerializeField] Sprite yellow;
    [SerializeField] Sprite green;
    [SerializeField] Pivot pivot;
    void Update()
    {
        if (GameManagerScript.Instance.player.launched)
        {
            if (pivot.angle >= 35)
            {
                //red highlight
                speedometerImage.sprite = red;
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
