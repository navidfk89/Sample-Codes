using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gamplay.Merchant.Models;

internal class MerchantSettings : SerializedScriptableObject, IMerchantConfig
{
	[SerializeField]
	private Dictionary<int, MerchantData> _merchantsDic;

	public IMerchantData GetMerchantData(int ascension)
	{
		while (ascension >= 0)
		{
			if (!_merchantsDic.ContainsKey(ascension))
			{
				ascension--;
				continue;
			}
			return _merchantsDic[ascension];
		}
		throw new MissingDataException("Merchant Data is Missing.");
	}
}
