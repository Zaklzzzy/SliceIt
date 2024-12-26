using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sliceable : MonoBehaviour
{
    [SerializeField] private Color _particleColor;

    public Color GetColor() => _particleColor;
}
