namespace Base
{
	public delegate void ProgressUpdatedDelegate(float progress);
	
	public interface IProgressUpdatable
	{
		event ProgressUpdatedDelegate OnProgressUpdated;
	}
}