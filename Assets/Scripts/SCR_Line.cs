using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SCR_Line : MonoBehaviour
{

    public LineRenderer lineRenderer;
    public GameObject anchorPrefab;
    public GameObject controlPrefab;

    public List<Vector3> splinePoints;
    private List<Vector3> fullSpline;
    private int interpFrameCnt=50;


    private void Update()
    {
        if(splinePoints!=null)
        {
            if(fullSpline!=null)
                fullSpline.Clear();
            PopulateFullSpline();
            lineRenderer.positionCount=fullSpline.Count;
            lineRenderer.SetPositions(fullSpline.ToArray());
        }
    }
    
    private void PopulateFullSpline()
    {
        if(fullSpline==null)
        {
            fullSpline=new List<Vector3>();
        }
        for(int i=1;i<splinePoints.Count;i+=3)
        {
            // Debug.Log(i);
            // Debug.Log(splinePoints.Count);
            List<Vector3> pointArray=GenerateSplinePoints(i-1);
            if(pointArray!=null)
            {
                fullSpline.InsertRange(interpFrameCnt*(i-1)/3,pointArray);
                fullSpline.RemoveAt(fullSpline.Count - 1);
            }
        }
    }
    public void NewAnchor(Vector3 mousePosition)
    {
        if(splinePoints==null)
        {
            splinePoints=new List<Vector3>();
            AddAnchor(mousePosition);
            return;
        }
        //Add the mousePosition as point but also add the ControlPoints between the last point in "points" and mousePosition.
        AddAnchor(mousePosition);
    }

    private void AddAnchor(Vector3 point)
    {   
        if(splinePoints.Any())
        {
            Vector3[] cp=AddControlPoints(splinePoints.Last(),point);
            splinePoints.Add(cp[0]);
            GameObject tempGO=Instantiate(controlPrefab,cp[0],Quaternion.identity,transform);
            tempGO.GetComponent<SCR_PointController>().setParentLineAndIndex(this,splinePoints.Count-1);
            splinePoints.Add(cp[1]);
            tempGO=Instantiate(controlPrefab,cp[1],Quaternion.identity,transform);
            tempGO.GetComponent<SCR_PointController>().setParentLineAndIndex(this,splinePoints.Count-1);
            splinePoints.Add(point);
            tempGO=Instantiate(anchorPrefab,point,Quaternion.identity,transform);
            tempGO.GetComponent<SCR_PointController>().setParentLineAndIndex(this,splinePoints.Count-1);

        }
        else
        {
            splinePoints.Add(point);
            GameObject tempGO=Instantiate(anchorPrefab,point,Quaternion.identity,transform);
            tempGO.GetComponent<SCR_PointController>().setParentLineAndIndex(this,splinePoints.Count-1);
        }
    }

    private Vector3[] AddControlPoints(Vector3 firstAnchor, Vector3 secondAnchor)
    {
        Vector3[] cp=new Vector3[2];
        cp[0]=firstAnchor+new Vector3(1,0,1);
        cp[1]=secondAnchor+new Vector3(-1,0,-1);
        return cp;
    }

    public Vector3 QuadraticLerp(Vector3 a, Vector3 b, Vector3 c, float t)
    {
        Vector3 p0=Vector3.Lerp(a,b,t);
        Vector3 p1=Vector3.Lerp(b,c,t);
        return Vector3.Lerp(p0,p1,t);
    }

    public Vector3 CubicLerp(Vector3 a,Vector3 b, Vector3 c, Vector3 d, float t)
    {
        Vector3 p0=QuadraticLerp(a,b,c,t);
        Vector3 p1=QuadraticLerp(b,c,d,t);
        return Vector3.Lerp(p0,p1,t);
    }

    public List<Vector3> GenerateSplinePoints(int pointIndex)
    {
        if(splinePoints.Count>=4)
        {
            float interpRatio=0f;
            List<Vector3> methodResult=new List<Vector3>();
            for(int i=0; i<interpFrameCnt+1;i++)
            {
                interpRatio=(float)i/interpFrameCnt;
                methodResult.Add(CubicLerp(splinePoints[pointIndex],splinePoints[pointIndex+1],splinePoints[pointIndex+2],splinePoints[pointIndex+3],interpRatio));
            }
            return methodResult;
        }
        return null;
    }
}