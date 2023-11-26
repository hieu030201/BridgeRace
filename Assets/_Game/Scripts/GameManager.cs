using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private EnableDisableNavMeshObstacle navMeshObs;
    void Start()
    {
        navMeshObs = GetComponent<EnableDisableNavMeshObstacle>();
        ObjectPooler.Instance.OnInit();
        BrickSpawn.Instance.OnInit();
    
    }

   
}
