using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(Rigidbody))]

public class Sliceable : MonoBehaviour
{
    [SerializeField] private Color _particleColor;

    private void Awake()
    {
        gameObject.GetComponent<Rigidbody>().isKinematic = true;
    }

    public Color GetColor() => _particleColor;
}
