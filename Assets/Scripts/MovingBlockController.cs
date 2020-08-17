using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class MovingBlockController : MonoBehaviour
{

    [SerializeField] private Direction direction;
    [SerializeField] private float waitTime = 500;
    [SerializeField] private float distance = 3;
    [SerializeField] private float duration;

    private Vector3 originalPosition;
    private Vector3 targetPosition;

    private enum Direction
    {
        North,
        South,
        East,
        West,
        Up,
        Down
    }
    // Start is called before the first frame update
    void Start()
    {
        originalPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        switch (direction)
        {
            case Direction.North:
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z + distance);
                break;
            case Direction.South:
                targetPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z - distance);
                break;
            case Direction.East:
                targetPosition = new Vector3(transform.position.x + distance, transform.position.y, transform.position.z);
                break;
            case Direction.West:
                targetPosition = new Vector3(transform.position.x - distance, transform.position.y, transform.position.z);
                break;
            case Direction.Up:
                targetPosition = new Vector3(transform.position.x, transform.position.y + distance, transform.position.z);
                break;
            case Direction.Down:
                targetPosition = new Vector3(transform.position.x, transform.position.y - distance, transform.position.z);
                break;
            default:
                break;
        }

        MoveToTarget();
    }

    private void MoveToTarget()
    {
        transform.DOMove(targetPosition, duration).SetEase(Ease.Linear).OnComplete(MoveToStart);
    }

    private void MoveToStart()
    {
        transform.DOMove(originalPosition, duration).SetEase(Ease.Linear).OnComplete(MoveToTarget);
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    print(other.tag);
    //    print(other.gameObject.tag);
    //    if(other.gameObject.tag == "Player")
    //    {
    //        other.gameObject.transform.parent = transform;
    //        //ther.gameObject.GetComponent<CharacterController>().transform.parent = transform;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if(other.gameObject.tag == "Player")
    //    {
    //        other.gameObject.transform.parent = null;
    //        //other.gameObject.GetComponent<CharacterController>().transform.parent = null;
    //    }
    //}
}
