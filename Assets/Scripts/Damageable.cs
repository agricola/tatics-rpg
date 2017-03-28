using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField]
    private int health = 100;

    public int Health
    {
        get
        {
            return health;
        }
    }

    private void Start()
    {
        EventManager.Instance.AddListener<TakeDamageEvent>(OnTakeDamageEvent);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<TakeDamageEvent>(OnTakeDamageEvent);
        }
    }

    private void OnTakeDamageEvent(TakeDamageEvent e)
    {
        if (e.Defender == gameObject) TakeDamage(e.Damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }
}
