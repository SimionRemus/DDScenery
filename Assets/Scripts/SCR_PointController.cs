using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_PointController : MonoBehaviour
{
    public bool isSelected;
    public SCR_Line lineParentScript;
    public int linePointIndex;
    private Color unselectedColor;


    // Start is called before the first frame update
    void Start()
    {
        isSelected=false;
        unselectedColor=gameObject.GetComponent<Renderer>().material.GetColor("_Color");    
    }

    // Update is called once per frame
    void Update()
    {
        if (isSelected)
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color",Color.red);
            
        }
        else
        {
            gameObject.GetComponent<Renderer>().material.SetColor("_Color",unselectedColor); 
        }
    }

    public void setParentLineAndIndex(SCR_Line linerend,int index)
    {
        lineParentScript=linerend;
        linePointIndex=index;
    }

    
    private void OnMouseDown()
    {
        Vector3 mp=GetMousePosition();
        if(isSelected && mp!=Vector3.zero)
            transform.position=mp;
    }

   
    private void OnMouseDrag()
    {
        Vector3 mp=GetMousePosition();
        if(isSelected && mp!=Vector3.zero)
            transform.position=mp;
    }

    private Vector3 GetMousePosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray,out RaycastHit raycastHit))
        {
            if (raycastHit.transform.gameObject.name == "Plane")
            {
                return raycastHit.point;
            }
        }
        return Vector3.zero;
    }

    private void OnMouseUp()
    {
        lineParentScript.splinePoints[linePointIndex]=transform.position;
    }
}
