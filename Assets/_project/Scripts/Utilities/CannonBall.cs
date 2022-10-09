using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CannonBallMovementDirectionType
{
    UP, DOWN, LEFT, RIGHT
}

public abstract class CannonBall : MonoBehaviour, IPooledObject
{
    [Header("** Components **")]
    [Space]

    [SerializeField] private AudioClip _shootSound;
    [SerializeField] private Vector3 _moveDirection;
    [SerializeField] protected CannonBallMovementDirectionType movementDirectionType;

    [Header("** Variable **")]
    [Space]

    [SerializeField] protected float _movementSpeed = 1f;
    [SerializeField] protected float _timeToAutomaticReturn = 3f;
    [SerializeField] protected float _damage = 1f;
    

    public void OnObjectSpawn()
    {
        GameController.Instance.OnplaySfx(_shootSound);
        StartCoroutine(ObjectPooler.Instance.ReturnToPoolAfterSeconds(TagString(), this.gameObject, _timeToAutomaticReturn));
    }

    protected abstract string TagString();

    protected void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.TryGetComponent(out IDestroyableObject target))
        {
            target.GetDamage(_damage);
            ObjectPooler.Instance.ReturnToPool(TagString(), gameObject);
        }
    }

    private void Start()
    {
        SetMovementDirection();
    }

    private void Update()
    {
        transform.Translate(_moveDirection * _movementSpeed * Time.deltaTime);
    }

    private void SetMovementDirection()
    {
        switch (movementDirectionType)
        {
            case CannonBallMovementDirectionType.UP:
                _moveDirection = Vector3.up;
                break;
            case CannonBallMovementDirectionType.DOWN:
                _moveDirection = Vector3.down;
                break;
            case CannonBallMovementDirectionType.LEFT:
                _moveDirection = Vector3.left;
                break;
            case CannonBallMovementDirectionType.RIGHT:
                _moveDirection = Vector3.right;
                break;
        }
    }
}
