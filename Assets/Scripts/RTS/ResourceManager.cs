﻿using UnityEngine;
using System.Collections;

namespace RTS {

	public static class ResourceManager {
		public static float ScrollSpeed { get { return 25;}}
		public static float RotateSpeed { get { return 100;}}
		public static int ScrollWidth { get { return 25; } }

		//Remember to change this once the game is not played on a flat surface
		public static float MinCameraHeight { get { return 10;}}

		public static float MaxCameraHeight { get { return 40; } }
		public static float RotateAmount {get { return 10; } }

	}
}