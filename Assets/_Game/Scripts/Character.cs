using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;
using static UnityEngine.GraphicsBuffer;

public class Character : MonoBehaviour
{
    [SerializeField] protected Animator anim;
    ObjectPooler objectPooler;
    BrickSpawn brickSpawn;
    protected Rigidbody rb;
    #region BRICK
    public GameObject spawnPoint;
    [SerializeField] protected SkinnedMeshRenderer skinnedMeshRenderer;
    protected float brickIndexStack = 0;
    protected NavMeshAgent navMeshagent;
    [HideInInspector] public float newYRotation;
    #endregion
    [SerializeField] private GameObject planeMoving;
    public Transform target;
    public float arrivalThreshold = 0.1f;
    protected string currentAnim;

    #region Up Bridge
    #endregion
    protected void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    public virtual void Start()
    {
        objectPooler = ObjectPooler.Instance;
        brickSpawn = BrickSpawn.Instance;
        navMeshagent = this.GetComponent<NavMeshAgent>();
    }
    public virtual void Update()
    {
        newYRotation = transform.eulerAngles.y;
    }
    protected virtual void OnTriggerEnter(Collider other)
    {
        Renderer prefabRenderer = other.gameObject.GetComponent<Renderer>();
        if (other.gameObject.CompareTag("Brick") && skinnedMeshRenderer.material.name == prefabRenderer.material.name)
        {
            Vector3 posRespawn = new Vector3();
            posRespawn = other.gameObject.transform.position;
            StartCoroutine(DelayReSpawn(posRespawn));

            AddBrickToPoint(other.gameObject);
        }
        if (other.gameObject.CompareTag("Stair") && spawnPoint.transform.childCount != 0 && skinnedMeshRenderer.material.color != other.gameObject.gameObject.GetComponent<Renderer>().material.color)
        {
            other.gameObject.gameObject.GetComponent<MeshRenderer>().material.color = skinnedMeshRenderer.material.color;
          
            other.gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().material.color = skinnedMeshRenderer.material.color;
            RemoveBrickToPoint(other.gameObject); 
        }
        if (other.gameObject.CompareTag("Plane2"))
        {
            planeMoving = BrickSpawn.Instance.Planes[1];
        }
     
    }
  
    protected void AddBrickToPoint(GameObject other)
    {
        other.transform.SetParent(spawnPoint.transform);
        Vector3 posNewBrick = new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + brickIndexStack, spawnPoint.transform.position.z);
        other.transform.position = posNewBrick;
        other.transform.rotation = spawnPoint.transform.rotation;
        brickIndexStack += 0.17f;
    }

    protected void RemoveBrickToPoint(GameObject other)
    {
        int lastChildIndex = spawnPoint.transform.childCount - 1;
     
        Queue<GameObject> brickPool = objectPooler.poolDictionary[skinnedMeshRenderer.material.color];

        objectPooler.AddToEnqueue(spawnPoint.transform.GetChild(lastChildIndex).gameObject, brickPool);
        brickIndexStack -= 0.18f;
    }
  
    IEnumerator DelayReSpawn(Vector3 posRespawn)
    {
        yield return new WaitForSeconds(3f);
        brickSpawn.Spawn(posRespawn,planeMoving);
    }

    //protected void ChangeAnim(string animName)
    //{
    //    if(currentAnim != animName)
    //    {
    //        anim.ResetTrigger(animName);

    //        currentAnim = animName;

    //        anim.SetTrigger(currentAnim);
    //    }
    //}
    protected void TargetToWin()
    {
        float ToTarget = Vector3.Distance(gameObject.transform.position, target.position);
        if (ToTarget <= arrivalThreshold)
        {

            ChangeAnim("win");
        }
    }
    protected void ChangeAnim(string animName)
    {
        if (currentAnim != animName)
        {
            anim.ResetTrigger(animName);

            currentAnim = animName;

            anim.SetTrigger(currentAnim);
        }
    } 
}
