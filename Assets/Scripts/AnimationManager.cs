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
        EventManager.Instance.AddListener<ToggleWalkEvent>(OnWalkToggle);
	}

    private void OnDestroy()
    {
        EventManager.Instance.RemoveListener<ToggleWalkEvent>(OnWalkToggle);
    }

    private void OnWalkToggle(ToggleWalkEvent e)
    {
        if (e.walker == gameObject)
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
