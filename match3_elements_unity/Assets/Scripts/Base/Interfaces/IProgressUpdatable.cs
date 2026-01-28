namespace Base
{
	public delegate void OnProgressUpdatedDelegate(float progress);
	
	public interface IProgressUpdatable
	{
		event OnProgressUpdatedDelegate OnProgressUpdated;
	}
}