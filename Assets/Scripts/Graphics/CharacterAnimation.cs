using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimation : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Rigidbody referenceBody;

    private enum Direction { Up, Down, Left, Right }
    private Direction direction = Direction.Down;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        direction = GetDirection(referenceBody.velocity);
        Animate(direction);
    }

    private Direction GetDirection(Vector3 velocity)
    {
        if (Mathf.Abs(velocity.x) > 0.1f)
        {
            if (velocity.x < 0)
            {
                return Direction.Left;
            }

            if (velocity.x > 0)
            {
                return Direction.Right;
            }

            return direction;
        }

        if (Mathf.Abs(velocity.z) > 0.1f)
        {
            if (velocity.z < 0)
            {
                return Direction.Down;
            }

            if (velocity.z > 0)
            {
                return Direction.Up;
            }

            return direction;
        }

        return direction;
    }

    private void Animate(Direction direction)
    {
        if (referenceBody.velocity.sqrMagnitude > 0.1f)
        {
            if (direction == Direction.Up)
            {
                animator.Play("WalkBack");
                return;
            }
            if (direction == Direction.Down)
            {
                animator.Play("WalkFront");
                return;
            }
            if (direction == Direction.Left)
            {
                animator.Play("WalkRight");
                FlipDirection(direction);
                return;
            }
            if (direction == Direction.Right)
            {
                animator.Play("WalkRight");
                FlipDirection(direction);
                return;
            }
        }

        if (direction == Direction.Up)
        {
            animator.Play("IdleBack");
            return;
        }
        if (direction == Direction.Down)
        {
            animator.Play("IdleFront");
            return;
        }
        if (direction == Direction.Left)
        {
            animator.Play("IdleRight");
            FlipDirection(direction);
            return;
        }
        if (direction == Direction.Right)
        {
            animator.Play("IdleRight");
            FlipDirection(direction);
            return;
        }
    }

    private void FlipDirection(Direction direction)
    {
        if (direction == Direction.Left)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f);
            return;
        }

        transform.localScale = new Vector3(1f, 1f, 1f);
        return;
    }

    public void SetReference(GameObject reference)
    {
        referenceBody = reference.GetComponent<Rigidbody>();
    }
}
