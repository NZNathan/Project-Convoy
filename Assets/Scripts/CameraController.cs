using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [Header("Camera Movement")]
    public float panSpeed = 3f;
    public float scrollSpeed = 10f;
    public float rotateSpeed = 10f;

    //Camera Switching
    public static Camera activeCamera;
    public Camera taticalCam;
    private bool taticalCamOn;
    private Camera mainCam;

    //Camera Zoom
    private float minScroll = 1f;
    private float maxScroll = 10f;

    //Camera Rotation
    private Vector3 lastMousePosition;

    // Use this for initialization
    void Start ()
    {
        mainCam = Camera.main;
        activeCamera = mainCam;
    }

    private void panCamera()
    {
        //Movement Keys
        bool wKey = Input.GetKey(KeyCode.W);
        bool aKey = Input.GetKey(KeyCode.A);
        bool sKey = Input.GetKey(KeyCode.S);
        bool dKey = Input.GetKey(KeyCode.D);

        //Sprint key
        bool shiftKey = Input.GetKey(KeyCode.LeftShift);

        Vector3 panTranslation = Vector3.zero;

        if (wKey)
        {
            panTranslation += Vector3.forward * Time.deltaTime * panSpeed;
        }
        if (sKey)
        {
            panTranslation += Vector3.back * Time.deltaTime * panSpeed;
        }
        if (aKey)
        {
            panTranslation += Vector3.left * Time.deltaTime * panSpeed;
        }
        if (dKey)
        {
            panTranslation += Vector3.right * Time.deltaTime * panSpeed;
        }

        if (shiftKey)
        {
            panTranslation *= 2;
        }

        //Save old y position so we can undo any movement on y axis
        float yPosition = transform.position.y;

        //To keep movement consistant over slower or faster time scales
        panTranslation /= Time.timeScale;

        //Move Camera
        transform.Translate(panTranslation, Space.Self);

        //Undo y movement
        transform.position = new Vector3(transform.position.x, yPosition, transform.position.z);
    }

    private void rotateCamera()
    {
        // Mouse Rotation - on middle mouse click
        if (Input.GetMouseButton(2))
        {
            // if the game window is separate from the editor window and the editor
            // window is active then you go to right-click on the game window the
            // rotation jumps if  we don't ignore the mouseDelta for that frame.
            Vector3 mouseDelta;
            if (lastMousePosition.x >= 0 &&
                lastMousePosition.y >= 0 &&
                lastMousePosition.x <= Screen.width &&
                lastMousePosition.y <= Screen.height)
                mouseDelta = Input.mousePosition - lastMousePosition;
            else
                mouseDelta = Vector3.zero;

            var rotation = Vector3.up * Time.deltaTime * rotateSpeed * mouseDelta.x;
            rotation += Vector3.left * Time.deltaTime * rotateSpeed * mouseDelta.y;

            //To keep movement consistant over slower or faster time scales
            rotation /= Time.timeScale;

            //Rotate the wrapper object
            transform.Rotate(rotation, Space.Self);

            // Make sure z rotation stays locked
            rotation = transform.rotation.eulerAngles;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        lastMousePosition = Input.mousePosition;
    }

    private void zoomCamera()
    {
        Vector3 scroll = Vector3.zero;
        scroll.y = -Input.mouseScrollDelta.y * Time.deltaTime * scrollSpeed;

        if(transform.position.y + scroll.y > maxScroll || transform.position.y + scroll.y < minScroll)
        {
            scroll.y = 0;
        }

        //To keep movement consistant over slower or faster time scales
        scroll /= Time.timeScale;

        //Move Camera
        transform.Translate(scroll, Space.World);
    }
	
    private void switchCamera()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            taticalCamOn = !taticalCamOn;

            taticalCam.gameObject.SetActive(taticalCamOn);
            mainCam.gameObject.SetActive(!taticalCamOn);

            if (taticalCamOn)
                activeCamera = taticalCam;
            else
                activeCamera = mainCam;
        }
    }
	// Update is called once per frame
	void Update ()
    {
        if (!taticalCamOn)
        {
            panCamera();
            rotateCamera();
            zoomCamera();
        }

        switchCamera();
    }
}
