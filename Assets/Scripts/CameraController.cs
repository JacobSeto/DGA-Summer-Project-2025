using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraController : MonoBehaviour
{

   [SerializeField] private Camera _camera;
    [SerializeField] private Collider2D[] pen;

    //Player Location
    public Transform playerLoc;

    //Player Body (used to check speed)
    public Rigidbody2D playerBody;

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
    private float movementSpeed=5f;
    private Vector3 currentPosition;
    //Screen Bound
    private int activePenn=0;
    private Vector3 penPostition;
    //
    [SerializeField] private PlayerController player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Starting main camera Size
        levelView = _camera.transform.position;
        levelSize = _camera.orthographicSize;
        
        currentSize = levelSize;
        currentPosition = levelView;
        penPostition = pen[activePenn].transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        movementSpeed = (levelView - playerLoc.position).magnitude/2000;

        for (int i = 0; i < pen.Length; i++)
        {
            if (pen[i].OverlapPoint(player.transform.position))
            {
                activePenn = i;
            }
        }
        
        if (Input.GetButtonDown("Jump"))
        {
            Debug.Log("Clicked");
            zoomed += 1;
            if (zoomed == 3) { zoomed = 1; }
            Debug.Log("Zoom level: " + zoomed);
        }
        
        
        if (zoomed == 2) // Whole Level overview
        {
            currentSize = levelSize;
            currentPosition = new Vector3(levelView.x, levelView.y, -10f);
        }
        if (zoomed == 3) // Inspecting Level
        {
            currentSize = zoomScrollSize;
            
        }
        if (zoomed==1 || player.launched) // Following Player
        {
            zoomed = 1;
            currentSize = playerSize;
            currentPosition = new Vector3(playerLoc.position.x + (Mathf.Clamp(playerBody.linearVelocityX, -currentSize*(16f/10f), currentSize * (16f / 10f)) ), 
                playerLoc.position.y + (Mathf.Clamp(playerBody.linearVelocityY, -currentSize, currentSize)),
                -10f);
            //currentPosition = new Vector3(playerLoc.position.x + playerBody.linearVelocityX, playerLoc.position.y + playerBody.linearVelocityY, -10f);
        }


        //Zooming Camera
        currentSize = Mathf.Clamp(currentSize, playerSize, levelSize);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, currentSize, ref velocity, smoothing);

        //Moving Camera to and from player (This needs to be changed to incorperate player Speed aka Zoomed=1 )
        if (zoomed != 3)
        {
            // float posX = Mathf.Clamp(currentPosition.x, levelView.x - levelSize - currentSize, levelView.x + levelSize + currentSize);
            //float posY = Mathf.Clamp(currentPosition.y, levelView.y - levelSize - currentSize, levelView.y  +levelSize + currentSize);
            //currentPosition=new Vector3 (posX, posY, -10f);
            if (zoomed == 1) 
            { 
                currentPosition = Bind(currentPosition); 
            }
            transform.position = Vector3.Slerp(transform.position, currentPosition, movementSpeed);
            
        }
        else 
        {
            //Camera Scrolling in Zoomed = 3
            if (Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                Vector3 camPos = transform.position;
                camPos.x = transform.position.x + zoomScrollSpeed * Input.GetAxis("Horizontal");
                camPos.y = transform.position.y + zoomScrollSpeed * Input.GetAxis("Vertical");

                camPos.x = Mathf.Clamp(camPos.x, levelView.x - levelSize - zoomScrollSize, levelView.x + levelSize + zoomScrollSize);
                camPos.y = Mathf.Clamp(camPos.y, levelView.y - levelSize, levelView.y + levelSize);

                transform.position = camPos;             
            }
        }


    }
    private Vector3 Bind(Vector3 curPosition)
    {
        Vector3 boundedPosition = curPosition;
        int i = 0;
        while(!pen[activePenn].OverlapPoint(new Vector2(boundedPosition.x, boundedPosition.y)) && i<5000 ) 
        {
            boundedPosition = boundedPosition - (boundedPosition - playerLoc.position) * .05f;
            i++;
        }
        boundedPosition.z = -10f;
        return boundedPosition;
    }
}
