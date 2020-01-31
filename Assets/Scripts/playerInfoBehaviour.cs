using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.SceneManagement;

public class playerInfoBehaviour : Bolt.EntityBehaviour<IPlayerInfo>
{
    //public bool updateInfo = false;
    private string temp;
    private bool left;
    private string pastHealth;
    private bool newHeal;
    private bool drawing;

    public override void Attached()
    {
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        if (entity.IsOwner)
            state.Username = sr.ReadLine();
        sr.Close();
        state.TurnRoll = 0;
        state.BoughtItem = -1;
    }

    public override void SimulateOwner()
    {
        GameObject serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player Info");
        if (serverInfo != null)
        {
            for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs.Length; i++)
            {
                if (state.Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().PlayerIDs[i])
                {
                    if (i == 0)
                        state.Color = "Red";
                    else if (i == 1)
                        state.Color = "Blue";
                    else if (i == 2)
                        state.Color = "Green";
                    else if (i == 3)
                        state.Color = "Yellow";
                    else if (i == 4)
                        state.Color = "Cyan";
                    else if (i == 5)
                        state.Color = "Magenta";
                }
            }

            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnOrder[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().TurnIterator] != state.Name)
            {
                state.DrewEncounter = false;
                state.EndedTurn = false;
                state.SearchedTreasureTrove = false;
                state.ForkInTheRoadActive = false;
            }

            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] != state.Name)
            {
                //state.EndedCombatTurn = false;
            }

            if (state.LeftGroup)
            {
                left = true;
                for (int i = 0; i < 6; i++)
                {
                    if (state.Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i])
                        left = false;
                }
                for (int i = 0; i < 5; i++)
                {
                    if (state.Name == serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Spectators[i])
                        left = false;
                }
                if (left)
                {
                    state.LeftGroup = false;
                }
            }

            if (state.CardsDrawing > 0 && !drawing)
            {
                drawing = true;
                StartCoroutine(DrawTimer());
            }

            if (state.Health != null)
            {
                if (int.Parse(state.Health) <= 0)
                {
                    state.Health = 0 + "";
                    state.IsDead = true;
                }
                else
                    state.IsDead = false;
            }
        }

        if (state.CanResolveTrade)
        {
            state.CanResolveTrade = false;
            StartCoroutine(unready());
        }

        for (int i = 0; i < state.Inventory.Length; i++)
        {
            if (state.Inventory[i] != null && state.Inventory[i] != "")
            {
                for (int j = 0; j < state.Inventory.Length; j++)
                {
                    if (state.Inventory[j] == null || state.Inventory[j] == "")
                    {
                        if (j < i)
                        {
                            temp = state.Inventory[j];
                            state.Inventory[j] = state.Inventory[i];
                            state.Inventory[i] = temp;
                            j = 100;
                        }
                    }
                }
            }
        }

        //sync boss revealed
        for(int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss1)
                state.RevealedBoss1 = true;
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss2)
                state.RevealedBoss2 = true;
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RevealedBoss3)
                state.RevealedBoss3 = true;
        }

        ////reset heal
        //if (state.HealAmount > 0)
        //{
        //    for (int i = 0; i < players.Length; i++)
        //    {
        //        if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == state.HealTarget && !newHeal)
        //        {
        //            pastHealth = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health;
        //            newHeal = true;
        //        }
        //    }
        //}

        //for (int i = 0; i < players.Length; i++)
        //{
        //    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == state.HealTarget)
        //    {
        //        if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health != pastHealth)
        //        {
        //            state.HealTarget = null;
        //            state.HealAmount = 0;
        //            pastHealth = null;
        //            newHeal = false;
        //        }
        //        else
        //            pastHealth = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health;
        //    }
        //}

        if (!state.InCombat)
            state.SpentAC = 0;
    }

    IEnumerator unready()
    {
        yield return new WaitForSeconds(1);
        state.TradeReady = false;
        state.TradeTarget = null;
    }

    public void updateInfo()
    {
		if (entity.IsOwner)
		{
			string fileName = "playerInfo";
			var sr = File.OpenText(fileName);
			state.Username = sr.ReadLine();
			state.Name = sr.ReadLine();
			state.Race = sr.ReadLine();
			state.Class = sr.ReadLine();
			state.Strength = sr.ReadLine();
			state.Dexterity = sr.ReadLine();
			state.Intellect = sr.ReadLine();
			state.Health = sr.ReadLine();
			state.MaxHealth = sr.ReadLine();
            state.Level = sr.ReadLine();
            state.Damage = sr.ReadLine();
			state.PhysicalPower = sr.ReadLine();
			state.SpellPower = sr.ReadLine();
			state.DamageReduction = sr.ReadLine();
			state.Gold = sr.ReadLine();
			state.Image = sr.ReadLine();
            state.AbilityCharges = sr.ReadLine();
            state.StrengthMod = sr.ReadLine();
            state.DexterityMod = sr.ReadLine();
            state.IntellectMod = sr.ReadLine();
            state.DamageMod = sr.ReadLine();
            //============== Inventory ==============
            for (int i = 0; i < 7; i++)
				state.Inventory[i] = sr.ReadLine();
			//============== Equipment ==============
			for (int i = 0; i < 6; i++)
				state.Equipment[i] = sr.ReadLine();
			sr.Close();
		}
    }

    IEnumerator DrawTimer()
    {
        yield return new WaitForSeconds(2);
        state.CardsDrawing = 0;
        drawing = false;
    }
}