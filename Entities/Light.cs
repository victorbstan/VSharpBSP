using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace VSharpBSP.Entities
{
    [ExecuteInEditMode]
    public class Light : Entity
    {
        public Color color = Color.white;

        protected override void Awake()
        {
            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();
        }
    }
}
