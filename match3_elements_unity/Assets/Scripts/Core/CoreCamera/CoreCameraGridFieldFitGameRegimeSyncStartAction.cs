using Base;
using Core.Enums;
using Core.Grid;
using UnityEngine;
using Zenject;

namespace Core.CoreCamera
{
	public sealed class CoreCameraGridFieldFitGameRegimeSyncStartAction : GameRegimeSyncStartAction, ITickable
	{
		private readonly IGridField gridField;
		private readonly Camera camera;
		public override byte priority => (byte)CoreGameRegimeSyncStartActionPriority.CameraGridFieldFit;

		[Inject]
		public CoreCameraGridFieldFitGameRegimeSyncStartAction(IGridField gridField, Camera camera)
		{
			this.gridField = gridField;
			this.camera = camera;
		}

		public override void Perform()
			=> FitCameraToGridField();

		public void Tick()
			=> FitCameraToGridField();

		private void FitCameraToGridField()
		{
			ChangeCameraOrthographicSizeForGrid();
			CenterCameraOnGrid();
		}

		private void ChangeCameraOrthographicSizeForGrid()
		{
			var gridWidth = gridField.GetGridWidth();
			var gridHeight = gridField.GetGridHeight();
			
			var screenAspect = Screen.width / (float)Screen.height;
			var targetAspect = gridWidth / gridHeight;

			if (targetAspect > screenAspect)
				camera.orthographicSize = gridWidth / (2f * screenAspect);
			else
				camera.orthographicSize = gridHeight / 2f;
		}
		
		private void CenterCameraOnGrid()
		{
			var gridCenter = gridField.GetGridCenter();
			var cameraPos = camera.transform.position;
            
			camera.transform.position = new Vector3(
				gridCenter.x,
				gridCenter.y,
				cameraPos.z
			);
		}
	}
}