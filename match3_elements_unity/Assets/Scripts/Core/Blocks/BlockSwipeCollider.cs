using UnityEngine;

namespace Core.Blocks
{
	[RequireComponent(typeof(BoxCollider2D))]
	public sealed class BlockSwipeCollider : MonoBehaviour
	{
		[field: SerializeField]
		public BlockEntity blockEntity { get; private set; }
	}
}