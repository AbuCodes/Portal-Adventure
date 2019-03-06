using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class FollowerAI : MonoBehaviour
{
    // This script is primarily designed for enemies within their environments.
    // Attributes are opened for modification to make each NPC/Enemy completely unique.
    // NavMesh is required.


    #region Public Variables
    [Header("Enemy Attributes:")]
    public int totalHP = 1;
    public float speed = 5F;
    public float attackRange = 2F;
    public float agroDistance = 10F;
    public float roamRadius = 20F;
    public float resurrection = 5F;

    //Enemies stored
    //public List<Transform> myEnemies = new List<Transform>();

    //[HideInInspector]
    public int HP;

    [Header("References:")]
    public Transform player;
    public Transform originalPlayer;

    [HideInInspector]
    public bool inPlayerBounds = false;
    #endregion

    #region Private Variables
    Vector3 _spawnPosition;
    UnityEngine.AI.NavMeshAgent _navComponent;
    bool _roaming = false;
    CharacterController _controller;
    #endregion


    // Use this for initialization
    void Start()
    {
        player = GameObject.Find("Captain").GetComponent<Transform>();
        originalPlayer = player;
        _controller = GetComponent<CharacterController>();
        _navComponent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        _spawnPosition = transform.position;
        HP = totalHP;
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsDead())
        {
            if (InAggroRange())
            {
                Chase();

                if (!InAttackRange())
                    Chase();
                else
                    _navComponent.destination = transform.position;
            }
            else
            {
                if (!_roaming)
                    FreeRoam();
                else
                    CheckRoam();
            }
        }
    }

    //Returns a bool upon Proximity to the player and/or object prefab to follow
    bool InAggroRange()
    {
        return (Vector3.Distance(transform.position, player.position) < agroDistance);
    }

    //Returns a bool upon Proximity to the distance necessary to be upclose without being "on top"
    bool InAttackRange()
    {
        return (Vector3.Distance(transform.position, player.position) < attackRange);
    }

    //Movement to player logic
    void Chase()
    {
        Vector3 point = player.position;
        point.y = 0;
        _navComponent.SetDestination(point);
    }

    //Movement logic
    void CheckRoam()
    {
        if (!_navComponent.pathPending)
        {
            float dist = _navComponent.remainingDistance;
            if (dist != Mathf.Infinity && _navComponent.pathStatus == UnityEngine.AI.NavMeshPathStatus.PathComplete && _navComponent.remainingDistance <= _navComponent.stoppingDistance)
            {
                if (!_navComponent.hasPath || _navComponent.velocity.sqrMagnitude == 0f)
                    _roaming = false;
            }
        }
    }

    //Movement logic
    void FreeRoam()
    {
        _roaming = true;
        Vector3 randomDirection = Random.insideUnitSphere * roamRadius;
        randomDirection += _spawnPosition;
        UnityEngine.AI.NavMeshHit hit;
        UnityEngine.AI.NavMesh.SamplePosition(randomDirection, out hit, roamRadius, 1);
        Vector3 finalPosition = hit.position;
        _navComponent.destination = finalPosition;
    }

    public bool IsDead()
    {
        return HP <= 0;
    }

    public void GetHit(double damage)
    {
        HP = HP - (int)damage;
        if (HP <= 0)
        {
            HP = 0;
            Die();
        }
    }

    void Die()
    {
        /*Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = false;

        _controller.enabled = false;
        this.enabled = false;
        Invoke("Ressurect", resurrection);
        */
        transform.position = _spawnPosition;
        HP = 1;
        this.gameObject.SetActive(false);
    }

    /*void Ressurect()
    {
        HP = totalHP;
        Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach (Renderer r in rs)
            r.enabled = true;

        _controller.enabled = true;
        transform.position = _spawnPosition;
        _navComponent.destination = _spawnPosition;
        this.enabled = true;
    }*/
}