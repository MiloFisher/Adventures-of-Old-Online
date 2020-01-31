using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class equipmentStatManager : MonoBehaviour
{
    private GameObject[] players;
    private string characterName;
    private int headDR;
    private int headPP;
    private int headSP;
    private int headStr;
    private int headDex;
    private int headInt;
    private int chestDR;
    private int chestPP;
    private int chestSP;
    private int chestStr;
    private int chestDex;
    private int chestInt;
    private int legsDR;
    private int legsPP;
    private int legsSP;
    private int legsStr;
    private int legsDex;
    private int legsInt;
    private int handsDR;
    private int handsPP;
    private int handsSP;
    private int handsStr;
    private int handsDex;
    private int handsInt;
    private int mainDMG;
    private int mainSP;
    private int mainStr;
    private int mainDex;
    private int mainInt;
    private int offDMG;
    private int offSP;
    private int offStr;
    private int offDex;
    private int offInt;

    private int physicalPower;
    private int spellPower;
    private int damageReduction;
    private int damage;
    private int strMod;
    private int dexMod;
    private int intMod;

    private string head;
    private string chest;
    private string legs;
    private string hands;
    private string mainhand;
    private string offhand;

    private bool relentlessPursuit = false;
    private bool lifeAndDeath = false;
    private bool warFrenzy = false;
    private bool bloodAssassin = false;
    private bool divineBulwark = false;
    private bool arcaneWard = false;
    private bool chargeEnergy = false;
    private bool channelLight = false;
    private bool crashingWaves = false;
    private bool channelNature = false;
    private bool heavensBlessing = false;
    private bool naturesCall = false;
    private bool relentlessAssault = false;
    private bool undeadLegion = false;
    private bool faithfulStrikes = false;
    private bool enhancedPyrotechnics = false;
    private bool callBeast = false;
    private bool swordAndBoard = false;

    private string id;
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                id = "";
                head = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[0];
                chest = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[1];
                legs = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[2];
                hands = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[3];
                mainhand = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[4];
                offhand = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5];

                //head stats
                if (head == null || head == "")
                {
                    headPP = 0;
                    headSP = 0;
                    headDR = 0;
                    headStr = 0;
                    headDex = 0;
                    headInt = 0;
                }
                else
                {
                    id = head.Substring(1, head.Length - 1);
                    if (id == "0")//greathelm
                    {
                        headPP = 1;
                        headSP = 0;
                        headDR = 4;
                        headStr = 0;
                        headDex = -1;
                        headInt = 0;
                    }
                    else if (id == "4")//hood
                    {
                        headPP = 1;
                        headSP = 0;
                        headDR = 2;
                        headStr = 0;
                        headDex = 0;
                        headInt = 0;
                    }
                    else if (id == "8")//hat
                    {
                        headPP = 0;
                        headSP = 1;
                        headDR = 1;
                        headStr = 0;
                        headDex = 0;
                        headInt = 0;
                    }
                    else if (id == "12")//mythical greathelm
                    {
                        headPP = 2;
                        headSP = 0;
                        headDR = 5;
                        headStr = 0;
                        headDex = -1;
                        headInt = 0;
                    }
                    else if (id == "16")//shrouded hood
                    {
                        headPP = 2;
                        headSP = 0;
                        headDR = 3;
                        headStr = 0;
                        headDex = 0;
                        headInt = 0;
                    }
                    else if (id == "20")//enchanted hat
                    {
                        headPP = 0;
                        headSP = 2;
                        headDR = 2;
                        headStr = 0;
                        headDex = 0;
                        headInt = 0;
                    }
                    else if (id == "25")//ranger's guise
                    {
                        headPP = 2;
                        headSP = 0;
                        headDR = 3;
                        headStr = 0;
                        headDex = 5;
                        headInt = 0;
                        relentlessPursuit = true;
                    }
                    else if (id == "29")//crown of undeath
                    {
                        headPP = 0;
                        headSP = 2;
                        headDR = 2;
                        headStr = 0;
                        headDex = 0;
                        headInt = 5;
                        lifeAndDeath = true;
                    }
                    else if (id == "30")//warbringer's visage
                    {
                        headPP = 2;
                        headSP = 0;
                        headDR = 5;
                        headStr = 5;
                        headDex = 0;
                        headInt = 0;
                        warFrenzy = true;
                    }
                }

                //chest stats
                if (chest == null || chest == "")
                {
                    chestPP = 0;
                    chestSP = 0;
                    chestDR = 0;
                    chestStr = 0;
                    chestDex = 0;
                    chestInt = 0;
                }
                else
                {
                    id = chest.Substring(1, chest.Length - 1);
                    if (id == "1")//chestplate
                    {
                        chestPP = 1;
                        chestSP = 0;
                        chestDR = 6;
                        chestStr = 0;
                        chestDex = -2;
                        chestInt = 0;
                    }
                    else if (id == "5")//cloak
                    {
                        chestPP = 1;
                        chestSP = 0;
                        chestDR = 4;
                        chestStr = 0;
                        chestDex = 0;
                        chestInt = 0;
                    }
                    else if (id == "9")//robes
                    {
                        chestPP = 0;
                        chestSP = 1;
                        chestDR = 1;
                        chestStr = 0;
                        chestDex = 0;
                        chestInt = 0;
                    }
                    else if (id == "13")//mythical breastplate
                    {
                        chestPP = 2;
                        chestSP = 0;
                        chestDR = 7;
                        chestStr = 0;
                        chestDex = -2;
                        chestInt = 0;
                    }
                    else if (id == "17")//shrouded cloak
                    {
                        chestPP = 2;
                        chestSP = 0;
                        chestDR = 5;
                        chestStr = 0;
                        chestDex = 0;
                        headInt = 0;
                    }
                    else if (id == "21")//enchanted robes
                    {
                        chestPP = 0;
                        chestSP = 2;
                        chestDR = 2;
                        chestStr = 0;
                        chestDex = 0;
                        chestInt = 0;
                    }
                    else if (id == "24")//zyreth's raiments
                    {
                        chestPP = 0;
                        chestSP = 2;
                        chestDR = 2;
                        chestStr = 0;
                        chestDex = 0;
                        chestInt = 5;
                        arcaneWard = true;
                    }
                    else if (id == "27")//cloak of the scarlet slayer
                    {
                        chestPP = 2;
                        chestSP = 0;
                        chestDR = 5;
                        chestStr = 0;
                        chestDex = 5;
                        chestInt = 0;
                        bloodAssassin = true;
                    }
                    else if (id == "28")//aegis of alar'akyr
                    {
                        chestPP = 2;
                        chestSP = 0;
                        chestDR = 7;
                        chestStr = 5;
                        chestDex = 0;
                        chestInt = 0;
                        divineBulwark = true;
                    }
                }

                //legs stats
                if (legs == null || legs == "")
                {
                    legsPP = 0;
                    legsSP = 0;
                    legsDR = 0;
                    legsStr = 0;
                    legsDex = 0;
                    legsInt = 0;
                }
                else
                {
                    id = legs.Substring(1, legs.Length - 1);
                    if (id == "2")//greaves
                    {
                        legsPP = 1;
                        legsSP = 0;
                        legsDR = 5;
                        legsStr = 0;
                        legsDex = -2;
                        legsInt = 0;
                    }
                    else if (id == "6")//boots
                    {
                        legsPP = 1;
                        legsSP = 0;
                        legsDR = 3;
                        legsStr = 0;
                        legsDex = 0;
                        legsInt = 0;
                    }
                    else if (id == "10")//shoes
                    {
                        legsPP = 0;
                        legsSP = 1;
                        legsDR = 1;
                        legsStr = 0;
                        legsDex = 0;
                        legsInt = 0;
                    }
                    else if (id == "14")//mythical greaves
                    {
                        legsPP = 2;
                        legsSP = 0;
                        legsDR = 6;
                        legsStr = 0;
                        legsDex = -2;
                        legsInt = 0;
                    }
                    else if (id == "18")//shrouded boots
                    {
                        legsPP = 2;
                        chestSP = 0;
                        legsDR = 4;
                        legsStr = 0;
                        legsDex = 0;
                        headInt = 0;
                    }
                    else if (id == "22")//enchanted shoes
                    {
                        legsPP = 0;
                        legsSP = 2;
                        legsDR = 2;
                        legsStr = 0;
                        legsDex = 0;
                        legsInt = 0;
                    }
                    else if (id == "31")//wildwalkers
                    {
                        legsPP = 0;
                        legsSP = 2;
                        legsDR = 2;
                        legsStr = 5;
                        legsDex = 0;
                        legsInt = 0;
                        channelNature = true;
                    }
                    else if (id == "33")//wavedancers
                    {
                        legsPP = 2;
                        legsSP = 0;
                        legsDR = 4;
                        legsStr = 0;
                        legsDex = 5;
                        legsInt = 0;
                        crashingWaves = true;
                    }
                }

                //hands stats
                if (hands == null || hands == "")
                {
                    handsPP = 0;
                    handsSP = 0;
                    handsDR = 0;
                    handsStr = 0;
                    handsDex = 0;
                    handsInt = 0;
                }
                else
                {
                    id = hands.Substring(1, hands.Length - 1);
                    if (id == "3")//gloves
                    {
                        handsPP = 1;
                        handsSP = 0;
                        handsDR = 3;
                        handsStr = 0;
                        handsDex = -1;
                        handsInt = 0;
                    }
                    else if (id == "7")//wraps
                    {
                        handsPP = 1;
                        handsSP = 0;
                        handsDR = 1;
                        handsStr = 0;
                        handsDex = 0;
                        handsInt = 0;
                    }
                    else if (id == "11")//cuffs
                    {
                        handsPP = 0;
                        handsSP = 1;
                        handsDR = 1;
                        handsStr = 0;
                        handsDex = 0;
                        handsInt = 0;
                    }
                    else if (id == "15")//mythical gauntlets
                    {
                        handsPP = 2;
                        handsSP = 0;
                        handsDR = 4;
                        handsStr = 0;
                        handsDex = -1;
                        handsInt = 0;
                    }
                    else if (id == "19")//shrouded wraps
                    {
                        handsPP = 2;
                        chestSP = 0;
                        handsDR = 2;
                        handsStr = 0;
                        handsDex = 0;
                        headInt = 0;
                    }
                    else if (id == "23")//enchanted cuffs
                    {
                        handsPP = 0;
                        handsSP = 2;
                        handsDR = 2;
                        handsStr = 0;
                        handsDex = 0;
                        handsInt = 0;
                    }
                    else if (id == "26")//bracers of eidiril
                    {
                        handsPP = 2;
                        handsSP = 0;
                        handsDR = 4;
                        handsStr = 0;
                        handsDex = 0;
                        handsInt = 5;
                        channelLight = true;
                    }
                    else if (id == "32")//chi caller
                    {
                        handsPP = 2;
                        handsSP = 0;
                        handsDR = 2;
                        handsStr = 0;
                        handsDex = 5;
                        handsInt = 0;
                        chargeEnergy = true;
                    }
                }

                //main hand stats
                if (mainhand == null || mainhand == "")
                {
                    mainDMG = 0;
                    mainSP = 0;
                    mainStr = 0;
                    mainDex = 0;
                    mainInt = 0;
                }
                else
                {
                    id = mainhand.Substring(1, mainhand.Length - 1);
                    if (id == "7" || id == "10" || id == "13")//10 damagers
                    {
                        mainDMG = 10;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "0" || id == "3" || id == "6")//7 damagers
                    {
                        mainDMG = 7;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "9" || id == "12")//5 damagers
                    {
                        mainDMG = 5;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "2" || id == "5")//3 damagers
                    {
                        mainDMG = 3;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "11")//staff +
                    {
                        mainDMG = 4;
                        mainSP = 2;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "4")//staff
                    {
                        mainDMG = 2;
                        mainSP = 1;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 0;
                    }
                    else if (id == "14")//the grand conjurer's staff
                    {
                        mainDMG = 4;
                        mainSP = 2;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 5;
                        enhancedPyrotechnics = true;
                    }
                    else if (id == "15")//kairena's bow of the hunt
                    {
                        mainDMG = 10;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 5;
                        mainInt = 0;
                        callBeast = true;
                    }
                    else if (id == "16")//heaven's breath
                    {
                        mainDMG = 5;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 5;
                        heavensBlessing = true;
                    }
                    else if (id == "17")//dagger of the aelestines
                    {
                        mainDMG = 5;
                        mainSP = 0;
                        mainStr = 0;
                        mainDex = 5;
                        mainInt = 0;
                        relentlessAssault = true;
                    }
                    else if (id == "18")//faith's edge
                    {
                        mainDMG = 10;
                        mainSP = 0;
                        mainStr = 5;
                        mainDex = 0;
                        mainInt = 0;
                        faithfulStrikes = true;
                    }
                    else if (id == "19")//death's call
                    {
                        mainDMG = 4;
                        mainSP = 2;
                        mainStr = 0;
                        mainDex = 0;
                        mainInt = 5;
                        undeadLegion = true;
                    }
                    else if (id == "20")//the defender
                    {
                        mainDMG = 5;
                        mainSP = 0;
                        mainStr = 5;
                        mainDex = 0;
                        mainInt = 0;
                        swordAndBoard = true;
                    }
                    else if (id == "21")//naturespine
                    {
                        mainDMG = 4;
                        mainSP = 2;
                        mainStr = 5;
                        mainDex = 0;
                        mainInt = 0;
                        naturesCall = true;
                    }
                }

                //off hand stats
                if (offhand == null || offhand == "")
                {
                    offDMG = 0;
                    offSP = 0;
                    offStr = 0;
                    offDex = 0;
                    offInt = 0;
                }
                else
                {
                    id = offhand.Substring(1, offhand.Length - 1);
                    if (id == "1" || id == "8")//shields
                    {
                        offDMG = 0;
                        offSP = 0;
                        offStr = 0;
                        offDex = 0;
                        offInt = 0;
                    }
                    else if (id == "9" || id == "12")//5 damagers
                    {
                        offDMG = 5;
                        offSP = 0;
                        offStr = 0;
                        offDex = 0;
                        offInt = 0;
                    }
                    else if (id == "2" || id == "5")//3 damagers
                    {
                        offDMG = 3;
                        offSP = 0;
                        offStr = 0;
                        offDex = 0;
                        offInt = 0;
                    }
                    else if (id == "16")//heaven's breath
                    {
                        offDMG = 5;
                        offSP = 0;
                        offStr = 0;
                        offDex = 0;
                        offInt = 5;
                        heavensBlessing = true;
                    }
                    else if (id == "17")//dagger of the aelestines
                    {
                        offDMG = 5;
                        offSP = 0;
                        offStr = 0;
                        offDex = 5;
                        offInt = 0;
                        relentlessAssault = true;
                    }
                    else if (id == "20")//the defender
                    {
                        offDMG = 5;
                        offSP = 0;
                        offStr = 5;
                        offDex = 0;
                        offInt = 0;
                        swordAndBoard = true;
                    }
                }

                //sums stats and write to player info
                physicalPower = headPP + chestPP + legsPP + handsPP;
                spellPower = headSP + chestSP + legsSP + handsSP + mainSP + offSP;
                damageReduction = headDR + chestDR + legsDR + handsDR;
                damage = mainDMG + offDMG;
                strMod = headStr + chestStr + legsStr + handsStr + mainStr + offStr + 5 * players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ValorRunesActive;
                dexMod = headDex + chestDex + legsDex + handsDex + mainDex + offDex + 5 * players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SwiftnessRunesActive;
                intMod = headInt + chestInt + legsInt + handsInt + mainInt + offInt + 5 * players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WisdomRunesActive;

                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PhysicalPower = physicalPower + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower = spellPower + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction = damageReduction + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod = damage + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod = strMod + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod = dexMod + "";
                players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod = intMod + "";

                if (relentlessPursuit)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Relentless Pursuit";
                else if (lifeAndDeath)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Life and Death";
                else if (warFrenzy)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "War Frenzy";
                else if (bloodAssassin)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Blood Assassin";
                else if (divineBulwark)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Divine Bulwark";
                else if (arcaneWard)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Arcane Ward";
                else if (chargeEnergy)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Charge Energy";
                else if (channelLight)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Channel Light";
                else if (crashingWaves)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Crashing Waves";
                else if (channelNature)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Channel Nature";
                else if (heavensBlessing)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Heaven's Blessing";
                else if (naturesCall)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Nature's Call";
                else if (relentlessAssault)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Relentless Assault";
                else if (undeadLegion)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Undead Legion";
                else if (faithfulStrikes)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Faithful Strikes";
                else if (enhancedPyrotechnics)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Enhanced Pyrotechnics";
                else if (callBeast)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Call Beast";
                else if (swordAndBoard)
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = "Sword and Board";
                else
                    players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect = null;

                relentlessPursuit = false;
                lifeAndDeath = false;
                warFrenzy = false;
                bloodAssassin = false;
                divineBulwark = false;
                arcaneWard = false;
                chargeEnergy = false;
                channelLight = false;
                crashingWaves = false;
                channelNature = false;
                heavensBlessing = false;
                naturesCall = false;
                relentlessAssault = false;
                undeadLegion = false;
                faithfulStrikes = false;
                enhancedPyrotechnics = false;
                callBeast = false;
                swordAndBoard = false;
            }
        }
    }
}
