using Core.Blocks;
using Core.Input;

namespace Core.BlocksSwipe
{
	public interface IBlocksOnGridSwipeModel
	{
		void TrySwipeBlockTo(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData);
	}
}