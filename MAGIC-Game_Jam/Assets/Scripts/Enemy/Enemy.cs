using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum AIState { Patrol, Pursuit, Return };
    private AIState mode;
    private float internalTimer = 0.0f;
    private Animator animController;
    private Rigidbody rb;


    [Header("Basic")]
    [SerializeField] private Transform eyes = null;
    [SerializeField] private float walkSpeed = 1.0f;

    [Header("Patrol")]
    [SerializeField] private Transform[] path = null;
    [SerializeField] private float waitTimeSeconds = 2;
    [SerializeField] private float detectionDistance = 4.0f;
    [SerializeField, Range(15, 90)] private int detectionAngle = 45;
    private bool walking = false;
    private int nextNode = 0;
    private Vector2 lastPatrolPoint = Vector2.zero;

    [Header("Pursuit")]
    [SerializeField] private float attentionSpan = 5.0f; // how long the AI will chase the player if it's unable to meaningfully pursue
    private Transform pursuitTarget;



    // Start is called before the first frame update
    void Start()
    {
        animController = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (mode)
        {
            case AIState.Patrol: // AI is patrolling and has not found the player
                if (path != null)
                {
                    if (!walking) // waiting at a patrol point
                    {
                        if (CheckTimer(waitTimeSeconds))
                        {
                            // waited long enough, move to next point
                            walking = true;
                        }
                    }
                    else // walking between points
                    {
                        MoveTowards(path[nextNode].position);
                        if (Vector2.Distance(path[nextNode].position, transform.position) < 0.1)
                        {
                            // reached goal, stop walking
                            walking = false;
                            internalTimer = 0;
                            ++nextNode;
                        }
                    }

                    // check if player has been spotted
                    if (DetectPlayer())
                    {
                        lastPatrolPoint = transform.position;
                        ChangeMode(AIState.Pursuit);
                    }
                }
                break;
            case AIState.Pursuit:
                if (!pursuitTarget)
                {
                    ChangeMode(AIState.Return);
                    walking = true;
                }
                else
                {
                    if (internalTimer > attentionSpan)
                    {
                        pursuitTarget = null;
                        ChangeMode(AIState.Return);
                    }
                }
                break;
            case AIState.Return:
                MoveTowards(lastPatrolPoint);
                if (Vector2.Distance(lastPatrolPoint, transform.position) < 0.1)
                {
                    // reached goal, stop walking
                    walking = false;
                    internalTimer = 0;
                    ++nextNode;
                }
                break;
            default:
                break;
        }
    }

    private void MoveTowards(Vector2 goal)
    {
        Vector2 dist = path[nextNode].position - transform.position;
        bool left = (dist.x < 0);

        // Move towards the goal up to its location
        transform.position += new Vector3(Mathf.Min(dist.x, Time.deltaTime * walkSpeed * (left ? -1 : 1)), transform.position.y);

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
        if (!gm.IsInStealth()) return false;
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

    private void OnDrawGizmos()
    {
        if (eyes)
        {
            Debug.DrawRay(eyes.position, eyes.position + (Quaternion.AngleAxis(Mathf.Deg2Rad * detectionAngle / 2, Vector3.forward) * (eyes.forward)));
            Debug.DrawRay(eyes.position, eyes.position + (Quaternion.AngleAxis(Mathf.Deg2Rad * -detectionAngle / 2, Vector3.forward) * (eyes.forward)));
        }
    }
}
