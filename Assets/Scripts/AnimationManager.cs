using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private readonly string WalkTrigger = "ToggleWalk";
    private readonly string FightTrigger = "ToggleFight";

	private void Start()
	{
        animator = GetComponent<Animator>();
        EventManager.Instance.AddListener<AnimationEvent>(OnAnimationEvent);
	}

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<AnimationEvent>(OnAnimationEvent);
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
}
