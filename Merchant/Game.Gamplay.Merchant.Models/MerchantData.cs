using System;
using System.Collections.Generic;
using Game.Random;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Game.Gamplay.Merchant.Models;

public class MerchantData : SerializedScriptableObject, IMerchantData
{
	[Header("Colored Cards")]
	[SerializeField]
	private int _cardsColoredAttackCount;

	[SerializeField]
	private int _cardsColoredSkillCount;

	[SerializeField]
	private int _cardsColoredPowerCount;

	[SerializeField]
	private RangeInt _cardColoredCommonCost;

	[SerializeField]
	private RangeInt _cardColoredUncommonCost;

	[SerializeField]
	private RangeInt _cardColoredRareCost;

	[SerializeField]
	private float _cardColoredOffPercent;

	[SerializeField]
	private Dictionary<RarityType, float> _cardRarityChances;

	[Header("Colorless Cards")]
	[SerializeField]
	private int _cardsColorlessUncommonCount;

	[SerializeField]
	private RangeInt _cardColorlessUncommonCost;

	[SerializeField]
	private int _cardsColorlessRareCount;

	[SerializeField]
	private RangeInt _cardColorlessRareCost;

	[Header("Relics")]
	[SerializeField]
	private int _relicsShopCount;

	[SerializeField]
	private int _relicsNoneShopCount;

	[SerializeField]
	private RangeInt _relicCommonCost;

	[SerializeField]
	private RangeInt _relicUncommonCost;

	[SerializeField]
	private RangeInt _relicRareCost;

	[SerializeField]
	private RangeInt _relicShopCost;

	[Header("Potions")]
	[SerializeField]
	private int _potionsCommonCount;

	[SerializeField]
	private int _potionsUncommonCount;

	[SerializeField]
	private int _potionsRareCount;

	[SerializeField]
	private RangeInt _potionCommonCost;

	[SerializeField]
	private RangeInt _potionUncommonCost;

	[SerializeField]
	private RangeInt _potionRareCost;

	[Header("Removal Service")]
	[SerializeField]
	private int _firstUseCost;

	[SerializeField]
	private int _extraCostPerUsing;

	public int CardsColoredCount => _cardsColoredAttackCount + _cardsColoredSkillCount + _cardsColoredPowerCount;

	public float CardColoredOffPercent => _cardColoredOffPercent;

	public int CardsColoredAttackCount => _cardsColoredAttackCount;

	public int CardsColoredSkillCount => _cardsColoredSkillCount;

	public int CardsColoredPowerCount => _cardsColoredPowerCount;

	public int CardsColorlessCount => _cardsColorlessUncommonCount + _cardsColorlessRareCount;

	public int CardsColorlessUncommonCount => _cardsColorlessUncommonCount;

	public int CardsColorlessRareCount => _cardsColorlessRareCount;

	public int RelicsCount => _relicsShopCount + _relicsNoneShopCount;

	public int RelicsShopCount => _relicsShopCount;

	public int RelicsNoneShopCount => _relicsNoneShopCount;

	public int PotionsCount => _potionsCommonCount + _potionsUncommonCount + _potionsRareCount;

	public int PotionsCommonCount => _potionsCommonCount;

	public int PotionsUncommonCount => _potionsUncommonCount;

	public int PotionsRareCount => _potionsRareCount;

	public int RemovalSerivceCost => _firstUseCost;

	public int RemovelServiceExtraCostPerUsing => _extraCostPerUsing;

	public int GetColoredCardCost(RarityType rarityType, IRandomGenerator randomManager)
	{
		return rarityType switch
		{
			RarityType.common => randomManager.GetRandom(_cardColoredCommonCost), 
			RarityType.uncommon => randomManager.GetRandom(_cardColoredUncommonCost), 
			RarityType.rare => randomManager.GetRandom(_cardColoredRareCost), 
			_ => throw new InvalidOperationException($"Rarity {rarityType} dose not handled in this scope!"), 
		};
	}

	public float GetCardRarityChance(RarityType rarityType)
	{
		if (_cardRarityChances.TryGetValue(rarityType, out var chance))
		{
			return chance;
		}
		throw new MissingDataException($"Rariry:{rarityType} not defined!");
	}

	public int GetColorlessCardCost(RarityType rarityType, IRandomGenerator randomManager)
	{
		return rarityType switch
		{
			RarityType.uncommon => randomManager.GetRandom(_cardColorlessUncommonCost), 
			RarityType.rare => randomManager.GetRandom(_cardColorlessRareCost), 
			_ => throw new InvalidOperationException($"Rarity {rarityType} dose not handled in this scope!"), 
		};
	}

	public int GetRelicCost(RarityType rarityType, IRandomGenerator randomManager)
	{
		switch (rarityType)
		{
		case RarityType.common:
		case RarityType.special:
			return randomManager.GetRandom(_relicCommonCost);
		case RarityType.uncommon:
			return randomManager.GetRandom(_relicUncommonCost);
		case RarityType.rare:
			return randomManager.GetRandom(_relicRareCost);
		case RarityType.shop:
			return randomManager.GetRandom(_relicShopCost);
		default:
			throw new InvalidOperationException($"Rarity {rarityType} dose not handled in this scope!");
		}
	}

	public int GetPotionCost(RarityType rarityType, IRandomGenerator randomManager)
	{
		return rarityType switch
		{
			RarityType.common => randomManager.GetRandom(_potionCommonCost), 
			RarityType.uncommon => randomManager.GetRandom(_potionUncommonCost), 
			RarityType.rare => randomManager.GetRandom(_potionRareCost), 
			_ => throw new InvalidOperationException($"Rarity {rarityType} dose not handled in this scope!"), 
		};
	}
}
