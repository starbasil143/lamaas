using System.Collections.Generic;
using UnityEngine;

public class ExpHandler: MonoBehaviour
{
    // For the Player
    public int goldAmount = 0;
    [SerializeField] ParticleSystem exp;
    [SerializeField] Transform player;

    List<ParticleSystem.Particle> particles = new List<ParticleSystem.Particle>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
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
                goldAmount++;
                p.remainingLifetime = 0;
                particles[i] = p;

            }
            exp.SetTriggerParticles(ParticleSystemTriggerEventType.Enter, particles);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
