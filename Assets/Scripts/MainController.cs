using Dreamteck.Splines;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Dreamteck.Splines.ParticleController;
using static UnityEditor.PlayerSettings;

public class MainController : MonoBehaviour
{
    public static MainController instance;

    public GameObject manPrefab, bloodPrefab, smokePrefab, mansParent;

    public GameObject leftUpStartZone, rightDownStartZone;

    public Button play;

    public SplineFollower cameraSpline, mansParentSpline;

    public bool isPlay;

    public List<ManController> manControllers;

    public float leftMoveZone, rightMoveZone;

    Vector3 distToMans;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        play.onClick.AddListener(Play);

        leftMoveZone = 1;
        rightMoveZone = -1;
        UpdatePlayButton();

        distToMans = Camera.main.transform.position - mansParent.transform.position;
    }

    private void Update()
    {
        Camera.main.transform.position = mansParent.transform.position + distToMans;
    }

    public void SetPositions(List<Vector3> positions)
    {
        if (manControllers.Count == 0)
        {
            return;
        }

        int stepPos = (int)Mathf.Floor(positions.Count / manControllers.Count);

        int currStepPos = 0;

        foreach(var controller in manControllers)
        {
            Vector3 tempPos = new Vector3(positions[currStepPos].x , 0, positions[currStepPos].y);
            controller.MoveToNewPos(tempPos);
            //controller.gameObject.transform.localPosition = tempPos;
            currStepPos += stepPos;
        }
    }

    public void Play()
    {
        play.gameObject.SetActive(false);
        isPlay = true;

        //cameraSpline.followSpeed = 1;
        mansParentSpline.followSpeed = 1;

        foreach (ManController controller in manControllers)
        {
            controller.PlayAnimation("Run");
        }
    }

    private void UpdatePlayButton()
    {
        if(isPlay)
        {
            return;
        }
        play.gameObject.SetActive(mansParent.transform.childCount > 0);
    }

    public void AddMan(Vector3 pos, bool isRaise)
    {
        GameObject man = Instantiate(manPrefab, mansParent.transform);
        man.transform.position = pos;

        manControllers.Add(man.GetComponent<ManController>());

        if (isRaise)
        {
            man.GetComponent<ManController>().PlayAnimation("Run");
        }

        if (pos.x < leftMoveZone)
        {
            leftMoveZone = pos.x;
        }

        if(pos.x > rightMoveZone)
        {
            rightMoveZone = pos.x;
        }
        UpdatePlayButton();
    }

    public void RemoveMan(ManController manController)
    {
        Vector3 bloodPos = manController.transform.position;
        try
        {
            manControllers.Remove(manController);
        }
        catch
        {

        }

        Destroy(manController.gameObject);

        leftMoveZone = 1;
        rightMoveZone = -1;

        foreach(ManController controller in manControllers)
        {
            if (controller.gameObject.transform.localPosition.x < leftMoveZone)
            {
                leftMoveZone = controller.gameObject.transform.localPosition.x;
            }

            if (controller.gameObject.transform.localPosition.x > rightMoveZone)
            {
                rightMoveZone = controller.gameObject.transform.localPosition.x;
            }
        }

        UpdatePlayButton();

        if(isPlay)
        {
            GameObject blood = Instantiate(bloodPrefab);
            blood.transform.position = bloodPos;
        }
    }

    public void CreateSmoke(Vector3 pos)
    {
        GameObject smoke = Instantiate(smokePrefab);
        smoke.transform.position = pos;
    }
}
