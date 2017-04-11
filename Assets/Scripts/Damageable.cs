using UnityEngine;

public class Damageable : MonoBehaviour {

    [SerializeField]
    private int health = 10;

    public int Health
    {
        get
        {
            return health;
        }
    }

    private void OnEnable()
    {
        EventManager.Instance.AddListener<ChangeHealthEvent>(OnChangeHealthEvent);
    }

    private void OnDisable()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<ChangeHealthEvent>(OnChangeHealthEvent);
        }
    }

    private void OnChangeHealthEvent(ChangeHealthEvent e)
    {
        if (e.Defender.gameObject == gameObject) TakeDamage(e.Damage);
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
        {
            EventManager.Instance.Raise<AnimationEvent>(
                new AnimationDeathEvent(AnimationStatus.Start, gameObject));
        }
    }
}
