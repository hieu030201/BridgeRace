using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : Character
{
    private NavMeshAgent agent;
    [SerializeField] private List<GameObject> planes;
    private int randomValue;
    private bool isMoveBridge = false;
    private bool isSearchBrick = false;

    [SerializeField] public float velo;
    [SerializeField] private List<Transform> pointBridge;
    private Transform closestTransform;
    private bool pointToBridge = false;
    private float distanceToTarget;


    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
        isMoveBridge = false;

    }
    public override void Update()
    {
        velo = agent.velocity.z;
        base.Update();
        State();
    }
    private Vector3 SearchBrick(int currentPlane)
    {
        if (planes[currentPlane].transform.childCount != 0)
        {
            Vector3 brickTarget = transform.position;
            for (int i = 0; i < planes[currentPlane].transform.childCount; i++)
            {
                if (planes[currentPlane].transform.GetChild(i).GetComponent<MeshRenderer>().material.color == skinnedMeshRenderer.material.color)
                {
                    if (brickTarget == transform.position)
                    {
                        brickTarget = planes[currentPlane].transform.GetChild(i).position;
                        continue;
                    }

                    float distance = Vector3.Distance(transform.position, brickTarget);

                    if (distance > Vector3.Distance(transform.position, planes[currentPlane].transform.GetChild(i).position))
                    {
                        brickTarget = planes[currentPlane].transform.GetChild(i).position;
                    }
                }
            }
            return brickTarget;
        }
        return transform.position;
    }
    private Vector3 UpBridge()
    {
        Vector3 characterPoint = gameObject.transform.position;
        if (characterPoint != null && pointBridge.Count > 0)
        {
            float temptPoint = Vector3.Distance(characterPoint, pointBridge[0].position);
     
            for (int i = 0; i < pointBridge.Count; i++)
            {
                float distance = Vector3.Distance(characterPoint, pointBridge[i].position);
         
                if(distance <= temptPoint)
                {
                    temptPoint = distance;
                    closestTransform = pointBridge[i];
                }

                //else
                //{
                //    closestTransform = pointBridge[0];
                //}
            }
        }
        return closestTransform.transform.position;
    }

    private void State()
    {
        randomValue = Random.Range(10, 15);
        if (transform.GetChild(1).childCount < randomValue && !isMoveBridge && !pointToBridge) 
        {
            isSearchBrick = true;
            agent.destination = SearchBrick(0);
        }
        else
        {
            isSearchBrick = false;
        }

        if (!isSearchBrick && transform.GetChild(1).childCount > 0 && !pointToBridge)
        {
            isMoveBridge = true;
            agent.destination = UpBridge();
            float distanceToDestination = Vector3.Distance(agent.transform.position, agent.destination);
            if (distanceToDestination <= arrivalThreshold) 
            {
                pointToBridge = true;
                isMoveBridge = false;
            }
            else 
            {
                pointToBridge = false;
            }
        }
        else
        {
            isMoveBridge = false;
        }

        isSearchBrick = true;
        if (pointToBridge)
        {
            if (isSearchBrick && transform.GetChild(1).childCount < randomValue)
            {
                agent.destination = SearchBrick(1);
                isMoveBridge = false;
            }

            if(transform.GetChild(1).childCount >= randomValue)
            {
                isMoveBridge = true;
                isSearchBrick = false;
            }

            if (isMoveBridge)
            {
         
                if (isMoveBridge && transform.GetChild(1).childCount != 0)
                {
                    agent.destination = target.transform.position;
                }
            }

            //else if(transform.GetChild(1).childCount > 0) {
            //    isSearchBrick = false;
            //}
            //if (!isSearchBrick && transform.GetChild(1).childCount > 0)
            //{
            //    agent.destination = target.transform.position;
            //}
        }
        float ToTarget = Vector3.Distance(transform.position, target.position);
        if (ToTarget <= arrivalThreshold)
        {

            ChangeAnim("win");
        }
        else
        {
            ChangeAnim("run");
        }


        //if (isSearchBrick && transform.GetChild(1).childCount == 0) 
        //{
        //    isMoveBridge = false;
        //}
        //else
        //{
        //    isMoveBridge = true;
        //    UpBridge();

        //}
    }


}
