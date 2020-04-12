using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public string action = "";

    Vector3 Pos_FirstPerson = new Vector3(0, 0, 0.59f);
    Vector3 scale = new Vector3(0.5f, 0.5f, 0.5f);
    GameObject _cameraObject;
    Vector3 originalPos;
    Camera mainCamera;
    float originalFOV;
    int oiginalMask;

    bool alreadyPassed;
    bool highQualety;

    //private void Start()
    //{
    //    _cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
    //    mainCamera = _cameraObject.GetComponent<Camera>();
    //    originalPos = _cameraObject.transform.localPosition;
    //    originalFOV = mainCamera.fieldOfView;
    //    oldMask = mainCamera.cullingMask;
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player" & alreadyPassed == false)
        {
            _cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
            mainCamera = _cameraObject.GetComponent<Camera>();
            originalPos = _cameraObject.transform.localPosition;
            originalFOV = mainCamera.fieldOfView;
            oiginalMask = mainCamera.cullingMask;
            alreadyPassed = true;
        }
    }
    ///---- execute change
    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "Player")
        {
            if (action == "FP") ZoomIn(_cameraObject);
            else
            if (action == "Scale") ScaleDown(_cameraObject);
        }
    }
    ///--- revert to before state
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            if (action == "FP")
                StartCoroutine(ZoomOut(_cameraObject));
            else
            if (action == "Scale") StartCoroutine(ScaleUp(_cameraObject));
        }
    }
    ///////// --- Functions
    ///
    void ScaleDown(GameObject camera)
    {
        camera.transform.parent.transform.localScale = Vector3.Lerp(camera.transform.parent.transform.localScale, scale/10 ,2 *Time.deltaTime);//Crane Scale
        mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV/2, 2 * Time.deltaTime);//Camera Field of View

    }
    private IEnumerator ScaleUp(GameObject camera)
    {
        while (true)
        {
            camera.transform.parent.transform.localScale = Vector3.Lerp(camera.transform.parent.transform.localScale, scale, 2 * Time.deltaTime);//Crane Scale
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV, 2 * Time.deltaTime);//Camera Field of View
            if (camera.transform.parent.transform.localScale == scale)
            {
                break;
            }
            yield return null;
        }
    }
    void ZoomIn(GameObject camera)
    {
        if (camera.transform.localPosition != Pos_FirstPerson)
        {
            mainCamera.cullingMask = 0;
            camera.GetComponent<StructuredVolumeSampling>().enabled = false;
            Debug.Log(mainCamera.cullingMask);
            float speed = 10 * Time.deltaTime;
            camera.transform.localPosition = Vector3.MoveTowards(camera.transform.localPosition, Pos_FirstPerson, speed);
        }
    }
    private IEnumerator ZoomOut(GameObject camera)
    {
        while (true)
        {
            camera.GetComponent<StructuredVolumeSampling>().enabled = true;
            mainCamera.cullingMask = oiginalMask;
            float speed = 10 * Time.deltaTime;
            camera.transform.localPosition = Vector3.MoveTowards(camera.transform.localPosition, originalPos, speed);

            if (camera.transform.localPosition == originalPos)
            {
                break;
            }
            yield return null;
        }
    }
}
