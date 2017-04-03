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

    private void OnDisable()
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
        GameObject def = e.Defender;
        if (def == gameObject)
        {
            Character character = def.GetComponent<Character>();
            TriggerReceiveHitAnimation(
                e.Direction,
                character,
                () => RaiseChangeHealthEvent(e.Damage, character)
                );
        }
    }

    private void RaiseChangeHealthEvent(int damage, Character character )
    {
        EventManager.Instance.Raise(new ChangeHealthEvent(damage, character));
    }

    private void OnAnimationEvent(AnimationEvent e)
    {
        if (e is AnimationWalkEvent)
        {
            OnWalkToggle(e as AnimationWalkEvent);
        }
        else if (e is AnimationDeathEvent)
        {
            OnDeathToggle(e as AnimationDeathEvent);
        }
    }

    private void OnDeathToggle(AnimationDeathEvent e)
    {
        if (e.Actor != gameObject) return;
        switch (e.Status)
        {
            case AnimationStatus.Start:
                EventManager.Instance.Raise<AnimationEvent>(
                    new AnimationDeathEvent(AnimationStatus.Finish, gameObject));
                // pointless ATM, add animation trigger here in future!
                break;
            case AnimationStatus.Finish:
                EventManager.Instance.Raise(new CharacterDeathEvent(e.Actor));
                Destroy(gameObject);
                
                break;
            default:
                break;
        }
    }

    private void OnWalkToggle(AnimationWalkEvent e)
    {
        if (e.Actor == gameObject)
        {
            animator.SetTrigger(WalkTrigger);
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

    public void TriggerReceiveHitAnimation(
        Vector2 direction,
        Character character,
        Action changeHealth)
    {
        StartCoroutine(HitCoroutine(direction, character, changeHealth));
    }

    /*IEnumerator AttackMovementCoroutine(Vector2 direction)
    {
        Vector3 oldPos = transform.position;
        yield return new WaitForSeconds(0.5f);
        transform.position = oldPos + (Vector3) direction;
        yield return new WaitForSeconds(0.5f);
        transform.position = oldPos;
    }*/

    IEnumerator HitCoroutine(Vector2 direction, Character defender, Action changeHealth)
    {
        Material mat = GetComponent<Renderer>().material;
        Color oldColor = mat.color;
        Vector3 oldPos = transform.position;
        mat.color = Color.red;
        transform.position = oldPos + (Vector3) direction;
        yield return new WaitForSeconds(0.1f);
        transform.position = oldPos;
        mat.color = oldColor;
        changeHealth();
        EnableInputIfCharacterEvil(defender);
    }

    private void EnableInputIfCharacterEvil(Character character)
    {
        if (!character.IsGood) EventManager.Instance.Raise(new InputToggleEvent(true));
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
