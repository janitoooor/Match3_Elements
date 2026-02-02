namespace Core.Balloons
{
	public interface IBalloonGenerator
	{
		IBalloonEntity GenerateBalloon(out bool isInstantiated);
	}
}