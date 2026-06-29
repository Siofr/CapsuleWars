using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHealth = 1;
    private int currentHealth;

    public PlayerController playerController;

    void Start()
    {
        currentHealth = startingHealth;
        playerController = GetComponentInParent<PlayerController>();
    }

    public void Damage(int damage)
    {
        Debug.Log("Damage Dealt");

        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            DeathBehaviour();
        }
    }

    private void ResetPlayerHealth()
    {
        currentHealth = startingHealth;
    }

    public void DeathBehaviour()
    {
        Debug.Log("Death Event Invoked");
        playerController.Respawn();
        ResetPlayerHealth();
    }
}
