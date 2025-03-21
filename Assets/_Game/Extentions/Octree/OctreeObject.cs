using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class OctreeObject
    {
        public Bounds bounds;
        public OctreeObject(GameObject gameObject)
        {
            bounds = gameObject.GetComponent<Collider>().bounds;
        }

        public bool Intersects(Bounds boundsToCheck)
        {
            return bounds.Intersects(boundsToCheck);
        }
    }
}