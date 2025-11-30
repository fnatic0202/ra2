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
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	[Desc("Monitors production limits and provides notifications when limits are reached.",
		"Note: Actual limits are enforced via BuildableInfo.BuildLimit - this trait only provides player feedback.",
		"Used to notify players when hero limit is reached (e.g., max 3 heroes).")]
	public class ProductionLimitMonitorInfo : ConditionalTraitInfo
	{
		[Desc("Production queue types to monitor (e.g., 'Hero.Allied', 'Hero.Soviet').")]
		public readonly string[] MonitoredTypes = { };

		[NotificationReference("Speech")]
		[Desc("Notification to play when limit is reached.",
			"The filename of the audio is defined per faction in notifications.yaml.")]
		public readonly string LimitReachedNotification = null;

		[Desc("Text notification to display when limit is reached.")]
		public readonly string LimitReachedTextNotification = null;

		public override object Create(ActorInitializer init) { return new ProductionLimitMonitor(init.Self, this); }
	}

	public class ProductionLimitMonitor : ConditionalTrait<ProductionLimitMonitorInfo>, INotifyOtherProduction
	{
		readonly Actor self;
		bool hasWarned = false;

		public ProductionLimitMonitor(Actor self, ProductionLimitMonitorInfo info)
			: base(info)
		{
			this.self = self;
		}

		void INotifyOtherProduction.UnitProducedByOther(Actor self, Actor producer, Actor produced, string productionType, TypeDictionary init)
		{
			// Reset warning when a unit is produced (player will need to be warned again if they try to produce more)
			hasWarned = false;
		}

		/// <summary>
		/// Check if production limit would be reached and notify player
		/// Called externally when production is attempted
		/// </summary>
		public bool CheckLimitAndNotify(string productionType, string actorType)
		{
			if (IsTraitDisabled)
				return false;

			// Check if we're monitoring this production type
			if (Info.MonitoredTypes.Length > 0 && !Info.MonitoredTypes.Contains(productionType))
				return false;

			// Check if limit would be reached
			var actorInfo = self.World.Map.Rules.Actors[actorType];
			var buildable = actorInfo.TraitInfo<BuildableInfo>();

			if (buildable.BuildLimit <= 0)
				return false; // No limit

			var inQueue = self.Owner.PlayerActor.TraitsImplementing<ProductionQueue>()
				.SelectMany(q => q.AllQueued())
				.Count(pi => pi.Item == actorType);

			var owned = self.World.ActorsHavingTrait<Buildable>()
				.Count(a => a.Info.Name == actorType && a.Owner == self.Owner && !a.IsDead);

			var wouldExceedLimit = (inQueue + owned) >= buildable.BuildLimit;

			if (wouldExceedLimit && !hasWarned)
			{
				hasWarned = true;

				// Play limit reached notification
				if (!string.IsNullOrEmpty(Info.LimitReachedNotification))
				{
					Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech",
						Info.LimitReachedNotification, self.Owner.Faction.InternalName);
				}

				if (!string.IsNullOrEmpty(Info.LimitReachedTextNotification))
				{
					TextNotificationsManager.AddTransientLine(Info.LimitReachedTextNotification, self.Owner);
				}
			}

			return wouldExceedLimit;
		}
	}
}
