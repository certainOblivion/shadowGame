using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using Visibility;

public class ShadowVisualizer : MonoBehaviour
{
    public GameObject Light;

    List<Vector2> mTriangle;
    List<ShadowCaster> mShadowCasters;
    VisibilityComputer mVisibility;
    // Use this for initialization
    void Start()
    {
        mVisibility = new VisibilityComputer(Light.transform.position, Light.GetComponent<Light>().range);
        mShadowCasters = new List<ShadowCaster>();
    }

    // Update is called once per frame
    void Update()
    {
        
        mShadowCasters = FindObjectsOfType<ShadowCaster>().ToList();
        
        foreach (ShadowCaster shadowCaster in mShadowCasters)
        {
            mVisibility.AddSquareOccluder(shadowCaster.gameObject.transform.position, shadowCaster.gameObject.GetComponent<MeshRenderer>().bounds.size.x, shadowCaster.gameObject.transform.rotation.eulerAngles.z);
        }

        List<Vector2> encounters = mVisibility.Compute();
        mTriangle = encounters;
        ShadowHelper.DrawVisibility(encounters.ToArray(), Light.transform.position, Color.cyan);

        mVisibility.ClearOccluders();

    }
}
