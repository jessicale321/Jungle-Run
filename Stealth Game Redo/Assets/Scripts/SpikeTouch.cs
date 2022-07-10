using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DefaultNamespace
{
    public class SpikeTouch : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.CompareTag("Player")) {
                Debug.Log("Player touch spike");
                StartCoroutine(GameManager.ReloadLevel());
            }
        }
    }
}
