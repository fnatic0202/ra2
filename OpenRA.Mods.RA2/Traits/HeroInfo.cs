#region Copyright & License Information
/*
 * Copyright (c) The OpenRA Developers and Contributors
 * This file is part of OpenRA, which is free software. It is made
 * available to you under the terms of the GNU General Public License
 * as published by the Free Software Foundation, either version 3 of
 * the License, or (at your option) any later version. For more
 * information, see COPYING.
 */
#endregion

using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Provides hero metadata and information.",
		"Used to identify heroes and provide additional UI information.")]
	public class HeroInfoInfo : TraitInfo
	{
		[Desc("Short hero name (e.g., 'Tanya', 'Yuri').")]
		public readonly string ShortName = "Hero";

		[Desc("Hero title or role (e.g., 'Elite Commando', 'Psychic Master').")]
		public readonly string Title = "Hero";

		[Desc("Hero description.")]
		public readonly string Description = "A powerful hero unit.";

		[Desc("Hero role classification (e.g., 'Assassin', 'Tank', 'Support', 'Artillery', 'Controller').")]
		public readonly string Role = "DPS";

		[Desc("Difficulty rating for players (e.g., 'Easy', 'Medium', 'Hard', 'Very Hard').")]
		public readonly string Difficulty = "Medium";

		[Desc("Is this unit a hero? Used for quick identification.")]
		public readonly bool IsHero = true;

		public override object Create(ActorInitializer init) { return new HeroInfo(this); }
	}

	public class HeroInfo
	{
		public readonly HeroInfoInfo Info;

		public HeroInfo(HeroInfoInfo info)
		{
			Info = info;
		}

		public string GetShortName() => Info.ShortName;
		public string GetTitle() => Info.Title;
		public string GetDescription() => Info.Description;
		public string GetRole() => Info.Role;
		public string GetDifficulty() => Info.Difficulty;
		public bool IsHeroUnit() => Info.IsHero;
	}
}
