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
    [SerializeField] private LayerMask wallLayer;
    // Bounce Check
    private int bounces;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Starting main camera Size
        levelView = _camera.transform.position;
        levelSize = _camera.orthographicSize;
        bounces = 0;
        currentSize = levelSize;
        currentPosition = levelView;
        penPostition = pen[activePenn].transform.position;
        Screen.SetResolution(640,360, true);
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
            zoomed += 1;
            if (zoomed == 3) { zoomed = 1; }
        }

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            zoomed = 1;
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
            /* //Original: Screen Bound + ahead
             currentPosition = new Vector3(playerLoc.position.x + (Mathf.Clamp(playerBody.linearVelocityX, -currentSize*(16f/10f), currentSize * (16f / 10f)) ), 
                 playerLoc.position.y + (Mathf.Clamp(playerBody.linearVelocityY, -currentSize, currentSize)),
                 -10f); */

            //Option 1: Centered
             //currentPosition = new Vector3(playerLoc.position.x, playerLoc.position.y, -10f); 

            //Option 2: Slightly ahead
            float frac = .2f;
            currentPosition = new Vector3(playerLoc.position.x + (Mathf.Clamp(playerBody.linearVelocityX, -currentSize * (16f / 10f) * frac, currentSize * (16f / 10f) * frac)),
            playerLoc.position.y + (Mathf.Clamp(playerBody.linearVelocityY, -currentSize * frac, currentSize * frac)));
            //currentPosition = new Vector3(playerLoc.position.x + playerBody.linearVelocityX, playerLoc.position.y + playerBody.linearVelocityY, -10f);
        }


        //Zooming Camera
        currentSize = Mathf.Clamp(currentSize, playerSize, levelSize);
        _camera.orthographicSize = Mathf.SmoothDamp(_camera.orthographicSize, currentSize, ref velocity, smoothing);

        //Moving Camera to and from player (This needs to be changed to incorperate player Speed aka Zoomed=1 )
        if (zoomed != 3)
        {

            if (zoomed == 1) 
            { 
                currentPosition = Bind(currentPosition); 
            }
            transform.position = Vector3.Slerp(transform.position, currentPosition, .025f);

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
        Vector2 pl = new Vector2(playerLoc.position.x, playerLoc.position.y);
        float sc = 1;
        /*if (Physics2D.Linecast(pl.,pl +   new Vector2(0, sc) , LayerMask.NameToLayer("Wall"),-100,100) && Physics2D.Linecast(pl, pl + new Vector2(0, -sc), LayerMask.NameToLayer("Wall"), -100, 100)) 
        { 
            boundedPosition = playerLoc.position;
            Debug.Log("Centered");
        }
        if (Physics2D.Linecast(pl, pl + new Vector2(sc, 0), LayerMask.NameToLayer("Wall"), -100, 100) && Physics2D.Linecast(pl, pl + new Vector2(-sc, 0), LayerMask.NameToLayer("Wall"), -100, 100))
        {
            boundedPosition = playerLoc.position;
            Debug.Log("Centered");
        } */

        if (Physics2D.CircleCast(playerBody.position, .1f, Vector2.up, 2.5f, wallLayer) && Physics2D.CircleCast(playerBody.position, .1f, Vector2.down, 2.5f, wallLayer))
        {
            boundedPosition = playerLoc.position;
        }
        else if (Physics2D.CircleCast(playerBody.position, .1f, Vector2.left, 2.5f, wallLayer) && Physics2D.CircleCast(playerBody.position, .1f, Vector2.right, 2.5f, wallLayer))
        {
            boundedPosition = playerLoc.position;
        }
        else { 

            while (!pen[activePenn].OverlapPoint(new Vector2(boundedPosition.x, boundedPosition.y)) && i < 5000)
            {
                boundedPosition = boundedPosition - (boundedPosition - playerLoc.position) * .05f;
                i++;
            }
        }
        boundedPosition.z = -10f;
        return boundedPosition;
    }
}
