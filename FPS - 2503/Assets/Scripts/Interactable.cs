using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private bool destroyOnInteract;

    public virtual void Interact(GameObject player)
    {
        if (destroyOnInteract)
            Destroy(gameObject);
    }
}
