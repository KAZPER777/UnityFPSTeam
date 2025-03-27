using UnityEngine;

public class HealthPickup : Interactable
{
    [SerializeField] private float health = 50f;

        public override void Interact(GameObject player)
    {
        player.GetComponent<playerController>().AddHealth(health);
    }
}
