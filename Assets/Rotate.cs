using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotate : MonoBehaviour
{
    private Vector3 eulers;
    [SerializeField] private float speed = 1f;

    void Update()
    {
        eulers.y += Time.deltaTime * speed;
        transform.rotation = Quaternion.Euler(eulers);
    }
}
