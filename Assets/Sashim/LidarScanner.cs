using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LidarScanner : MonoBehaviour
{
    public VisualEffect lidar_vfx;

    public float V_OFFSET = .3f;
    public float H_OFFSET = .3f;

    public int maxPoints = 2000;
    public List<Vector3> lidarPoints;
    public List<Color> lidarPointsColor;
    public float pointLife = 20f;
    public float lidarCooldown = 0.001f;
    public Color[] colors;

    List<float> pointAge;
    int positionBufferProp;
    int colorBufferProp;
    int pointCountProp;
    float lastPointTime = 0f;

    public int pointCount = 0;
    GraphicsBuffer positionBuffer;
    GraphicsBuffer directionBuffer;
    GraphicsBuffer colorBuffer;
    void Awake()
    {

        positionBufferProp = Shader.PropertyToID("positionBuffer");
        colorBufferProp = Shader.PropertyToID("colorBuffer");
        lidar_vfx.SetGraphicsBuffer(positionBufferProp, positionBuffer);
        lidar_vfx.SetGraphicsBuffer(colorBufferProp, colorBuffer);

        pointCountProp = Shader.PropertyToID("pointCount");
    }
    void Start()
    {
        pointAge = new List<float>();
        lidarPoints = new List<Vector3>();
        positionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxPoints, sizeof(float) * 3);
        colorBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, maxPoints, sizeof(float) * 4);

        lidar_vfx.SetFloat("Lifetime", pointLife);
    }

    // Update is called once per frame
    void Update()
    {
        lidar_vfx.SetInt(pointCountProp, pointCount - 1);
        if (Input.GetKey(KeyCode.Space) && pointCount < maxPoints && Time.time - lastPointTime >= lidarCooldown)
        {
            Vector3 direction = GetTargetDirection();
            RaycastHit hit;
            if (Physics.Raycast(transform.position, direction, out hit))
            {
                lidarPoints.Add(hit.point);
                pointAge.Add(pointLife);
                pointCount++;
                lastPointTime = Time.time;

                switch (hit.collider.gameObject.layer)
                {
                    case 11:
                        lidarPointsColor.Add(colors[1]);
                        break;
                    case 12:
                        lidarPointsColor.Add(colors[2]);
                        break;
                    case 13:
                        lidarPointsColor.Add(colors[3]);
                        break;
                    case 14:
                        lidarPointsColor.Add(colors[4]);
                        break;
                    case 15:
                        lidarPointsColor.Add(colors[5]);
                        break;
                    case 16:
                        lidarPointsColor.Add(colors[6]);
                        break;
                    default:
                        lidarPointsColor.Add(colors[0]);
                        break;
                }
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

        for (int i = 0; i < pointCount; i++)
        {
            pointAge[i] -= Time.deltaTime;
            if (pointAge[i] <= 0)
            {
                pointAge.RemoveAt(i);
                lidarPoints.RemoveAt(i);
                lidarPointsColor.RemoveAt(i);
                pointCount--;
                i--;
            }
        }
        positionBuffer.SetData(lidarPoints);
        colorBuffer.SetData(lidarPointsColor);
        lidar_vfx.SetGraphicsBuffer(colorBufferProp, colorBuffer);
        lidar_vfx.SetGraphicsBuffer(positionBufferProp, positionBuffer);

    }

    Vector3 GetTargetDirection()
    {
        Vector3 x_off = transform.right * Random.Range(-H_OFFSET, H_OFFSET);
        Vector3 y_off = transform.up * Random.Range(-V_OFFSET, V_OFFSET);
        return transform.forward + x_off + y_off;
    }

    void OnDrawGizmos()
    {
        for (int i = 0; i < lidarPoints.Count; i++)
        {
            Gizmos.DrawSphere(lidarPoints[i], 0.1f);
        }
    }
}
