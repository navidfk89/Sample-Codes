using Game.Random;

namespace Game.Gamplay.Merchant.Models;

public interface IMerchantData
{
	int CardsColoredCount { get; }

	float CardColoredOffPercent { get; }

	int CardsColoredAttackCount { get; }

	int CardsColoredSkillCount { get; }

	int CardsColoredPowerCount { get; }

	int CardsColorlessCount { get; }

	int CardsColorlessUncommonCount { get; }

	int CardsColorlessRareCount { get; }

	int RelicsCount { get; }

	int RelicsShopCount { get; }

	int RelicsNoneShopCount { get; }

	int PotionsCount { get; }

	int PotionsCommonCount { get; }

	int PotionsUncommonCount { get; }

	int PotionsRareCount { get; }

	int RemovalSerivceCost { get; }

	int RemovelServiceExtraCostPerUsing { get; }

	int GetColoredCardCost(RarityType rarityType, IRandomGenerator randomManager);

	float GetCardRarityChance(RarityType rarityType);

	int GetColorlessCardCost(RarityType rarityType, IRandomGenerator randomManager);

	int GetRelicCost(RarityType rarityType, IRandomGenerator randomManager);

	int GetPotionCost(RarityType rarityType, IRandomGenerator randomManager);
}
