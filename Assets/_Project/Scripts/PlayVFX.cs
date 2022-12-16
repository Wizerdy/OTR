using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFX : MonoBehaviour
{
    [SerializeField] private ParticleSystem bloodDashParticle;

    public void PlayBloodDashParticle() {
        bloodDashParticle.Play();
    }
}




