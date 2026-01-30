using Core.Blocks;
using Core.Input;

namespace Core.Grid
{
	public interface IBlocksOnGridSwipeModel
	{
		void TrySwipeBlockTo(IBlockEntity blockEntity, SwipeDirectionData swipeDirectionData);
	}
}