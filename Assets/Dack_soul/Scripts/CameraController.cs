using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    public PlayerInput PI;

    public GameObject CameraHandle;
    public GameObject PlayerHandle;


    public float horizontalSpeed = 100f;
    public float verticalSpeed = 80f;

    private float tempEulerx;

    private GameObject playerRigdibody;

    public GameObject cam;
    private Vector3 smoothVelocity;
    // Use this for initialization
    void Awake()
    {
        CameraHandle = transform.parent.gameObject;
        PlayerHandle = CameraHandle.transform.parent.gameObject;
        playerRigdibody = PlayerHandle.GetComponent<ActorController>().PlayerHandle;
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 tempEurle = playerRigdibody.transform.eulerAngles;

        PlayerHandle.transform.Rotate(Vector3.up, PI.JRight * horizontalSpeed * Time.fixedDeltaTime);
        //CameraHandle.transform.Rotate(Vector3.right,PI.JUp*-verticalSpeed*Time.deltaTime);
        tempEulerx -= PI.JUp * verticalSpeed * Time.fixedDeltaTime;
        tempEulerx = Mathf.Clamp(tempEulerx,-40,30);
        CameraHandle.transform.localEulerAngles = new Vector3(tempEulerx,0,0);
        playerRigdibody.transform.eulerAngles = tempEurle;
        //lerp的方式 做跟踪的平滑处理
        //cam.transform.position = Vector3.Lerp(cam.transform.position, this.transform.position,0.1f);
        cam.transform.position = Vector3.SmoothDamp(cam.transform.position, this.transform.position, ref smoothVelocity, 0.05f);
        cam.transform.eulerAngles = this.transform.eulerAngles;

    }
}
