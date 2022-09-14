using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SCR_LineCreator : MonoBehaviour
{
    public GameObject linePrefab;
    private GameObject lineGO;
    private GameObject selectedPoint;
    private SCR_Line activeLine;

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            if(lineGO==null)
            {
                lineGO=Instantiate(linePrefab);
                activeLine=lineGO.GetComponent<SCR_Line>();
            }
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            if(Physics.Raycast(ray,out RaycastHit raycastHit))
            {
                if (raycastHit.transform.gameObject.name == "Plane")
                {
                    if(lineGO)
                    {
                        activeLine.NewAnchor(raycastHit.point);
                    }
                }
                else
                {
                    if(selectedPoint)
                    {
                        selectedPoint.GetComponent<SCR_PointController>().isSelected=false;
                    }
                    selectedPoint=raycastHit.transform.gameObject;
                    selectedPoint.GetComponent<SCR_PointController>().isSelected=true;   
                }
            }

        }
        if(Input.GetMouseButtonDown(1))
        {
            if(selectedPoint)
            {
                selectedPoint.GetComponent<SCR_PointController>().isSelected=false;
                selectedPoint=null;  
            }
            else
            {
                lineGO=null;
            }

        }
    }
}
