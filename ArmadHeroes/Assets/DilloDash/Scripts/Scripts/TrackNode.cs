using UnityEngine;
using System.Collections.Generic;

namespace DilloDash
{
    public class TrackNode : MonoBehaviour
    {
        private Vector2 g;   //Line equation gradient of node
        private float c;   //Line equation offset of node
        private Vector3 left;   //Left most point of node
        private Vector3 right;   //Right most point of node
        private float sqrDist;   //The sqr distance between the centre and the left/right point 
        private Vector2 gNorm;   //Normal gradient

        public TrackNode[] nextNodes;   //List of all potential next nodes on the track
        private List<TrackNode> previousNodes = new List<TrackNode>();   //List of all potential prev nodes on the track
        [Range(0.0f, 100.0f)]
        public float trackPercent;   //The percent around the track were this node is located.
        [SerializeField] public GameObject spawnPivot = null;
        [SerializeField] bool spawnPoint = false;

        void Awake()
        {
            //Setup the constant data for this node for use in later calculations
            float scale = transform.localScale.x / 2.0f;
            sqrDist = scale * scale;
            left = transform.position + (Quaternion.Euler(0, 0, transform.eulerAngles.z) * (Vector3.left * scale));
            right = transform.position + (Quaternion.Euler(0, 0, transform.eulerAngles.z) * (Vector3.right * scale));
            g.x = (left.y - right.y);
            g.y = (left.x - right.x);
            c = (g.y * left.y) - (g.x * left.x);
            gNorm.y = g.x;
            gNorm.x = -g.y;

        }

        void Start()
        {
            transform.GetComponentInChildren<SpriteRenderer>().enabled = GameControllerDD.Singleton().debugMode;
            //Needs to be called after awake has finished on all nodes which sets up all the previous nodes
            for (int i = 0; i < nextNodes.Length; ++i)
            {
                nextNodes[i].SetPreviousNode(this);
            }
        }

        //Sets up a previous node from the next node
        public void SetPreviousNode(TrackNode _prev)
        {
            previousNodes.Insert(0, _prev);
        }

        public Vector3 GetLeftPoint()
        {
            return left;
        }

        public Vector3 GetRightPoint()
        {
            return right;
        }

        public float GetTrackPercent()
        {
            return trackPercent;
        }

        //Given a position will find the closest previous node
        public TrackNode GetClosestPreviousNode(Vector3 _pos)
        {
            int closestPreviousNode = 0;
            //Get distance to first previous node
            float toNode = previousNodes[closestPreviousNode].DistToNode(_pos);
            //Loop through all previous nodes and determine if any others are closer
            for (int i = 1; i < previousNodes.Count; ++i)
            {
                float newDist = previousNodes[i].DistToNode(_pos);
                if (toNode > newDist)
                {
                    toNode = newDist;
                    closestPreviousNode = i;
                }
            }
            return previousNodes[closestPreviousNode];
        }

        //Given a position will get the next closest node to that position and will determine the percent around the track
        public float PercentAroundTrack(Vector3 _pos)
        {
            //Get shortest distance from node line to position
            float fromNode = DistToNode(_pos);
            int closestNextNode = 0;
            //Determine which next node is closest to position
            float toNode = nextNodes[closestNextNode].DistToNode(_pos);
            for (int i = 1; i < nextNodes.Length; ++i)
            {
                float newDist = nextNodes[i].DistToNode(_pos);
                if (toNode > newDist)
                {
                    toNode = newDist;
                    closestNextNode = i;
                }
            }
            //Get shortest distance from next node line to position
            float progressToNextNode = (fromNode / (fromNode + toNode));
            
            float nextNodeTrackPercent = nextNodes[closestNextNode].GetTrackPercent();
            if (nextNodeTrackPercent == 0.0f)
            {
                nextNodeTrackPercent = 100.0f;
            }
            //Using the weighted distance between both nodes to get a percent around the track
            return trackPercent + ((nextNodeTrackPercent - trackPercent) * progressToNextNode);
        }

        //Will get the shortest distance from the position to the node line
        float DistToNode(Vector3 _pos)
        {
            //Using gradient and normal equations to get the points of intersection between the position and closest point on the node line
            float cNorm = (gNorm.y * _pos.y) - (gNorm.x * _pos.x);
            Vector3 intersect;
            intersect.x = ((cNorm * g.y) - (c * gNorm.y)) / ((g.x * gNorm.y) - (gNorm.x * g.y));
            if (float.IsNaN(intersect.x))
            {
                intersect.x = 0.0f;
            }
            intersect.y = ((g.x * intersect.x) + c) / g.y;
            if (float.IsNaN(intersect.y))
            {
                intersect.y = 0.0f;
            }
            intersect.z = 0.0f;
            //Get distance to intersection point
            float dist = (intersect - transform.position).sqrMagnitude;
            //If out of range of node then determine closest side
            if (sqrDist < dist)
            {
                dist = (intersect - left).sqrMagnitude;
                float rightDist = (intersect - right).sqrMagnitude;
                if (dist > rightDist)
                {
                    return Vector3.Distance(right, _pos);
                }
                return Vector3.Distance(left, _pos);
            }
            else
            {
                return Vector3.Distance(intersect, _pos);
            }
        }

        public TrackNode GetLastSpawnableNode()
        {
            if(spawnPoint)
            {
                return this;
            }
            else
            {
                return previousNodes[0].GetLastSpawnableNode();
            }
        }

        public Vector3 GetSpawnPivot()
        {
            return spawnPivot.transform.position;
        }
    }
}