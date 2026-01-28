using System;

namespace Base.Gui
{
	public interface IGuiWidget : IDisposable
	{
		void Open();
		void Close();
	}
}