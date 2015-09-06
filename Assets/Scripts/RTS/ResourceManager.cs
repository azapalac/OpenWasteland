using UnityEngine;
using System.Collections;
using UnityEngine.UI;
namespace RTS {

	public static class ResourceManager {

		public static int WorldObjectLayer { get { return 8; } }
		public static float ScrollSpeed { get { return 25; } }
		public static float RotateSpeed { get { return 100; } }
		public static int ScrollWidth { get { return 25; } }
		private static GameObject selectionBox;

		public static GameObject GetSelectionBox { get { return selectionBox;} }
		public static void StoreSelectionBoxItems(GameObject s){
			selectionBox = s;
		}
		//Remember to change this once the game is not played on a flat surface - Use raycasting!!
		public static float MinCameraHeight { get { return 10;}}

		public static float MaxCameraHeight { get { return 40; } }
		public static float RotateAmount {get { return 10; } }
		private static Vector3 invalidPosition = new Vector3(-99999, -99999, -99999);
		public static Vector3 InvalidPosition { get { return invalidPosition;} }
	}
}