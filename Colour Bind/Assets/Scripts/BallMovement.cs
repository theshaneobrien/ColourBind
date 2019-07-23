using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour
{
    [SerializeField]
    private GameGrid gameGrid;
    private Vector3 fp;   //First touch position
    private Vector3 lp;   //Last touch position
    private float dragDistance;  //minimum distance for a swipe to be registered
    public void SetGameGrid(GameGrid grid)
    {
        gameGrid = grid;
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlayerInputs();
    }

    public void PlayerInputs()
    {
        //TODO: Detect what platform we are on...
        KeyControls(); 
        //TouchControls();
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
                    fp = touch.position;
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Moved) // update the last position based on where they moved
                {
                    lp = touch.position;
                }
                else if (touch.phase == TouchPhase.Ended) //check if the finger is removed from the screen
                {
                    lp = touch.position;  //last touch position. Ommitted if you use list

                    //Check if drag distance is greater than 20% of the screen height
                    if (Mathf.Abs(lp.x - fp.x) > dragDistance || Mathf.Abs(lp.y - fp.y) > dragDistance)
                    {//It's a drag
                     //check if the drag is vertical or horizontal
                        if (Mathf.Abs(lp.x - fp.x) > Mathf.Abs(lp.y - fp.y))
                        {   //If the horizontal movement is greater than the vertical movement...
                            if ((lp.x > fp.x))  //If the movement was to the right)
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
                            if (lp.y > fp.y)  //If the movement was up
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
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
        }

        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
        }
    }

    public void MoveLeft()
    {
        //TODO:Check if we can move in the desired direction
        if (gameGrid.ValidateLeftMovement(-10))
        {
            StartCoroutine(MoveTileToPos(transform, new Vector3(transform.position.x - 1, transform.position.y, transform.position.z)));
        }
    }

    public void MoveRight()
    {
        if (gameGrid.ValidateRightMovement(10))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveTileToPos(transform, new Vector3(transform.position.x + 1, transform.position.y, transform.position.z)));
        }
    }

    public void MoveUp()
    {
        if (gameGrid.ValidateUpMovement(1))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveTileToPos(transform, new Vector3(transform.position.x, transform.position.y, transform.position.z + 1)));
        }
    }

    public void MoveDown()
    {
        if (gameGrid.ValidateDownMovement(-1))
        {
            //TODO:Check if we can move in the desired direction
            StartCoroutine(MoveTileToPos(transform, new Vector3(transform.position.x, transform.position.y, transform.position.z - 1)));
        }
    }
    private IEnumerator MoveTileToPos(Transform tileToMove, Vector3 desiredPos)
    {
        while (Vector3.Distance(tileToMove.transform.position, desiredPos) > 0f)
        {
            tileToMove.transform.position = Vector3.MoveTowards(tileToMove.transform.position, desiredPos, 0.1f);
            yield return null;
        }
    }

}
