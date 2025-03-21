using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Octrees
{
    public class Octree
    {
        public OctreeNode root;//node gốc của Octree
        public Bounds bounds; //Vùng bao ngoài cùng của Octree
        public Graph graph;
        
        List<OctreeNode> emptyLeaves = new List<OctreeNode>();//Danh sách các node rỗng

        public Octree(List<GameObject> worldObjects, float minNodeSize, Graph graph)
        {
            this.graph = graph;
            
            CalculateBounds(worldObjects);//Tính toán cùng bao ngoài cùng của Octree
            CreateTree(worldObjects, minNodeSize);//Tạo Octree
            
            GetEmptyLeaves(root);//Tìm các node rỗng

            GetEdges();
        }

        private void GetEdges()
        {
            foreach (OctreeNode leaf in emptyLeaves)
            {
                foreach (OctreeNode otherLeaf in emptyLeaves)
                {
                    if (leaf.bounds.Intersects(otherLeaf.bounds))
                    {
                        graph.AddEdge(leaf, otherLeaf);
                    }
                }
            }
        }

        private void GetEmptyLeaves(OctreeNode node)
        {
            if (node.IsLeaf && node.objects.Count == 0)
            {
                emptyLeaves.Add(node);
                graph.AddNode(node);
                return;
            }

            if (node.childNode == null)
                return;

            foreach (OctreeNode child in node.childNode)
            {
                GetEmptyLeaves(child);
            }

            for (int i = 0; i < node.childNode.Length - 1; i++)
            {
                for (int j = i + 1; j < node.childNode.Length; j++)
                {
                    graph.AddEdge(node.childNode[i], node.childNode[j]);
                }
            }
            
        }

        private void CreateTree(List<GameObject>worldObjects, float minNodeSize)
        {
            root = new OctreeNode(bounds, minNodeSize);//Khởi tạo node gốc
            foreach (var obj in worldObjects)
            {
                root.Divide(obj);//Chia nhỏ Octree và thêm object vào đúng vị trí
            }
        }

        private void CalculateBounds(List<GameObject> worldObjects)
        {
            //Duyệt từng obj trong mảng để xác định vùng bao ngoài
            foreach (var obj in worldObjects)
            {
                //Encapsulate là để mở rộng bounds và đưa collider vào
                bounds.Encapsulate(obj.GetComponent<Collider>().bounds);
            }
            /*Summary
             * Điều chỉnh bound thành 1 khối lập phương
             */
            //Căn chỉnh kích thước của bounds
            Vector3 size = Vector3.one * Mathf.Max (bounds.size.x, bounds.size.y, bounds.size.z) * 0.6f;
            //Cập nhật bounds thành cube với kích thước mới; Và tâm không đổi
            bounds.SetMinMax(bounds.center - size, bounds.center + size);
        }
    }
    
}

