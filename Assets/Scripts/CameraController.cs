using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

   [SerializeField] private Camera _camera;

    //Player Location
    public Transform player;

    //Starting Camera Parameters
    private Vector3 levelView;
    private float levelSize;

    //Zoom Variables
    private float currentSize;
    public float playerSize;
    private float smoothing =.5f;
    private float velocity=0f; // remember to ask about this 
    private int zoomed = 2;
    //Camera Scrolling Variables
    public float zoomScrollSize;
    public float zoomScrollSpeed;

    //Follow variables
    private float movementSpeed=0f;
    private Vector3 currentPosition;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Starting main camera Size
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
        if (zoomed==1) // Following Player
        {
            currentSize -= (levelSize - playerSize) / 10;
            currentPosition = new Vector3(player.position.x, player.position.y, -10f);
        }
        
        if (zoomed == 2) // Whole Level overview
        {
            currentSize += (levelSize - playerSize) / 10;
            currentPosition = new Vector3(levelView.x, levelView.y, -10f);
        }
        if (zoomed == 3) // Inspecting Level
        {
            currentSize -= (levelSize - zoomScrollSize) / 10;
            
        }
        
        //Zooming Camera
        currentSize = Mathf.Clamp(currentSize, playerSize, levelSize);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, currentSize, ref velocity, smoothing);

        //Moving Camera to and from player (This needs to be changed to incorperate player Speed aka Zoomed=1 )
        if (zoomed != 3)
        {
            transform.position = Vector3.Slerp(transform.position, currentPosition, movementSpeed);
        }
        else 
        {
            //Camera Scrolling in Zoomed = 3
            if (Input.GetButton("Fire1"))
            {
                Vector3 camPos = transform.position;
                camPos.x = transform.position.x - zoomScrollSpeed * ((Input.mousePositionDelta.x / Screen.width));
                camPos.y = transform.position.y - zoomScrollSpeed * ((Input.mousePositionDelta.y / Screen.height));

                camPos.x = Mathf.Clamp(camPos.x, levelView.x - levelSize, levelView.x + levelSize);
                camPos.y = Mathf.Clamp(camPos.y, levelView.y - levelSize, levelView.y + levelSize);

                transform.position = camPos;
            }
        }

    }
}
