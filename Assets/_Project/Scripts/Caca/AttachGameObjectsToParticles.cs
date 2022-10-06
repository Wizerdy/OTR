﻿using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.Rendering.Universal;

[RequireComponent(typeof(ParticleSystem))]
public class AttachGameObjectsToParticles : MonoBehaviour
{
    public GameObject m_Prefab;
    [SerializeField] bool dynamicIntensity = false;
    [SerializeField] bool dynamicColor = false;

    private ParticleSystem m_ParticleSystem;
    private List<GameObject> m_Instances = new List<GameObject>();
    private ParticleSystem.Particle[] m_Particles;

    private static FieldInfo m_FalloffField = typeof(Light2D).GetField("m_FalloffIntensity", BindingFlags.NonPublic | BindingFlags.Instance);

    public void SetFalloff(Light2D light, float falloff) {
        m_FalloffField.SetValue(light, falloff);
    }

    // Start is called before the first frame update
    void Start()
    {
        m_ParticleSystem = GetComponent<ParticleSystem>();
        m_Particles = new ParticleSystem.Particle[m_ParticleSystem.main.maxParticles];
    }

    // Update is called once per frame
    void LateUpdate()
    {
        int count = m_ParticleSystem.GetParticles(m_Particles);

        while (m_Instances.Count < count)
            m_Instances.Add(Instantiate(m_Prefab, m_ParticleSystem.transform));

        bool worldSpace = (m_ParticleSystem.main.simulationSpace == ParticleSystemSimulationSpace.World);
        for (int i = 0; i < m_Instances.Count; i++)
        {
            if (i < count)
            {
                if (worldSpace)
                    m_Instances[i].transform.position = m_Particles[i].position;
                else
                    m_Instances[i].transform.localPosition = m_Particles[i].position;
                m_Instances[i].SetActive(true);
                
                if (dynamicIntensity)
                    m_Instances[i].GetComponent<Light2D>().intensity = Mathf.Clamp(m_Instances[i].GetComponent<Light2D>().intensity, m_Particles[i].startSize, m_Particles[i].GetCurrentSize(m_ParticleSystem)) * 3;
                if (dynamicColor)
                    m_Instances[i].GetComponent<Light2D>().color = m_Particles[i].GetCurrentColor(m_ParticleSystem);
                SetFalloff(m_Instances[i].GetComponent<Light2D>(), Mathf.Clamp(m_Instances[i].GetComponent<Light2D>().falloffIntensity, m_Particles[i].startSize, m_Particles[i].GetCurrentSize(m_ParticleSystem)) * 3);
            }
            else
            {
                m_Instances[i].SetActive(false);
            }
        }
    }
}
