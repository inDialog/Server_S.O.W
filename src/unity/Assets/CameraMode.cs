using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CameraMode : MonoBehaviour
{
    public Toggle toggle;
    Camera mainCamera;
    GameObject originalState;
    float originalFOV;
    Vector3 Pos_FirstPerson = new Vector3(0, 0, 0.59f);
    StructuredVolumeSampling volumeSampling;
    MoveII moveII;
    bool stop;
    int origginalMask;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = this.GetComponent<Camera>();
        originalState = new GameObject();
        originalState.transform.localPosition = transform.localPosition;
        originalState.transform.localRotation = transform.localRotation;
        originalState.transform.localScale = this.transform.parent.localScale;
        origginalMask = mainCamera.cullingMask;
        originalFOV = mainCamera.fieldOfView;
        volumeSampling = GetComponent<StructuredVolumeSampling>();
        moveII = mainCamera.transform.parent.GetComponent<MoveII>();
    }


    public IEnumerator ScaleDown()
    {
        while (true)
        {
            moveII.small = true;
            this.transform.parent.transform.localScale = Vector3.Lerp(this.transform.parent.transform.localScale, originalState.transform.localScale / 10, 2 * Time.deltaTime);//Crane Scale
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV / 1.2f, 2 * Time.deltaTime);//Camera Field of View
            if (this.transform.parent.transform.localScale == originalState.transform.localScale)
                break;

            if (Input.GetKey(KeyCode.Escape))
            {
                ScaleUp();
                break;
            }
            yield return null;
        }
    }
    public IEnumerator ScaleUp()
    {
        while (true)
        {
            moveII.small = false;
            this.transform.parent.transform.localScale = Vector3.Lerp(this.transform.parent.transform.localScale, originalState.transform.localScale, 2 * Time.deltaTime);//Crane Scale
            mainCamera.fieldOfView = Mathf.Lerp(mainCamera.fieldOfView, originalFOV, 2 * Time.deltaTime);//Camera Field of View
            if (this.transform.parent.transform.localScale == originalState.transform.localScale)
            {
                break;
            }
            yield return null;
        }
    }
    public IEnumerator ZoomIn()
    {
        while (true)
        {
            moveII.stop = true;
            if (transform.localPosition != Pos_FirstPerson)
            {
                float speed = 10 * Time.deltaTime;
                transform.localPosition = Vector3.MoveTowards(transform.localPosition, Pos_FirstPerson, speed);
                volumeSampling.enabled = false;
                mainCamera.cullingMask = 0;
            }
            else
            {
                //float _speed = Time.deltaTime * 70;
                //mainCamera.transform.Rotate(new Vector3(Input.GetAxis("Vertical") * _speed, 0, 0));
                //mainCamera.transform.Rotate(new Vector3(0, Input.GetAxis("Horizontal") * _speed, 0));
                FreeCam();

                if (stop)
                    break;
                if(Input.GetKey(KeyCode.Escape))
                {
                    ZoomOut();
                    break;
                }
            }
            yield return null;
        }

    }
    public IEnumerator ZoomOut()
    {
        while (true)
        {
            volumeSampling.enabled = toggle.isOn;

            float speed = 30 * Time.deltaTime;
            this.transform.localPosition = Vector3.MoveTowards(this.transform.localPosition, originalState.transform.localPosition, speed);
            if (this.transform.localPosition == originalState.transform.position)
            {
                mainCamera.transform.localRotation = originalState.transform.localRotation;
                moveII.stop = false;
                mainCamera.cullingMask = origginalMask;
                break;
            }
            yield return null;
        }
    }
 
    public float cameraSensitivity = 90;
    public float normalMoveSpeed = 10;
    public float slowMoveFactor = 0.25f;
    public float fastMoveFactor = 3;

    void FreeCam()
    {
        if (Input.GetMouseButton(0))
        {
            Camera mycam = GetComponent<Camera>();

            float sensitivity = 0.05f;
            Vector3 vp = mycam.ScreenToViewportPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, mycam.nearClipPlane));
            vp.x -= 0.5f;
            vp.y -= 0.5f;
            vp.x *= sensitivity;
            vp.y *= sensitivity;
            vp.x += 0.5f;
            vp.y += 0.5f;
            Vector3 sp = mycam.ViewportToScreenPoint(vp);

            Vector3 v = mycam.ScreenToWorldPoint(sp);
            mainCamera.transform.LookAt(v, Vector3.up);

        }
    }
}
