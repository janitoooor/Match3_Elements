using System;
using Core.Blocks;
using UnityEngine;

namespace Core.SpawnContainer
{
	[Serializable]
	public sealed class SpawnContainer
	{
		[field: SerializeField]
		public SpawnContainerType containerType { get; private set; }
		
		[field: SerializeField]
		public Transform transform { get; private set; }
	}
}