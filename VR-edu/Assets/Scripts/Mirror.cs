 using UnityEngine;
 
/// <summary>
/// 镜子管理脚本 —— 挂在新建的Camera上
/// </summary>
 [ExecuteInEditMode]
  public class Mirror : MonoBehaviour
  {
     public GameObject mirrorPlane;  //镜子
     public Camera mainCamera;   //主摄像机
     private Camera mirrorCamera; //镜像摄像机
 
     private void Start()
     {
         mirrorCamera = GetComponent<Camera>();
     }

    private void Update()
     {
             if (null == mirrorPlane || null == mirrorCamera || null == mainCamera) return;
             //将主摄像机的世界坐标位置转换为镜子的局部坐标位置
            Vector3 postionInMirrorSpace = mirrorPlane.transform.
             InverseTransformPoint(mainCamera.transform.position);
             //一般y为镜面的法线方向
            postionInMirrorSpace.y = -postionInMirrorSpace.y;
            //postionInMirrorSpace.x = -postionInMirrorSpace.x;
             //转回到世界坐标系的位置
            mirrorCamera.transform.position = mirrorPlane.transform.TransformPoint(postionInMirrorSpace);
         }

     }
