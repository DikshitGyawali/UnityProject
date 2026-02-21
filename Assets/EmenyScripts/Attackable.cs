using System.Collections;
using UnityEngine;
using System;
public class Attackable : MonoBehaviour
{
    [SerializeField] protected bool damageable = false;
    [SerializeField] protected int health;
    [SerializeField] protected float invulnarebleTime = .2f;
    public bool givesUpwardForce = false;
    public int upwardForce = 11;
    protected bool canBeHit;
    public int currentHealth;
    protected event Action<int> OnHealthChanged;

    protected virtual void Start()
    {
        currentHealth = health;
        canBeHit = true;
    }
    public virtual void Damage(int healthAmount)
    {
        if (damageable && canBeHit && currentHealth > 0)
        {
            canBeHit = false;
            currentHealth -= healthAmount;
            OnHealthChanged?.Invoke(currentHealth);
            if (currentHealth > 0)
                StartCoroutine(this.BeingHit());
        }
    }
    protected virtual IEnumerator BeingHit()
    {
        yield return new WaitForSeconds(invulnarebleTime);
        canBeHit = true;
    }
}
