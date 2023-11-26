using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class PlayerController : Character
{
    private NavMeshAgent agent;
    #region MOVEMENT
    [SerializeField] private FloatingJoystick joystick;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float rotateSpeed;
    private Vector3 _moveVector;
    [SerializeField] public float velo;
    private bool isMoving = true;
    #endregion
    public GameObject victoryPanel;
    public float rotationSpeed = 180.0f;
    // Update is called once per frame
    public override void Start()
    {
        base.Start();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void Update()
    {
        base.Update();
        velo = agent.velocity.z;
        float ToTarget = Vector3.Distance(gameObject.transform.position, target.position);
        if (ToTarget <= arrivalThreshold)
        {
            isMoving = false;
        }
        Move();
    }
    private void Move()
    {
        if (isMoving)
        {
            _moveVector = Vector3.zero;
            _moveVector.x = joystick.Horizontal * moveSpeed * Time.deltaTime;
            _moveVector.z = joystick.Vertical * moveSpeed * Time.deltaTime;

            if (joystick.Horizontal != 0 || joystick.Vertical != 0)
            {
                Vector3 direction = Vector3.RotateTowards(transform.forward, _moveVector, rotateSpeed * Time.deltaTime, 0.0f);
                transform.rotation = Quaternion.LookRotation(direction);
                ChangeAnim("run");
            }
            else if (joystick.Horizontal == 0 && joystick.Vertical == 0)
            {
                ChangeAnim("idle");
            }
            transform.position += _moveVector;
        }
        
       
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Finish"))
        {
           
            victoryPanel.SetActive(true);
            ChangeAnim("win");
 
            float rotationAngle = rotationSpeed * Time.deltaTime;

            Vector3 directionToOther = (other.transform.position - transform.position).normalized;

            Quaternion targetRotation = Quaternion.LookRotation(directionToOther);

            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationAngle);
            StartCoroutine(DelayFinish());
        }
    }
    IEnumerator DelayFinish()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
    }



}
