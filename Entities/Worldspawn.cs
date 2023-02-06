using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VSharpBSP.Entities
{
    [ExecuteInEditMode]
    [Serializable]
    public class Worldspawn : Entity
    {
        [Header("Debug Settings")]
        public bool debugSpawn = false;
        
        // Track all the potential Entities where a player could spawn
        public HashSet<GameObject> playerSpawns = new HashSet<GameObject>();
        // When an Entity GO initializes, and is part of a team,
        // it should register itself in this dictionary of team names
        public Dictionary<string, HashSet<GameObject>> teams = new Dictionary<string, HashSet<GameObject>>();
        // When an Entity GO initializes, and has a targetname (is a target),
        // it should register itself in this dictionary of targets
        public Dictionary<string, HashSet<GameObject>> targets = new Dictionary<string, HashSet<GameObject>>();
        // DEBUGGING
        public List<string> teamNames = new List<string>();
        public List<string> targetNames = new List<string>();

        //
        // UNITY METHODS
        //

        protected override void Awake()
        {
            base.Awake();
            FindSpawnPoints();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
            PlacePlayer();
        }

        //
        // CUSTOM METHODS
        //

        public Vector3 GetTargetLocation(string tName)
        {
            return targets[tName].First().transform.position;
        }

        public void TeamTriggerEnter(string thisTeamname, Collider other)
        {
            foreach(GameObject memberGO in teams[thisTeamname])
            {
                Entity memberEnt = memberGO.GetComponent<Entity>();
                memberEnt.TeamTriggerEnter(other);
            }
        }

        public void TeamTriggerExit(string thisTeamname, Collider other)
        {
            foreach(GameObject memberGO in teams[thisTeamname])
            {
                Entity memberEnt = memberGO.GetComponent<Entity>();
                memberEnt.TeamTriggerExit(other);
            }
        }

        protected void FindSpawnPoints()
        {
            Entities.Info[] infoObjs = (Info[])GameObject.FindObjectsOfType(typeof(Entities.Info));
            foreach (Info info in infoObjs)
            {
                if (info.classname == "info_player_deathmatch")
                {
                    playerSpawns.Add(info.gameObject);
                }
            }
        }

        protected void PlacePlayer()
        {
            if (playerSpawns.Count == 0 || debugSpawn == true) return;

            //Info item = playerSpawns[Random.Range(0, playerSpawns.Count)];
            GameObject spawnPoint = playerSpawns.ElementAt(UnityEngine.Random.Range(0, playerSpawns.Count));
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.transform.position = spawnPoint.transform.position;
        }
    }
}
