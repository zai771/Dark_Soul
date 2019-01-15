using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    //
    public GameObject PlayerHandle;
    //playerinput
    public PlayerInput PI;
    //
    private Rigidbody PlayerRigidbody;

    public float WalkSpeed;
    public float RunSpeed;

    public float jumpBack = 2.0f;
    [SerializeField]//把私有的 动画编辑器  在面板上显示
    private Animator PlayerAnimator;
    private Vector3 movingVec;
    //跳跃
    private Vector3 thrustVec;

    private bool lockPlane;
    // Use this for initialization
    void Start()
    {
        PI = GetComponent<PlayerInput>();
        PlayerAnimator = PlayerHandle.GetComponent<Animator>();
        PlayerRigidbody = GetComponent<Rigidbody>();

    }

    // Update is called once per frame
    void Update()
    {
        ////////////////////////////////下面是设置动画的代码//////////////////////////////////////
        //判断下降的速度 超过一定的值，就播翻滚的动画
        if (PlayerRigidbody.velocity.magnitude > 0f)
        { PlayerAnimator.SetTrigger("isRoll"); }

        //设置跳跃动画
        if (PI.jump)
        { PlayerAnimator.SetTrigger("jump"); }

        //设置走路--跑步动画
        float targetRunMulti = PI.Dmag * ((PI.run) ? 2.0f : 1.0f);
        PlayerAnimator.SetFloat("forward", Mathf.Lerp(PlayerAnimator.GetFloat("forward"), targetRunMulti, 0.5f));

        ////////////////////////////////下面是移动的代码//////////////////////////////////////

        //角色旋转
        if (PI.Dmag > 0.1f)
        {//下面这句代码的意思是， 从当前的方向，转到目标的方向，  做一个插值运算， 让旋转更平滑
            Vector3 targetForward = Vector3.Slerp(PlayerHandle.transform.forward, PI.Dvec, 0.3f);
            PlayerHandle.transform.forward = targetForward;
        }
        //角色移动
        if (lockPlane == false)
        {
            movingVec = PI.Dmag * PlayerHandle.transform.forward * WalkSpeed * ((PI.run) ? RunSpeed : 1.0f);
        }

    }

    private void FixedUpdate()
    {
        //PlayerRigidbody.position += movingVec*Time.fixedDeltaTime;
        PlayerRigidbody.velocity = new Vector3(movingVec.x, PlayerRigidbody.velocity.y, movingVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    /// <summary>
    /// 发送消息的代码
    /// </summary>
    #region  发送消息的代码
    public void OnJumpEnter()
    {
        //print("OnJumpEnter "); 
        PI.inputEnable = false;
        thrustVec = new Vector3(0, PI.jumpValue, 0);
        lockPlane = true;
    }

    public void IsGround()
    {
        //print("isground");
        PlayerAnimator.SetBool("isGround", true);
    }

    public void IsNotGround()
    {
        //print("isnotground");
        PlayerAnimator.SetBool("isGround", false);
    }
    public void OnEnterGround()
    {
        PI.inputEnable = true;
        lockPlane = false;

    }
    public void OnFallEnter()
    {

        PI.inputEnable = false;
        lockPlane = true;
    }
    public void OnRollEnter()
    {
        thrustVec = new Vector3(0, PI.rollValue, 0);
        PI.inputEnable = false;
        lockPlane = true;
    }
    public void OnRollUpdate()
    {
        thrustVec = PlayerHandle.transform.forward * 2f;
    }
    public void OnJumpBackEnter()
    {

        PI.inputEnable = false;
        lockPlane = true;
    }
    public void OnJumpBackUpdate()
    {
        thrustVec = PlayerHandle.transform.forward * PlayerAnimator.GetFloat("jumpBack");
    }


    #endregion
}
