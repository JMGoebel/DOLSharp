/*
 * DAWN OF LIGHT - The first free open source DAoC server emulator
 * 
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; either version 2
 * of the License, or (at your option) any later version.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place - Suite 330, Boston, MA  02111-1307, USA.
 *
 */
using System;
using System.Collections;
using System.Reflection;
using DOL.Events;
using DOL.GS.PacketHandler;
using log4net;

namespace DOL.GS.Trainer
{
	/// <summary>
	/// Bainshee Trainer
	/// </summary>	
	[NPCGuildScript("Bainshee Trainer", eRealm.Hibernia)]		// this attribute instructs DOL to use this script for all "Bainshee Trainer" NPC's in Albion (multiple guilds are possible for one script)
	public class BainsheeTrainer : GameTrainer
	{
		/// <summary>
		/// Defines a logger for this class.
		/// </summary>
		private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

		/// <summary>
		/// This hash constrain all item template the trainer can give
		/// </summary>	
		private static IDictionary allStartupItems = new Hashtable();

		/// <summary>
		/// This function is called at the server startup
		/// </summary>	
		[GameServerStartedEvent]
		public static void OnServerStartup(DOLEvent e, object sender, EventArgs args)
		{	
			// TODO find level 5 trainer gift
		}

		/// <summary>
		/// Interact with trainer
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
 		public override bool Interact(GamePlayer player)
 		{		
 			if (!base.Interact(player)) return false;
								
			// check if class matches.				
			if (player.CharacterClass.ID == (int) eCharacterClass.Bainshee)
			{
				player.Out.SendTrainerWindow();
				player.Out.SendMessage(this.Name + " says, \"Our way is a hard one, " + player.Name + ". I can only train you in skills. You must gain knowledge and wisdom on your own.\"", eChatType.CT_Say, eChatLoc.CL_ChatWindow);

			}
			else if (CanPromotePlayer(player)) 
			{
				player.Out.SendMessage(this.Name + " says, \"Do you wish to train as a [Bainshee] and walk the Path of Affinity?\"", eChatType.CT_System, eChatLoc.CL_PopupWindow);
			} 
			else 
			{
				player.Out.SendMessage(this.Name + " says, \"You must seek elsewhere for your training.\"", eChatType.CT_Say, eChatLoc.CL_ChatWindow);
			}
			return true;
 		}

		/// <summary>
		/// checks whether a player can be promoted or not
		/// </summary>
		/// <param name="player"></param>
		/// <returns></returns>
		public bool CanPromotePlayer(GamePlayer player) 
		{
			return (player.Level>=5 && player.Gender == 1 && player.CharacterClass.ID == (int) eCharacterClass.Magician && (player.Race == (int) eRace.Celt || player.Race == (int) eRace.Elf
				|| player.Race == (int) eRace.Lurikeen));
		}

		/// <summary>
		/// Talk to trainer
		/// </summary>
		/// <param name="source"></param>
		/// <param name="text"></param>
		/// <returns></returns>
		public override bool WhisperReceive(GameLiving source, string text)
		{				
			if (!base.WhisperReceive(source, text)) return false;			
			GamePlayer player = source as GamePlayer;			
	
			switch (text) 
			{
				case "Bainshee":
					if (CanPromotePlayer(player))
						PromotePlayer(player, (int)eCharacterClass.Bainshee, "Well met then, " + source.GetName(0, false) + ". It is a hard road, but I see hardness is no stranger to you. Welcome, Bainshee, welcome. Here, take this. It will aid you in your first encounters as a Bainshee.", null);
				
				break;
			}
			return true;		
		}
	}
}
