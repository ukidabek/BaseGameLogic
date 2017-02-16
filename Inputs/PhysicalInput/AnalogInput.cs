using UnityEngine;

using System;
using System.Collections;

namespace BaseGameLogic.Inputs
{
	[Serializable]
	public class AnalogInput : PhysicalInput
	{
		[SerializeField]
		private string horizontalAxisName = string.Empty;

		[SerializeField]
		private string verticaAxisName = string.Empty;

		private Vector2 oldAxisReading = Vector2.zero;
		[SerializeField]
		private Vector2 axisReading = Vector2.zero;

		[SerializeField]
		private bool invertHorizontaAxis = false;

		[SerializeField]
		private bool invertVerticaAxis = false;

		public override bool PositiveReading 
		{
			get 
			{
				return !axisReading.Equals (oldAxisReading);
			}
		}

		public Vector2 Axis 
		{
			get 
			{ 
				return this.axisReading;
			}
		}

		public override void Read ()
		{
			if (horizontalAxisName == string.Empty)
				return;
				
			oldAxisReading = axisReading;
			axisReading.x = Input.GetAxis (horizontalAxisName);
			if (invertHorizontaAxis)
			{
				axisReading.x *= -1;
			}

			axisReading.y = Input.GetAxis (verticaAxisName);
			if (invertVerticaAxis) 
			{
				axisReading.y *= -1;
			}
		}
	}
}