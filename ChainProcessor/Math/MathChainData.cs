using UnityEngine; 

using System.Collections;
using System.Collections.Generic;

using BaseGameLogic.ChainProcessing;

public class MathChainData : ChainData 
{
	public float Data = 0f;

	public MathChainData (float data)
	{
		this.Data = data;
	}
	

	public override void OnInspektorGui ()
	{
//		throw new System.NotImplementedException ();
	}

	protected override ChainData Add (ChainData data)
	{
		MathChainData mathData = data as MathChainData;
		float a = this.Data + mathData.Data;

		return new MathChainData (a);
	}

	protected override ChainData Subtract (ChainData data)
	{
		MathChainData mathData = data as MathChainData;
		float a = this.Data - mathData.Data;

		return new MathChainData (a);
	}

	protected override ChainData Multiply (ChainData data)
	{
		MathChainData mathData = data as MathChainData;
		float a = this.Data * mathData.Data;

		return new MathChainData (a);
	}
}
