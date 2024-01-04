using System;
using System.Collections.Generic;
using Game.Cards.Models;
using Game.Potions.Models;
using Game.Relics.Models;

namespace Game.Gamplay.Merchant;

public interface IMerchantGameplayController : IGameplayController
{
	IEnumerable<ICardInShopData> ColoredCards { get; }

	IEnumerable<ICardInShopData> ColorlessCards { get; }

	IEnumerable<IRelicInShopData> Relics { get; }

	IEnumerable<IPotionInShopData> Potions { get; }

	bool RemovalServiceEnable { get; }

	int RemovalServiceCost { get; }

	bool TryPurchase(ICardInShopData card);

	bool TryPurchase(IRelicInShopData relic);

	bool TryPurchase(IPotionInShopData potion);

	bool TryUseRemovalService(Guid cardInDeckId);
}
