using UnityEngine;

public class AudioParticles : MonoBehaviour
{
    public Color minColor = new Color(0.4f, 0.16f, 0.86f, 1.0f);
    public Color maxColor = new Color(0.86f, 0.16f, 0.5f, 1.0f);

    public bool enableEmission = true;
    public float minEmission = 0f;
    public float maxEmission = 8f;

    public bool enableStartLife = true;
    public float minStartLife = 0.1f;
    public float maxStartLife = 8f;

    public bool enableStartSpeed = true;
    public float minStartSpeed = 0.05f;
    public float maxStartSpeed = 5f;

    public bool enableStartSize = false;
    public float minStartSize = 2f;
    public float maxStartSize = 4f;

    public bool enableSimSpeed = false;
    public float minSimSpeed = 1f;
    public float maxSimSpeed = 2f;


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

        if(enableEmission)
        {
            float emissionMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minEmission, maxEmission);
            int emissionRounded = (int)Mathf.Round(emissionMapped);
            psComponent.Emit(emissionRounded);
        }
        if(enableStartLife)
        {
            float startLifeMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minStartLife, maxStartLife);
            ps.startLifetime = startLifeMapped;
        }
        if(enableStartSpeed)
        {
            float startSpeedMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minStartSpeed, maxStartSpeed);
            ps.startSpeed = startSpeedMapped;
        }
        if (enableStartSize)
        {
            float startSizeMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minStartSize, maxStartSize);
            ps.startSize = startSizeMapped;
        }
        if (enableSimSpeed)
        {
            float simSpeedMapped = VizUtils.remap(AudioPeer.averageBandBuffer, 0f, 1f, minSimSpeed, maxSimSpeed);
            ps.simulationSpeed = simSpeedMapped;
        }

    }
}
