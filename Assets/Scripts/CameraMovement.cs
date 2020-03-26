using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {
    Camera Camera; 
    public float slowdown = 10;

    public float PanSpeed = 100; 

    bool dragging = false;

    Vector3 oldPos;

    Vector3 panOrigin; 
	// Use this for initialization
	void Start () {
        Camera = GetComponent<Camera>(); 
	}
	
	// Update is called once per frame
	void Update () {

        if (Input.GetMouseButtonDown(0))
        {
            dragging = true;
            oldPos = transform.position;
            panOrigin = Camera.main.ScreenToViewportPoint(Input.mousePosition);                    //Get the ScreenVector the mouse clicked
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition) - panOrigin;    //Get the difference between where the mouse clicked and where it moved
            transform.position = oldPos + -pos * PanSpeed;                                         //Move the position of the camera to simulate a drag, speed * 10 for screen to worldspace conversion
        }

        if (Input.GetMouseButtonUp(0))
        {
            dragging = false;
        }





        float inputX = Input.GetAxis("Horizontal");
        float inputY = Input.GetAxis("Vertical");

        if (inputX != 0)
            inputX /= slowdown;

        if (inputY != 0)
            inputY /= slowdown; 

        transform.position += new Vector3(inputX, inputY, 0);


        float zoom = Input.GetAxis("Zoom"); 
        if (zoom > 0 && Camera.orthographicSize > 0)
        {
            Camera.orthographicSize--; 
        }
        else if (zoom < 0)
        {
            Camera.orthographicSize++; 
        }

    }
}
