using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class abilitiesManager : MonoBehaviour
{
    public GameObject generalScripts;
    public GameObject[] pages;
    public GameObject[] abilities;
    public GameObject[] names;
    public GameObject[] descriptions;
    public GameObject[] covers;
    public GameObject quest;
    public GameObject lobby;
    public GameObject actionPhase;
    public GameObject preview;
    public GameObject dealingDamage;
    public GameObject receivingDamage;
    public GameObject resolve;
    public GameObject roll;
    public GameObject failMessage;
    public GameObject reinforcingCharge;
    public GameObject healWindow;
    public GameObject resurrectRoll;
    public GameObject skeletalCompanion;
    public GameObject soulShredWindow;
    public GameObject scavengeForLootWindow;
    public GameObject transfusion;
    public GameObject masterSpellcaster;
    public GameObject crownOfUndeath;
    public GameObject wildwalkers;


    private GameObject player;
    private GameObject[] players;
    private GameObject serverInfo;
    private int playerID;
    private string characterName;
    private int page;
    private List<string> unlockedAbilities = new List<string>();
    private List<string> lockedAbilities = new List<string>();
    private int level;
    private int strength;
    private int dexterity;
    private int intellect;
    private int abilityCharges;
    private bool inCombat;
    private bool beforeCombat;
    private bool yourCombatTurn;
    private bool yourAction;
    private bool whileAttacking;
    private bool whileDefending;
    private bool afterAttacking;
    private bool isDead;
    private bool beforeAttacking;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnEnable()
    {
        page = 0;
        if (players != null)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = null;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 0;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //activate only the page that is dictated by page int
        for (int i = 0; i < pages.Length; i++)
        {
            if(i == page)
                pages[i].SetActive(true);
            else
                pages[i].SetActive(false);
        }
    }

    public void Setup()
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

    public void FormatAbilities()
    {
        for (int i = 0; i < unlockedAbilities.Count; i++)
        {
            abilities[i].SetActive(true);
            covers[i].SetActive(false);
            AssignAbility(i, unlockedAbilities[i]);
        }
        int mod = 4 - unlockedAbilities.Count % 4;
        if (mod == 4)
            mod = 0;
        for (int i = unlockedAbilities.Count + mod; i < lockedAbilities.Count + unlockedAbilities.Count + mod; i++)
        {
            abilities[i].SetActive(true);
            covers[i].SetActive(true);
            AssignAbility(i, lockedAbilities[i - (unlockedAbilities.Count + mod)]);
        }
        for (int i = unlockedAbilities.Count; i < unlockedAbilities.Count + mod; i++)
        {
            abilities[i].SetActive(false);
        }
        for (int i = lockedAbilities.Count + unlockedAbilities.Count + mod; i < 20; i++)
        {
            abilities[i].SetActive(false);
        }
    }

    public void AssignStats()
    {
        level = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level);
        strength = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
        dexterity = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
        intellect = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
        abilityCharges = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);

        bool fightingInCombat = false;
        for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
        {
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                fightingInCombat = true;
        }
        inCombat = fightingInCombat;
        beforeCombat = lobby.activeInHierarchy;
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] == characterName && actionPhase.activeInHierarchy)
            yourAction = true;
        else
            yourAction = false;
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatIterator] == characterName)
            yourCombatTurn = true;
        else
            yourCombatTurn = false;
        whileAttacking = dealingDamage.activeInHierarchy;
        whileDefending = receivingDamage.activeInHierarchy;
        afterAttacking = resolve.activeInHierarchy;
        isDead = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead;
        beforeAttacking = roll.activeInHierarchy;
    }

    void SpendAbilityCharge()
    {
        int temp = abilityCharges;
        temp--;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = temp + "";
        quest.GetComponent<questController>().abilityChargesSpent++;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpentAC++;


        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Channel Nature")
            wildwalkers.SetActive(true);
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterSpellcaster)
            masterSpellcaster.SetActive(true);
    }

    bool HasAbilityCharges()
    {
        return int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges) > 0;
    }

    bool EnemyIsFrozen()
    {
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze > 0)
                return true;
        }
        return false;
    }

    void AssignAbility(int i, string n)
    {
        abilities[i].GetComponent<Image>().sprite = (Sprite)Resources.Load("Abilities/" + n, typeof(Sprite));
        //names[i].GetComponent<Text>().text = n;
        switch (n)
        {
            //Warrior Abilities
            case "Reinforcing Charge":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Move to another player’s location and aid that player in combat (They must be in combat).";
                break;
            case "Bash":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "After a successfully power roll, deal 2x your damage to the enemy instead of normal damage (Doesn’t stack).";
                break;
            case "Rally":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "When in combat, all damaged allies recover 12 health and all allies gain +2 to their next power roll (+2 doesn’t stack with any other bonuses to power rolls), and you gain +5 damage reduction until your next turn.";
                break;
            case "Revenge":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "If your power roll fails to beat the monster’s power level, you deal your Strength in damage to the monster while receiving the monster’s damage as well.";
                break;
            case "Berserk":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "The next time you deal damage, deal additional damage equal to the amount of health that your character is missing.";
                break;
            //Hunter Abilities
            case "Evade":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Successfully flee from any combat (even if text prevents fleeing except boss fights).";
                break;
            case "Trap":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "Before any power rolls in combat, lay out a trap that last the entire combat or until triggered.  The trap is triggered when you fail a power roll, and when triggered the trap prevents the monster from damaging you for this turn and deals your Dexterity in damage to it.";
                break;
            case "Quick Shot":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Deal your damage to the enemy monster (can be used any number of times on hunter’s turn during combat).";
                break;
            case "Hunter's Mark":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "Mark a monster, gaining +3 to your next power roll & +5 damage for 1 turn (cannot stack).";
                break;
            case "Arrow Volley":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "On a successful power roll after dealing normal damage,  deal your damage again, and roll a dice.  If the dice is even, deal your damage to the monster again and repeat until you roll an odd number.";
                break;
            //Sorcerer Abilities
            case "Fireball":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, deal damage with your Intellect instead of damage.";
                break;
            case "Freeze":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "If the target is not frozen, make a spell power roll instead of a normal power roll, and on a successful roll, the monster is frozen until the end of your next turn, and cannot deal its damage to anyone while frozen.  Still deals base damage.";
                break;
            case "Portal":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Teleport yourself to any tile on the board (as long as the tile has been unlocked and is not past any barriers that are locked).";
                break;
            case "Arcane Infusion":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, gain 2 ability charges.";
                break;
            case "Meteor":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, use all your ability charges and deal 10 damage to a monster for each.";
                break;
            //Paladin Abilities
            case "Righteous Fury":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Deal your weapon damage to an enemy on a successful power roll and heal yourself for the damage done.";
                break;
            case "Salvation":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "May be cast after death to resurrect you, if you have an ability charge (may only be cast once per combat instance).";
                break;
            case "Divine Protection":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Ignore the next damage you receive (does not stack, but lasts though different combats until used- can only be cast by the player once during combat instance).";
                break;
            case "Justicar's Smite":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "On a successful power roll instead of dealing normal damage, deal your Strength as damage and recover your Strength as health.";
                break;
            case "Judge, Jury, and Executioner":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "Your next power roll has +5 and if it is a critical hit, instantly defeat the monster (Cannot stack and instant defeat does not affect Bosses).";
                break;
            //Cleric Abilities
            case "Heal":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Restore 18 health to any player at any time.";
                break;
            case "Holy Rejuvenation":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "Restore 9 health to all players at any time.";
                break;
            case "Battle Medic":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Cast before combat with allies and the first time an ally falls below half health, cast Heal on them for free.  Lasts until combat is over or every ally has fallen below half health.";
                break;
            case "Ultimate Sacrifice":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "Upon death in combat, if you have an ability charge, all other players are healed to full health and given an ability charge (can only be used per combat instance).";
                break;
            case "Spirit Bomb":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "On a successful power roll, deal damage to a monster equal to your Intellect level and split up the amount dealt for you and your allies to recover as health.";
                break;
            //Necromancer Abilities
            case "Dark Resurrection":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Roll a dice.  If it is 4 or higher, bring a dead player back to life.";
                break;
            case "Death Blast":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, you deal damage with your Intellect instead of your normal damage and if this kills a monster, gain 2 ability charges.";
                break;
            case "Feed":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, consume an enemy monster’s health, damaging the enemy one health at a time, healing yourself for the amount lost, until yours is full or the monster is dead (cannot cast against bosses).";
                break;
            case "Raise the Dead":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "After defeating a monster, cast this to raise a Skeletal Companion.  The Skeletal Companion aids you in your next combat and takes a turn in that combat just like a player.  The Skeletal Companion has a power equal to 3x your spell power and adds 2 dice rolls during power rolls just like a player.  The Skeletal Companion has damage equal to your spell power and health equal to 4x your spell power.  The Skeletal Companion taunts on every one of your turns while it is still “alive”.  The Skeletal Companion automatically dies when the combat is over if it did not die during the combat. (Cannot stack and should it occur though special items, recasting this ability replaces the current Skeletal Companion with a new one, but this can only be cast once during a combat instance)";
                break;
            case "Soul Shred":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "Make a spell power roll instead of a normal power roll, and on a successful roll, spend as much health as you would like (leaving at least 1 left over) to deal that amount as damage to an enemy.";
                break;
            //Rogue Abilities
            case "Surprise Attack":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Lower an enemy monster’s power by 2 (can only be used before the first combat roll of each combat, but the effect lasts the entire combat.  The effect does not affect bosses).";
                break;
            case "Raze":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "On a successful power roll, add your Dexterity level to your damage (Cannot stack).";
                break;
            case "Stealth":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Can maneuver around a monster undetected, preventing combat, but awarding 1 less treasure than the monster holds (the monster is then shuffled back into the deck and can only be cast before combat).";
                break;
            case "Combination Strike":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "After a successful power roll, cast this to take another turn.  If your second power roll is successful, you have +10 damage this turn.";
                break;
            case "Preparation":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "Gain your Dexterity level worth of points toward your next power roll, and for every point your power roll surpasses the monster’s power, deal that much as extra damage to your normal damage (Cannot stack).";
                break;
            //Druid Abilities
            case "Worg Transformation":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Move an extra 2 spaces after rolling for movement (stays in Worg Transformation until another transformation is activated).";
                break;
            case "Bear Transformation":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "Deal damage with your Strength instead of your weapon damage for the rest of the combat phase (all power rolls made while in transformation are spell power rolls).";
                break;
            case "Tend to Wounds":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "Restore 20 health if you are in a Transformation, or 10 if you are not.";
                break;
            case "Infused Strike":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "Add your spell power to your damage for 1 turn (can stack).";
                break;
            case "Thick Hide":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "While in a transformation in combat, gain your Strength as damage reduction for the rest of combat (Cannot stack).";
                break;
            //Monk Abilities
            case "The Way of the Stone Fist":
                names[i].GetComponent<Text>().text = "Level 1: " + n;
                descriptions[i].GetComponent<Text>().text = "Gain +8 damage for the rest of the combat instance (cannot stack).";
                break;
            case "Flowing Waves Stance":
                names[i].GetComponent<Text>().text = "Level 5: " + n;
                descriptions[i].GetComponent<Text>().text = "For your next 2 turns, when receiving damage, roll a dice, and if the dice is 4 or higher, you dodge the attack, ignoring the damage.";
                break;
            case "Rejuvenating Mantra":
                names[i].GetComponent<Text>().text = "Level 10: " + n;
                descriptions[i].GetComponent<Text>().text = "For your next 5 turns, recover 4 health each turn if you are in combat, or 8 health each turn if you are not.";
                break;
            case "Rapid Strikes":
                names[i].GetComponent<Text>().text = "Level 15: " + n;
                descriptions[i].GetComponent<Text>().text = "On a successful power roll,  after you deal your damage, deal  an extra 5 damage for each of the following (if your Dexterity is at least 8, if your Dexterity is at least 10, if your Dexterity is at least 12, if your Dexterity is at least 14, if your Dexterity is at least 16) for a max of 5 attacks of 5 damage each.";
                break;
            case "Inner Focus":
                names[i].GetComponent<Text>().text = "Level 20: " + n;
                descriptions[i].GetComponent<Text>().text = "While in combat, you have +4 damage and +2 to power rolls until you take any damage (cannot stack).";
                break;
            //Strength Abilities
            case "Intimidation":
                names[i].GetComponent<Text>().text = "Str 8: " + n;
                descriptions[i].GetComponent<Text>().text = "Before your next power roll, cast this to reduce the power of the enemy you are attacking by 4 for 1 combat round.";
                break;
            case "Taunt":
                names[i].GetComponent<Text>().text = "Str 10: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "When in combat with others, on another player’s turn, before they make their power roll, declare taunt to receive any damage that your ally might take that turn instead of them.";
                break;
            case "Second Wind":
                names[i].GetComponent<Text>().text = "Str 12: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "While in combat, every round you do not take damage, recover 2 health instead.  While not in combat, each turn you take that you do not encounter anything, recover  8 health.";
                break;
            case "Master Combatant":
                names[i].GetComponent<Text>().text = "Str 14: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "Once per combat instance you can add your Strength either to your damage or damage reduction for one combat round.";
                break;
            //Dexterity Abilities
            case "Scavenge for Loot":
                names[i].GetComponent<Text>().text = "Dex 8: " + n;
                descriptions[i].GetComponent<Text>().text = "Cast when drawing a treasure card and roll a dice.  If the roll is a 4 or higher, draw an additional treasure card.";
                break;
            case "Dodge":
                names[i].GetComponent<Text>().text = "Dex 10: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "When in combat, on your turn, if your power roll fails, you can declare dodge and call out a number (1-6) and roll a dice after.  If the roll matches the number you said, you dodge the attack, ignoring the damage.";
                break;
            case "Sprint":
                names[i].GetComponent<Text>().text = "Dex 12: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "Move an extra 2 tiles during the movement phase (does not stack with other movement bonuses).";
                break;
            case "Master Adventurer":
                names[i].GetComponent<Text>().text = "Dex 14: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "When encountering a card that requires either your strength, dexterity, or intellect to surpass a certain value to succeed, roll a dice, and if the roll is 4 or higher, you automatically succeed.";
                break;
            //Intellect Abilities
            case "Arcane Shield":
                names[i].GetComponent<Text>().text = "Int 8: " + n;
                descriptions[i].GetComponent<Text>().text = "Grants 2x the caster’s spell power as damage reduction for 1 combat round.";
                break;
            case "Focus":
                names[i].GetComponent<Text>().text = "Int 10: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "When in combat, on your turn, after rolling your 2 dice, you may declare focus and re-roll any dice that land on 1 (cannot use Focus more than once per power roll or if the roll was a critical failure).";
                break;
            case "Transfusion":
                names[i].GetComponent<Text>().text = "Int 12: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "At any non-combat point, the player may discard as many treasure cards as they would like, gaining an ability charge for each.";
                break;
            case "Master Spellcaster":
                names[i].GetComponent<Text>().text = "Int 14: " + n + " (Passive)";
                descriptions[i].GetComponent<Text>().text = "After casting a spell, the player rolls a dice and if it lands on 5 or 6, the ability charge spent is refunded to the player.";
                break;
            default:
                descriptions[i].GetComponent<Text>().text = "Error: Failed to link name to ability.";
                break;
        }
    }

    public void SetAbilities()
    {
        //clear lists
        unlockedAbilities.Clear();
        lockedAbilities.Clear();

        //assign abilities to locked or unlocked
        switch (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class)
        {
            case "Warrior":
                unlockedAbilities.Add("Reinforcing Charge");
                if (level >= 5)
                    unlockedAbilities.Add("Bash");
                else
                    lockedAbilities.Add("Bash");
                if (level >= 10)
                    unlockedAbilities.Add("Rally");
                else
                    lockedAbilities.Add("Rally");
                if (level >= 15)
                    unlockedAbilities.Add("Revenge");
                else
                    lockedAbilities.Add("Revenge");
                if (level >= 20)
                    unlockedAbilities.Add("Berserk");
                else
                    lockedAbilities.Add("Berserk");
                break;
            case "Hunter":
                unlockedAbilities.Add("Evade");
                if (level >= 5)
                    unlockedAbilities.Add("Trap");
                else
                    lockedAbilities.Add("Trap");
                if (level >= 10)
                    unlockedAbilities.Add("Quick Shot");
                else
                    lockedAbilities.Add("Quick Shot");
                if (level >= 15)
                    unlockedAbilities.Add("Hunter's Mark");
                else
                    lockedAbilities.Add("Hunter's Mark");
                if (level >= 20)
                    unlockedAbilities.Add("Arrow Volley");
                else
                    lockedAbilities.Add("Arrow Volley");
                break;
            case "Sorcerer":
                unlockedAbilities.Add("Fireball");
                if (level >= 5)
                    unlockedAbilities.Add("Freeze");
                else
                    lockedAbilities.Add("Freeze");
                if (level >= 10)
                    unlockedAbilities.Add("Portal");
                else
                    lockedAbilities.Add("Portal");
                if (level >= 15)
                    unlockedAbilities.Add("Arcane Infusion");
                else
                    lockedAbilities.Add("Arcane Infusion");
                if (level >= 20)
                    unlockedAbilities.Add("Meteor");
                else
                    lockedAbilities.Add("Meteor");
                break;
            case "Paladin":
                unlockedAbilities.Add("Righteous Fury");
                if (level >= 5)
                    unlockedAbilities.Add("Salvation");
                else
                    lockedAbilities.Add("Salvation");
                if (level >= 10)
                    unlockedAbilities.Add("Divine Protection");
                else
                    lockedAbilities.Add("Divine Protection");
                if (level >= 15)
                    unlockedAbilities.Add("Justicar's Smite");
                else
                    lockedAbilities.Add("Justicar's Smite");
                if (level >= 20)
                    unlockedAbilities.Add("Judge, Jury, and Executioner");
                else
                    lockedAbilities.Add("Judge, Jury, and Executioner");
                break;
            case "Cleric":
                unlockedAbilities.Add("Heal");
                if (level >= 5)
                    unlockedAbilities.Add("Holy Rejuvenation");
                else
                    lockedAbilities.Add("Holy Rejuvenation");
                if (level >= 10)
                    unlockedAbilities.Add("Battle Medic");
                else
                    lockedAbilities.Add("Battle Medic");
                if (level >= 15)
                    unlockedAbilities.Add("Ultimate Sacrifice");
                else
                    lockedAbilities.Add("Ultimate Sacrifice");
                if (level >= 20)
                    unlockedAbilities.Add("Spirit Bomb");
                else
                    lockedAbilities.Add("Spirit Bomb");
                break;
            case "Necromancer":
                unlockedAbilities.Add("Dark Resurrection");
                if (level >= 5)
                    unlockedAbilities.Add("Death Blast");
                else
                    lockedAbilities.Add("Death Blast");
                if (level >= 10)
                    unlockedAbilities.Add("Feed");
                else
                    lockedAbilities.Add("Feed");
                if (level >= 15)
                    unlockedAbilities.Add("Raise the Dead");
                else
                    lockedAbilities.Add("Raise the Dead");
                if (level >= 20)
                    unlockedAbilities.Add("Soul Shred");
                else
                    lockedAbilities.Add("Soul Shred");
                break;
            case "Rogue":
                unlockedAbilities.Add("Surprise Attack");
                if (level >= 5)
                    unlockedAbilities.Add("Raze");
                else
                    lockedAbilities.Add("Raze");
                if (level >= 10)
                    unlockedAbilities.Add("Stealth");
                else
                    lockedAbilities.Add("Stealth");
                if (level >= 15)
                    unlockedAbilities.Add("Combination Strike");
                else
                    lockedAbilities.Add("Combination Strike");
                if (level >= 20)
                    unlockedAbilities.Add("Preparation");
                else
                    lockedAbilities.Add("Preparation");
                break;
            case "Druid":
                unlockedAbilities.Add("Worg Transformation");
                if (level >= 5)
                    unlockedAbilities.Add("Bear Transformation");
                else
                    lockedAbilities.Add("Bear Transformation");
                if (level >= 10)
                    unlockedAbilities.Add("Tend to Wounds");
                else
                    lockedAbilities.Add("Tend to Wounds");
                if (level >= 15)
                    unlockedAbilities.Add("Infused Strike");
                else
                    lockedAbilities.Add("Infused Strike");
                if (level >= 20)
                    unlockedAbilities.Add("Thick Hide");
                else
                    lockedAbilities.Add("Thick Hide");
                break;
            case "Monk":
                unlockedAbilities.Add("The Way of the Stone Fist");
                if (level >= 5)
                    unlockedAbilities.Add("Flowing Waves Stance");
                else
                    lockedAbilities.Add("Flowing Waves Stance");
                if (level >= 10)
                    unlockedAbilities.Add("Rejuvenating Mantra");
                else
                    lockedAbilities.Add("Rejuvenating Mantra");
                if (level >= 15)
                    unlockedAbilities.Add("Rapid Strikes");
                else
                    lockedAbilities.Add("Rapid Strikes");
                if (level >= 20)
                    unlockedAbilities.Add("Inner Focus");
                else
                    lockedAbilities.Add("Inner Focus");
                break;
        }

        //add general abilities to locked or unlocked lists

        //strength based
        if (strength >= 8)
            unlockedAbilities.Add("Intimidation");
        else
            lockedAbilities.Add("Intimidation");
        if (strength >= 10)
            unlockedAbilities.Add("Taunt");
        else
            lockedAbilities.Add("Taunt");
        if (strength >= 12)
        {
            unlockedAbilities.Add("Second Wind");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind = true;
        }
        else
        {
            lockedAbilities.Add("Second Wind");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind = false;
        }
        if (strength >= 14)
            unlockedAbilities.Add("Master Combatant");
        else
            lockedAbilities.Add("Master Combatant");

        //dexterity based
        if (dexterity >= 8)
            unlockedAbilities.Add("Scavenge for Loot");
        else
            lockedAbilities.Add("Scavenge for Loot");
        if (dexterity >= 10)
            unlockedAbilities.Add("Dodge");
        else
            lockedAbilities.Add("Dodge");
        if (dexterity >= 12)
        {
            unlockedAbilities.Add("Sprint");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Sprint = true;
        }
        else
        {
            lockedAbilities.Add("Sprint");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Sprint = false;
        }
        if (dexterity >= 14)
        {
            unlockedAbilities.Add("Master Adventurer");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer = true;
        }
        else
        {
            lockedAbilities.Add("Master Adventurer");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterAdventurer = false;
        }

        //intellect based
        if (intellect >= 8)
            unlockedAbilities.Add("Arcane Shield");
        else
            lockedAbilities.Add("Arcane Shield");
        if (intellect >= 10)
            unlockedAbilities.Add("Focus");
        else
            lockedAbilities.Add("Focus");
        if (intellect >= 12)
            unlockedAbilities.Add("Transfusion");
        else
            lockedAbilities.Add("Transfusion");
        if (intellect >= 14)
        {
            unlockedAbilities.Add("Master Spellcaster");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterSpellcaster = true;
        }
        else
        {
            lockedAbilities.Add("Master Spellcaster");
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterSpellcaster = false;
        }
    }

    public void Next()
    {
        page++;
        if (page >= pages.Length)
            page = pages.Length - 1;
    }

    public void Back()
    {
        page--;
        if (page < 0)
            page = 0;
    }

    public void Quit()
    {
        generalScripts.GetComponent<gameMenuManager>().exit();
    }

    IEnumerator Failed()
    {
        failMessage.SetActive(true);
        yield return new WaitForSeconds(1);
        failMessage.SetActive(false);
    }

    public void Ability1()
    {
        UseAbility(unlockedAbilities[0]);
    }
    public void Ability2()
    {
        UseAbility(unlockedAbilities[1]);
    }
    public void Ability3()
    {
        UseAbility(unlockedAbilities[2]);
    }
    public void Ability4()
    {
        UseAbility(unlockedAbilities[3]);
    }
    public void Ability5()
    {
        UseAbility(unlockedAbilities[4]);
    }
    public void Ability6()
    {
        UseAbility(unlockedAbilities[5]);
    }
    public void Ability7()
    {
        UseAbility(unlockedAbilities[6]);
    }
    public void Ability8()
    {
        UseAbility(unlockedAbilities[7]);
    }
    public void Ability9()
    {
        UseAbility(unlockedAbilities[8]);
    }
    public void Ability10()
    {
        UseAbility(unlockedAbilities[9]);
    }
    public void Ability11()
    {
        UseAbility(unlockedAbilities[10]);
    }
    public void Ability12()
    {
        UseAbility(unlockedAbilities[11]);
    }
    public void Ability13()
    {
        UseAbility(unlockedAbilities[12]);
    }
    public void Ability14()
    {
        UseAbility(unlockedAbilities[13]);
    }
    public void Ability15()
    {
        UseAbility(unlockedAbilities[14]);
    }
    public void Ability16()
    {
        UseAbility(unlockedAbilities[15]);
    }
    public void Ability17()
    {
        UseAbility(unlockedAbilities[16]);
    }
    public void Ability18()
    {
        UseAbility(unlockedAbilities[17]);
    }
    public void Ability19()
    {
        UseAbility(unlockedAbilities[18]);
    }
    public void Ability20()
    {
        UseAbility(unlockedAbilities[19]);
    }

    public void UseAbility(string ability)
    {
        switch (ability)
        {
            //Warrior Abilities
            case "Reinforcing Charge":
                ReinforcingCharge();
                break;
            case "Bash":
                Bash();
                 break;
            case "Rally":
                Rally();
                break;
            case "Revenge":
                Revenge();
                break;
            case "Berserk":
                Berserk();
                break;
            //Hunter Abilities
            case "Evade":
                Evade();
                break;
            case "Trap":
                Trap();
                break;
            case "Quick Shot":
                QuickShot();
                break;
            case "Hunter's Mark":
                HuntersMark();
                break;
            case "Arrow Volley":
                ArrowVolley();
                break;
            //Sorcerer Abilities
            case "Fireball":
                Fireball();
                break;
            case "Freeze":
                Freeze();
                break;
            case "Portal":
                Portal();
                break;
            case "Arcane Infusion":
                ArcaneInfusion();
                break;
            case "Meteor":
                Meteor();
                break;
            //Paladin Abilities
            case "Righteous Fury":
                RighteousFury();
                break;
            case "Salvation":
                Salvation();
                break;
            case "Divine Protection":
                DivineProtection();
                break;
            case "Justicar's Smite":
                JusticarsSmite();
                break;
            case "Judge, Jury, and Executioner":
                JudgeJuryAndExecutioner();
                break;
            //Cleric Abilities
            case "Heal":
                Heal();
                break;
            case "Holy Rejuvenation":
                HolyRejuvenation();
                break;
            case "Battle Medic":
                BattleMedic();
                break;
            case "Ultimate Sacrifice":
                UltimateSacrifice();
                break;
            case "Spirit Bomb":
                SpiritBomb();
                break;
            //Necromancer Abilities
            case "Dark Resurrection":
                DarkResurrection();
                break;
            case "Death Blast":
                DeathBlast();
                break;
            case "Feed":
                Feed();
                break;
            case "Raise the Dead":
                RaiseTheDead();
                break;
            case "Soul Shred":
                SoulShred();
                break;
            //Rogue Abilities
            case "Surprise Attack":
                SurpriseAttack();
                break;
            case "Raze":
                Raze();
                break;
            case "Stealth":
                Stealth();
                break;
            case "Combination Strike":
                CombinationStrike();
                break;
            case "Preparation":
                Preparation();
                break;
            //Druid Abilities
            case "Worg Transformation":
                WorgTransformatiom();
                break;
            case "Bear Transformation":
                BearTransformation();
                break;
            case "Tend to Wounds":
                TendToWounds();
                break;
            case "Infused Strike":
                InfusedStrike();
                break;
            case "Thick Hide":
                ThickHide();
                break;
            //Monk Abilities
            case "The Way of the Stone Fist":
                TheWayOfTheStoneFist();
                break;
            case "Flowing Waves Stance":
                FlowingWavesStance();
                break;
            case "Rejuvenating Mantra":
                RejuvenatingMantra();
                break;
            case "Rapid Strikes":
                RapidStrikes();
                break;
            case "Inner Focus":
                InnerFocus();
                break;
            //Strength Abilities
            case "Intimidation":
                Intimidation();
                break;
            case "Taunt":
                Taunt();
                break;
            case "Second Wind":
                SecondWind();
                break;
            case "Master Combatant":
                MasterCombatant();
                break;
            //Dexterity Abilities
            case "Scavenge for Loot":
                ScavengeForLoot();
                break;
            case "Dodge":
                Dodge();
                break;
            case "Sprint":
                Sprint();
                break;
            case "Master Adventurer":
                MasterAdventurer();
                break;
            //Intellect Abilities
            case "Arcane Shield":
                ArcaneShield();
                break;
            case "Focus":
                Focus();
                break;
            case "Transfusion":
                Transfusion();
                break;
            case "Master Spellcaster":
                MasterSpellcaster();
                break;
            default:
                Debug.Log("Failed to use ability: " + ability);
                break;
        }
    }

    //************************************************** WARRIOR ABILITIES **************************************************
    void ReinforcingCharge()
    {
        // Move to another player’s location and aid that player in combat (They must be in combat).
        if (HasAbilityCharges() && beforeCombat && !isDead)
        {
            SpendAbilityCharge();
            reinforcingCharge.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Bash()
    {
        // After a successful power roll, deal 2x your damage to the enemy instead of normal damage (Doesn’t stack).
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Bash && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Bash = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Rally()
    {
        // When in combat, all damaged allies recover 12 health and all allies gain +2 to their next power roll (+2 doesn’t
        // stack with any other bonuses to power rolls), and you gain +5 damage reduction until your next turn.
        if (HasAbilityCharges() && inCombat && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyCast = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyDefense = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Revenge()
    {
        // If your power roll fails to beat the monster’s power level, you deal your Strength in damage to the monster while
        // receiving the monster’s damage as well.
        if (HasAbilityCharges() && whileDefending && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Revenge && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Revenge = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Berserk()
    {
        // The next time you deal damage, deal additional damage equal to the amount of health that your character is missing.
        if (HasAbilityCharges() && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** HUNTER ABILITIES **************************************************
    void Evade()
    {
        // Successfully flee from any combat (even if text prevents fleeing except boss fights).
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Evade = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Trap()
    {
        // Before any power rolls in combat, lay out a trap that last the entire combat or until triggered.  The trap is
        // triggered when you fail a power roll, and when triggered the trap prevents the monster from damaging you for this
        // turn and deals your Dexterity in damage to it.
        if (HasAbilityCharges() && beforeCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void QuickShot()
    {
        // Deal your damage to the enemy monster (can be used any number of times on hunter’s turn during combat).
        if (HasAbilityCharges() && yourCombatTurn && !isDead)
        {
            SpendAbilityCharge();
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Call Beast")
            {
                int dex = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
                int dmg = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);
                if (dex > dmg)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = dex;
                else
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = dmg;
            }
            else
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().QuickShot = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void HuntersMark()
    {
        // Mark a monster, gaining +3 to your next power roll & +5 damage for 1 turn (cannot stack).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void ArrowVolley()
    {
        // On a successful power roll after dealing normal damage,  deal your damage again, and roll a dice.  If the dice is
        // even, deal your damage to the monster again and repeat until you roll an odd number.
        if (HasAbilityCharges() && whileAttacking && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArrowVolley = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** SORCERER ABILITIES **************************************************
    void Fireball()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, deal damage with your Intellect
        // instead of damage.
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Enhanced Pyrotechnics")
                SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Fireball = true;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Freeze()
    {
        // If the target is not frozen, make a spell power roll instead of a normal power roll, and on a successful roll, the
        // monster is frozen until the end of your next turn, and cannot deal its damage to anyone while frozen.  Still deals base damage.
        if (HasAbilityCharges() && yourAction && !EnemyIsFrozen() && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze = 2;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Portal()
    {
        // Teleport yourself to any tile on the board (as long as the tile has been unlocked and is not past any barriers
        // that are locked).
        if (HasAbilityCharges() && !inCombat && !isDead)
        {
            SpendAbilityCharge();
            generalScripts.GetComponent<gameMenuManager>().exit();
            generalScripts.GetComponent<gameMenuManager>().map();
            player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<movement>().moveRadius = 100;
            player.GetComponent<movement>().calcMovement();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void ArcaneInfusion()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, gain 2 ability charges.
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneInfusion = true;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Meteor()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, use all your ability charges and
        // deal 10 damage to a monster for each.
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Meteor = true;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** PALADIN ABILITIES **************************************************
    void RighteousFury()
    {
        // Deal your weapon damage to an enemy on a successful power roll and heal yourself for the damage done.
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RighteousFury && !isDead)
        {
            if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect != "Faithful Strikes")
                SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RighteousFury = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Salvation()
    {
        // May be cast after death to resurrect you, if you have an ability charge (may only be cast once per combat instance).
        if (HasAbilityCharges() && isDead && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastSalvation)
        {
            SpendAbilityCharge();
            if (inCombat)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastSalvation = true;
            int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
            hp = 1;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void DivineProtection()
    {
        // Ignore the next damage you receive (does not stack, but lasts though different combats until used- can only be cast
        // by the player once during combat instance).
        if (HasAbilityCharges() && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastDivineProtection && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection && !isDead)
        {
            SpendAbilityCharge();
            if (inCombat)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastDivineProtection = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void JusticarsSmite()
    {
        // On a successful power roll instead of dealing normal damage, deal your Strength as damage and recover your Strength
        // as health.
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JusticarsSmite && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JusticarsSmite = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void JudgeJuryAndExecutioner()
    {
        // Your next power roll has +5 and if it is a critical hit, instantly defeat the monster (Cannot stack and instant
        // defeat does not affect Bosses).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** CLERIC ABILITIES **************************************************
    void Heal()
    {
        // Restore 18 health to any player at any time.
        if (HasAbilityCharges() && !isDead)
        {
            SpendAbilityCharge();
            healWindow.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void HolyRejuvenation()
    {
        // Restore 9 health to all players at any time.
        if (HasAbilityCharges() && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HolyRejuvenation = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void BattleMedic()
    {
        // Cast before combat with allies and the first time an ally falls below half health, cast Heal on them for free.
        // Lasts until combat is over or every ally has fallen below half health.
        if (HasAbilityCharges() && beforeCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedic && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void UltimateSacrifice()
    {
        // Upon death in combat, if you have an ability charge, all other players are healed to full health and given an
        // ability charge (can only be used per combat instance).
        if (HasAbilityCharges() && isDead && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastUltimateSacrifice)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastUltimateSacrifice = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void SpiritBomb()
    {
        // On a successful power roll, deal damage to a monster equal to your Intellect level and split up the amount dealt
        // for you and your allies to recover as health.
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBomb && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBomb = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** NECROMANCER ABILITIES **************************************************
    void DarkResurrection()
    {
        // Roll a dice.  If it is 4 or higher, bring a dead player back to life.
        if (HasAbilityCharges() && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 21 && !isDead)
        {
            SpendAbilityCharge();
            resurrectRoll.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void DeathBlast()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, you deal damage with your Intellect
        // instead of your normal damage and if this kills a monster, gain 2 ability charges.
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DeathBlast = true;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Feed()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, consume an enemy monster’s health,
        // damaging the enemy one health at a time, healing yourself for the amount lost, until yours is full or the monster is
        // dead (cannot cast against bosses).
        if (HasAbilityCharges() && yourAction && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Feed = true;
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void RaiseTheDead()
    {
        // After defeating a monster, cast this to raise a Skeletal Companion.  The Skeletal Companion aids you in your next combat
        // and takes a turn in that combat just like a player.  The Skeletal Companion has a power equal to 3x your spell power and
        // adds 2 dice rolls during power rolls just like a player.  The Skeletal Companion has damage equal to your spell power and
        // health equal to 4x your spell power.  The Skeletal Companion taunts on every one of your turns while it is still “alive”.
        // The Skeletal Companion automatically dies when the combat is over if it did not die during the combat. (Cannot stack and
        // should it occur though special items, recasting this ability replaces the current Skeletal Companion with a new one, but
        // this can only be cast once during a combat instance).
        
        if (HasAbilityCharges() && (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanRaiseTheDead || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Undead Legion") && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanRaiseTheDead = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion = true;
            skeletalCompanion.GetComponent<skeletalCompanionController>().health = skeletalCompanion.GetComponent<skeletalCompanionController>().maxHealth;
            skeletalCompanion.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void SoulShred()
    {
        // Make a spell power roll instead of a normal power roll, and on a successful roll, spend as much health as you would like
        // (leaving at least 1 left over) to deal that amount as damage to an enemy.
        if (HasAbilityCharges() && yourAction && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShred = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShredAmount = 0;
            soulShredWindow.SetActive(true);
            actionPhase.GetComponent<actionPhaseController>().spellAttack();
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** ROGUE ABILITIES **************************************************
    void SurpriseAttack()
    {
        // Lower an enemy monster’s power by 2 (can only be used before the first combat roll of each combat, but the effect
        // lasts the entire combat.  The effect does not affect bosses).
        if (HasAbilityCharges() && beforeCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SurpriseAttack = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Raze()
    {
        // On a successful power roll, add your Dexterity level to your damage (Cannot stack).
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Raze && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Raze = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Stealth()
    {
        // Can maneuver around a monster undetected, preventing combat, but awarding 1 less treasure than the monster holds
        // (the monster is then shuffled back into the deck and can only be cast before combat).
        if (HasAbilityCharges() && beforeCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Stealth = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void CombinationStrike()
    {
        // After a successful power roll, cast this to take another turn.  If your second power roll is successful, you have
        // +10 damage this turn.
        if (HasAbilityCharges() && whileAttacking && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationStrike < 2 && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationStrike = 2;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Preparation()
    {
        // Gain your Dexterity level worth of points toward your next power roll, and for every point your power roll
        // surpasses the monster’s power, deal that much as extra damage to your normal damage (Cannot stack).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PreparationAmount = 0;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** DRUID ABILITIES **************************************************
    void WorgTransformatiom()
    {
        // Move an extra 2 spaces after rolling for movement (stays in Worg Transformation until another transformation is
        // activated).
        if (HasAbilityCharges() && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation = true;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation = false;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void BearTransformation()
    {
        // Deal damage with your Strength instead of your weapon damage for the rest of the combat phase (all power rolls
        // made while in transformation are spell power rolls).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation = true;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation = false;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void TendToWounds()
    {
        // Restore 20 health if you are in a Transformation, or 10 if you are not.
        if (HasAbilityCharges() && !isDead)
        {
            SpendAbilityCharge();
            int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
            int max = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);

            //The Shadow Ability - Lightless Void
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WorgTransformation || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
                    health += 20;
                else
                    health += 10;
            }

            if (health > max)
                health = max;

            //Spider Queen Aranne Ability - Venomous Bite
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void InfusedStrike()
    {
        // Add your spell power to your damage for 1 turn (can stack).
        if (HasAbilityCharges() && inCombat && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InfusedStrike += 1;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void ThickHide()
    {
        // While in a transformation in combat, gain your Strength as damage reduction for the rest of combat (Cannot stack).
        if (HasAbilityCharges() && inCombat && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** MONK ABILITIES **************************************************
    void TheWayOfTheStoneFist()
    {
        // Gain +8 damage for the rest of the combat instance (cannot stack).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void FlowingWavesStance()
    {
        // For your next 2 turns, when receiving damage, roll a dice, and if the dice is 4 or higher, you dodge the attack,
        // ignoring the damage.
        if (HasAbilityCharges() && inCombat && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance == 0 && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance = 2;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void RejuvenatingMantra()
    {
        // For your next 5 turns, recover 4 health each turn if you are in combat, or 8 health each turn if you are not.
        if (HasAbilityCharges() && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra == 0 && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RejuvenatingMantra = 5;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void RapidStrikes()
    {
        // On a successful power roll,  after you deal your damage, deal  an extra 5 damage for each of the following
        // (if your Dexterity is at least 8, if your Dexterity is at least 10, if your Dexterity is at least 12, if your
        // Dexterity is at least 14, if your Dexterity is at least 16) for a max of 5 attacks of 5 damage each.
        if (HasAbilityCharges() && whileAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RapidStrikes && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RapidStrikes = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void InnerFocus()
    {
        // While in combat, you have +4 damage and +2 to power rolls until you take any damage (cannot stack).
        if (HasAbilityCharges() && inCombat && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** STRENGTH ABILITIES **************************************************
    void Intimidation()
    {
        // Before your next power roll, cast this to reduce the power of the enemy you are attacking by 4 for 1 combat round.
        if (HasAbilityCharges() && beforeAttacking && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intimidation && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intimidation = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Taunt()
    {
        // Passive:
        // When in combat with others, on another player’s turn, before they make their power roll, declare taunt to receive
        // any damage that your ally might take that turn instead of them.
        bool noOneTaunting = true;
        bool someoneTakingDamage = false;
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
                noOneTaunting = false;
        }
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TakingDamage)
                someoneTakingDamage = true;
        }
        if (inCombat && !yourCombatTurn && noOneTaunting && someoneTakingDamage && !isDead)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void SecondWind()
    {
        // Passive:
        // While in combat, every round you do not take damage, recover 2 health instead.  While not in combat, each turn you
        // take that you do not encounter anything, recover  8 health.
        
    }
    void MasterCombatant()
    {
        // Passive:
        // Once per combat instance you can add your Strength either to your damage or damage reduction for one combat round.
        if ((whileAttacking || whileDefending) && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasUsedMasterCombatant && !isDead)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasUsedMasterCombatant = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterCombatant = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }

    //************************************************** DEXTERITY ABILITIES **************************************************
    void ScavengeForLoot()
    {
        // Cast when drawing a treasure card and roll a dice.  If the roll is a 4 or higher, draw an additional treasure card
        if (HasAbilityCharges() && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DrawingTreasure && !isDead)
        {
            SpendAbilityCharge();
            scavengeForLootWindow.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Dodge()
    {
        // Passive:
        // When in combat, on your turn, if your power roll fails, you can declare dodge and call out a number (1-6) and roll
        // a dice after.  If the roll matches the number you said, you dodge the attack, ignoring the damage.

    }
    void Sprint()
    {
        // Passive:
        // Move an extra 2 tiles during the movement phase (does not stack with other movement bonuses).

    }
    void MasterAdventurer()
    {
        // Passive:
        // When encountering a card that requires either your strength, dexterity, or intellect to surpass a certain value to
        // succeed, roll a dice, and if the roll is 4 or higher, you automatically succeed.

    }

    //************************************************** INTELLECT ABILITIES **************************************************
    void ArcaneShield()
    {
        // Grants 2x the caster’s spell power as damage reduction for 1 combat round.
        if (HasAbilityCharges() && inCombat && !isDead)
        {
            SpendAbilityCharge();
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneShield = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Focus()
    {
        // Passive:
        // When in combat, on your turn, after rolling your 2 dice, you may declare focus and re-roll any dice that land on 1
        // (cannot use Focus more than once per power roll or if the roll was a critical failure).
        if (beforeAttacking && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CanCastFocus && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastFocus && !isDead)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Focus = true;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastFocus = true;
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void Transfusion()
    {
        // Passive:
        // At any non-combat point, the player may discard as many treasure cards as they would like, gaining an ability charge
        // for each.
        if (!inCombat && !isDead)
        {
            transfusion.SetActive(true);
            gameObject.SetActive(false);
        }
        else
            StartCoroutine(Failed());
    }
    void MasterSpellcaster()
    {
        // Passive:
        // After casting a spell, the player rolls a dice and if it lands on 5 or 6, the ability charge spent is refunded to
        // the player.

    }

}
