using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuraParticles : MonoBehaviour
{
    public Color minColor;
    public Color maxColor;
    public float minEmission = 10f;
    public float maxEmission = 70f;
    public float minStartLife = 1f;
    public float maxStartLife = 1.5f;
    public float minStartSpeed = 0.01f;
    public float maxStartSpeed = 0.5f;

    private ParticleSystem psComponent;
    ParticleSystem.MainModule ps;
    ParticleSystem.Particle[] particles = new ParticleSystem.Particle[AudioPeer.NUM_FREQ_BANDS];

    // Start is called before the first frame update
    void Start()
    {
        psComponent = GetComponent<ParticleSystem>();
        ps = psComponent.main;
    }

    // Update is called once per frame
    void Update()
    {
        ps.startColor = Color.Lerp(minColor, maxColor, AudioPeer.averageBandBuffer);

        float emissionMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minEmission, maxEmission);
        int emissionRounded = (int)Mathf.Round(emissionMapped);
        psComponent.Emit(emissionRounded);

        float startLifeMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minStartLife, maxStartLife);
        //ps.startLifetime = startLifeMapped;

        float startSpeedMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minStartSpeed, maxStartSpeed);
        ps.startSpeed = startSpeedMapped;
    }
}
