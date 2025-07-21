using TMPro;
using UnityEngine;

public class ZookeeperCounter : MonoBehaviour
{
    private Transform child;
    private TMP_Text text;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        child = gameObject.transform.GetChild(0);
        text = child.GetComponent<TMP_Text>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(GameManagerScript.Instance.numZookeepers().ToString());
    }
}
