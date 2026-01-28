using Base;
using Meta.Enums;
using UnityEngine;

namespace Meta
{
	[CreateAssetMenu(menuName = "Meta/Create Prefabs Container", order = 0, fileName = "MetaPrefabsContainer")]
	public sealed class MetaPrefabsContainer : PrefabsContainer<MetaPrefabsKeys>, IMetaPrefabsContainer { }
}