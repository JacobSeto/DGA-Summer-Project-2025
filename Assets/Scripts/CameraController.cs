using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

   [SerializeField] private Camera _camera;

    //Player Location
    public Transform player;

    //Starting Parameters
    private Vector3 levelView;
    private float levelSize;

    //Zoom Variables
    private float currentSize;
    public float playerSize;
    private float smoothing =.5f;
    private float velocity=0f; // remember to ask about this 
    private int zoomed = 1;
    //Camera Scrolling Variables
    public float zoomScrollSize;
    public float zoomScrollSpeed;

    //Follow variables
    private float movementSpeed=0f;
    private Vector3 currentPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        levelView = _camera.transform.position;
        levelSize = _camera.orthographicSize;
        currentSize = levelSize;
        currentPosition = levelView;
    }

    // Update is called once per frame
    void Update()
    {
        movementSpeed = (levelView - player.position).magnitude/2000;


        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Clicked");
            zoomed += 1;
            if (zoomed == 4) { zoomed = 1; }
        }
        if (zoomed==1)
        {
            currentSize -= (levelSize - playerSize) / 10;
            currentPosition = new Vector3(player.position.x, player.position.y, -10f);
        }
        
        if (zoomed == 2)
        {
            currentSize += (levelSize - playerSize) / 10;
            currentPosition = new Vector3(levelView.x, levelView.y, -10f);
        }
        if (zoomed == 3)
        {
            currentSize -= (levelSize - zoomScrollSize) / 10;
            
        }
        

        currentSize = Mathf.Clamp(currentSize, playerSize, levelSize);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, currentSize, ref velocity, smoothing);
        if (zoomed != 3)
        {
            transform.position = Vector3.Slerp(transform.position, currentPosition, movementSpeed);
        }
        else 
        {
            if (Input.GetButton("Fire1"))
            { 
                transform.position = Vector3.Slerp(transform.position, transform.position + (zoomScrollSpeed *Input.mousePositionDelta), movementSpeed);
            }
        }

    }
}
