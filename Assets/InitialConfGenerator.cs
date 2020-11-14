﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class InitialConfGenerator : MonoBehaviour
{
    public float temperature        = 300.0f;
    public float kb                 = 0.8317e-4f; // mass:Da, length:Å, time:0.01ps
    public float box_size           = 5.0f;
    public uint  total_particle_num = 500;
    public Rigidbody lj_particle;

    // Start is called before the first frame update
    void Start()
    {
        // generate initial position
        List<Vector3> coords_list = new List<Vector3>();
        uint positioned_particle_num = 0;
        uint trial_time              = 0;
        while (positioned_particle_num < total_particle_num && trial_time < 500)
        {
            trial_time += 1;
            Vector3 new_coord = new Vector3(Random.Range(-box_size, box_size),
                                            Random.Range(-box_size, box_size),
                                            Random.Range(-box_size, box_size));
            bool acceptable = true;
            if (coords_list.Count == 0) 
            {
                coords_list.Add(new_coord); 
                positioned_particle_num += 1;
                continue;
            }

            foreach (Vector3 fixed_pos in coords_list)
            {
                if ((fixed_pos - new_coord).magnitude < 1.0f) 
                { 
                    acceptable = false;
                    break;
                }
            }
            if (acceptable)
            {
                coords_list.Add(new_coord);
                positioned_particle_num += 1;
                trial_time = 0;
            }
        }

        foreach (Vector3 coord_vec in coords_list)
        {
            Rigidbody new_particle = Instantiate(lj_particle, coord_vec, transform.rotation);
            float sigma = Mathf.Sqrt(kb * temperature / new_particle.mass);
            new_particle.velocity = new Vector3(NormalizedRandom.Generate(0.0f, sigma),
                                                NormalizedRandom.Generate(0.0f, sigma),
                                                NormalizedRandom.Generate(0.0f, sigma));
            new_particle.gameObject.GetComponent<Particle>().SetBoxSize(box_size);

        }
        Debug.Log(coords_list.Count.ToString() + " particle generated.");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Rigidbody new_particle = Instantiate(lj_particle, transform.position, transform.rotation);
            float sigma = Mathf.Sqrt(kb * temperature / new_particle.mass);
            new_particle.velocity  = new Vector3(NormalizedRandom.Generate(0.0f, sigma),
                                                 NormalizedRandom.Generate(0.0f, sigma),
                                                 NormalizedRandom.Generate(0.0f, sigma));
        }
    }
}
