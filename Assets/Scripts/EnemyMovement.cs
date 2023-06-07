using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement : MonoBehaviour
{
    
    public GameObject mainCharacter,sniperGun;
    public int enemyHealth;
     Animator enemyanim;
     BoxCollider bc;
    Rigidbody rb;
    NavMeshAgent nmesh;
    // Start is called before the first frame update
    void Start()
    {
        
        enemyHealth = 100;
        rb = GetComponent<Rigidbody>();
        enemyanim=GetComponent<Animator>();
        nmesh=GetComponent<NavMeshAgent>();
        bc = GetComponent<BoxCollider>();

    }

    // Update is called once per frame
    void Update()
    {
        if (nmesh.enabled==true)
        {
            if (nmesh.SetDestination(mainCharacter.transform.position - new Vector3(0.5f, 0, 0)))
            {
                enemyanim.SetBool("Run", true);
            }
            if (Vector3.Distance(this.gameObject.transform.position, mainCharacter.transform.position) < 5f)
            {
                enemyanim.SetBool("Attack", true);
                SoundManager.Instance.enemyAttack.Play();
            }

            else
            {
                enemyanim.SetBool("Attack", false);
            }
        }
        if (enemyHealth<1)
        {
            EnemyDie();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Bullet" )
        {
            if (sniperGun.activeInHierarchy==true)
            {
                enemyHealth = enemyHealth - 100;
            }
            else
            {
                enemyHealth = enemyHealth - 25;
            }
            Destroy(collision.gameObject);
        }
    }
    private void EnemyDie()
    {
        enemyanim.SetBool("Attack", false);
        enemyanim.SetBool("Die", true);
        nmesh.enabled = false;
        Destroy(this.gameObject, 10);
    }


}
