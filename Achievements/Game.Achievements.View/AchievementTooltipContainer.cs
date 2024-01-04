using System.Collections.Generic;
using Game.Achievements.Models;
using Game.Tooltips.Controllers;
using Game.Tooltips.Models;
using Game.Tooltips.Views;

namespace Game.Achievements.View;

public class AchievementTooltipContainer : AbstractToolTipContainer
{
	private IAchievementData _data;

	public void SetData(IAchievementData data)
	{
		_data = data;
	}

	protected override IEnumerable<IToolTipData> PrepareToolTipData()
	{
		return new ToolTipData[1]
		{
			new ToolTipData(_data.Name, _data.ParameterEffect.Description)
		};
	}

	public override TipViewData GetTipViewData()
	{
		return new TipViewData(_tipTransform, _rectTransform, followMouse: false, followTarget: true, isDown: true);
	}
}
