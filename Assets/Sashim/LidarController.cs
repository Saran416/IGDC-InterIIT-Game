using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class LidarController : MonoBehaviour
{
    public int numberOfPoints = 100;
    public VisualEffect lidar_vfx;
    public Vector4[] lidarPoints;
    public Vector3[] lidarDirections;

    bool canShoot = true;
    GraphicsBuffer positionBuffer;
    GraphicsBuffer directionBuffer;
    void Start()
    {
        lidarPoints = new Vector4[numberOfPoints*numberOfPoints];
        lidarDirections = new Vector3[numberOfPoints*numberOfPoints];
        positionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, numberOfPoints*numberOfPoints, sizeof(float) * 4);
        directionBuffer = new GraphicsBuffer(GraphicsBuffer.Target.Structured, numberOfPoints*numberOfPoints, sizeof(float) * 3);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && canShoot){   
            StartCoroutine(GetLidarPoints());
        }
    }

    IEnumerator GetLidarPoints(){
        canShoot = false;
        lidar_vfx.Reinit();
        int pointcount = 0;
        for (float i=1; i < numberOfPoints; i++){
            for (float j=1; j < numberOfPoints; j++){
                Ray ray = Camera.main.ViewportPointToRay(new Vector2(j/numberOfPoints, i/numberOfPoints));
                Debug.Log(i/numberOfPoints);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit)){
                    lidarPoints[(int)(i*numberOfPoints + j)] = new Vector4(hit.point.x, hit.point.y, hit.point.z, 1f);
                    lidarDirections[(int)(i*numberOfPoints + j)] = hit.normal;
                }
                pointcount++;
            }
            positionBuffer.SetData(lidarPoints);
            lidar_vfx.SetGraphicsBuffer("LidarPointsBuffer", positionBuffer);
            lidar_vfx.SetGraphicsBuffer("LidarDirectionBuffer", directionBuffer);
            yield return null;
        }
        yield return new WaitForSeconds(5f);
        for (float i=1; i < numberOfPoints; i++){
            for (float j=1; j < numberOfPoints; j++){
                lidarPoints[(int)(i*numberOfPoints + j)]  = Vector4.zero;
            }
        }
        canShoot = true;
    }

    void OnDestroy()
    {
        // Release the ComputeBuffer when done
        if (positionBuffer != null)
            positionBuffer.Release();
            
        if (directionBuffer != null)
            directionBuffer.Release();
    }

    void OnDrawGizmos(){
        for (int i=0; i < lidarPoints.Length; i++){
            Gizmos.DrawSphere(lidarPoints[i], 0.1f);
        }
    }
}
