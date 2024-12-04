using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LidarScanner : MonoBehaviour
{
    public VisualEffect lidar_vfx;

    public float V_OFFSET = 20;
    public float H_OFFSET = 20;

    public int maxPoints = 100;
    public List<Vector3> lidarPoints;
    public float pointLife = 5f;
    public float lidarCooldown = 0.1f;

    List<float> pointAge;
    int positionBufferProp;
    int pointCountProp;
    float lastPointTime = 0f;

    int pointCount = 0;
    GraphicsBuffer positionBuffer;
    GraphicsBuffer directionBuffer;
    void Awake(){
        positionBufferProp = Shader.PropertyToID("positionBuffer");
        lidar_vfx.SetGraphicsBuffer(positionBufferProp, positionBuffer);

        pointCountProp = Shader.PropertyToID("pointCount");
    }
    void Start()
    {
        pointAge = new List<float>();
        lidarPoints = new List<Vector3>();
        positionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxPoints, sizeof(float) * 3);

        lidar_vfx.SetFloat("Lifetime", pointLife);
    }

    // Update is called once per frame
    void Update()
    {
        lidar_vfx.SetInt(pointCountProp, pointCount-1);
        if (Input.GetKey(KeyCode.Space) && pointCount < maxPoints && Time.time - lastPointTime >= lidarCooldown){
            Vector3 direction = GetTargetDirection();
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit)){
                lidarPoints.Add(hit.point);
                pointAge.Add(pointLife);
                pointCount++;
                lastPointTime = Time.time;
            }
            lidar_vfx.Play();
            // for (int i = 0; i < maxPoints; i++){
            //     Vector3 direction = GetTargetDirection();
            //     RaycastHit hit;
            //     if (Physics.Raycast(transform.position, direction, out hit)){
            //         lidarPoints[i] = hit.point;
            //     }
            // }
        }

        for (int i = 0; i < pointCount; i++){
            pointAge[i] -= Time.deltaTime;
            if (pointAge[i] <= 0){
                pointAge.RemoveAt(i);
                lidarPoints.RemoveAt(i);
                pointCount--;
                i--;
            }
        }
        positionBuffer.SetData(lidarPoints);
        lidar_vfx.SetGraphicsBuffer(positionBufferProp, positionBuffer);
        
    }

    Vector3 GetTargetDirection(){
        Vector3 x_off = transform.right * Random.Range(-H_OFFSET, H_OFFSET);
        Vector3 y_off = transform.up * Random.Range(-V_OFFSET, V_OFFSET);
        return transform.forward + x_off + y_off;
    }

    void OnDrawGizmos(){
        for (int i = 0; i < lidarPoints.Count; i++){
            Gizmos.DrawSphere(lidarPoints[i], 0.1f);
        }
    }
}
