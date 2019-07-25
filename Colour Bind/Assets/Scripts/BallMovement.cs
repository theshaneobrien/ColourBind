using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private GameGrid gameGrid;
    private Vector3 firstTouchPos;   //First touch position
    private Vector3 lastTouchPos;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public bool gamePaused = false;

    private bool playerIsMoving = false;
    public void SetGameGrid(GameGrid grid)
    {
        gameGrid = grid;
    }


    // Start is called before the first frame update
    void Start()
    {
        dragDistance = Screen.height * 5 / 100; //dragDistance is 15% height of the screen
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            gamePaused = !gamePaused;
        }

        if (!gamePaused)
        {
            PlayerInputs();
        }
    }

    public void PlayerInputs()
    {
        if (!playerIsMoving)
        {
            if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer ||
               Application.platform == RuntimePlatform.WebGLPlayer || Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.LinuxPlayer)
            {
                KeyControls();
            }
            else
            {
                TouchControls();
            }
            //TODO: Detect what platform we are on...
        }
    }

    public void TouchControls()
    {
        //TODO: Figure out touch inputesif (Input.touchCount == 1) // user is touching the screen with a single touch
        {
            if (Input.touches.Length > 0)
            {
                Touch touch = Input.GetTouch(0); // get the touch
                if (touch.phase == TouchPhase.Began) //check for the first touch
                {
                    firstTouchPos = touch.position;
                    lastTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lastTouchPos = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lastTouchPos = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lastTouchPos.x - firstTouchPos.x) > dragDistance || Mathf.Abs(lastTouchPos.y - firstTouchPos.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lastTouchPos.x - firstTouchPos.x) > Mathf.Abs(lastTouchPos.y - firstTouchPos.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lastTouchPos.x > firstTouchPos.x))  //If the movement was to the right)
                            {   //Right swipe
                                Debug.Log("Right Swipe");
                                MoveRight();
                            }
                            else
                            {   //Left swipe
                                Debug.Log("Left Swipe");
                                MoveLeft();
                            }
                        }
                        else
                        {   //the vertical movement is greater than the horizontal movement
                            if (lastTouchPos.y > firstTouchPos.y)  //If the movement was up
                            {   //Up swipe
                                Debug.Log("Up Swipe");
                                MoveUp();
                            }
                            else
                            {   //Down swipe
                                Debug.Log("Down Swipe");
                                MoveDown();
                            }
                        }
                    }
                    else
                    {   //It's a tap as the drag distance is less than 20% of the screen height
                        Debug.Log("Tap");
                    }
                }
            }
        }
    }

    public void KeyControls()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            MoveLeft();
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            MoveRight();
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            MoveUp();
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }

    public void MoveLeft()
    {
        //TODO:Check if we can move in the desired direction
        if (gameGrid.ValidateLeftMovement(-10))
        {
            StartCoroutine(MoveBallPos(transform, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z)));
        }
    }

    public void MoveRight()
    {
        if (gameGrid.ValidateRightMovement(10))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveBallPos(transform, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z)));
        }
    }

    public void MoveUp()
    {
        if (gameGrid.ValidateUpMovement(1))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveBallPos(transform, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1)));
        }
    }

    public void MoveDown()
    {
        if (gameGrid.ValidateDownMovement(-1))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveBallPos(transform, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1)));
        }
    }
    private IEnumerator MoveBallPos(Transform ball, Vector3 desiredPos)
    {
        playerIsMoving = true;
        float startTime = Time.time;

        float journeyLength;

        journeyLength = Vector3.Distance(ball.transform.position, desiredPos);

        Vector3 heading = new Vector3(-ball.transform.position.z, ball.transform.position.y, ball.transform.position.x) - new Vector3(-desiredPos.z, desiredPos.y, desiredPos.x);

        while (Vector3.Distance(ball.transform.position, desiredPos) > 0.01f)
        {
            float distCovered = (Time.time - startTime) * 4f;
            float fracJourney = distCovered / journeyLength;
            ball.transform.position = Vector3.Lerp(ball.transform.position, desiredPos, fracJourney * 40 * Time.deltaTime);
            //Add rotation
            ball.transform.Rotate(heading*3.6f, Space.World);
            yield return null;
        }
        ball.transform.position = desiredPos;
        playerIsMoving = false;
    }

}
