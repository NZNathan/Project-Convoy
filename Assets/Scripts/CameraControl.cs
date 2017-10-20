using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    // WASD Panning
    public float panTimeConstant = 20f; // Time to reach max panning speed

    // Mouse right-down rotation
    public float rotateSpeed = 10; // mouse down rotation speed about x and y axes
    public float zoomSpeed = 2;    // zoom speed

    public float panSpeed = 20f;
    Vector3 panTranslation;
    Vector3 rotateTranslation;
    bool wKeyDown = false;
    bool aKeyDown = false;
    bool sKeyDown = false;
    bool dKeyDown = false;
    bool qKeyDown = false;
    bool eKeyDown = false;

    Vector3 lastMousePosition;

    public static CameraControl cameraRig;

    //Zoom Control
    float yLock = 0f; //Camera height
    float yMin = -10f;
    float yMax = 50f;

    void Start()
    {
        cameraRig = this;
    }

    public CameraControl instance()
    {
        return cameraRig;
    }

    void Update()
    {
        //
        // WASDQE Panning

        // read key inputs
        wKeyDown = Input.GetKey(KeyCode.W);
        aKeyDown = Input.GetKey(KeyCode.A);
        sKeyDown = Input.GetKey(KeyCode.S);
        dKeyDown = Input.GetKey(KeyCode.D);
        qKeyDown = Input.GetKey(KeyCode.Q);
        eKeyDown = Input.GetKey(KeyCode.E);

        // determine panTranslation
        panTranslation = Vector3.zero;
        rotateTranslation = Vector3.zero;

        if (dKeyDown && !aKeyDown)
            panTranslation += Vector3.right * Time.deltaTime * panSpeed;
        else if (aKeyDown && !dKeyDown)
            panTranslation += Vector3.left * Time.deltaTime * panSpeed;

        if (wKeyDown && !sKeyDown)
            panTranslation += Vector3.forward * Time.deltaTime * panSpeed;
        //transform.Translate(new Vector3(0f,0f, 0.1f), Space.World );
        else if (sKeyDown && !wKeyDown)
            panTranslation += Vector3.back * Time.deltaTime * panSpeed;

        if (eKeyDown && !qKeyDown) 
        {
            rotateTranslation += Vector3.down * Time.deltaTime * panSpeed;
            panTranslation += Vector3.right * Time.deltaTime * panSpeed;
        }
        else if (qKeyDown && !eKeyDown)
        {
            rotateTranslation += Vector3.up * Time.deltaTime * panSpeed;
            panTranslation += Vector3.left * Time.deltaTime * panSpeed;
        }

        //Move Camera
        transform.Translate(panTranslation, Space.Self);
        //Rotate Camera
        transform.Rotate(rotateTranslation, Space.Self);

        //Lock the Y-axis for movement
        transform.position = new Vector3(transform.position.x, yLock, transform.position.z);
        //Lock the Z-axis for rotation
        Vector3 rotation2 = transform.rotation.eulerAngles;
        

        //Zoom for mouse
        yLock -= Input.mouseScrollDelta.y * Time.deltaTime * panSpeed;
        if (yLock < yMin)
            yLock = yMin;
        if (yLock > yMax)
            yLock = yMax;

        rotation2.z = 0;
        rotation2.x += (yLock - rotation2.x) * 0.25f;
        transform.rotation = Quaternion.Euler(rotation2);


        //
        // Mouse Rotation
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
            transform.Rotate(rotation, Space.Self);

            // Make sure z rotation stays locked
            rotation = transform.rotation.eulerAngles;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        lastMousePosition = Input.mousePosition;
    }
}