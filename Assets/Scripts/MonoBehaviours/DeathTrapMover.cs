using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using System;

public class DeathTrapMover : MonoBehaviour
{
    [Header("Trap Movement Settings")]
    [SerializeField] private float jumpPowers;       
    [SerializeField] private Transform[] pathTransforms;
    [SerializeField] private AnimationCurve animationpatrolCurve;
    [SerializeField] private AnimationCurve animationAttackCurve;
    [SerializeField] private float attackModeDuration = 4;

    private Vector3[] path;
    private Sequence movementSequence;    

    public float AttackModeDuration { get { return attackModeDuration; } }

    private void Start()
    {
        Buildpath();       
    }

    public void AttackMode(Transform player)
    {
        movementSequence.Pause();
        Sequence attackSequence = DOTween.Sequence();
        attackSequence.Pause();
        Vector3[] attackPath = new Vector3[2];
        attackPath[0] = transform.position;
        attackPath[1] = player.transform.position + player.transform.forward/2 + Vector3.up *1.1f;
        attackSequence.Append(transform.DOPath(attackPath,attackModeDuration, PathType.Linear, PathMode.Full3D, 10).SetEase(animationAttackCurve));
        attackSequence.Play();
    }

    private void Buildpath()
    {
        path = CalculatePath(pathTransforms);
        movementSequence = DOTween.Sequence();
        movementSequence.Pause();
        movementSequence.Append(transform.DOPath(path, 10, PathType.Linear, PathMode.Full3D, 10).SetEase(animationpatrolCurve));
        movementSequence.Join(transform.DORotate(new Vector3(0, 360, 0), movementSequence.Duration(false), RotateMode.FastBeyond360).SetRelative());
        Array.Reverse(path);
        movementSequence.Append(transform.DOPath(path, 10, PathType.Linear, PathMode.Full3D, 10));
        movementSequence.Join(transform.DORotate(new Vector3(0, -360, 0), movementSequence.Duration(false) / 2, RotateMode.FastBeyond360).SetRelative());
        movementSequence.Play();
        movementSequence.SetLoops(-1, LoopType.Restart);        
    }

    private Vector3[] CalculatePath(Transform[] pathTransform)
    { 
        path = new Vector3[pathTransform.Length];
        for (int i = 0; i < pathTransform.Length; i++)
        {
            path[i] = pathTransform[i].position;
        }
        return path;       
    }   
}
