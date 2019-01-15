using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== key setting  =====")]
    //存储键位的变量
    public string KeyUp = "w";
    public string KeyDown = "s";
    public string KeyLeft = "a";
    public string keyRight = "d";
    public string keyRun = "left shift";
    public string keyJump;
    private Vector3 thrustVec;
    //存储 输入的数值。
    public float Dup; //存的是Z 轴的值  WS输入的值。
    public float Dright;//存的是X轴的值  AD 输入的值 
    //方向键的变量
    public string keyJUp="up";
    public string keyJDown="down";
    public string keyJLeft="left";
    public string keyJRight="right";
    //存储 方向键输入的数值。
    public float JUp;
    public float JRight;

    //跳跃的高度
    public float jumpValue;
    public float rollValue=2.0f;
    [Header("===== out value   =====")]
   
    //为了能让变化输入的值，  能有个平滑的过度效果，   先把输入的值，存到下面的变量。 
    //再用mathf.smoothdamp,过度成 Dup  Dright.
    private float targetDup;
    private float targetDright;
    //配合mathf.smoothdamp使用， 传参不传值。这个方法需要， 具体是什么得再看看API
    private float velocityDup;
    private float velocityDright;

    //0点到输入点  的距离。
    public float Dmag;
    //角色的正前方 角色的向量
    public Vector3 Dvec;

    [Header("====== bool value =====")]
    public bool run = false;
    public bool inputEnable;
    //判断跳跃
    public bool jump=false;
    private bool lastJump;
    private bool newjump;



    void Start()
    {

    }
    void Update()
    {
        //取得另一套按键 控制方向的值
        //JUp = (Input.GetKey(keyJUp) ? 1.0f : 0) - (Input.GetKey(keyJDown)?1.0f:0);
        JUp = Input.GetAxis("Mouse Y"); ;
        //JRight = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft)?1.0f:0);
        JRight = Input.GetAxis("Mouse X"); ;

        #region   取到移动的值，  取到旋转的向量
        //下面两句是把  输入的值 存到一个变量里面， 方便可以从当前的值  过度到 输入的值
        //输入  Z轴的值  WS的值
        targetDup = (Input.GetKey(KeyUp) ? 1.0f : 0) - (Input.GetKey(KeyDown) ? 1.0f : 0);
        //输入  X轴的值  AD的值 
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(KeyLeft) ? 1.0f : 0);
        //输入的开关，可以平滑的过度 停止移动
        if (inputEnable == false)
        {
            targetDup = 0;
            targetDright = 0;
        }
        //从当前的值 过度到目标值，  并且把目标值 存储起来
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDaxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDaxis.x;
        float Dup2 = tempDaxis.y;
        //从0 到输入停留点的距离
        Dmag = Mathf.Sqrt(Dup2 * Dup2) + (Dright2 * Dright2);
        //取到一个向量。
        Dvec = Dright2 * transform.right + Dup2 * transform.forward;
        //判断是不是按下了RUN
        run = Input.GetKey(keyRun);
        #endregion
        ///跳跃  从结果出发，实现 按下键 jump变成true 松开键 为false

       newjump = Input.GetKey(keyJump);
        #region  不懂的代码，没有效果
        ////jump = newjump;
        if (newjump != lastJump && newjump == true)
        {
            this.jump = true;
            //print("jump true!!!");
        }
        else { this.jump = false; }
        lastJump = newjump;
        #endregion

    }
    /// <summary>
    /// 相当于normallizead的作用  把参数最大值为1
    /// </summary>
    /// <param 输入存储XZ轴的值="input"></param>
    /// <returns></returns>
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);

        
        return output;
    }

}
