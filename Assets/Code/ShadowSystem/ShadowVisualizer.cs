using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Helper;
using Visibility;

public class ShadowVisualizer : MonoBehaviour
{
    public GameObject LightObject;
    public float LightRange;

    List<ShadowCaster> mShadowCasters;
    VisibilityComputer mVisibility;
    // Use this for initialization
    void Start()
    {
        mVisibility = new VisibilityComputer(LightObject.transform.position, LightRange);
        mShadowCasters = new List<ShadowCaster>();
    }

    // Update is called once per frame
    void Update()
    {        
        mShadowCasters = FindObjectsOfType<ShadowCaster>().ToList();
        
        foreach (ShadowCaster shadowCaster in mShadowCasters)
        {
            mVisibility.AddSquareOccluder(shadowCaster.Position, shadowCaster.Bounds.x, /*boundSize.y,*/ shadowCaster.Rotation);
        }

        List<Vector2> encounters = mVisibility.Compute();
#if UNITY_EDITOR
        ShadowHelper.DrawVisibility(encounters.ToArray(), LightObject.transform.position, Color.cyan);
#endif

        mVisibility.ClearOccluders();

    }
}
