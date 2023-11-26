using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnableDisableNavMeshObstacle : MonoBehaviour
{
    [SerializeField] private GameObject character;
    private NavMeshObstacle navMeshObstacle;
    GameObject targetObject;
    NavMeshAgent _velocity;
    private float velocityByCharacter;
    private void Start()
    {

        // Get the NavMeshObstacle component attached to this GameObject
        navMeshObstacle = GetComponent<NavMeshObstacle>();
        //Renderer charactorRender = character.GetComponentInChildren<Renderer>();
        targetObject = GameObject.FindWithTag("Character");
        _velocity = targetObject.GetComponent<NavMeshAgent>();

    }
    private void Update()
    {
        velocityByCharacter = _velocity.velocity.z;
    }

    private void OnTriggerEnter(Collider other)
    {
        Renderer stairRender = gameObject.GetComponent<Renderer>();
        Renderer charactorRender = other.gameObject.GetComponentInChildren<Renderer>();
        if (other.CompareTag("Character") && stairRender.material.color == charactorRender.material.color || velocityByCharacter < 0)
        {
            // Disable the NavMeshObstacle when the player enters the trigger area
            navMeshObstacle.enabled = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the collider belongs to the player or a specific object/tag
        if (other.CompareTag("Character") )
        {
            // Enable the NavMeshObstacle when the player exits the trigger area
            navMeshObstacle.enabled = true;
        }
    }
}
