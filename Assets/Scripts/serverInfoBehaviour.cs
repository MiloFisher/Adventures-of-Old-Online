using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class serverInfoBehaviour : Bolt.EntityBehaviour<IServerInfo>
{
    private string targetName;
    private string lastName;
    private string lastName2;
    private string lastName3;
    private string lastName4;
    private string lastName5;
    private string lastName6;
    private string lastName7;
    private string lastName8;
    private string lastName9;
    private string lastName10;
    private string lastName11;
    private bool dealt2cards;
    private bool scrollNullfied;
    private bool quickshotNullified;
    private bool damageNullified;
    private bool treasureNullified;
    private bool treasureNullified1;
    private bool encounterNullified;
    private bool bossNullified;
    private bool combatNullified;
    private bool storeNullified;
    private bool combatOver;
    private int activeCombatParty;
    private int baseBossPower = 18;
    private int primed = -1;
    private int hp = 0;
    private bool alreadyAssignedReward;
    public override void Attached()
    {

        string fileName = "playersInGame";
        var sr = File.OpenText(fileName);
        if (entity.IsOwner)
            state.Players = sr.ReadLine();
        sr.Close();
        if (state.Players == "0")
        {
            Destroy(gameObject);
        }
        else
        {
            if (state.Players != null && state.Players != "")
            {
                state.Area = 1;
                shuffleEncounterDeck(int.Parse(state.Players));
                shuffleTreasureDeck();
                shuffleBossDeck();
                shuffleQuestDecks();
                state.TurnIterator = 0;
                state.EncounterIterator = 0;
            }
        }  
    }

    public override void SimulateOwner()
    {
        //set playerIDs
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            state.PlayerIDs[i] = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ID = i;
        }

        //Chel'xith, Hell Lord Ability - Domination
        if (state.MonsterAbility == 27)
        {
            string weakestPlayer = players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            int lowestHealth = int.Parse(players[0].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
            for (int i = 1; i < players.Length; i++)
            {
                if (int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health) < lowestHealth && !players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    weakestPlayer = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                    lowestHealth = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                }
            }
            state.DominatedPlayer = weakestPlayer;
        }
        else
            state.DominatedPlayer = null;

        //store refresh/ fill
        for (int i = 0; i < state.Store.Length; i++)
        {
            if (state.Store[i] == null || state.Store[i] == "")
            {
                //for (int j = 0; j < state.TreasureDeck.Length; j++)
                //{
                //    if (state.TreasureDeck[j] != null)
                //    {
                //        state.Store[i] = state.TreasureDeck[j];
                //        state.TreasureDeck[j] = null;
                //        j = state.TreasureDeck.Length;
                //    }
                //}
                state.Store[i] = state.TreasureDeck[state.TreasureIterator];
                state.TreasureIterator++;
            }
        }

        storeNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BoughtItem > -1)
                storeNullified = false;
        }

        if (storeNullified)
            lastName11 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BoughtItem > -1 && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName11)
            {
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter = false;
                state.Store[players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BoughtItem] = state.TreasureDeck[state.TreasureIterator];
                state.TreasureIterator++;
                lastName11 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //turn management
        state.EveryoneSetTurnOrder = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnRoll == 0)
                state.EveryoneSetTurnOrder = false;
        }

        if (state.EveryoneSetTurnOrder)
        {

            if (!state.HasSetTurnOrder)
            {
                //List<string> temp = new List<string>();
                string temp = "";
                int it = 0;
                for (int k = 6; k > 0; k--)
                {
                    for (int i = 0; i < players.Length; i++)
                    {
                        if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnRoll == k)
                        {
                            //found first person
                            temp = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                            it = i;
                            i = 100;
                            k = 0;
                        }
                    }
                }
                //for (int i = 0; i < temp.Count; i++)
                //    state.TurnOrder[i] = temp[i];
                state.TurnOrder[0] = temp;
                for (int i = 1; i < players.Length; i++)
                {
                    it++;
                    if (it >= players.Length)
                        it = 0;
                    state.TurnOrder[i] = players[it].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                }
                state.HasSetTurnOrder = true;
            }
        }

        if (state.HasSetTurnOrder)
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedTurn && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName)
                {
                    if (state.TurnIterator < players.Length - 1)
                        state.TurnIterator++;
                    else
                        state.TurnIterator = 0;
                    lastName = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedTurn = false;
                }
            }
            //if (state.TurnIterator > players.Length - 1)
            //    state.TurnIterator = 0;
        }

        //move encounter iterator
        encounterNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter == true)
                encounterNullified = false;
        }

        if (encounterNullified)
            lastName2 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName2)
            {
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter = false;
                state.EncounterIterator++;
                lastName2 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //move encounter iterator from fork in the road
        encounterNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ForkInTheRoadActive == true)
                encounterNullified = false;
        }

        if (encounterNullified)
            lastName7 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ForkInTheRoadActive && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName7)
            {
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrewEncounter = false;
                state.EncounterIterator+=2;
                lastName7 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //move treasure iterator as result of treasure trove
        treasureNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SearchedTreasureTrove == true)
                treasureNullified = false;
        }

        if (treasureNullified)
            lastName3 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SearchedTreasureTrove && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName3)
            {
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SearchedTreasureTrove = false;
                state.TreasureIterator+=3;
                lastName3 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                //treasure trove clear
                for (int k = 0; k < players.Length; k++)
                {
                    if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "10,18")
                        state.SearchedTreasureTroves[0] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "21,13")
                        state.SearchedTreasureTroves[1] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "20,6")
                        state.SearchedTreasureTroves[2] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "16,11")
                        state.SearchedTreasureTroves[3] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "10,7")
                        state.SearchedTreasureTroves[4] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "6,12")
                        state.SearchedTreasureTroves[5] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "8,0")
                        state.SearchedTreasureTroves[6] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "15,5")
                        state.SearchedTreasureTroves[7] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                    else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location == "23,4")
                        state.SearchedTreasureTroves[8] = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Location;
                }
            }
        }

        //set combat leader
        if ((state.CombatParty[0] == null || state.CombatParty[0] == "") && (state.Spectators[0] == null || state.Spectators[0] == ""))
        {
            for (int i = 0; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InCombat)
                {
                    state.CombatParty[0] = state.TurnOrder[state.TurnIterator];
                    for (int k = 0; k < players.Length; k++)
                    {
                        if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == state.CombatParty[0])
                        {
                            state.CombatStarter = players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                            if(!players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                                monsterAssign(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Encounter);
                            else
                                monsterAssign(state.BossDeck[state.Area - 1]);
                            k = 100;
                        }
                    }
                }
            }
        }

        //set assisting players
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsAssisting)
            {
                for (int k = 0; k < 6; k++)
                {
                    if (state.CombatParty[k] == null || state.CombatParty[k] == "")
                    {
                        state.CombatParty[k] = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsAssisting = false;
                        k = 100;
                    }
                }
            }
        }

        //set spectating players
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsSpectating)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (state.Spectators[k] == null || state.Spectators[k] == "")
                    {
                        state.Spectators[k] = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsSpectating = false;
                        k = 100;
                    }
                }
            }
        }

        //rid of players that leave
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup)
            {
                for (int k = 0; k < 5; k++)
                {
                    if (state.Spectators[k] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                    {
                        state.Spectators[k] = null;
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup = false;
                        k = 100;
                    }
                }
                for (int k = 0; k < 6; k++)
                {
                    if (state.CombatParty[k] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                    {
                        state.CombatParty[k] = null;
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LeftGroup = false;
                        k = 100;
                    }
                }
            }
        }

        //rid of duplicate names
        for (int i = 0; i < state.CombatParty.Length; i++)
        {
            for (int k = 0; k < state.CombatParty.Length; k++)
            {
                if (i != k && state.CombatParty[i] == state.CombatParty[k])
                {
                    if (state.CombatParty[k] != null && state.CombatParty[k] != "")
                    {
                        state.CombatParty[k] = null;
                    }
                }
            }
        }
        for (int i = 0; i < state.Spectators.Length; i++)
        {
            for (int k = 0; k < state.Spectators.Length; k++)
            {
                if (i != k && state.Spectators[i] == state.Spectators[k])
                {
                    if (state.Spectators[k] != null && state.Spectators[k] != "")
                    {
                        state.Spectators[k] = null;
                    }
                }
            }
        }

        //slide combat party down
        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < state.CombatParty.Length - 1; i++)
            {
                if ((state.CombatParty[i] == null || state.CombatParty[i] == "") && (state.CombatParty[i + 1] != null && state.CombatParty[i] != ""))
                {
                    string temp = state.CombatParty[i];
                    state.CombatParty[i] = state.CombatParty[i + 1];
                    state.CombatParty[i + 1] = temp;
                }
            }
        }

        //slide spectators down
        for (int k = 0; k < 6; k++)
        {
            for (int i = 0; i < state.Spectators.Length - 1; i++)
            {
                if ((state.Spectators[i] == null || state.Spectators[i] == "") && (state.Spectators[i + 1] != null && state.Spectators[i] != ""))
                {
                    string temp = state.Spectators[i];
                    state.Spectators[i] = state.Spectators[i + 1];
                    state.Spectators[i + 1] = temp;
                }
            }
        }

        //move combat turn order
        activeCombatParty = -1;
        for (int i = 0; i < state.CombatParty.Length; i++)
        {
            if (state.CombatParty[i] != null && state.CombatParty[i] != "")
                activeCombatParty++;
        }

        combatNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn)
                combatNullified = false;
        }

        if (combatNullified)
            lastName4 = null;

        for (int k = 0; k < players.Length; k++)
        {
            if (state.CombatParty[state.CombatIterator] != lastName4 && players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == state.CombatParty[state.CombatIterator] && players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().EndedCombatTurn)
            {
                if (state.CombatIterator < activeCombatParty)
                    state.CombatIterator++;
                else
                    state.CombatIterator = 0;
                lastName4 = state.CombatParty[state.CombatIterator]; 

                //Elemental Anomaly Ability - Charging Power
                if (state.MonsterAbility == 7)
                {
                    bool magicalPresence = false;
                    for (int i = 0; i < state.CombatParty.Length; i++)
                    {
                        for (int j = 0; j < players.Length; j++)
                        {
                            if (state.CombatParty[i] == players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                            {
                                if (players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Sorcerer" || players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Necromancer")
                                {
                                    magicalPresence = true;
                                }
                            }
                        }
                    }
                    if (!magicalPresence)
                        state.MonsterDamage++;
                }

                //Ancient Crocodile Ability - Ancient Devourer
                if(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BitByCrocodile)
                    state.MonsterDamage+=4;

                //Starving Wolf Pack Ability - Feeding Frenzy
                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WolfHeal)
                {
                    state.MonsterHealth += 5;
                    if (state.MonsterHealth > state.MonsterMaxHealth)
                        state.MonsterHealth = state.MonsterMaxHealth;
                }

                //Cinderosa Ability - Punishing Flames
                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitByCinderosa)
                    state.CinderosaStreak++;
                else
                    state.CinderosaStreak = 0;

                //alternate turn tick
                if (state.TurnTick == 1)
                    state.TurnTick = -1;
                else
                    state.TurnTick = 1;

                k = 100;
            }
        }

        

        //Mythic Serpent Ability - Legendary Beast
        if (state.MonsterAbility == 18)
        {
            int bd = 0;
            int bp = 0;
            for (int i = 1; i < players.Length; i++)
            {
                bd += 4 * players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpentAC;
                bp += players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpentAC;
            }
            state.MonsterBonusDamage = bd;
            state.MonsterBonusPower = bp;
        }
        else
        {
            state.MonsterBonusDamage = 0;
            state.MonsterBonusPower = 0;
        }

        for (int k = 0; k < players.Length; k++)
        {
            if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == state.CombatParty[state.CombatIterator])
            {
                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                {
                    if (state.CombatIterator < activeCombatParty)
                        state.CombatIterator++;
                    else
                        state.CombatIterator = 0;
                    k = 100;
                    lastName4 = state.CombatParty[state.CombatIterator];
                }
                else if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone && primed == -1)
                {
                    primed = k;
                }
                else if (state.DominatedPlayer == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && hp == 0)
                {
                    if(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health != null)
                        hp = int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                }
                else if (primed == k && !players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone)
                {
                    primed = -1;
                    if (state.CombatIterator < activeCombatParty)
                        state.CombatIterator++;
                    else
                        state.CombatIterator = 0;
                    k = 100;
                    lastName4 = state.CombatParty[state.CombatIterator];
                }
                else if (hp != int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health) && hp > 0)
                {
                    hp = 0;
                    if (state.CombatIterator < activeCombatParty)
                        state.CombatIterator++;
                    else
                        state.CombatIterator = 0;
                    k = 100;
                    lastName4 = state.CombatParty[state.CombatIterator];
                }
            }
        }

        combatOver = true;
        for (int i = 0; i < state.CombatParty.Length; i++)
        {
            if (state.CombatParty[i] != null && state.CombatParty[i] != "")
                combatOver = false;
        }
        if (combatOver)
            state.CombatIterator = 0;

        if(state.CombatIterator > activeCombatParty)
            state.CombatIterator = 0;

        //deal outgoing damage
        damageNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage > 0)
                damageNullified = false;
        }

        if (damageNullified)
            lastName5 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage > 0 && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName5)
            {
                state.MonsterHealth -= players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage;
                if (state.MonsterHealth < 0)
                    state.MonsterHealth = 0;
                lastName5 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 0;
            }
        }

        //deal damage from quickshot
        quickshotNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot > 0)
                quickshotNullified = false;
        }

        if (quickshotNullified)
            lastName10 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot > 0 && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName10)
            {
                state.MonsterHealth -= players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot;
                if (state.MonsterHealth < 0)
                    state.MonsterHealth = 0;
                lastName10 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //deal damage from scroll of harming
        scrollNullfied = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming)
                scrollNullfied = false;
        }

        if (scrollNullfied)
            lastName9 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName9)
            {
                state.MonsterHealth -= 10;
                if (state.MonsterHealth < 0)
                    state.MonsterHealth = 0;
                lastName9 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = 0;
            }
        }

        //move treasure iterator as result of drawing treasures
        treasureNullified1 = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing > 0)
                treasureNullified1 = false;
        }

        if (treasureNullified1)
            lastName6 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing > 0 && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName6)
            {
                //players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SearchedTreasureTrove = false;
                state.TreasureIterator += players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CardsDrawing;
                lastName6 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //increment area
        bossNullified = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss)
                bossNullified = false;
        }

        if (bossNullified)
            lastName8 = null;

        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().KilledBoss && players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name != lastName8)
            {
                state.Area++;
                lastName8 = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
            }
        }

        //boss reward controller
        List<int> rolls = new List<int>();
        bool assignRewards = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (!players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll > 0)
                    rolls.Add(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll);
                else
                    assignRewards = false;
            }
        }

        if (assignRewards && !alreadyAssignedReward)
        {
            alreadyAssignedReward = true;
            StartCoroutine(AssignLoot(rolls,players));
        }

        bool clearAll = true;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LootRoll > 0)
                clearAll = false;
        }

        if (clearAll)
        {
            state.LegendaryReceiver = null;
            state.HighestRoll = 0;
            alreadyAssignedReward = false;
        }

    }

    IEnumerator AssignLoot(List<int> rolls, GameObject[] players)
    {
        yield return new WaitForSeconds(1);
        int highest = 0;
        int highestID = 0;
        bool tied = false;
        List<int> tiedIDs = new List<int>();
        for (int i = 0; i < rolls.Count; i++)
        {
            if (rolls[i] > highest)
            {
                rolls[i] = highest;
                highestID = i;
            }
        }
        for (int i = 0; i < rolls.Count; i++)
        {
            for (int j = 0; j < rolls.Count; j++)
            {
                if (i != j && rolls[i] == rolls[j] && rolls[i] == highest)
                {
                    tiedIDs.Add(i);
                    tied = true;
                }
            }
        }
        if (tied)
        {
            int rando = Random.Range(0, tiedIDs.Count);
            state.LegendaryReceiver = players[tiedIDs[rando]].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
        }
        else
            state.LegendaryReceiver = players[highestID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
    }

    void Update()
    {
        GameObject[] players = GameObject.FindGameObjectsWithTag("Player Info");
        //trade completions
        for (int i = 0; i < players.Length; i++)
        {
            for (int j = 0; j < players.Length; j++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name && players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeTarget == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady && players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TradeReady)
                    {
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanResolveTrade = true;
                        players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanResolveTrade = true;
                        //state.AcceptedTrade[0] = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                        //state.AcceptedTrade[1] = players[j].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                    }
                }
            }
        }

        //deal 2 treasure cards to each player
        if (!dealt2cards && state.HasSetTurnOrder)
        {
            state.TreasureIterator += players.Length * 2;
            dealt2cards = true;
        } 
    }

    public void shuffleEncounterDeck(int players)
    {
        //monster cards ids 0-19 
        //treasure cards ids 0-19
        //2 of each monster card
        //3 of each treasure card
        List<string> encounters = new List<string>();
        List<string> easyStack = new List<string>();
        List<string> mediumStack = new List<string>();
        List<string> hardStack = new List<string>();
        int id;

        easyStack.Add("m0");//tiny troll
        easyStack.Add("m0");
        easyStack.Add("m1");//ugly succubus
        easyStack.Add("m1");
        easyStack.Add("m2");//sadistic imp
        easyStack.Add("m2");
        easyStack.Add("m3");//carniverous plant
        easyStack.Add("m3");
        easyStack.Add("m4");//feral bear
        easyStack.Add("m4");
        easyStack.Add("m5");//rogue bandits
        easyStack.Add("m5");
        easyStack.Add("m6");//juvenile dragon
        easyStack.Add("m6");

        mediumStack.Add("m7");//elemental anomaly
        mediumStack.Add("m7");
        mediumStack.Add("m8");//colossal spider
        mediumStack.Add("m8");
        mediumStack.Add("m9");//ancient crocodile
        mediumStack.Add("m9");
        mediumStack.Add("m10");//scavenging looter
        mediumStack.Add("m10");
        mediumStack.Add("m11");//cynical cyclops
        mediumStack.Add("m11");
        mediumStack.Add("m12");//floating eye
        mediumStack.Add("m12");
        mediumStack.Add("m13");//starving wolfpack
        mediumStack.Add("m13");

        hardStack.Add("m14");//goblin horde
        hardStack.Add("m14");
        hardStack.Add("m15");//skeleton army
        hardStack.Add("m15");
        hardStack.Add("m16");//disturbed giant
        hardStack.Add("m16");
        hardStack.Add("m17");//shadow nightmare
        hardStack.Add("m17");
        hardStack.Add("m18");//mythic serpent
        hardStack.Add("m18");
        hardStack.Add("m19");//enraged goliath
        hardStack.Add("m19");

        for (int i = 0; i < 20; i++)//fills encounters list
        {
            for (int j = 0; j < 3; j++)
            {
                encounters.Add("e" + i);
            }
        }

        for (int i = 0; i < 20; i++)//fills stacks with encounters
        {
            id = Random.Range(0, encounters.Count);//fills easy
            easyStack.Add(encounters[id]);
            encounters.Remove(encounters[id]);

            id = Random.Range(0, encounters.Count);//fills medium
            mediumStack.Add(encounters[id]);
            encounters.Remove(encounters[id]);

            id = Random.Range(0, encounters.Count);//fills hard
            hardStack.Add(encounters[id]);
            encounters.Remove(encounters[id]);
        }

        if (players == 3)//remove cards from stacks based on player amount
        {
            for (int i = 0; i < 17; i++)//removes 17 cards from each stack
            {
                id = Random.Range(0, easyStack.Count);
                easyStack.Remove(easyStack[id]);

                id = Random.Range(0, mediumStack.Count);
                mediumStack.Remove(mediumStack[id]);

                id = Random.Range(0, hardStack.Count);
                hardStack.Remove(hardStack[id]);
            }
        }
        else if (players == 4)
        {
            for (int i = 0; i < 11; i++)//removes 11 cards from each stack
            {
                id = Random.Range(0, easyStack.Count);
                easyStack.Remove(easyStack[id]);

                id = Random.Range(0, mediumStack.Count);
                mediumStack.Remove(mediumStack[id]);

                id = Random.Range(0, hardStack.Count);
                hardStack.Remove(hardStack[id]);
            }
        }
        else if (players == 5)
        {
            for (int i = 0; i < 6; i++)//removes 6 cards from each stack
            {
                id = Random.Range(0, easyStack.Count);
                easyStack.Remove(easyStack[id]);

                id = Random.Range(0, mediumStack.Count);
                mediumStack.Remove(mediumStack[id]);

                id = Random.Range(0, hardStack.Count);
                hardStack.Remove(hardStack[id]);
            }
        }
        else if (players == 6)
        {
            //keep all cards
        }

        for (int i = 0; i < state.EncounterDeck.Length; i++)//add stacks randomly in order to main deck
        {
            if (easyStack.Count > 0)//adds easystack randomly until empty
            {
                id = Random.Range(0, easyStack.Count);
                state.EncounterDeck[i] = easyStack[id];
                easyStack.Remove(easyStack[id]);
            }
            else if (mediumStack.Count > 0)//adds mediumstack randomly until empty
            {
                id = Random.Range(0, mediumStack.Count);
                state.EncounterDeck[i] = mediumStack[id];
                mediumStack.Remove(mediumStack[id]);
            }
            else if (hardStack.Count > 0)//adds hardstack randomly until empty
            {
                id = Random.Range(0, hardStack.Count);
                state.EncounterDeck[i] = hardStack[id];
                hardStack.Remove(hardStack[id]);
            }
            else
            {
                state.EncounterDeck[i] = null;
            }
        }
    }

    public void shuffleTreasureDeck()
    {
        List<string> treasures = new List<string>();
        int id;
        treasures.Add("t0");//lesser healing potion
        treasures.Add("t0");
        treasures.Add("t0");
        treasures.Add("t0");
        treasures.Add("t1");//healing potion
        treasures.Add("t1");
        treasures.Add("t1");
        treasures.Add("t1");
        treasures.Add("t2");//greater healing potion
        treasures.Add("t2");
        treasures.Add("t2");
        treasures.Add("t2");
        treasures.Add("t3");//scroll of resurrection
        treasures.Add("t3");
        treasures.Add("t3");
        treasures.Add("t3");
        treasures.Add("t4");//lesser ability well
        treasures.Add("t4");
        treasures.Add("t4");
        treasures.Add("t5");//ability well
        treasures.Add("t5");
        treasures.Add("t5");
        treasures.Add("t6");//greater ability well
        treasures.Add("t6");
        treasures.Add("t6");
        treasures.Add("t7");//scroll of harming
        treasures.Add("t7");
        treasures.Add("t7");
        treasures.Add("t8");//scroll of teleportation
        treasures.Add("t8");
        treasures.Add("t8");
        treasures.Add("t8");
        treasures.Add("t9");//rune of valor
        treasures.Add("t9");
        treasures.Add("t9");
        treasures.Add("t10");//rune of swiftness
        treasures.Add("t10");
        treasures.Add("t10");
        treasures.Add("t11");//rune of wisdom
        treasures.Add("t11");
        treasures.Add("t11");
        treasures.Add("a0");//greathelm
        treasures.Add("a0");
        treasures.Add("a1");//chestplate
        treasures.Add("a1");
        treasures.Add("a2");//greaves
        treasures.Add("a2");
        treasures.Add("a3");//gloves
        treasures.Add("a3");
        treasures.Add("a4");//hood
        treasures.Add("a4");
        treasures.Add("a5");//cloak
        treasures.Add("a5");
        treasures.Add("a6");//boots
        treasures.Add("a6");
        treasures.Add("a7");//wraps
        treasures.Add("a7");
        treasures.Add("a8");//hat
        treasures.Add("a8");
        treasures.Add("a9");//robes
        treasures.Add("a9");
        treasures.Add("a10");//shoes
        treasures.Add("a10");
        treasures.Add("a11");//cuffs
        treasures.Add("a11");
        treasures.Add("a12");//mythical greathelm
        treasures.Add("a13");//mythical chestplate
        treasures.Add("a14");//mythical greaves
        treasures.Add("a15");//mythical gloves
        treasures.Add("a16");//shrouded hood
        treasures.Add("a17");//shrouded cloak
        treasures.Add("a18");//shrouded boots
        treasures.Add("a19");//shrouded wraps
        treasures.Add("a20");//enchanted hat
        treasures.Add("a21");//enchanted robes
        treasures.Add("a22");//enchanted shoes
        treasures.Add("a23");//enchanted cuffs
        treasures.Add("w0");//greatsword
        treasures.Add("w0");
        treasures.Add("w1");//greatshield
        treasures.Add("w1");
        treasures.Add("w2");//shortsword
        treasures.Add("w2");
        treasures.Add("w3");//longbow
        treasures.Add("w3");
        treasures.Add("w4");//staff
        treasures.Add("w4");
        treasures.Add("w5");//mace
        treasures.Add("w5");
        treasures.Add("w6");//warhammer
        treasures.Add("w6");
        treasures.Add("w7");//mythical greatsword
        treasures.Add("w8");//mythical greatshield
        treasures.Add("w9");//mythical shortsword
        treasures.Add("w10");//shrouded longbow
        treasures.Add("w11");//enchanted staff
        treasures.Add("w12");//mythical mace
        treasures.Add("w13");//mythical warhammer

        for (int i = 0; i < state.TreasureDeck.Length; i++)//fill treasure deck with random order of treasures
        {
            id = Random.Range(0, treasures.Count);
            state.TreasureDeck[i] = treasures[id];
            treasures.Remove(treasures[id]);
        }
    }

    public void shuffleBossDeck()
    {
        List<string> bosses = new List<string>();
        int id;
        bosses.Add("b0");//cinderosa
        bosses.Add("b1");//king of the undead
        bosses.Add("b2");//the all-seer
        bosses.Add("b3");//magistrax the gorgon
        bosses.Add("b4");//the shadow
        bosses.Add("b5");//ophretes the kraken
        bosses.Add("b6");//giant king magnor
        bosses.Add("b7");//chel'xith, hell lord
        bosses.Add("b8");//spider queen aranne

        for (int i = 0; i < state.BossDeck.Length; i++)//fill boss deck with random order of bosses
        {
            id = Random.Range(0, bosses.Count);
            state.BossDeck[i] = bosses[id];
            bosses.Remove(bosses[id]);
        }
    }

    public void shuffleLegendaryDeck(string[] playerClasses)
    {
        List<string> legendaries = new List<string>();
        int id;
        bool hasWarrior = false;
        bool hasHunter = false;
        bool hasSorcerer = false;
        bool hasCleric = false;
        bool hasPaladin = false;
        bool hasRogue = false;
        bool hasNecromancer = false;
        bool hasDruid = false;
        bool hasMonk = false;

        for (int i = 0; i < playerClasses.Length; i++)//finds which classes are in play
        {
            if (playerClasses[i] == "Warrior")
                hasWarrior = true;
            else if (playerClasses[i] == "Hunter")
                hasHunter = true;
            else if (playerClasses[i] == "Sorcerer")
                hasSorcerer = true;
            else if (playerClasses[i] == "Cleric")
                hasCleric = true;
            else if (playerClasses[i] == "Paladin")
                hasPaladin = true;
            else if (playerClasses[i] == "Rogue")
                hasRogue = true;
            else if (playerClasses[i] == "Necromancer")
                hasNecromancer = true;
            else if (playerClasses[i] == "Druid")
                hasDruid = true;
            else if (playerClasses[i] == "Monk")
                hasMonk = true;
        }

        if(hasSorcerer)
            legendaries.Add("a24");//zyreth's raiments
        if (hasHunter)
            legendaries.Add("a25");//ranger's guise
        if (hasCleric)
            legendaries.Add("a26");//bracers of eidiril
        if (hasRogue)
            legendaries.Add("a27");//cloak of the scarlet slayer
        if (hasPaladin)
            legendaries.Add("a28");//aegis of alar'akyr
        if (hasNecromancer)
            legendaries.Add("a29");//crown of undeath
        if (hasWarrior)
            legendaries.Add("a30");//warbringer's visage
        if (hasDruid)
            legendaries.Add("a31");//wild walkers
        if (hasMonk)
            legendaries.Add("a32");//chi caller
        if (hasMonk)
            legendaries.Add("a33");//wavedancers
        if (hasSorcerer)
            legendaries.Add("w14");//the grand conjurer's staff
        if (hasHunter)
            legendaries.Add("w15");//kairena's bow of the hunt
        if (hasCleric)
            legendaries.Add("w16");//heaven's breath
        if (hasRogue)
            legendaries.Add("w17");//dagger of the aelestines
        if (hasPaladin)
            legendaries.Add("w18");//faith's edge
        if (hasNecromancer)
            legendaries.Add("w19");//death's call
        if (hasWarrior)
            legendaries.Add("w20");//the defender
        if (hasDruid)
            legendaries.Add("w21");//naturespine

        for (int i = 0; i < state.LegendaryDeck.Length; i++)//fill legendary deck with random order of legendaries
        {
            if (legendaries.Count > 0)
            {
                id = Random.Range(0, legendaries.Count);
                state.LegendaryDeck[i] = legendaries[id];
                legendaries.Remove(legendaries[id]);
            }
        }
    }

    public void shuffleQuestDecks()
    {
        List<string> questList1 = new List<string>();
        List<string> questList2 = new List<string>();
        List<string> questList3 = new List<string>();
        for (int i = 0; i < 8; i++)
        {
            questList1.Add("q" + i);
        }
        for (int i = 8; i < 16; i++)
        {
            questList2.Add("q" + i);
        }
        for (int i = 16; i < 24; i++)
        {
            questList3.Add("q" + i);
        }

        int random;
        for (int i = 0; i < 8; i++)
        {
            random = Random.Range(0, questList1.Count);
            state.QuestDeck1[i] = questList1[random];
            questList1.Remove(questList1[random]);

            random = Random.Range(0, questList2.Count);
            state.QuestDeck2[i] = questList2[random];
            questList2.Remove(questList2[random]);

            random = Random.Range(0, questList3.Count);
            state.QuestDeck3[i] = questList3[random];
            questList3.Remove(questList3[random]);
        }
    }

    public string drawTreasureCard()
    {
        string drawnCard = null;
        for (int i = 0; i < state.TreasureDeck.Length; i++)
        {
            if (state.TreasureDeck[i] != null)
            {
                drawnCard = state.TreasureDeck[i];
                state.TreasureDeck[i] = null;
                i = state.TreasureDeck.Length;
            }
        }
        return drawnCard;
    }

    public string drawEncounterCard()
    {
        string drawnCard = null;
        drawnCard = state.EncounterDeck[state.EncounterIterator];
        //state.EncounterDeck[state.EncounterIterator] = null;
        //state.EncounterIterator++;
        return drawnCard;
    }

    public string drawBossCard()
    {
        string drawnCard = null;
        for (int i = 0; i < state.BossDeck.Length; i++)
        {
            if (state.BossDeck[i] != null)
            {
                drawnCard = state.BossDeck[i];
                state.BossDeck[i] = null;
                i = state.BossDeck.Length;
            }
        }
        return drawnCard;
    }

    public string drawQuestCard(int id)
    {
        string drawnCard = null;
        if (state.Area == 3)
        {
            drawnCard = state.QuestDeck3[id];
        }
        else if (state.Area == 2)
        {
            drawnCard = state.QuestDeck2[id];
        }
        else
        {
            drawnCard = state.QuestDeck1[id];
        }

        return drawnCard;
    }

    public void monsterAssign(string monster)
    {
        state.Monster = monster;
        switch (monster)
        {
            case "m0"://tiny troll
                state.MonsterLevel = 1;
                state.MonsterPower = 7;
                state.MonsterDamage = 6;
                state.MonsterHealth = 6;
                state.MonsterMaxHealth = 6;
                state.MonsterAbility = 0;
                state.MonsterTreasures = 1;
                break;
            case "m1"://ugly succubus
                state.MonsterLevel = 2;
                state.MonsterPower = 9;
                state.MonsterDamage = 9;
                state.MonsterHealth = 8;
                state.MonsterMaxHealth = 8;
                state.MonsterAbility = 1;
                state.MonsterTreasures = 1;
                break;
            case "m2"://sadistic imp
                state.MonsterLevel = 4;
                state.MonsterPower = 11;
                state.MonsterDamage = 15;
                state.MonsterHealth = 12;
                state.MonsterMaxHealth = 12;
                state.MonsterAbility = 2;
                state.MonsterTreasures = 1;
                break;
            case "m3"://carnivorous plant
                state.MonsterLevel = 5;
                state.MonsterPower = 14;
                state.MonsterDamage = 17;
                state.MonsterHealth = 15;
                state.MonsterMaxHealth = 15;
                state.MonsterAbility = 3;
                state.MonsterTreasures = 1;
                break;
            case "m4"://feral bear
                state.MonsterLevel = 6;
                state.MonsterPower = 15;
                state.MonsterDamage = 18;
                state.MonsterHealth = 18;
                state.MonsterMaxHealth = 18;
                state.MonsterAbility = 4;
                state.MonsterTreasures = 1;
                break;
            case "m5"://rogue bandits
                state.MonsterLevel = 8;
                state.MonsterPower = 17;
                state.MonsterDamage = 20;
                state.MonsterHealth = 22;
                state.MonsterMaxHealth = 22;
                state.MonsterAbility = 5;
                state.MonsterTreasures = 1;
                break;
            case "m6"://juvenile dragon
                state.MonsterLevel = 9;
                state.MonsterPower = 20;
                state.MonsterDamage = 25;
                state.MonsterHealth = 30;
                state.MonsterMaxHealth = 30;
                state.MonsterAbility = 6;
                state.MonsterTreasures = 2;
                break;
            case "m7"://elemental anomaly
                state.MonsterLevel = 12;
                state.MonsterPower = 23;
                state.MonsterDamage = 22;
                state.MonsterHealth = 28;
                state.MonsterMaxHealth = 28;
                state.MonsterAbility = 7;
                state.MonsterTreasures = 2;
                break;
            case "m8"://colossal spider
                state.MonsterLevel = 13;
                state.MonsterPower = 24;
                state.MonsterDamage = 24;
                state.MonsterHealth = 30;
                state.MonsterMaxHealth = 30;
                state.MonsterAbility = 8;
                state.MonsterTreasures = 2;
                break;
            case "m9"://ancient crocodile
                state.MonsterLevel = 14;
                state.MonsterPower = 25;
                state.MonsterDamage = 26;
                state.MonsterHealth = 32;
                state.MonsterMaxHealth = 32;
                state.MonsterAbility = 9;
                state.MonsterTreasures = 2;
                break;
            case "m10"://scavenging looter
                state.MonsterLevel = 16;
                state.MonsterPower = 27;
                state.MonsterDamage = 27;
                state.MonsterHealth = 40;
                state.MonsterMaxHealth = 40;
                state.MonsterAbility = 10;
                state.MonsterTreasures = 0;
                break;
            case "m11"://cynical cyclops
                state.MonsterLevel = 17;
                state.MonsterPower = 28;
                state.MonsterDamage = 28;
                state.MonsterHealth = 36;
                state.MonsterMaxHealth = 36;
                state.MonsterAbility = 11;
                state.MonsterTreasures = 2;
                break;
            case "m12"://flaoting eye
                state.MonsterLevel = 18;
                state.MonsterPower = 29;
                state.MonsterDamage = 30;
                state.MonsterHealth = 38;
                state.MonsterMaxHealth = 38;
                state.MonsterAbility = 12;
                state.MonsterTreasures = 2;
                break;
            case "m13"://starving wolf pack
                state.MonsterLevel = 19;
                state.MonsterPower = 32;
                state.MonsterDamage = 30;
                state.MonsterHealth = 40;
                state.MonsterMaxHealth = 40;
                state.MonsterAbility = 13;
                state.MonsterTreasures = 3;
                break;
            case "m14"://goblin horde
                state.MonsterLevel = 21;
                state.MonsterPower = 34;
                state.MonsterDamage = 32;
                state.MonsterHealth = 45;
                state.MonsterMaxHealth = 45;
                state.MonsterAbility = 14;
                state.MonsterTreasures = 3;
                break;
            case "m15"://skeletal army
                state.MonsterLevel = 22;
                state.MonsterPower = 35;
                state.MonsterDamage = 34;
                state.MonsterHealth = 48;
                state.MonsterMaxHealth = 48;
                state.MonsterAbility = 15;
                state.MonsterTreasures = 3;
                break;
            case "m16"://disturbed giant
                state.MonsterLevel = 24;
                state.MonsterPower = 37;
                state.MonsterDamage = 35;
                state.MonsterHealth = 50;
                state.MonsterMaxHealth = 50;
                state.MonsterAbility = 16;
                state.MonsterTreasures = 3;
                break;
            case "m17"://shadow nightmare
                state.MonsterLevel = 26;
                state.MonsterPower = 39;
                state.MonsterDamage = 30;
                state.MonsterHealth = 52;
                state.MonsterMaxHealth = 52;
                state.MonsterAbility = 17;
                state.MonsterTreasures = 3;
                break;
            case "m18"://mythic serpent
                state.MonsterLevel = 28;
                state.MonsterPower = 41;
                state.MonsterDamage = 38;
                state.MonsterHealth = 55;
                state.MonsterMaxHealth = 55;
                state.MonsterAbility = 18;
                state.MonsterTreasures = 3;
                break;
            case "m19"://enraged goliath
                state.MonsterLevel = 29;
                state.MonsterPower = 44;
                state.MonsterDamage = 40;
                state.MonsterHealth = 120;
                state.MonsterMaxHealth = 120;
                state.MonsterAbility = 19;
                state.MonsterTreasures = 4;
                break;
            case "b0"://cinderosa
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 30 + (state.Area - 1) * 5;
                state.MonsterHealth = 180 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 180 + (state.Area - 1) * 100;
                state.MonsterAbility = 20;
                state.MonsterTreasures = 1;
                break;
            case "b1"://king of the undead
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 35 + (state.Area - 1) * 5;
                state.MonsterHealth = 220 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 220 + (state.Area - 1) * 100;
                state.MonsterAbility = 21;
                state.MonsterTreasures = 1;
                break;
            case "b2"://the all-seer
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 28 + (state.Area - 1) * 5;
                state.MonsterHealth = 190 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 190 + (state.Area - 1) * 100;
                state.MonsterAbility = 22;
                state.MonsterTreasures = 1;
                break;
            case "b3"://magistrax the gorgon
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 30 + (state.Area - 1) * 5;
                state.MonsterHealth = 170 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 170 + (state.Area - 1) * 100;
                state.MonsterAbility = 23;
                state.MonsterTreasures = 1;
                break;
            case "b4"://the shadow
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 25 + (state.Area - 1) * 5;
                state.MonsterHealth = 180 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 180 + (state.Area - 1) * 100;
                state.MonsterAbility = 24;
                state.MonsterTreasures = 1;
                break;
            case "b5"://ophretes the kraken
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 28 + (state.Area - 1) * 5;
                state.MonsterHealth = 190 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 190 + (state.Area - 1) * 100;
                state.MonsterAbility = 25;
                state.MonsterTreasures = 1;
                break;
            case "b6"://giant-king magnor
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 30 + (state.Area - 1) * 5;
                state.MonsterHealth = 230 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 230 + (state.Area - 1) * 100;
                state.MonsterAbility = 26;
                state.MonsterTreasures = 1;
                break;
            case "b7"://chel'xith, hell lord
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 32 + (state.Area - 1) * 5;
                state.MonsterHealth = 210 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 210 + (state.Area - 1) * 100;
                state.MonsterAbility = 27;
                state.MonsterTreasures = 1;
                break;
            case "b8"://spider queen aranne
                state.MonsterLevel = 0;
                state.MonsterPower = baseBossPower + (state.Area - 1) * 12;
                state.MonsterDamage = 25 + (state.Area - 1) * 5;
                state.MonsterHealth = 190 + (state.Area - 1) * 100;
                state.MonsterMaxHealth = 190 + (state.Area - 1) * 100;
                state.MonsterAbility = 28;
                state.MonsterTreasures = 1;
                break;
        }
    }
}
