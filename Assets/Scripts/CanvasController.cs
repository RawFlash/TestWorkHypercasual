using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class CanvasController : MonoBehaviour, IPointerClickHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    public LineRenderer lineRenderer;

    private bool isClick;

    List<Vector3> pointsPos = new List<Vector3>();
    public List<Vector3> pointsDraw = new List<Vector3>();

    public SplineComputer splineComputer;

    public void OnPointerClick(PointerEventData eventData)
    {
        /*
        if (!MainController.instance.isPlay)
        {
            Plane plane = new(Vector3.up, new Vector3(0, 0.02f, 0));
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);


            if (plane.Raycast(ray, out float hitDist))
            {
                Vector3 hitPoint = ray.GetPoint(hitDist);

                MainController.instance.AddMan(hitPoint, false);
            }
        }
        */
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isClick = true;
    }

    private void FixedUpdate()
    {
        SplinePoint[] splinePoints = new SplinePoint[pointsDraw.Count];

        for (int i = 0; i < pointsDraw.Count; i++)
        {
            //pointsDraw[i] = new Vector3(pointsDraw[i].x, pointsDraw[i].y, Camera.main.gameObject.transform.position.z + 0.4f);

            splinePoints[i] = new SplinePoint(new Vector3(pointsDraw[i].x, pointsDraw[i].y, Camera.main.gameObject.transform.position.z + 0.45f));

        }

        splineComputer.SetPoints(splinePoints);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        

        if (isClick)
        {
            //Debug.Log(Camera.main.ScreenToWorldPoint(new Vector3(eventData.position.x, eventData.position.y, 0.7f))) ;


            Vector3 tempPos = new(eventData.position.x, eventData.position.y, 0.7f);

            

            Vector3 tempWorldPos = Camera.main.ScreenToWorldPoint(tempPos);

            pointsPos.Add(tempWorldPos);



            Debug.Log(tempWorldPos);
            Vector3 tempPos2 = new(tempWorldPos.x, tempWorldPos.y, 0);
            pointsDraw.Add(tempPos2);

            

            /*
             * 
             * 

            Vector3[] tempMas = new Vector3[pointsDraw.Count];
            for (int i =0; i < pointsDraw.Count; i++) 
            {
                pointsDraw[i] = new Vector3(pointsDraw[i].x, pointsDraw[i].y, tempPos2.z);
                tempMas[i] = pointsDraw[i];

                //Debug.Log(i + " " + point);
            }

            lineRenderer.positionCount = tempMas.Length;
            lineRenderer.SetPositions(tempMas);
            */

            /*
            //tempPos2 = new Vector3(tempPos2.x, tempPos2.y,  );

            */
        }


        /*
        if(MainController.instance.isPlay && isClick)
        {
            Plane plane = new(Vector3.up, new Vector3(0, 0, 0));
            Ray ray = Camera.main.ScreenPointToRay(eventData.position);


            if (plane.Raycast(ray, out float hitDist))
            {
                Vector3 hitPoint = ray.GetPoint(hitDist);

                float newPosX = hitPoint.x;


                if (newPosX < MainController.instance.leftUpStartZone.transform.position.x - MainController.instance.leftMoveZone)
                {
                    newPosX = MainController.instance.leftUpStartZone.transform.position.x - MainController.instance.leftMoveZone;
                }

                if (newPosX > MainController.instance.rightDownStartZone.transform.position.x - MainController.instance.rightMoveZone)
                {
                    newPosX = MainController.instance.rightDownStartZone.transform.position.x - MainController.instance.rightMoveZone;
                }

                //Debug.Log(newPosX);

                MainController.instance.mansParent.GetComponent<SplineFollower>().motion.offset = new Vector2(newPosX, 0);
            }
        }
        */
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        MainController.instance.SetPositions(pointsPos);

        isClick = false;
        pointsPos = new List<Vector3>();
        pointsDraw = new List<Vector3>();

        
        splineComputer.SetPoints(new SplinePoint[0]);

        //lineRenderer.positionCount = 0;

        if (!MainController.instance.isPlay)
        {
            MainController.instance.Play();
        }

    }
}
