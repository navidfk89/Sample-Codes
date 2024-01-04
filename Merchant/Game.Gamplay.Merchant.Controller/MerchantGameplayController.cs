using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Game.Cards;
using Game.Cards.Models;
using Game.Gamplay.Merchant.Models;
using Game.Gamplay.Profile;
using Game.Gamplay.Views;
using Game.Potions.Models;
using Game.Random;
using Game.Relics;
using Game.Relics.Controllers;
using Game.Relics.Models;
using UnityEngine;

namespace Game.Gamplay.Merchant;

internal class MerchantGameplayController : AbstractGameplayController, IMerchantGameplayController, IGameplayController
{
	private IMerchantGameplayView _view;

	private IMerchantData _config;

	private CardInShopData[] _coloredCards;

	private CardInShopData[] _colorlessCards;

	private RelicInShopData[] _relics;

	private PotionInShopData[] _potions;

	private bool _replacingItemEnable;

	private Dictionary<RarityType, float> _rarityChances;

	private IRandomGenerator _randomGenerator;

	private IGameplayProfileManager _gameplayProfile;

	private IGameplayCardInventory _gameplayCardInventory;

	private IGameplayRelicInventory _gameplayRelicInventory;

	private IGameplayPotionInventory _gameplayPotionInventory;

	public override GameplayType GameplayType => GameplayType.merchant;

	public IEnumerable<ICardInShopData> ColoredCards => _coloredCards;

	public IEnumerable<ICardInShopData> ColorlessCards => _colorlessCards;

	public IEnumerable<IRelicInShopData> Relics => _relics;

	public IEnumerable<IPotionInShopData> Potions => _potions;

	public bool RemovalServiceEnable { get; private set; }

	public int RemovalServiceCost { get; private set; }

	public MerchantGameplayController(IMerchantData config, IMerchantGameplayView view, IGameplayManager gameplayManager)
		: base(gameplayManager)
	{
		_view = view;
		_config = config;
		ResolveDependencies();
	}

	public override async Task Prepare()
	{
		CreateCardRarityRoll();
		InitializeColoredCards();
		InitializeColorelessCards();
		InitializeRelics();
		InitializePotions();
		InitializeRemovalService();
		_view.Prepare(this);
		ApplyDiscount();
		await Task.CompletedTask;
	}

	public override async Task Start()
	{
		await Task.CompletedTask;
	}

	protected override async Task Finish()
	{
		Terminate();
		await Task.CompletedTask;
	}

	public override void Terminate()
	{
	}

	public bool TryPurchase(ICardInShopData card)
	{
		if (card.Cost > _gameplayProfile.Profile.Gold)
		{
			_view.ShowDialog("You don't have enough gold!");
			return false;
		}
		_gameplayCardInventory.AddCardToDeck(card.StaticData.Type, ObtainType.Purchased);
		_gameplayProfile.ReduceGold(card.Cost, GoldSourceType.Shop);
		if (_replacingItemEnable)
		{
			ICardInShopData newCard = ReplaceCard(card);
			_view.ReplaceCard(card, newCard);
		}
		else
		{
			_view.ShowCardSoldOut(card);
		}
		return true;
	}

	public bool TryPurchase(IRelicInShopData relic)
	{
		if (relic.Cost > _gameplayProfile.Profile.Gold)
		{
			_view.ShowDialog("You hava not enough gold!");
			return false;
		}
		_gameplayRelicInventory.AddRelic(relic.StaticData.Type, ObtainType.Purchased);
		_gameplayProfile.ReduceGold(relic.Cost, GoldSourceType.Shop);
		if (_replacingItemEnable)
		{
			IRelicInShopData newRelic = ReplaceRelic(relic);
			_view.ReplaceRelic(relic, newRelic);
		}
		else
		{
			_view.ShowRelicSoldOut(relic);
		}
		RelicType type = relic.StaticData.Type;
		RelicType relicType = type;
		if (relicType == RelicType.SmilingMask || relicType == RelicType.TheCourier || relicType == RelicType.MembershipCard)
		{
			ApplyDiscount();
		}
		return true;
	}

	public bool TryPurchase(IPotionInShopData potion)
	{
		if (potion.Cost > _gameplayProfile.Profile.Gold)
		{
			_view.ShowDialog("You hava not enough gold!");
			return false;
		}
		if (!_gameplayPotionInventory.HasEmptySlot)
		{
			_view.ShowDialog("All potion slots are full!");
			return false;
		}
		_gameplayPotionInventory.AddPotion(potion.StaticData.Type, ObtainType.Purchased);
		_gameplayProfile.ReduceGold(potion.Cost, GoldSourceType.Shop);
		if (_replacingItemEnable)
		{
			IPotionInShopData newPotion = ReplacePotion(potion);
			_view.ReplacePotion(potion, newPotion);
		}
		else
		{
			_view.ShowPotionSoldOut(potion);
		}
		return true;
	}

	public bool TryUseRemovalService(Guid cardInDeckId)
	{
		if (!RemovalServiceEnable)
		{
			_view.ShowDialog("Removal service in unavailable!");
			return false;
		}
		if (RemovalServiceCost > _gameplayProfile.Profile.Gold)
		{
			_view.ShowDialog("You hava not enough gold!");
			return false;
		}
		_gameplayProfile.ReduceGold(RemovalServiceCost, GoldSourceType.Shop);
		_gameplayProfile.LogUseRemovalService();
		_gameplayCardInventory.RemoveCardFromDeck(cardInDeckId);
		_view.ShowRemovalServiceSoldOut();
		return true;
	}

	private void ResolveDependencies()
	{
		_randomGenerator = ServiceLocator.Resolve<IRandomManager>().GetRandomGenerator(RandomType.Merchant);
		_gameplayProfile = ServiceLocator.Resolve<IGameplayProfileManager>();
		_gameplayCardInventory = ServiceLocator.Resolve<IGameplayCardInventory>();
		_gameplayRelicInventory = ServiceLocator.Resolve<IGameplayRelicInventory>();
		_gameplayPotionInventory = ServiceLocator.Resolve<IGameplayPotionInventory>();
	}

	private void CreateCardRarityRoll()
	{
		_rarityChances = new Dictionary<RarityType, float>
		{
			{
				RarityType.common,
				_config.GetCardRarityChance(RarityType.common)
			},
			{
				RarityType.uncommon,
				_config.GetCardRarityChance(RarityType.uncommon)
			},
			{
				RarityType.rare,
				_config.GetCardRarityChance(RarityType.rare)
			}
		};
	}

	private void InitializeColoredCards()
	{
		int totalCount = _config.CardsColoredCount;
		_coloredCards = new CardInShopData[totalCount];
		int index = 0;
		int attackCardsCount = _config.CardsColoredAttackCount;
		for (int k = 0; k < attackCardsCount; k++)
		{
			_coloredCards[index++] = CreateColoredCard(CardCategory.hit);
		}
		int skillCardsCount = _config.CardsColoredSkillCount;
		for (int j = 0; j < skillCardsCount; j++)
		{
			_coloredCards[index++] = CreateColoredCard(CardCategory.mastery);
		}
		int powerCardsCount = _config.CardsColoredPowerCount;
		for (int i = 0; i < powerCardsCount; i++)
		{
			_coloredCards[index++] = CreateColoredCard(CardCategory.passive);
		}
		int randomOffIndex = _randomGenerator.GetRandom(0, totalCount);
		int offCost = Mathf.RoundToInt((float)_coloredCards[randomOffIndex].Cost * _config.CardColoredOffPercent);
		_coloredCards[randomOffIndex].ApplyOff(offCost);
	}

	private void InitializeColorelessCards()
	{
		int totalCount = _config.CardsColorlessCount;
		_colorlessCards = new CardInShopData[totalCount];
		int index = 0;
		int uncommonCardsCount = _config.CardsColorlessUncommonCount;
		for (int j = 0; j < uncommonCardsCount; j++)
		{
			_colorlessCards[index++] = CreateColorlessCard(RarityType.uncommon);
		}
		int rareCardsCount = _config.CardsColorlessRareCount;
		for (int i = 0; i < rareCardsCount; i++)
		{
			_colorlessCards[index++] = CreateColorlessCard(RarityType.rare);
		}
	}

	private void InitializeRelics()
	{
		int totalCount = _config.RelicsCount;
		_relics = new RelicInShopData[totalCount];
		int index = 0;
		int noneShopRelicsCount = _config.RelicsNoneShopCount;
		for (int j = 0; j < noneShopRelicsCount; j++)
		{
			_relics[index++] = CreateRelic(shopRarity: false);
		}
		int shopRelicsCount = _config.RelicsShopCount;
		for (int i = 0; i < shopRelicsCount; i++)
		{
			_relics[index++] = CreateRelic(shopRarity: true);
		}
	}

	private void InitializePotions()
	{
		int totalCount = _config.PotionsCount;
		_potions = new PotionInShopData[totalCount];
		int index = 0;
		int commonPotionsCount = _config.PotionsCommonCount;
		for (int k = 0; k < commonPotionsCount; k++)
		{
			_potions[index++] = CreatePotion(RarityType.common);
		}
		int uncommonPotionsCount = _config.PotionsUncommonCount;
		for (int j = 0; j < uncommonPotionsCount; j++)
		{
			_potions[index++] = CreatePotion(RarityType.uncommon);
		}
		int rarePotionsCount = _config.PotionsRareCount;
		for (int i = 0; i < rarePotionsCount; i++)
		{
			_potions[index++] = CreatePotion(RarityType.rare);
		}
	}

	private void InitializeRemovalService()
	{
		RemovalServiceEnable = true;
		int extraCost = _gameplayProfile.Profile.RemovalServiceUsedCount * _config.RemovelServiceExtraCostPerUsing;
		RemovalServiceCost = _config.RemovalSerivceCost + extraCost;
	}

	private RarityType CardRariryRoll(IRandomGenerator randomGenerator)
	{
		return randomGenerator.GetRandomObject(_rarityChances);
	}

	private CardInShopData CreateColoredCard(CardCategory category)
	{
		ICardData cardData;
		do
		{
			cardData = _gameplayCardInventory.GetRandomCard(category, _randomGenerator, CardRariryRoll);
		}
		while (cardData == null || HasCardInOffer(cardData, _coloredCards));
		int randomCost = _config.GetColoredCardCost(cardData.Rarity, _randomGenerator);
		return new CardInShopData(cardData, randomCost);
	}

	private CardInShopData CreateColorlessCard(RarityType? speceficRarity = null)
	{
		ICardData cardData;
		do
		{
			RarityType rarity = ((!speceficRarity.HasValue) ? CardRariryRoll(_randomGenerator) : speceficRarity.Value);
			cardData = _gameplayCardInventory.GetRandomColorlessCard(rarity, _randomGenerator);
		}
		while ((cardData == null && speceficRarity.HasValue) || HasCardInOffer(cardData, _colorlessCards));
		int randomCost = _config.GetColorlessCardCost(cardData.Rarity, _randomGenerator);
		return new CardInShopData(cardData, randomCost);
	}

	private RelicInShopData CreateRelic(bool shopRarity)
	{
		IRelicData relicData;
		do
		{
			relicData = ((!shopRarity) ? _gameplayRelicInventory.GetRandomRelic() : _gameplayRelicInventory.GetRandomShopRelic());
		}
		while (HasRelicInOffer(relicData));
		int randomCost = _config.GetRelicCost(relicData.Rarity, _randomGenerator);
		return new RelicInShopData(relicData, randomCost);
	}

	private PotionInShopData CreatePotion(RarityType rarity)
	{
		IPotionData potionData = _gameplayPotionInventory.GetRandomPotion(rarity);
		int randomCost = _config.GetPotionCost(rarity, _randomGenerator);
		return new PotionInShopData(potionData, randomCost);
	}

	private bool HasCardInOffer(ICardData cardData, IEnumerable<CardInShopData> offersList)
	{
		foreach (CardInShopData card in offersList)
		{
			if (card == null || card.StaticData != cardData)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private bool HasRelicInOffer(IRelicData relicData)
	{
		RelicInShopData[] relics = _relics;
		foreach (RelicInShopData relic in relics)
		{
			if (relic != null && relic.StaticData == relicData)
			{
				return true;
			}
		}
		return false;
	}

	private void ApplyDiscount()
	{
		float discount = GetRelicsDiscountAffect();
		CardInShopData[] coloredCards = _coloredCards;
		foreach (CardInShopData card2 in coloredCards)
		{
			card2.ApplyDiscount(discount);
		}
		CardInShopData[] colorlessCards = _colorlessCards;
		foreach (CardInShopData card in colorlessCards)
		{
			card.ApplyDiscount(discount);
		}
		PotionInShopData[] potions = _potions;
		foreach (PotionInShopData potion in potions)
		{
			potion.ApplyDiscount(discount);
		}
		RelicInShopData[] relics = _relics;
		foreach (RelicInShopData relic in relics)
		{
			relic.ApplyDiscount(discount);
		}
		ApplyDiscountForRemovalService(discount);
		_view.UpdateGolds();
		_view.UpdateRemovalService();
	}

	private float GetRelicsDiscountAffect()
	{
		float discount = 1f;
		if (_gameplayRelicInventory.HasRelic(RelicType.MembershipCard))
		{
			MembershipCardRelicController membershipCard = _gameplayRelicInventory.GetRelic<MembershipCardRelicController>(RelicType.MembershipCard);
			float membershipCardDiscount = (float)membershipCard.GetDiscount() / 100f;
			discount *= 1f - membershipCardDiscount;
		}
		if (_gameplayRelicInventory.HasRelic(RelicType.TheCourier))
		{
			TheCourierRelicController theCourier = _gameplayRelicInventory.GetRelic<TheCourierRelicController>(RelicType.TheCourier);
			float theCourierDiscount = (float)theCourier.GetDiscount() / 100f;
			discount *= 1f - theCourierDiscount;
			_replacingItemEnable = true;
		}
		return discount;
	}

	private ICardInShopData ReplaceCard(ICardInShopData cardToRemove)
	{
		CardInShopData newCard;
		if (cardToRemove.StaticData.Class == ClassType.any)
		{
			newCard = CreateColorlessCard();
			ICardInShopData[] colorlessCards = _colorlessCards;
			colorlessCards.ReplaceElement(cardToRemove, newCard);
		}
		else
		{
			newCard = CreateColoredCard(cardToRemove.StaticData.Category);
			ICardInShopData[] colorlessCards = _coloredCards;
			colorlessCards.ReplaceElement(cardToRemove, newCard);
		}
		float discount = GetRelicsDiscountAffect();
		newCard.ApplyDiscount(discount);
		return newCard;
	}

	private IRelicInShopData ReplaceRelic(IRelicInShopData relicToRemove)
	{
		RelicInShopData newRelic = CreateRelic(relicToRemove.StaticData.Rarity == RarityType.shop);
		IRelicInShopData[] relics = _relics;
		relics.ReplaceElement(relicToRemove, newRelic);
		float discount = GetRelicsDiscountAffect();
		newRelic.ApplyDiscount(discount);
		return newRelic;
	}

	private IPotionInShopData ReplacePotion(IPotionInShopData potionToRemove)
	{
		PotionInShopData newPotion = CreatePotion(potionToRemove.StaticData.Rarity);
		IPotionInShopData[] potions = _potions;
		potions.ReplaceElement(potionToRemove, newPotion);
		float discount = GetRelicsDiscountAffect();
		newPotion.ApplyDiscount(discount);
		return newPotion;
	}

	private void ApplyDiscountForRemovalService(float discount)
	{
		int extraCost = _gameplayProfile.Profile.RemovalServiceUsedCount * _config.RemovelServiceExtraCostPerUsing;
		RemovalServiceCost = _config.RemovalSerivceCost + extraCost;
		int applyedDiscountCost = Mathf.RoundToInt((float)RemovalServiceCost * discount);
		RemovalServiceCost = applyedDiscountCost;
		if (_gameplayRelicInventory.HasRelic(RelicType.SmilingMask))
		{
			SmilingMaskRelicController smilingMask = _gameplayRelicInventory.GetRelic<SmilingMaskRelicController>(RelicType.SmilingMask);
			int smilingMaskDiscount = smilingMask.GetDiscount();
			if (RemovalServiceCost > smilingMaskDiscount)
			{
				RemovalServiceCost = smilingMaskDiscount;
			}
		}
	}
}
