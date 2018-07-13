using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CameraController : MonoBehaviour {

    public TargetObject[] targets;
    public TextMeshProUGUI displayCamera;

    public Button sound;
    public Sprite soundOn;
    public Sprite soundOff;

    public GameObject FPSController;
    public GameObject DualTouch;

    private float currentDistance;
    private Vector3 currentTarget;
    private int currentIndex = -1;

    private float timeWithoutTouch = 0;

    private float horizontalSpeed = 2.0F;
    private float verticalSpeed = -2.0F;
    private Vector2 lastMousePosition;
    private float lastSoundVolume;



    private void Start()
    {
        NextPosition();
    }

    // Update is called once per frame
    void Update () {

        //if (targets[currentIndex].cameraType == 1 && Input.GetKeyDown(KeyCode.Escape))
        //    NextPosition();

        if ( Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            timeWithoutTouch = 4;
            lastMousePosition.x = Input.mousePosition.x/Screen.width;
            lastMousePosition.y = Input.mousePosition.y/Screen.height;
        }

        if (Input.GetMouseButtonDown(0) && EventSystem.current.IsPointerOverGameObject())
        {
            if (currentIndex == 8)
            {
                gameObject.GetComponent<Camera>().enabled = true;
                FPSController.SetActive(false);
                DualTouch.SetActive(false);
            }

        }


        if (!Input.GetMouseButton(0))
        {
            timeWithoutTouch -= Time.deltaTime;
        }       


        if (timeWithoutTouch <= 0)
        {
            if (currentIndex >= 0)
            {                
                var targetRotation = Quaternion.LookRotation(currentTarget - transform.position);                
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 2 * Time.deltaTime);


                float magnitude = new Vector3(currentDistance, currentDistance, currentDistance).magnitude;
                Vector3 desired = currentTarget + (new Vector3(currentDistance, currentDistance, currentDistance));
                float dd = Vector3.Distance(this.transform.position, currentTarget);
                if (Mathf.Abs(dd - magnitude) > 0.1f)
                {
                    this.transform.position = Vector3.Lerp(this.transform.position, desired, Time.deltaTime * 2);
                }
                else
                {
                    transform.RotateAround(currentTarget, Vector3.up, Time.deltaTime * 30);
                }
            }
        }
        else
        {
            if (Input.GetMouseButton(0) )
            {

                transform.Rotate((lastMousePosition.y - (Input.mousePosition.y/Screen.height))*100, ((Input.mousePosition.x/Screen.width) - lastMousePosition.x)*100, 0);

                lastMousePosition.x = Input.mousePosition.x / Screen.width;
                lastMousePosition.y = Input.mousePosition.y / Screen.height;
                float z = transform.eulerAngles.z;
                transform.Rotate(0, 0, -z);
            }

        }
    }

    public void NextPosition()
    {
        if (targets.Length > 0)
        {
            timeWithoutTouch = 0;
            if (currentIndex < targets.Length - 1)
            {
                currentIndex++;
                if (targets[currentIndex].cameraType == 0)
                {
                    currentTarget = targets[currentIndex].gameObject.transform.position;
                    currentDistance = targets[currentIndex].distance;
                    displayCamera.text = targets[currentIndex].displayName;
                    gameObject.GetComponent<Camera>().enabled = true;
                    FPSController.SetActive(false);
                    DualTouch.SetActive(false);

                }
                else if (targets[currentIndex].cameraType == 1)
                {
                    displayCamera.text = targets[currentIndex].displayName;
                    gameObject.GetComponent<Camera>().enabled = false;
                    FPSController.SetActive(true);
                    DualTouch.SetActive(true);
                }

            }
            else
            {
                currentIndex = -1;
                NextPosition();
            }
        }
        
    }

    public void PreviousPosition()
    {
        if (targets.Length > 0)
        {
            timeWithoutTouch = 0;
            if (currentIndex > 0)
            {
                currentIndex--;
                if (targets[currentIndex].cameraType == 0)
                {
                    currentTarget = targets[currentIndex].gameObject.transform.position;
                    currentDistance = targets[currentIndex].distance;
                    displayCamera.text = targets[currentIndex].displayName;
                    gameObject.GetComponent<Camera>().enabled = true;
                    FPSController.SetActive(false);
                    DualTouch.SetActive(false);
                }
                else if (targets[currentIndex].cameraType == 1)
                {
                    
                    displayCamera.text = targets[currentIndex].displayName;
                    gameObject.GetComponent<Camera>().enabled = false;
                    FPSController.SetActive(true);
                    DualTouch.SetActive(true);
                }

            }
            else
            {
                currentIndex = targets.Length;
                PreviousPosition();
            }
        }

    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ToggleSound()
    {
        if (AudioListener.volume == 0)
        {
            AudioListener.volume = lastSoundVolume;
            sound.image.sprite = soundOn;
        }
        else
        {
            lastSoundVolume = AudioListener.volume;
            AudioListener.volume = 0;
            sound.image.sprite = soundOff;
        }
    }
}
