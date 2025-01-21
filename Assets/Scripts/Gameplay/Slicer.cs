using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slicer : MonoBehaviour
{
    [SerializeField] private ParticleSystem _sliceParticleSystem;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sliceable"))
        {
            var main = _sliceParticleSystem.main;
            main.startColor = other.GetComponent<Sliceable>().GetColor();
            other.GetComponent<Rigidbody>().isKinematic = false;
            _sliceParticleSystem.Play();
            LevelController.Instance.AddScore(1f);
        }
        else if(other.CompareTag("Block"))
        {
            gameObject.GetComponentInParent<KnifeController>().Stop();
        }
    }
}
