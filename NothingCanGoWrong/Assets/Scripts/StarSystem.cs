using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StarSystem : MonoBehaviour
{
    #region Singleton
    public static StarSystem instance;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
        }
        else
        {
            return;
        }

    }
    #endregion

    private float G = 100f;
    public List<GameObject> attractors;
    public int planetsDestroyed = 0;

    // Start is called before the first frame update
    void Start()
    {
        attractors = FindObjectsOfType<Attractor>().Select(a => a.gameObject).ToList();
        InitialVelocity();
    }

    private void FixedUpdate()
    {
        Gravity();
    }

    private void Gravity()
    {
        // Newton's law of universal gravitation: F = G * ((m1 * m2) / r2), F = Gravitational force, G = gravitational constant, m1 and m2 = mass of the objects,
        // r = distance between the two objects

        foreach (GameObject a in attractors)
        {
            foreach (GameObject b in attractors)
            {
                if (!a.Equals(b))
                {
                    float m1 = a.GetComponent<Rigidbody>().mass;
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    a.GetComponent<Rigidbody>().AddForce((b.transform.position - a.transform.position).normalized *
                        (G * (m1 * m2) / (r * r)));
                }
            }
        }
    }

    private void InitialVelocity()
    {
        foreach (GameObject a in attractors)
        {
            foreach (GameObject b in attractors)
            {
                if (!a.Equals(b))
                {
                    float m2 = b.GetComponent<Rigidbody>().mass;
                    float r = Vector3.Distance(a.transform.position, b.transform.position);

                    a.transform.LookAt(b.transform);

                    if (a.GetComponent<Attractor>().IsElipticalOrbit)
                    {
                        // Eliptic orbit = G * M  ( 2 / r - 1 / a), G = gravitational constant, M = mass of the central object, r = distance between the two objects,
                        // a = length of the semi major axis
                        if (!a.GetComponent<Rigidbody>().isKinematic)
                            a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) * ((2 / r) - (1 / (r * 1.5f))));
                    }
                    else
                    {
                        // Circular Orbit = ((G * M) / r)^0.5, G = gravitational constant, M = mass of the central object and r = distance between the two objects
                        // Ignore mass of the orbiting object when the orbiting object's mass is negligible, like the mass of the earth vs. mass of the sun
                        if (!a.GetComponent<Rigidbody>().isKinematic)
                            a.GetComponent<Rigidbody>().velocity += a.transform.right * Mathf.Sqrt((G * m2) / r);
                    }
                }
            }
        }
    }
}