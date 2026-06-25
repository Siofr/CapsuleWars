using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private int startingHealth = 1;
    private int currentHealth;

    public UnityEvent OnDeathEvent;

    void Start()
    {
        currentHealth = startingHealth;
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
        OnDeathEvent.Invoke();
        ResetPlayerHealth();
    }
}
