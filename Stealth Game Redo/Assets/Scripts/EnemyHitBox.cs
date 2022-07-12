using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class EnemyHitBox : MonoBehaviour
    {
        [SerializeField] private Collider hitBox;

        private void Start()
        {
            hitBox = GetComponent<Collider>();
            //hitBox.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                PlayerMvmt.playerState = State.isDead;
                PlayerMvmt.deathCause = CauseOfDeath.Enemy;
                StartCoroutine(GameManager.ReloadLevel());
            }
        }

        public void Active()
        {
            hitBox.enabled = true;
        }

        public void Inactive()
        {
            hitBox.enabled = false;
        }
    }
}


