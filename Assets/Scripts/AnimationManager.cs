using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private readonly string WalkTrigger = "ToggleWalk";
    private readonly string FightTrigger = "ToggleFight";
    private Action hitWithWeapon;

	private void Start()
	{
        animator = GetComponent<Animator>();
        EventManager.Instance.AddListener<AnimationEvent>(OnAnimationEvent);
        EventManager.Instance.AddListener<TakeDamageEvent>(OnTakeDamageEvent);
    }

    private void OnDestroy()
    {
        EventManager em = EventManager.Instance;
        if (em)
        {
            em.RemoveListener<AnimationEvent>(OnAnimationEvent);
            em.RemoveListener<TakeDamageEvent>(OnTakeDamageEvent);
        }
    }

    private void OnTakeDamageEvent(TakeDamageEvent e)
    {
        if (e.Defender == gameObject)
        {
            TriggerReceiveHitAnimation(e.Direction);
            if (!e.Defender.GetComponent<Character>().IsGood) EventManager.Instance.Raise(new InputToggleEvent(true));
        }
    }

    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is ToggleFightEvent)
        {
            OnFightToggle(e as ToggleFightEvent);
        }
        else if (e is ToggleWalkEvent)
        {
            OnWalkToggle(e as ToggleWalkEvent);
        }
    }

    private void OnWalkToggle(ToggleWalkEvent e)
    {
        if (e.Actor == gameObject)
        {
            animator.SetTrigger(WalkTrigger);
        }
    }

    private void OnFightToggle(ToggleFightEvent e)
    {
        if (e.Actor == gameObject)
        {
            animator.SetTrigger(FightTrigger);
        }
    }

    public void TriggerFightAnimation(Vector2 direction, GameObject def, int dmg)
    {
        if ((transform.localScale.x * direction.x) < 0)
        {
            Vector3 scale = transform.localScale;
            scale.x *= -1;
            transform.localScale = scale;
        }
        hitWithWeapon = (() => EventManager.Instance.Raise(new TakeDamageEvent(def, dmg, direction)));
        animator.SetTrigger(FightTrigger);
        //StartCoroutine(AttackMovementCoroutine(direction));
    }

    public void TriggerReceiveHitAnimation(Vector2 direction)
    {
        StartCoroutine(HitCoroutine(direction));
    }

    /*IEnumerator AttackMovementCoroutine(Vector2 direction)
    {
        Vector3 oldPos = transform.position;
        yield return new WaitForSeconds(0.5f);
        transform.position = oldPos + (Vector3) direction;
        yield return new WaitForSeconds(0.5f);
        transform.position = oldPos;
    }*/

    IEnumerator HitCoroutine(Vector2 direction)
    {
        Material mat = GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        Vector3 oldPos = transform.position;
        mat.color = Color.red;
        transform.position = oldPos + (Vector3) direction;
        yield return new WaitForSeconds(0.1f);
        transform.position = oldPos;
        mat.color = oldColor;
    }

    public void CheckScale(float direction)
    {
        Vector3 scale = transform.localScale;
        scale.x = direction > 0 ? 1 : -1;
        transform.localScale = scale;
    }

    public void SwapDirection()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void HitWithWeapon()
    {
        if (hitWithWeapon != null) hitWithWeapon();
        hitWithWeapon = null;
    }

    public void FinishAttack()
    {
        EventManager.Instance.Raise(new FinishCombatEvent(GetComponent<Character>()));
    }
}
