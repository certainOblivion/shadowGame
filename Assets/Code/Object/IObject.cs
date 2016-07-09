using UnityEngine;
using System.Collections;

public abstract class IObject : MonoBehaviour
{
    public Vector2 Position
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }

    public float Rotation
    {
        get
        {
            return transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
        }

        set
        {
            transform.Rotate(Vector3.forward, value * Mathf.Rad2Deg);
        }
    }

    public Vector2 Bounds
    {
        get
        {
            if (mBounds == Vector2.zero)
            {
                Quaternion rotationAtStart = transform.rotation;
                transform.rotation = Quaternion.identity;
                mBounds = GetComponent<MeshRenderer>().bounds.size;
                transform.rotation = rotationAtStart; 
            }
            return mBounds;
        }
    }

    private Vector2 mBounds;

}
