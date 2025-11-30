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

using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Handles hero death, marking hero as fallen instead of killed.",
		"Fallen heroes can be revived at their faction's hero altar.")]
	public class HeroDeathHandlerInfo : ConditionalTraitInfo, Requires<HeroInfoInfo>
	{
		[NotificationReference("Speech")]
		[Desc("Notification to play when hero falls.",
			"The filename of the audio is defined per faction in notifications.yaml.")]
		public readonly string FallenNotification = null;

		[Desc("Text notification to display when hero falls.")]
		public readonly string FallenTextNotification = null;

		[Desc("Condition to grant when hero is fallen (makes them invisible/inactive).")]
		[GrantedConditionReference]
		public readonly string FallenCondition = "hero_fallen";

		[Desc("Actor types of hero altars that can revive this hero.",
			"The handler will search for any of these altar types owned by the hero's player.")]
		public readonly string[] AltarActorTypes = { "gahero", "nahero" };

		public override object Create(ActorInitializer init) { return new HeroDeathHandler(init.Self, this); }
	}

	public class HeroDeathHandler : ConditionalTrait<HeroDeathHandlerInfo>, INotifyKilled
	{
		readonly Actor self;
		int fallenToken = Actor.InvalidConditionToken;

		public bool IsFallen { get; private set; }

		public HeroDeathHandler(Actor self, HeroDeathHandlerInfo info)
			: base(info)
		{
			this.self = self;
		}

		void INotifyKilled.Killed(Actor self, AttackInfo e)
		{
			// Don't handle if trait is disabled or hero is already fallen
			if (IsTraitDisabled || IsFallen)
				return;

			// Mark hero as fallen
			IsFallen = true;

			// Grant fallen condition to make hero invisible/inactive
			if (!string.IsNullOrEmpty(Info.FallenCondition))
				fallenToken = self.GrantCondition(Info.FallenCondition);

			// Find player's hero altar
			var altar = self.World.ActorsHavingTrait<HeroRevivalManager>()
				.FirstOrDefault(a => a.Owner == self.Owner &&
					Info.AltarActorTypes.Contains(a.Info.Name) &&
					!a.IsDead &&
					!a.Disposed);

			if (altar != null)
			{
				// Add hero to altar's revival queue
				var revivalManager = altar.Trait<HeroRevivalManager>();
				revivalManager.AddHeroToQueue(self);
			}
			else
			{
				// No altar available - hero is truly lost
				// Future enhancement: Could add a warning notification here
			}

			// Play notification
			if (!string.IsNullOrEmpty(Info.FallenNotification))
			{
				Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech",
					Info.FallenNotification, self.Owner.Faction.InternalName);
			}

			if (!string.IsNullOrEmpty(Info.FallenTextNotification))
			{
				TextNotificationsManager.AddTransientLine(Info.FallenTextNotification, self.Owner);
			}
		}

		/// <summary>
		/// Called by HeroRevivalManager when hero is revived
		/// </summary>
		public void OnRevived()
		{
			IsFallen = false;

			// Revoke fallen condition
			if (fallenToken != Actor.InvalidConditionToken)
				fallenToken = self.RevokeCondition(fallenToken);
		}

		protected override void TraitDisabled(Actor self)
		{
			if (IsFallen && fallenToken != Actor.InvalidConditionToken)
				fallenToken = self.RevokeCondition(fallenToken);

			IsFallen = false;
		}
	}
}
