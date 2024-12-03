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

    private void OnParticleTrigger()
    {
        //Debug.Log("Particle has collided");
        int triggerParticles = exp.GetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

        Debug.Log("Collided with Player");
        for (int i = 0; i < triggerParticles; i++)
        {
            ParticleSystem.Particle p = particles[i];
            p.remainingLifetime = 0;
            particles[i] = p;
            Player _player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();

            if (_player != null)
            {
                _player._exp += expValue;
                Debug.Log("Player now has " + _player._exp + " experience pointa");
            }
            exp.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);
        }

    }

    // Update is called once per frame
    void Update()
    {

    }
}