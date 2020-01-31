using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class temporaryEffectsManager : MonoBehaviour
{
    public GameObject characterSheet;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private string message;
    // Start is called before the first frame update
    void Start()
    {
        serverInfo = GameObject.FindGameObjectWithTag("Server Info");
        players = GameObject.FindGameObjectsWithTag("Player Info");
        string fileName = "playerInfo";
        var sr = File.OpenText(fileName);
        string username = sr.ReadLine();
        characterName = sr.ReadLine();
        sr.Close();
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                playerID = i;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if(characterSheet.GetComponent<characterSheetController>().characterName == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                playerID = i;
        }

        message = "";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Rally)
            message += "- Rally\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk)
            message += "- Berserk\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap)
            message += "- Trap\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark)
            message += "- Hunter's Mark\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection)
            message += "- Divine Protection\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner)
            message += "- Judge, Jury, and Executioner\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedic)
            message += "- BattleMedic\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion)
            message += "- Skeletal Companion\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack)
            message += "- Surprise Attack\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation)
            message += "- Preparation\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation)
            message += "- Worg Transformation\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
            message += "- Bear Transformation\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InfusedStrike > 0)
            message += "- Infused Strike\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide)
            message += "- Thick Hide\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist)
            message += "- The Way of the Stone Fist\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance > 0)
            message += "- Flowing Waves Stance\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra > 0)
            message += "- Rejuvenating Mantra\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus)
            message += "- Inner Focus\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intimidation)
            message += "- Intimidation\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
            message += "- Taunt\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind)
            message += "- Second Wind\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterCombatant)
            message += "- Master Combatant\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Sprint)
            message += "- Sprint\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer)
            message += "- Master Adventurer\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneShield)
            message += "- Arcane Shield\n";
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterSpellcaster)
            message += "- Master Spellcaster\n";

        gameObject.GetComponent<Text>().text = message;
    }
}
