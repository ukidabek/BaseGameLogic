using UnityEngine;
using System.Collections;

public class TableHelper
{
	public static int CorrectIndex<T>(T[] table, int index)
	{
		if (index < 0)
			return 0;

		if (index > table.Length - 1)
			return table.Length - 1;

		return index;
	}

	public static bool ContainValue<T>(T[] table, T value)
	{
		for (int i = 0; i < table.Length; i++) 
		{
			if (table [i].Equals(value))
				return true;
		}

		return false;
	}
}
