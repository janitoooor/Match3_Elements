using Base;
using Core.Enums;
using Core.Interfaces;
using UnityEngine;

namespace Core
{
	[CreateAssetMenu(menuName = "Match3/Core/Create Prefabs Container", order = 0, fileName = "CorePrefabsContainer")]
	public sealed class CorePrefabsContainer : PrefabsContainer<CorePrefabsKeys>, ICorePrefabsContainer { }
}