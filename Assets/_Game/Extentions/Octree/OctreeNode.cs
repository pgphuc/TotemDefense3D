using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class OctreeNode 
    {
        public List<OctreeObject> objects = new List<OctreeObject>();//Các object trong node này
        private static int nextID;//ID tự tăng cho mỗi node
        public readonly int id;

        public Bounds bounds;//Phạm vi của node
        Bounds[] childBounds = new Bounds[8];//Phạm vi của các node con
        public OctreeNode[] childNode;//Mảng chứa các node con
        public bool IsLeaf => childBounds == null;//không có childBounds

        private float minNodeSize;//Kích cỡ của 1 node nhỏ nhất

        public OctreeNode(Bounds bounds, float minNodeSize)
        {
            id = nextID++;//Cấp ID cho node
            this.bounds = bounds;
            this.minNodeSize = minNodeSize;

            //Tính toán kích thước của các node con
            Vector3 newSize = bounds.size * 0.5f; //Bằng 1/2 so với parent
            Vector3 centerOffset = bounds.size * 0.25f;//Bằng 1/4 so với parent
            Vector3 parentCenter = bounds.center;

            for (int i = 0; i < 8; i++)
            {
                Vector3 childCenter = parentCenter;
                childCenter.x += centerOffset.x * ((i & 1) == 0 ? -1 : 1);
                childCenter.y += centerOffset.y * ((i & 2) == 0 ? -1 : 1);
                childCenter.z += centerOffset.z * ((i & 4) == 0 ? -1 : 1);
                childBounds[i] = new Bounds(childCenter, newSize);
            }
        }

        public void Divide(GameObject gameObject) => Divide(new OctreeObject(gameObject));
        public void Divide(OctreeObject octreeObject)
        {
            //nếu node đã đạt kích thước nhỏ nhất thì không chia nhỏ nữa
            if (bounds.size.x <= minNodeSize)
            {
                AddObject(octreeObject);
                return;
            }
            
            //Tạo mảng chứa các node con nếu CHƯA CÓ
            childNode ??= new OctreeNode[8];
            
            bool intersectedChild = false;
            
            //Duyệt qua các node con và kiểm tra object có nằm trong không
            for (int i = 0; i < 8; i++)
            {
                childNode[i] ??= new OctreeNode(childBounds[i], minNodeSize);
                if (octreeObject.Intersects(childBounds[i]))
                {
                    childNode[i].Divide(octreeObject);
                    intersectedChild = true;
                }
            }
            if (!intersectedChild)
            {
                AddObject(octreeObject);
            }
        }

        public void AddObject(OctreeObject obj)
        {
            objects.Add(obj);
        }
        
        
        public void DrawNode()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(bounds.center, bounds.size);

            foreach (OctreeObject octreeObject in objects)
            {
                if (octreeObject.Intersects(bounds))
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawWireCube(bounds.center, bounds.size);
                }
            }            
            
            if (childNode == null)
                return;
            foreach (OctreeNode octreeNode in childNode)
            {
                if (octreeNode != null)
                {
                    octreeNode.DrawNode();
                }
            }
        }
    }
}
