using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ExpHandler : MonoBehaviour
{
    // For the Player
    public int expValue = 1;
    [SerializeField] ParticleSystem exp;
    [SerializeField] Transform player;

    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        if (player == null)
        {
            Debug.LogError("Error: playerCollider DNE");
        }
        exp = FindFirstObjectByType<ParticleSystem>();
        exp.trigger.SetCollider(0, player);

    }

    public void EmitXP(int amountOfXP)
    {
        exp.Emit(amountOfXP);
    }

    private void OnParticleTrigger()
    {
        int triggerParticles = exp.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        AudioManager.instance.PlayOneShot(FMODEvents.instance.sfx_xp, transform.position);
        for (int i = 0; i < triggerParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            particles[i] = p;
            Player _player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();

            if (_player != null)
            {
                _player.exp += expValue;
            }
            exp.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}