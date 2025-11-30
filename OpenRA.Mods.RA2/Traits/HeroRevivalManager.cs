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

using System.Collections.Generic;
using System.Linq;
using OpenRA.Mods.Common;
using OpenRA.Mods.Common.Traits;
using OpenRA.Primitives;
using OpenRA.Traits;

namespace OpenRA.Mods.RA2.Traits
{
	public class HeroRevivalItem
	{
		public readonly Actor Hero;
		public readonly int RevivalCost;
		public readonly int RevivalTime;
		public int RemainingTime;
		public bool IsReviving;

		public HeroRevivalItem(Actor hero, int cost, int time)
		{
			Hero = hero;
			RevivalCost = cost;
			RevivalTime = time;
			RemainingTime = time;
			IsReviving = false;
		}
	}

	[Desc("Manages hero revival queue for hero altar buildings.",
		"Allows fallen heroes to be revived for a cost and time.")]
	public class HeroRevivalManagerInfo : ConditionalTraitInfo, Requires<ProductionInfo>, Requires<ExitInfo>
	{
		[Desc("Base cost to revive a hero.")]
		public readonly int BaseRevivalCost = 500;

		[Desc("Additional cost per hero level.")]
		public readonly int CostPerLevel = 100;

		[Desc("Base time (in ticks) to revive a hero. 25 ticks = 1 second.")]
		public readonly int BaseRevivalTime = 1500; // 60 seconds

		[Desc("Sound to play when revival starts.")]
		public readonly string RevivalSound = null;

		[NotificationReference("Speech")]
		[Desc("Notification to play when revival completes.",
			"The filename of the audio is defined per faction in notifications.yaml.")]
		public readonly string RevivalCompleteNotification = null;

		[Desc("Text notification to display when revival completes.")]
		public readonly string RevivalCompleteTextNotification = null;

		[NotificationReference("Speech")]
		[Desc("Notification to play when player doesn't have enough money for revival.",
			"The filename of the audio is defined per faction in notifications.yaml.")]
		public readonly string InsufficientFundsNotification = null;

		[Desc("Text notification to display when player doesn't have enough money for revival.")]
		public readonly string InsufficientFundsTextNotification = null;

		[Desc("Production queue type used for spawning heroes (e.g. 'Hero.Allied', 'Hero.Soviet').")]
		public readonly string ProductionType = "Hero";

		public override object Create(ActorInitializer init) { return new HeroRevivalManager(init.Self, this); }
	}

	public class HeroRevivalManager : ConditionalTrait<HeroRevivalManagerInfo>, ITick, IResolveOrder, INotifyKilled, INotifyOwnerChanged
	{
		readonly Actor self;
		readonly List<HeroRevivalItem> revivalQueue = new();
		PlayerResources playerResources;
		Production[] productionTraits;

		public IEnumerable<HeroRevivalItem> RevivalQueue => revivalQueue;

		public HeroRevivalManager(Actor self, HeroRevivalManagerInfo info)
			: base(info)
		{
			this.self = self;
		}

		protected override void Created(Actor self)
		{
			playerResources = self.Owner.PlayerActor.Trait<PlayerResources>();
			productionTraits = self.TraitsImplementing<Production>()
				.Where(p => p.Info.Produces.Contains(Info.ProductionType))
				.ToArray();

			base.Created(self);
		}

		public void AddHeroToQueue(Actor hero)
		{
			if (IsTraitDisabled || hero == null || hero.IsDead)
				return;

			// Don't add if already in queue
			if (revivalQueue.Any(item => item.Hero == hero))
				return;

			// Calculate revival cost based on hero level
			var gainExperience = hero.TraitOrDefault<GainsExperience>();
			var heroLevel = gainExperience != null ? gainExperience.Level : 1;
			var cost = Info.BaseRevivalCost + (heroLevel * Info.CostPerLevel);

			var revivalItem = new HeroRevivalItem(hero, cost, Info.BaseRevivalTime);
			revivalQueue.Add(revivalItem);
		}

		void ITick.Tick(Actor self)
		{
			if (IsTraitDisabled || revivalQueue.Count == 0)
				return;

			var currentItem = revivalQueue[0];

			// If not yet reviving, try to start revival
			if (!currentItem.IsReviving)
			{
				// Check if player can afford revival
				if (!playerResources.TakeCash(currentItem.RevivalCost, true))
				{
					// Can't afford - notify player and wait
					if (!string.IsNullOrEmpty(Info.InsufficientFundsNotification))
					{
						Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech",
							Info.InsufficientFundsNotification, self.Owner.Faction.InternalName);
					}

					if (!string.IsNullOrEmpty(Info.InsufficientFundsTextNotification))
					{
						TextNotificationsManager.AddTransientLine(Info.InsufficientFundsTextNotification, self.Owner);
					}

					return;
				}

				// Start revival
				currentItem.IsReviving = true;

				// Play revival sound
				if (!string.IsNullOrEmpty(Info.RevivalSound))
					Game.Sound.Play(SoundType.World, Info.RevivalSound, self.CenterPosition);
			}

			// Process revival time
			if (currentItem.IsReviving)
			{
				currentItem.RemainingTime--;

				if (currentItem.RemainingTime <= 0)
				{
					// Revival complete - spawn hero
					ReviveHero(currentItem);
					revivalQueue.RemoveAt(0);
				}
			}
		}

		void ReviveHero(HeroRevivalItem item)
		{
			var hero = item.Hero;

			if (hero.Disposed || self.IsDead)
				return;

			// Find an exit point
			var productionTrait = productionTraits.FirstOrDefault(p => !p.IsTraitDisabled && !p.IsTraitPaused);
			if (productionTrait == null)
				return;

			// Get exit information
			var exit = self.RandomExitOrDefault(self.World, Info.ProductionType);
			if (exit == null)
				return;

			var exitCell = self.Location + exit.Info.ExitCell;
			var spawn = self.CenterPosition + exit.Info.SpawnOffset;
			var to = self.World.Map.CenterOfCell(exitCell);

			// Calculate facing
			WAngle initialFacing;
			if (!exit.Info.Facing.HasValue)
			{
				var delta = to - spawn;
				if (delta.HorizontalLengthSquared == 0)
					initialFacing = WAngle.Zero;
				else
					initialFacing = delta.Yaw;
			}
			else
				initialFacing = exit.Info.Facing.Value;

			// Prepare initialization dictionary
			var td = new TypeDictionary
			{
				new OwnerInit(self.Owner),
				new LocationInit(exitCell),
				new CenterPositionInit(spawn),
				new FacingInit(initialFacing)
			};

			// Restore hero health to full
			self.World.AddFrameEndTask(w =>
			{
				if (hero.Disposed)
					return;

				// Revoke fallen condition and restore hero
				var deathHandler = hero.TraitOrDefault<HeroDeathHandler>();
				if (deathHandler != null)
					deathHandler.OnRevived();

				// Restore hero health
				var health = hero.TraitOrDefault<Health>();
				if (health != null)
					health.InflictDamage(hero, hero, new Damage(-health.MaxHP), true);

				// Move hero to exit location
				hero.Trait<IPositionable>().SetPosition(hero, exitCell);
				hero.Trait<IPositionable>().SetCenterPosition(hero, spawn);

				var facing = hero.TraitOrDefault<IFacing>();
				if (facing != null)
					facing.Facing = initialFacing;

				// Play completion notification
				if (!string.IsNullOrEmpty(Info.RevivalCompleteNotification))
				{
					Game.Sound.PlayNotification(self.World.Map.Rules, self.Owner, "Speech",
						Info.RevivalCompleteNotification, self.Owner.Faction.InternalName);
				}

				if (!string.IsNullOrEmpty(Info.RevivalCompleteTextNotification))
				{
					TextNotificationsManager.AddTransientLine(Info.RevivalCompleteTextNotification, self.Owner);
				}
			});
		}

		void IResolveOrder.ResolveOrder(Actor self, Order order)
		{
			// Future enhancement: Could add manual revival triggering or queue management
		}

		void INotifyKilled.Killed(Actor self, AttackInfo e)
		{
			// If altar is destroyed, heroes in queue are lost
			// Could be enhanced to transfer to another altar
			revivalQueue.Clear();
		}

		void INotifyOwnerChanged.OnOwnerChanged(Actor self, Player oldOwner, Player newOwner)
		{
			// Clear queue on ownership change
			revivalQueue.Clear();

			// Update player resources reference
			playerResources = newOwner.PlayerActor.Trait<PlayerResources>();
		}

		/// <summary>
		/// Get revival progress for UI (0.0 to 1.0)
		/// </summary>
		public float GetRevivalProgress()
		{
			if (revivalQueue.Count == 0)
				return 0f;

			var currentItem = revivalQueue[0];
			if (!currentItem.IsReviving)
				return 0f;

			return 1f - ((float)currentItem.RemainingTime / currentItem.RevivalTime);
		}

		/// <summary>
		/// Get the current hero being revived, or null
		/// </summary>
		public Actor GetCurrentRevivingHero()
		{
			return revivalQueue.Count > 0 ? revivalQueue[0].Hero : null;
		}
	}
}
