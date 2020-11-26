using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAttack : MonoBehaviour //vp_DamageHandler
{
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;

    public float fireRate = 15f;
    //private float nextTimeToFire;
    public float damage = 20f;
    private Camera mainCam;

    public GameObject Player;
    public float AttackDistance = 10.0f;
    public float FollowDistance = 20.0f;

    [Range(0.0f, 1.0f)]
    public float AttackProbability = 0.5f;
    [Range(0.0f, 1.0f)]
    public float HitAccuracy = 0.5f;
    public float DamagePoints = 2.0f;

    public AudioSource GunSound = null;

    //protected override 
    void Awake()
    {
        //base.Awake();

        _navMeshAgent = GetComponent<NavMeshAgent>();

        _animator = GetComponent<Animator>();

        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (_navMeshAgent.enabled)
        {
            float dist = Vector3.Distance(Player.transform.position, this.transform.position);
            bool shoot = false;
            bool follow = (dist < FollowDistance);

            if (follow)
            {
                float random = Random.Range(0.0f, 1.0f);
                if (random > (1.0f - AttackProbability) && dist < AttackDistance)
                {
                    shoot = true;
                }
            }

            if (follow)
            {
                _navMeshAgent.SetDestination(Player.transform.position);
            }

            if (!follow || shoot)
                _navMeshAgent.SetDestination(transform.position);


            _animator.SetBool("Shoot", shoot);
            _animator.SetBool("Run", follow);

        }
    }

    public void ShootEvent()
    {
        /*if (m_Audio != null)
        {
            m_Audio.PlayOneShot(GunSound);
        }*/
        
        float random = Random.Range(0.0f, 1.0f);

        // The higher the accuracy is, the more likely the player will be hit
        bool isHit = random > 1.0f - HitAccuracy;
        RaycastHit hit;

        if (isHit && Physics.Raycast(mainCam.transform.position, mainCam.transform.forward, out hit))
        {
            print("Enemy Soldier HIT : " + hit.transform.gameObject.name);
           // print("Damage = " + damage);
           /* if (hit.transform.tag == Tags.ENEMY_TAG)
            {
                hit.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }
           */
            if (hit.transform.gameObject.name == "Soldier" || hit.transform.tag == Tags.ENEMY_TAG || hit.transform.gameObject.name == "Body" || hit.transform.gameObject.name == "Player")
            {
                print("Do Something");
                Player.transform.GetComponent<HealthScript>().ApplyDamage(damage);
            }

        }
        /*if (isHit)
        {
            //testing by message printing
            print("Enemy Soldier HIT Something: ");// + hit.transform.gameObject.name);
            Player.SendMessage("Damage", DamagePoints, 
                SendMessageOptions.DontRequireReceiver);
        }
        */
    }

    //xpublic override
    /*
    void Die()
    {
        if (!enabled || !vp_Utility.IsActive(gameObject))
            return;

        if (m_Audio != null)
        {
            m_Audio.pitch = Time.timeScale;
            m_Audio.PlayOneShot(DeathSound);
        }

        _navMeshAgent.enabled = false;

        _animator.SetBool("IsFollow", false);
        _animator.SetBool("Attack", false);

        _animator.SetTrigger("Die");

        Destroy(GetComponent<vp_SurfaceIdentifier>());

    }
    */
}
