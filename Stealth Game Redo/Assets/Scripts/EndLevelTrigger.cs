using UnityEngine;

namespace DefaultNamespace
{
	public class EndLevelTrigger : MonoBehaviour
	{
		private void OnTriggerEnter(Collider other) 
		{
			if (other.gameObject.CompareTag("Player"))
			{
				GameManager.instance.CompleteLevel();
			}    
		}
	}
}