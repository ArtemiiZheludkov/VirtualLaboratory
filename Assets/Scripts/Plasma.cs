using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirtualLaboratory
{
    public class Plasma : MonoBehaviour
    {   
        public GameObject electronPrefab;
        public GameObject ionPrefab;
        public int particleCount = 100;
        public float plasmaVolumeRadius = 5f;

        private List<GameObject> particles = new List<GameObject>();

        void Start()
        {
            // Створення електронів і іонів
            for (int i = 0; i < particleCount; i++)
            {
                Vector3 randomPosition = Random.insideUnitSphere * plasmaVolumeRadius;
                GameObject particle;

                if (i % 2 == 0) // Для простоти, чергуємо електрони та іони
                {
                    particle = Instantiate(electronPrefab, randomPosition, Quaternion.identity);
                }
                else
                {
                    particle = Instantiate(ionPrefab, randomPosition, Quaternion.identity);
                }

                particles.Add(particle);
            }
        }

        void Update()
        {
            // Основний цикл для взаємодії частинок
            foreach (var particle in particles)
            {
                // Взаємодія з іншими частинками (проста реалізація)
                foreach (var otherParticle in particles)
                {
                    if (particle != otherParticle)
                    {
                        float charge = particle.GetComponent<ParticleProperties>().charge;
                        Vector3 direction = otherParticle.transform.position - particle.transform.position;
                        float distance = direction.magnitude;

                        if (distance > 0.1f) // Уникнення дуже маленьких відстаней
                        {
                            // Кулонівська сила
                            float forceMagnitude = (charge * otherParticle.GetComponent<ParticleProperties>().charge) / (distance * distance);
                            Vector3 force = direction.normalized * forceMagnitude;

                            // Застосування сили до частинки
                            particle.GetComponent<Rigidbody>().AddForce(force);
                        }
                    }
                }
            }
        }
    }
}