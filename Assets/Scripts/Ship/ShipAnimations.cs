using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAnimations : MonoBehaviour
{
	private Animator animator;


	private void Start()
	{
		animator = GetComponent<Animator>();
	}

	public void PlayMoveAnimation()
	{
		animator.SetTrigger("Move");
	}

	public void PlayIdleAnimation()
	{
		animator.SetTrigger("Idle");
	}

	public void PlayJumpAnimation()
	{
		animator.SetTrigger("Jump");
	}

	public void PlayDeadAnimation()
	{
		animator.SetTrigger("Dead");
	}
}
