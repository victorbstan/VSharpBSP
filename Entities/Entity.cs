using System;
using System.Collections.Generic;
using System.Reflection;
using NativeSerializableDictionary;
using UnityEngine;
using UnityEngine.Serialization;

namespace VSharpBSP.Entities
{
    [Serializable]
    public abstract class Entity : MonoBehaviour
    {
        // Some reference documentation:
        // https://quakewiki.org/wiki/func_door
        
        // Entity name/type
        public string classname;
        // 1 	Starts open 	The entity is moved to the open position for its initial state
        // 4 	Don't link 	Restricts the entity from being linked to a matching door (preventing it from opening as part of a pair)
        // 8 	Gold key required 	The entity will not move unless the player has collected the Gold Key
        // 16 	Silver key required 	The entity will not move unless the player has collected the Silver key
        // 32 	Toggle 	The entity will toggle between open and closed states instead of automatically closing after a delay 
        public int spawnflags;
        public Vector3 origin;
        public Vector3 minangle;
        public string model;
        public float minbright;
        // The name of the door.
        // Only use this if you want the door to be locked until triggered by something else, such as a button. 
        public string targetname;
        public string target;
        // public string teamname;
        public string team;
        public string killtarget;
        // Sets the direction the door should move when it opens.
        // Angles from 0 to 359 will make it move horizontally.
        // You can also set the motion to be vertical by setting the angle to -1 (upward motion) or -2 (downward motion) instead. 
        public int angle;
        // Time after opening for it to reset and be triggerable again. Setting this to -1 will cause the door to remain open. 
        public float wait = 2f; // seconds
        public float speed = 16f; // units per second
        public float health;
        public string message;
        // TODO: sounds - list of sound type options (maybe should just be string of root directory
        public string sound1;
        public string sound2;
        public float damage;
        public float lip = 0.16f;

        // Configuration (set by map)
        public SerializableDictionary<string, string> attributes = new SerializableDictionary<string, string>();

        // Track trigger action state
        [Header("Protected Entity Attributes")]
        [SerializeField]
        protected Worldspawn worldspawn; 
        [SerializeField]
        protected int actionDir = 1;
        [SerializeField]
        protected bool action = false;
        [SerializeField]
        protected Vector3 actionStartPosition;
        [SerializeField]
        protected Vector3 actionEndPosition;
        [SerializeField]
        protected Quaternion actionStartRotation;
        [SerializeField]
        protected Quaternion actionEndRotation;
        [SerializeField]
        protected Vector3 actionStartSize;
        [SerializeField]
        protected BoundingBox boundingBox;
        // Ability flags
        [SerializeField]
        protected bool ableChangePosition = false;
        [SerializeField]
        protected bool ableChangeRotation = false;

        //
        // UNITY METHODS
        //

        protected virtual void Awake()
        {
            Init();
        }

        // Start is called before the first frame update
        protected virtual void Start()
        {
        }

        protected virtual void OnDrawGizmos()
        {
            if (target != null && worldspawn != null && worldspawn.targets.ContainsKey(target))
            {
                Util.DrawArrow(
                    boundingBox != null ? boundingBox.transform.position : transform.position,
                    worldspawn.GetTargetLocation(target)
                    );
            }
            
#if UNITY_EDITOR
            if (classname != "light")
            {
                UnityEditor.Handles.color = Color.white;
                UnityEditor.Handles.Label(
                    boundingBox != null ? boundingBox.transform.position : transform.position, 
                    ToString() 
                );
            }
#endif
        }

        //
        // CUSTOM METHODS
        //

        public virtual void Init()
        {
            if (!GetComponent<Worldspawn>())
                worldspawn = FindObjectOfType<Worldspawn>();
            
            ProcessAttributes();
            ProcessBoundingBox();
            ProcessAbilities();
            RegisterTeam();
            RegisterTarget();
        }

        //
        // CUSTOM EVENT HANDLING
        //
        
        public virtual void EntityTriggerEnter(Collider other)
        {
            Debug.Log(classname + " TRIGGER ENTER " + this.ToString() + " FROM: " + other.ToString());
            // If this entity is team member, call event on worldspawn
            if (team != string.Empty)
                worldspawn.TeamTriggerEnter(team, other);
            else
            {
                action = true;
                actionDir = 1;
            }
        }
        
        // Called by worldspawn if this entity is a member of a team
        public virtual void TeamTriggerEnter(Collider other)
        {
            Debug.Log(classname + " TEAM TRIGGER ENTER " + this.ToString() + " FROM: " + other.ToString());
            action = true;
            actionDir = 1;
        }

        public virtual void EntityTriggerExit(Collider other)
        {
            Debug.Log(classname + " TRIGGER EXIT " + this.ToString() + " FROM: " + other.ToString());
            // If this entity is team member, call event on worldspawn
            if (team != string.Empty)
                worldspawn.TeamTriggerExit(team, other);
            else
            {
                action = true;
                actionDir = -1;
            }
        }
        
        // Called by worldspawn if this entity is a member of a team
        public virtual void TeamTriggerExit(Collider other)
        {
            Debug.Log(classname + " TEAM TRIGGER EXIT " + this.ToString() + " FROM: " + other.ToString());
            action = true;
            actionDir = -1;
        }

        public virtual void ProcessAttributes()
        {
            foreach (var kvp in attributes)
            {
                string eKey = kvp.Key;
                string eVal = kvp.Value.Value;

                switch (eKey)
                {
                    case "_color" or "color":
                        TrySetColor("color", eVal);
                        break;
                    case "origin":
                        // just hardcoding this,
                        // NOTE models are always 0,
                        // child boundingBox and faces have proper position
                        origin = transform.position; 
                        break;
                    case "classname":
                        classname = eVal;
                        break;
                    case "model":
                        model = eVal.Replace("*", string.Empty);
                        break;
                    case "angle":
                        angle = int.Parse(eVal);
                        break;
                    case "lip":
                        lip = float.Parse(eVal) * Constants.f_scaleMultiple;
                        break;
                    case "speed":
                        speed = float.Parse(eVal);
                        break;
                    case "team":
                        team = eVal;
                        break;
                    case "target":
                        target = eVal;
                        break;
                    case "targetname":
                        targetname = eVal;
                        break;
                    case "wait":
                        wait = float.Parse(eVal);
                        break;
                    default:
                        //GetType().GetProperty(eKey).SetValue(this, eVal, null);
                        break;
                }
            }
        }

        public virtual void ProcessAbilities()
        {
            switch (classname)
            {
                case "func_door":
                    ableChangePosition = true;
                    ableChangeRotation = true;
                    MakeFacesMovable();
                    break;
                default:
                    break;
            }
        }

        public virtual void ProcessBoundingBox()
        {
            // NOTE: Not all entities have bounding boxes, most don't, only meshes do
            // Bounding box and children are the only thing that move or rotate
            boundingBox = GetComponentInChildren<BoundingBox>();
            if (boundingBox)
            {
                actionStartPosition = boundingBox.origin;
                actionStartSize = boundingBox.size;
                actionStartRotation = boundingBox.gameObject.transform.rotation;
            }
        }
        
        public virtual void AddAttributes(Dictionary<string, string> dict)
        {
            foreach (KeyValuePair<string, string> item in dict)
                attributes.AddDirect(item.Key, item.Value);
        }

        // DEPRECATED
        public void RegisterPlayerSpawn()
        {
            if (attributes.ContainsKey("info_player_deathmatch"))
            {
                Worldspawn wsComp = FindObjectOfType<Worldspawn>();
                Info infoComp = this.GetComponent<Info>();
                if (infoComp != null)
                {
                    wsComp.playerSpawns.Add(infoComp.gameObject);
                }
            }
        }

        public void RegisterTeam()
        {
            if (attributes.ContainsKey("team"))
            {
                string teamName = attributes.GetValue("team");
                if (worldspawn.teams.ContainsKey(teamName))
                {
                    worldspawn.teams[teamName].Add(this.gameObject);
                    worldspawn.teamNames.Add(teamName);
                }
                else
                {
                    worldspawn.teams.Add(teamName, new HashSet<GameObject>());
                    worldspawn.teams[teamName].Add(this.gameObject);
                    worldspawn.teamNames.Add(teamName); // DEBUGGING LIST
                }
            }
        }

        public void RegisterTarget()
        {
            if (attributes.ContainsKey("targetname"))
            {
                string targetName = attributes.GetValue("targetname");
                if (worldspawn.targets.ContainsKey(targetName))
                {
                    worldspawn.targets[targetName].Add(this.gameObject);
                    worldspawn.targetNames.Add(targetName);
                }
                else
                {
                    worldspawn.targets.Add(targetName, new HashSet<GameObject>());
                    worldspawn.targets[targetName].Add(this.gameObject);
                    worldspawn.targetNames.Add(targetName); // DEBUGGING LIST
                }
            }
        }
        
        public void LogAttributes()
        {
            foreach (var kvp in attributes)
                Debug.Log($"Attribute Key: {kvp.Key} -> Value: {kvp.Value.Value}");

            if (attributes.Count == 0)
                Debug.Log($"No attributes for: {classname}");
        }

        //
        // PRIVATE
        //
        
        private void MakeFacesMovable()
        {
            foreach (Transform childT in boundingBox.gameObject.transform)
                if (childT.gameObject)
                    childT.gameObject.isStatic = false;
        }

        private (bool, PropertyInfo) HasProperty(string prop)
        {
            PropertyInfo propInfo = GetType().GetProperty(prop);
            return (propInfo != null, propInfo);
        }

        private void TrySetColor(string prop, string value)
        {
            string[] cp = value.Split(" ");
            Color colorVal = new Color(
                float.Parse(cp[0]),
                float.Parse(cp[1]),
                float.Parse(cp[2]),
                1f
                );

            var field = this.GetType().GetField("color");
            if (field != null)
                field.SetValue(this, colorVal);
        }
    }
}

