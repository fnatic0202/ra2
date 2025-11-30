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

using System;
using System.Linq;
using OpenRA.Mods.Common.Traits;
using OpenRA.Mods.Common.Widgets;
using OpenRA.Primitives;
using OpenRA.Traits;
using OpenRA.Widgets;

namespace OpenRA.Mods.RA2.Widgets.Logic
{
	public class DevDebugPanelLogic : ChromeLogic
	{
		readonly World world;
		Actor selectedActor;
		GainsExperience gainsExperience;
		Health health;
		int selectionHash;

		[ObjectCreator.UseCtor]
		public DevDebugPanelLogic(Widget widget, World world)
		{
			this.world = world;

			var closeButton = widget.Get<ButtonWidget>("CLOSE_BUTTON");
			closeButton.OnClick = () => widget.Visible = false;

			// Unit info display
			var unitNameLabel = widget.Get<LabelWidget>("UNIT_NAME");
			var unitLevelLabel = widget.Get<LabelWidget>("UNIT_LEVEL");
			var unitHealthLabel = widget.Get<LabelWidget>("UNIT_HEALTH");

			unitNameLabel.GetText = () =>
			{
				UpdateSelection();
				return selectedActor != null ? selectedActor.Info.Name : "No Unit Selected";
			};

			unitLevelLabel.GetText = () =>
			{
				if (gainsExperience != null)
					return $"Level: {gainsExperience.Level}/{gainsExperience.MaxLevel}";
				return "Level: N/A";
			};

			unitHealthLabel.GetText = () =>
			{
				if (health != null)
					return $"Health: {health.HP}/{health.MaxHP}";
				return "Health: N/A";
			};

			// Veterancy level buttons (0-10)
			SetupVeterancyButton(widget, "VET_LEVEL_0", 0);
			SetupVeterancyButton(widget, "VET_LEVEL_1", 1);
			SetupVeterancyButton(widget, "VET_LEVEL_2", 2);
			SetupVeterancyButton(widget, "VET_LEVEL_3", 3);
			SetupVeterancyButton(widget, "VET_LEVEL_4", 4);
			SetupVeterancyButton(widget, "VET_LEVEL_5", 5);
			SetupVeterancyButton(widget, "VET_LEVEL_6", 6);
			SetupVeterancyButton(widget, "VET_LEVEL_7", 7);
			SetupVeterancyButton(widget, "VET_LEVEL_8", 8);
			SetupVeterancyButton(widget, "VET_LEVEL_9", 9);
			SetupVeterancyButton(widget, "VET_LEVEL_10", 10);

			// Health control buttons
			var healFullButton = widget.Get<ButtonWidget>("HEAL_FULL");
			healFullButton.OnClick = () =>
			{
				if (health != null && selectedActor != null && !selectedActor.IsDead)
				{
					var damage = health.MaxHP - health.HP;
					if (damage > 0)
						selectedActor.InflictDamage(selectedActor, new Damage(-damage));
				}
			};
			healFullButton.IsDisabled = () => health == null || selectedActor == null || selectedActor.IsDead;

			var damageHalfButton = widget.Get<ButtonWidget>("DAMAGE_HALF");
			damageHalfButton.OnClick = () =>
			{
				if (health != null && selectedActor != null && !selectedActor.IsDead)
				{
					var damage = health.MaxHP / 2;
					selectedActor.InflictDamage(selectedActor, new Damage(damage));
				}
			};
			damageHalfButton.IsDisabled = () => health == null || selectedActor == null || selectedActor.IsDead;

			var killButton = widget.Get<ButtonWidget>("KILL_UNIT");
			killButton.OnClick = () =>
			{
				if (selectedActor != null && !selectedActor.IsDead)
					selectedActor.Kill(selectedActor);
			};
			killButton.IsDisabled = () => selectedActor == null || selectedActor.IsDead;

			// Attribute modifiers
			var giveFirepowerButton = widget.Get<ButtonWidget>("GIVE_FIREPOWER");
			giveFirepowerButton.OnClick = () =>
			{
				if (selectedActor != null && !selectedActor.IsDead)
				{
					// Note: Firepower is typically controlled via conditions/modifiers
					// This is a placeholder for future implementation
				}
			};
			giveFirepowerButton.IsDisabled = () => selectedActor == null || selectedActor.IsDead;

			var giveSpeedButton = widget.Get<ButtonWidget>("GIVE_SPEED");
			giveSpeedButton.OnClick = () =>
			{
				if (selectedActor != null && !selectedActor.IsDead)
				{
					// Note: Speed modification typically requires condition-based modifiers
					// This is a placeholder for future implementation
				}
			};
			giveSpeedButton.IsDisabled = () =>
			{
				if (selectedActor == null || selectedActor.IsDead)
					return true;
				return selectedActor.TraitOrDefault<Mobile>() == null;
			};

			// Reset attributes button
			var resetButton = widget.Get<ButtonWidget>("RESET_ATTRIBUTES");
			resetButton.OnClick = () =>
			{
				if (selectedActor != null && !selectedActor.IsDead)
				{
					// Reset to initial state
					if (health != null)
						selectedActor.InflictDamage(selectedActor, new Damage(-(health.MaxHP - health.HP)));
				}
			};
			resetButton.IsDisabled = () => selectedActor == null || selectedActor.IsDead;
		}

		void SetupVeterancyButton(Widget widget, string id, int targetLevel)
		{
			var button = widget.Get<ButtonWidget>(id);
			button.OnClick = () =>
			{
				if (gainsExperience != null && selectedActor != null && !selectedActor.IsDead)
				{
					var currentLevel = gainsExperience.Level;
					var levelDiff = targetLevel - currentLevel;

					if (levelDiff > 0)
					{
						// Grant levels
						world.IssueOrder(new Order("DevLevelUp", selectedActor, false) { ExtraData = (uint)levelDiff });
					}
					// Note: Can't reduce levels in current implementation
					// Would need to recreate the actor or implement level reduction
				}
			};

			button.IsDisabled = () =>
			{
				if (gainsExperience == null || selectedActor == null || selectedActor.IsDead)
					return true;

				// Disable if already at target level
				return gainsExperience.Level == targetLevel;
			};

			button.IsHighlighted = () =>
			{
				if (gainsExperience == null)
					return false;
				return gainsExperience.Level == targetLevel;
			};
		}

		void UpdateSelection()
		{
			if (selectionHash == world.Selection.Hash)
				return;

			selectionHash = world.Selection.Hash;
			selectedActor = world.Selection.Actors.FirstOrDefault();

			if (selectedActor != null && !selectedActor.IsDead)
			{
				gainsExperience = selectedActor.TraitOrDefault<GainsExperience>();
				health = selectedActor.TraitOrDefault<Health>();
			}
			else
			{
				gainsExperience = null;
				health = null;
			}
		}
	}
}
