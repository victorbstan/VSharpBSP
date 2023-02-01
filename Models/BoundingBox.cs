using UnityEngine;
using System;

namespace VSharpBSP
{
	[Serializable]
	public class BoundingBox : MonoBehaviour
	{
		public Vector3 origin;
		public Vector3 size;

		public BoundingBox(Vector3 origin, Vector3 size)
		{
			this.origin = origin;
			this.size = size;
		}
	}
}

