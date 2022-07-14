using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyHitBox : MonoBehaviour
    {
        //[SerializeField] public CapsuleCollider hitBox;
        [SerializeField] private GameObject hitBox;

        private void Start()
        {
            //hitBox = GetComponent<Collider>();
            //hitBox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {

                PlayerMvmt.playerState = State.isDead;
                PlayerMvmt.deathCause = CauseOfDeath.Enemy;
                StartCoroutine(GameManager.ReloadLevel());
                //Enemy_AI.enemyState = EnemyState.Idle;
            }
        }

        public void Active()
        {
            //hitBox.enabled = true;
            //hitBox.SetActive(true);
        }

        public void Inactive()
        {
            //hitBox.enabled = false;
            //hitBox.SetActive(false);
        }
    }
}


