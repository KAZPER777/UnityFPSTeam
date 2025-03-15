using UnityEngine;

public class bossRoomTrigger : MonoBehaviour {
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gamemanager.instance.TryEnterBossRoom();
        }
    }

}
