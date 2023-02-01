using System;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;

namespace VSharpBSP.Entities
{
    public class Func : Entity
    {
        private Vector3 _targetPos;
        private float _waitCount = float.Epsilon;

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
                
            // Initial setup
            if (classname == "func_door")
            {
                actionEndPosition = CalcActionEndPosition();
                _targetPos = actionDir == 1 ? actionEndPosition : actionStartPosition;
                
                Util.DrawArrow(actionStartPosition, actionEndPosition);
            }
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            
            // Initial setup
            if (classname == "func_door")
            {
                actionEndPosition = CalcActionEndPosition();
                _targetPos = actionDir == 1 ? actionEndPosition : actionStartPosition;
            }
        }
        
        private void Update()
        {
            if (action && ableChangePosition && classname == "func_door")
            {
                // Wait
                if (actionDir < 0 && _waitCount >= float.Epsilon)
                {
                    _waitCount -= 1 * Time.deltaTime;
                    return;
                }
                
                // Pick a target position based on action direction
                _targetPos = actionDir == 1 ? actionEndPosition : actionStartPosition;
                
                // Debug.Log(
                //     classname 
                //     + " TARGET: " + _targetPos 
                //     + " | Start: " + actionStartPosition 
                //     + " End: " + actionEndPosition 
                //     + " ø " + angle
                //     + " <> " + actionDir
                // );

                // Move
                float step = speed * Time.deltaTime; // calculate distance to move
                boundingBox.gameObject.transform.position = Vector3.MoveTowards(
                    boundingBox.gameObject.transform.position,
                    _targetPos,
                    step
                    );
                
                // Finish
                if (BBoxDistanceTo(_targetPos) <= float.Epsilon)
                {
                    action = false;
                    actionDir *= -1; // flip direction
                    _waitCount = wait;
                }
            }
            else
            {
                // reset
                action = false;
                actionDir = 1;
            }
        }

        public override void Init()
        {
            base.Init();

            if (classname != null && classname.StartsWith("func_door"))
            {
                Transform bBoxTransform = transform.Find("BoundingBox");

                if (bBoxTransform == null)
                    return;

                GameObject bBoxGO = transform.Find("BoundingBox").gameObject;

                if (bBoxGO == null)
                    return;

                if (!bBoxGO.TryGetComponent<BoundingBox>(out var bBoxComp))
                    return;

                if (bBoxGO.GetComponent<BoxCollider>())
                    return;

                Debug.Log("SETTING UP BOX COLLIDERS FOR FUNC");

                // Add trigger component to bounding box
                BoxCollider bBoxCol = bBoxGO.AddComponent<BoxCollider>();
                bBoxCol.isTrigger = true;
                bBoxCol.size = bBoxComp.size;

                // Add collision manager to bounding box trigger collider
                bBoxGO.AddComponent<EntityTriggerManager>();

                // Add another trigger as a new Model child GameObject
                // This one should be bigger
                // It's stand-alone because it should not move when the bounding box moves (opens)
                GameObject triggerGO = new GameObject("Trigger");
                triggerGO.transform.parent = transform; // set as child of Model (self)
                triggerGO.transform.position = bBoxTransform.position; // set position
                BoxCollider triggerCol = triggerGO.AddComponent<BoxCollider>();
                triggerCol.isTrigger = true;
                // Set size with some extra padding
                triggerCol.size = new Vector3(
                    PadTrigger(bBoxComp.size.x),
                    bBoxComp.size.y,
                    PadTrigger(bBoxComp.size.z)
                    );

                // Add collision manager to large door trigger collider
                triggerGO.AddComponent<EntityTriggerManager>();
            }
        }

        //
        // ENTITY EVENTS
        //

        public override void EntityTriggerEnter(Collider other)
        {
            base.EntityTriggerEnter(other);
            if (classname.StartsWith("func_door"))
            {
                // TODO: do door open things here

            }
        }

        public override void EntityTriggerExit(Collider other)
        {
            base.EntityTriggerExit(other);
            if (classname.StartsWith("func_door"))
            {
                // TODO: do door close things here
            }
        }

        //
        // PRIVATE
        //

        private Vector3 CalcActionEndPosition()
        {
            Vector3 direction;
            switch (angle)
            {
                case -1: // Up
                    direction = Vector3.up;
                    break;
                case -2: // Down
                    direction = Vector3.down;
                    break;
                default: // Angle on horizontal plane
                    direction = Util.AngleToVector3(angle);
                    break;
            }
            
            return actionStartPosition + new Vector3(
                (boundingBox.size.x - lip) * direction.x,
                (boundingBox.size.y - lip) * direction.y, 
                (boundingBox.size.z - lip) * direction.z
            );
        }

        private float BBoxDistanceTo(Vector3 toTarget)
        {
            return Vector3.Distance(boundingBox.gameObject.transform.position, toTarget);
        }
        
        private float PadTrigger(float n)
        {
            // Should not need this, size should be positive values
            //if (n < 0) n -= Constants.doorTriggerPadding; else n += Constants.doorTriggerPadding;
            n += Constants.doorTriggerPadding;
            return n;
        }
    }
}
