using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum AIState { Patrol, Pursuit, Return };
    private AIState mode = AIState.Patrol;
    private float internalTimer = 0.0f;
    private Animator animController;
    //private Rigidbody rb;


    [Header("Basic")]
    [SerializeField] private Transform eyes = null;
    [SerializeField] private float walkSpeed = 1.0f;
    [SerializeField] private SkellymanSword sword;

    [Header("Patrol")]
    [SerializeField] private Transform[] path = null;
    [SerializeField] private float waitTimeSeconds = 2;
    [SerializeField] private float detectionDistance = 4.0f;
    [SerializeField, Range(15, 90)] private int detectionAngle = 45;
    private bool walking = false;
    public int nextNode = 0;
    private Vector2 lastPatrolPoint = Vector2.zero;

    [Header("Pursuit")]
    [SerializeField] private float attentionSpan = 5.0f; // how long the AI will chase the player if it's unable to meaningfully pursue
    private Transform pursuitTarget;



    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        //transform.position = new Vector3(path[0].position.x, transform.position.y, transform.position.z);
        //rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case AIState.Patrol: // AI is patrolling and has not found the player
                if (path != null)
                {
                    if (!Walking) // waiting at a patrol point
                    {
                        if (CheckTimer(waitTimeSeconds))
                        {
                            // waited long enough, move to next point
                            Walking = true;
                        }
                    }
                    else // walking between points
                    {
                        MoveTowards(path[nextNode].position);
                        if (Mathf.Abs((path[nextNode].position - transform.position).x) < 0.1)
                        {
                            // reached goal, stop walking
                            Walking = false;
                            internalTimer = 0;
                            nextNode = (nextNode + 1) % path.Length;
                        }
                    }

                    // check if player has been spotted
                    if (DetectPlayer())
                    {
                        lastPatrolPoint = transform.position;
                        ChangeMode(AIState.Pursuit);
                    }
                }
                else
                {
                    Walking = false;
                }
                break;
            case AIState.Pursuit:
                if (!pursuitTarget)
                {
                    ChangeMode(AIState.Return);
                    Walking = true;
                }
                else
                {
                    // attack if close enoughd
                    bool attacking = animController.GetCurrentAnimatorStateInfo(0).IsTag("Attack");
                    if (!attacking)
                    {
                        if ((pursuitTarget.position - transform.position).sqrMagnitude < 1.0f)
                        {
                            animController.SetTrigger("attack");
                            Sword.Toggle(true);
                        }
                        else
                        {
                            Sword.Toggle(false);
                            MoveTowards(pursuitTarget.position);
                        }
                    }
                    // see if it should give up on chasing player
                    GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                    if (((gm) && gm.IsInStealth()) || CheckTimer(attentionSpan))
                    {
                        pursuitTarget = null;
                        ChangeMode(AIState.Return);
                        Walking = true;
                    }
                }
                break;
            case AIState.Return:
                MoveTowards(lastPatrolPoint);
                if (Mathf.Abs((lastPatrolPoint - (Vector2)transform.position).x) < 0.1)
                {
                    // reached goal, return to patrol
                    ChangeMode(AIState.Patrol);
                }

                // check if player has been spotted
                //if (DetectPlayer())
                //{
                //    ChangeMode(AIState.Pursuit);
                //}
                break;
            default:
                break;
        }
    }


    private void Turn(bool right)
    {
        Vector3 currentRotation = transform.rotation.eulerAngles;
        if (right)
        {
            currentRotation.y = -90;
            transform.rotation = Quaternion.Euler(currentRotation);
        }
        else
        {
            currentRotation.y = 90;
            transform.rotation = Quaternion.Euler(currentRotation);
        }

        transform.Rotate(Vector3.up, Mathf.PI);
    }

    private bool Walking { get { return walking; } set { walking = value; animController.SetBool("isWalking", walking); } }

    private void MoveTowards(Vector2 goal)
    {
        Vector2 dist = goal - (Vector2)transform.position;
        Turn((Vector2.Dot(dist, Vector3.right) < 0));

        bool left = (dist.x < 0);

        // Move towards the goal up to its location
        transform.position += Vector3.right * Mathf.Min(Mathf.Abs(dist.x), Time.deltaTime * walkSpeed) * (left ? -1 : 1);

        //rb.velocity = new Vector3((dist.x > 0) ? walkSpeed : -walkSpeed, rb.velocity.y);
    }

    private bool CheckTimer(float threshold)
    {
        internalTimer += Time.deltaTime;
        if (internalTimer > threshold)
        {
            internalTimer = 0;
            return true;
        }
        return false;
    }

    private void ChangeMode(AIState changeTo)
    {
        // change mode and reset internal timer
        mode = changeTo;
        internalTimer = 0.0f;
    }

    private bool DetectPlayer()
    {
        if (!eyes) return false;

        GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();

        if (!gm) return false;

        Transform playerPos = gm.GetPlayer();
        Vector3 distToPlayer = playerPos.position - eyes.position;

        // cull by 4 methods:
        // 1 - stealth
        if (gm.IsInStealth()) return false;
        // 2 - distance
        if (distToPlayer.sqrMagnitude > (detectionDistance * detectionDistance)) return false;
        // 3 - angle
        if (Mathf.Acos(Vector3.Dot(distToPlayer.normalized, eyes.forward)) > (detectionAngle / 2) || Vector3.Dot(distToPlayer.normalized, eyes.forward) < 0) return false;
        // 4 - raycast
        RaycastHit rch;
        Physics.Raycast(eyes.position, distToPlayer.normalized, out rch, detectionDistance);
        if (rch.transform.tag != "Player") return false;

        pursuitTarget = playerPos;
        // player can be seen
        return true;
    }

    public SkellymanSword Sword { get { return sword; } }

    private void OnDrawGizmos()
    {
        if (eyes)
        {
            Vector3 d1 = (Quaternion.AngleAxis(detectionAngle / 2, Vector3.forward) * (eyes.forward)) * detectionDistance;
            Vector3 d2 = (Quaternion.AngleAxis(-detectionAngle / 2, Vector3.forward) * (eyes.forward)) * detectionDistance;
            Debug.DrawRay(eyes.position, d1);
            Debug.DrawRay(eyes.position, d2);
            Debug.DrawRay(eyes.position + d1, (d2 - d1));
        }
    }
}
