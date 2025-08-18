using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.ModLoader;
using Terraria.ID;
using Microsoft.Xna.Framework.Graphics;
using Terraria.DataStructures;
using Terraria.Audio;

namespace Revelations
{
	public class Revelations : Mod
	{
		public static readonly SoundStyle ElectricEelCharge = new SoundStyle("Revelations/Content/Sounds/ElectricEelCharge") {
			Volume = 0.25f,
			PitchVariance = 0.2f,
			MaxInstances = 3,
		};
		public static int EternalEnchantmentBodySlot;
/*
		public override void Load()
		{
			if (!Main.dedServ)
			{
				EternalEnchantmentBodySlot = EquipLoader.AddEquipTexture(
					mod: this,
					texture: "Revelations/Content/Costumes/EternalEnchantment_Body",
					type: EquipType.Body,
					item: null,
					name: "EternalEnchantment_Body",
					animation: new DrawAnimationVertical(5, 4)
				);
			}
		}
		*/
	}
}

