using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class previewPhaseController : MonoBehaviour
{
    public GameObject combatCycle;
    public bool succeeded;
    public bool crit;
    public string monster;
    public GameObject monsterDisplay;
    public GameObject monsterHealthDisplay;
    public GameObject monsterMaxHealthDisplay;
    public GameObject monsterHealthBar;
    public GameObject playerDisplay;
    public GameObject outgoingDamage;

    public GameObject monsterDisplay1;
    public GameObject playerDisplay1;
    public GameObject incomingDamage;
    public GameObject playerHealthDisplay;
    public GameObject playerMaxHealthDisplay;
    public GameObject playerHealthBar;
    public GameObject blockButton;
    public GameObject dodgeButton;

    public GameObject dealingDamage;
    public GameObject receivingDamage;

    public GameObject blockRollTitle;
    public GameObject blockRollScreen;
    public GameObject blockRollButton;
    public GameObject blockRollNumber;
    public GameObject blockSuccess;
    public GameObject blockFail;

    public GameObject dodgeRollScreen;
    public GameObject dodgeRollButton;
    public GameObject dodgeRollNumber;
    public GameObject rightArrow;
    public GameObject leftArrow;
    public GameObject dodgeRollPrediction;
    public GameObject dodgeSuccess;
    public GameObject dodgeFail;

    public GameObject inventoryDisplay;
    public GameObject discardDisplay;
    public GameObject abilityWindow;

    public GameObject energyFunnelRollButton;
    public GameObject resistRollDisplay;
    public GameObject resistNumber;
    public GameObject resistButton;
    public GameObject energyFunnelDisplay;
    public GameObject buttonBlocker;

    public int orcDamageBuff = 0;

    public GameObject skeletalCompanion;

    public GameObject flowingWavesStance;
    public GameObject fwsButton;
    public GameObject fwsNumber;
    public GameObject tauntCover;

    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private float length;
    private float offset;
    private int damage;
    private int healAmount;
    private int damageAmount;
    public int bonusDamage;
    private bool hasBlocked;
    private bool hasDodged;
    private int blockChance;
    private int blockAmount;
    private int blockTotal;
    private int prediction = 1;
    private int dodgeTotal;
    private bool doubleDamage;
    private bool tauntedFor;
    private bool tauntCoverWasOn;
    private bool useDivineProtection;
    private int damageTotal = 0;
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
        resistRollDisplay.SetActive(false);
        energyFunnelDisplay.SetActive(false);
    }

    void OnEnable()
    {
        if (players != null)
        {
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance > 0)
                FWS();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (succeeded)
        {
            energyFunnelRollButton.SetActive(false);
            dealingDamage.SetActive(true);
            receivingDamage.SetActive(false);
            //monster display
            monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Monster;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
            else
                monsterDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));

            //monster health display
            monsterHealthDisplay.GetComponent<Text>().text = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth + "/";
            monsterMaxHealthDisplay.GetComponent<Text>().text = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterMaxHealth + "";
            length = (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth / (float)serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterMaxHealth) * 350;
            monsterHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(length, 12);
            offset = (length - 350) / 2f;
            monsterHealthBar.transform.localPosition = new Vector2(offset, 0);
            if (length / 350f > .67f)
                monsterHealthBar.GetComponent<Image>().color = Color.green;
            else if (length / 350f > .33f)
                monsterHealthBar.GetComponent<Image>().color = Color.yellow;
            else
                monsterHealthBar.GetComponent<Image>().color = Color.red;

            if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack)
            {
                //player display
                playerDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));

                //damage display
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShred)
                {
                    damage = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShredAmount;
                    damageAmount = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShredAmount;
                }
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Feed)
                {
                    int playerHealthDeficit = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth) - int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                    int monsterHealth = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterHealth;
                    if (monsterHealth > playerHealthDeficit)
                    {
                        damage = playerHealthDeficit;
                        healAmount = playerHealthDeficit;
                    }
                    else
                    {
                        damage = monsterHealth;
                        healAmount = monsterHealth;
                    }
                }
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DeathBlast)
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBomb)
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JusticarsSmite)
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneInfusion)
                    damage = 0;
                else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Fireball)
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
                else
                    damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod) + bonusDamage + orcDamageBuff;

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Relentless Assault")
                    damage += players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RapidStrikes)
                {
                    int dex = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
                    for (int i = 8; i < 18; i += 2)
                    {
                        if (dex >= i)
                            damage += 5;
                    }
                }
                if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterCombatant)
                    damage += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus)
                    damage += 4;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TheWayOfTheStoneFist)
                    damage += 8;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Raze)
                    damage += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark)
                    damage += 5;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationDamage)
                    damage += 10;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves > 0)
                    damage += 10;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation)
                    damage += players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PreparationAmount;
                if (crit && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShred)
                    damage *= 2;
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Bash)
                    damage *= 2;

                //after crit bonus
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InfusedStrike > 0)
                    damage += players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InfusedStrike * int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk)
                    damage += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth) - int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Meteor)
                {
                    damage = 0;
                    for (int i = 0; i < int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges); i++)
                        damage += 10;
                }

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner && crit && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                    damage = 999;
            }
            else
            {
                //skeletal display
                playerDisplay.GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/skeletalCompanion", typeof(Sprite));
                damage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);
            }

            outgoingDamage.GetComponent<Text>().text = damage + "";
            damageTotal = damage;
        }
        else
        {
            if (!tauntCoverWasOn)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TakingDamage = true;
            else
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TakingDamage = false;
            tauntedFor = false;
            for (int i = 1; i < players.Length; i++)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
                    tauntedFor = true;
            }
            if (tauntedFor)
            {
                tauntCover.SetActive(true);
                tauntCoverWasOn = true;
            }
            else
                tauntCover.SetActive(false);

            //The All-Seer Ability - Energy Funnel
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 22)
                energyFunnelRollButton.SetActive(true);
            else
                energyFunnelRollButton.SetActive(false);

            dealingDamage.SetActive(false);
            receivingDamage.SetActive(true);
            //monster display
            monster = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().Monster;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InBossFight)
                monsterDisplay1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Boss Cards/" + monster, typeof(Sprite));
            else
                monsterDisplay1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Encounter Cards/" + monster, typeof(Sprite));

            if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack)
            {
                //player display
                playerDisplay1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/" + players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));

                //player health display
                playerHealthDisplay.GetComponent<Text>().text = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health + "/";
                playerMaxHealthDisplay.GetComponent<Text>().text = players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;
                length = (float.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health) / float.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth)) * 270;
                playerHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(length, 12);
                offset = (length - 270) / 2f;
                playerHealthBar.transform.localPosition = new Vector2(offset, 0);
                if (length / 270f > .67f)
                    playerHealthBar.GetComponent<Image>().color = Color.green;
                else if (length / 270f > .33f)
                    playerHealthBar.GetComponent<Image>().color = Color.yellow;
                else
                    playerHealthBar.GetComponent<Image>().color = Color.red;

                //damage display
                damage = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterDamage + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterBonusDamage;

                //Sadistic Imp Ability - Hellspawn
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 2)
                {
                    for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
                    {
                        for (int k = 0; k < players.Length; k++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                            {
                                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Paladin" || players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Cleric")
                                {
                                    damage -= 5;
                                }
                            }
                        }
                    }
                }

                //King of the Undead Ability - Undead Wrath
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 21)
                {
                    for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
                    {
                        for (int k = 0; k < players.Length; k++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                            {
                                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                                {
                                    damage += int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);
                                }
                            }
                        }
                    }
                }

                //Shadow Nightmare Ability - Shadow Copy
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 17)
                    damage += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);

                if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterCombatant)
                    damage -= int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);

                damage -= int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ThickHide)
                    damage -= int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyDefense)
                    damage -= 5;

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneShield)
                    damage -= 2 * int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Nature's Call" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BearTransformation)
                    damage -= 5;

                //freeze ability
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze > 0)
                        damage = 0;
                }

                //divine protection
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection && damage > 0)
                {
                    useDivineProtection = true;
                    damage = 0;
                }
                else
                    useDivineProtection = false;

                if (tauntCoverWasOn)
                    damage = 0;

                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap)
                    damage = 0;

                damage -= blockTotal;
                damage -= dodgeTotal;
                if (dodgeTotal > 0 && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Crashing Waves")
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves = 2;
                if (damage < 0)
                    damage = 0;
                incomingDamage.GetComponent<Text>().text = damage + "";

                //button displays
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "w1" || players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "w8")
                {
                    if (!hasBlocked)
                    {
                        blockButton.SetActive(true);
                        hasBlocked = true;
                    }
                }
                else
                    blockButton.SetActive(false);

                if (int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod) >= 10)
                {
                    if (!hasDodged)
                    {
                        dodgeButton.SetActive(true);
                        hasDodged = true;
                    }
                }
                else
                    dodgeButton.SetActive(false);

                //dodge roll display
                if (prediction == 1)
                    leftArrow.SetActive(false);
                else
                    leftArrow.SetActive(true);
                if (prediction == 6)
                    rightArrow.SetActive(false);
                else
                    rightArrow.SetActive(true);
                dodgeRollPrediction.GetComponent<Text>().text = prediction + "";
            }
            else
            {
                //skeletal display
                playerDisplay1.GetComponent<Image>().sprite = (Sprite)Resources.Load("Images/skeletalCompanion", typeof(Sprite));

                //skeletal health display
                playerHealthDisplay.GetComponent<Text>().text = skeletalCompanion.GetComponent<skeletalCompanionController>().health + "/";
                playerMaxHealthDisplay.GetComponent<Text>().text = skeletalCompanion.GetComponent<skeletalCompanionController>().maxHealth + "";
                length = ((float)skeletalCompanion.GetComponent<skeletalCompanionController>().health / (float)skeletalCompanion.GetComponent<skeletalCompanionController>().maxHealth) * 270;
                playerHealthBar.GetComponent<RectTransform>().sizeDelta = new Vector2(length, 12);
                offset = (length - 270) / 2f;
                playerHealthBar.transform.localPosition = new Vector2(offset, 0);
                if (length / 270f > .67f)
                    playerHealthBar.GetComponent<Image>().color = Color.green;
                else if (length / 270f > .33f)
                    playerHealthBar.GetComponent<Image>().color = Color.yellow;
                else
                    playerHealthBar.GetComponent<Image>().color = Color.red;

                //damage display
                damage = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterDamage + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterBonusDamage;

                //King of the Undead Ability - Undead Wrath
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 21)
                {
                    for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
                    {
                        for (int k = 0; k < players.Length; k++)
                        {
                            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                            {
                                if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                                {
                                    damage += int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);
                                }
                            }
                        }
                    }
                }

                //Shadow Nightmare Ability - Shadow Copy
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 17)
                    damage += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod);

                //freeze ability
                for (int i = 0; i < players.Length; i++)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze > 0)
                        damage = 0;
                }

                incomingDamage.GetComponent<Text>().text = damage + "";
                hasBlocked = false;
                hasDodged = false;
                blockButton.SetActive(false);
                dodgeButton.SetActive(false);
            }
        }
    }

    public void Resolve()
    {
        inventoryDisplay.SetActive(false);
        if (succeeded)
        {
            //if (crit)
            //    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = damage*2;
            //else
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = damageTotal;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RighteousFury)
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                hp += damage;  

                //The Shadow Ability - Lightless Void
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24)
                    hp -= damage;

                //Spider Queen Aranne Ability - Venomous Bite
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                if (hp > mxhp)
                    hp = mxhp;

                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShred)
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                hp -= damageAmount;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Feed)
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                hp += healAmount;

                //The Shadow Ability - Lightless Void
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24)
                    hp -= healAmount;

                //Spider Queen Aranne Ability - Venomous Bite
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                if (hp > mxhp)
                    hp = mxhp;

                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JusticarsSmite)
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                hp += int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);

                //The Shadow Ability - Lightless Void
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24)
                    hp -= int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);

                //Spider Queen Aranne Ability - Venomous Bite
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                if (hp > mxhp)
                    hp = mxhp;

                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            //Ophretes the Kraken Ability - Thrashing Waves
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 25)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitOphretes = true;

            //deactivate ability
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Berserk = false;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneInfusion)
            {
                int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                ac += 2;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Meteor)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = 0 + "";  
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBomb)
            {
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBombHeal = true;
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind)
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                //The Shadow Ability - Lightless Void
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
                    hp += 2;

                //Spider Queen Aranne Ability - Venomous Bite
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                if (hp > mxhp)
                    hp = mxhp;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Heaven's Blessing")
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HeavensBlessing = true;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Relentless Assault")
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault += 5;

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Blood Assassin")
            {
                int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                //The Shadow Ability - Lightless Void
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
                    hp += 5;

                //Spider Queen Aranne Ability - Venomous Bite
                if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                if (hp > mxhp)
                    hp = mxhp;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
            }

            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow++;
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RapidStrikes)
            {
                int dex = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);
                for (int i = 8; i < 18; i += 2)
                {
                    if (dex >= i)
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow++;
                }
            }
        }
        else
        {
            int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
            int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
            bool castHeal = false;

            if (doubleDamage)
                damage *= 2;
            doubleDamage = false;

            if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack)
            {
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Human")
                {
                    if (health >= maxHealth / 2f && health - damage < maxHealth / 2f)
                    {
                        int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                        ac++;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
                    }
                }
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Divine Bulwark")
                {
                    if (health >= maxHealth / 2f && health - damage < maxHealth / 2f)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection = true;
                    }
                }
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedic)
                {
                    if (health >= maxHealth / 2f && health - damage < maxHealth / 2f)
                    {
                        castHeal = true;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedic = false;
                    }
                }
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Djinni")
                {
                    int ac = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                    ac--;
                    if (ac < 0)
                        ac = 0;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
                }
                if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race == "Orc")
                    orcDamageBuff++;

                if (damage > 0)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InnerFocus = false;

                if (damage < 0)
                    damage = 0;

                health -= damage;
                if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
                else
                {
                    skeletalCompanion.GetComponent<skeletalCompanionController>().health = health;
                }

                if (damage == 0)
                {
                    if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SecondWind)
                    {
                        int hp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
                        int mxhp = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);
                        //The Shadow Ability - Lightless Void
                        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility != 24)
                            hp += 2;

                        //Spider Queen Aranne Ability - Venomous Bite
                        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                        if (hp > mxhp)
                            hp = mxhp;
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = hp + "";
                    }
                }

                if (castHeal)
                {
                    int extraHealth = 18;

                    //The Shadow Ability - Lightless Void
                    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24)
                        extraHealth = 0;

                    //Spider Queen Aranne Ability - Venomous Bite
                    if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

                    health += extraHealth;
                    if (health > maxHealth)
                        health = maxHealth;
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
                }
            }
            else
            {
                health -= damage;
                skeletalCompanion.GetComponent<skeletalCompanionController>().health = health;
            }

            hasBlocked = false;
            hasDodged = false;
            blockTotal = 0;
            dodgeTotal = 0;

            //Rogue Bandits Ability - Filthy Thiefs
            if (damage > 0 && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 5 && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class != "Warrior" && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class != "Rogue")
                discardDisplay.SetActive(true);

            //Ancient Crocodile Ability - Ancient Devourer
            if (damage > 0 && serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 9)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BitByCrocodile = true;

            //Starving Wolf Pack Ability - Feeding Frenzy
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 13)
            {
                bool beastCanHeal = true;
                for (int i = 0; i < serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty.Length; i++)
                {
                    for (int k = 0; k < players.Length; k++)
                    {
                        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().CombatParty[i] == players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                        {
                            if (players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Druid" || players[k].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class == "Hunter")
                            {
                                beastCanHeal = false;
                            }
                        }
                    }
                }
                if(beastCanHeal)
                    players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().WolfHeal = true;
            }

            //Cinderosa Ability - Punishing Flames
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 20 && damage > 0)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitByCinderosa = true;

            //Magistrax the Gorgon Ability - Turned to Stone
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 23 && damage > 0)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TurnedToStone = true;

            //Spider Queen Aranne Ability - Venomous Bite
            if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28 && damage > 0)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = true;

            //ability effects
            if(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Revenge)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod);

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod);

            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Arcane Ward")
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().OutgoingDamage = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower);

            //deactivate ability
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Revenge = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Trap = false;
            if (useDivineProtection)
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DivineProtection = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().TakingDamage = false;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RelentlessAssault = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitsInARow = 0;
        }

        //deactivate 1-turn abilities
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Bash = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyDefense = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HuntersMark = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Fireball = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneInfusion = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Meteor = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RighteousFury = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JusticarsSmite = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().JudgeJuryAndExecutioner = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBomb = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Feed = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SoulShred = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Raze = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationDamage = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Preparation = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PreparationAmount = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().InfusedStrike = 0;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RapidStrikes = false;
        tauntCoverWasOn = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MasterCombatant = false;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().ArcaneShield = false;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze > 0)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Freeze--;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationStrike > 0)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationStrike--;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance > 0)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().FlowingWaveStance--;
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves > 0)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CrashingWaves--;

        //Giant-King Magnor Ability - Slashing Frenzy
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 26 && !succeeded && int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health) > 0 && !players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
            combatCycle.GetComponent<combatCycle>().DecrementPhase();
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt)
        {
            combatCycle.GetComponent<combatCycle>().phase = 0;
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Taunt = false;
        }
        else if (!players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack && players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalCompanion)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SkeletalAttack = true;
            combatCycle.GetComponent<combatCycle>().DecrementPhase();
        }
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationStrike == 1)
        {
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().CombinationDamage = true;
            combatCycle.GetComponent<combatCycle>().DecrementPhase();
        }
        else
            combatCycle.GetComponent<combatCycle>().IncrementPhase();
    }

    public void EnergyFunnelRoll()
    {
        resistRollDisplay.SetActive(true);
    }

    public void ResistRoll()
    {
        resistButton.SetActive(false);
        int rando = Random.Range(1, 7);
        resistNumber.GetComponent<Text>().text = rando + "";
        StartCoroutine(WaitToResolve(rando));
    }

    IEnumerator WaitToResolve(int roll)
    {
        yield return new WaitForSeconds(1);
        if (roll <= 3)
        {
            energyFunnelDisplay.SetActive(true);
            if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges == "0")
                buttonBlocker.SetActive(true);
            else
                buttonBlocker.SetActive(false);
        }
        else
        {
            resistButton.SetActive(true);
            resistNumber.GetComponent<Text>().text = "";
            resistRollDisplay.SetActive(false);
            Resolve();
        }
    }

    public void DiscardAbilityCharge()
    {
        int charges = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
        charges--;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = charges + "";
        energyFunnelDisplay.SetActive(false);
        resistButton.SetActive(true);
        resistNumber.GetComponent<Text>().text = "";
        resistRollDisplay.SetActive(false);
        Resolve();
    }

    public void TakeDoubleDamage()
    {
        doubleDamage = true;
        energyFunnelDisplay.SetActive(false);
        resistButton.SetActive(true);
        resistNumber.GetComponent<Text>().text = "";
        resistRollDisplay.SetActive(false);
        Resolve();
    }

    public void Block()
    {
        blockButton.SetActive(false);
        blockFail.SetActive(false);
        blockSuccess.SetActive(false);
        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "w1")
        {
            blockChance = 2;
            blockAmount = 5;
        }
        else if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Equipment[5] == "w8")
        {
            blockChance = 4;
            blockAmount = 8;
        }

        if (players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().LegendaryEffect == "Sword and Board")
        {
            blockTotal = blockAmount;
        }
        else
        {
            blockRollTitle.GetComponent<Text>().text = "Block on a " + blockChance + " or lower";
            blockRollNumber.GetComponent<Text>().text = "";
            blockRollButton.SetActive(true);
            blockRollScreen.SetActive(true);
        }
    }

    public void BlockRoll()
    {
        int rando = Random.Range(1,7);
        blockRollNumber.GetComponent<Text>().text = rando + "";
        blockRollButton.SetActive(false);
        if (rando <= blockChance)
        {
            blockTotal = blockAmount;
            blockSuccess.SetActive(true);
        }
        else
        {
            blockTotal = 0;
            blockFail.SetActive(true);
        }

        StartCoroutine(blockDisposeTimer());
    }

    IEnumerator blockDisposeTimer()
    {
        yield return new WaitForSeconds(1);
        blockRollScreen.SetActive(false);
    }

    public void Dodge()
    {
        dodgeFail.SetActive(false);
        dodgeSuccess.SetActive(false);
        dodgeButton.SetActive(false);
        prediction = 1;
        dodgeRollNumber.GetComponent<Text>().text = "";
        dodgeRollButton.SetActive(true);
        dodgeRollScreen.SetActive(true);
    }

    public void DodgeRoll()
    {
        int rando = Random.Range(1, 7);
        dodgeRollNumber.GetComponent<Text>().text = rando + "";
        dodgeRollButton.SetActive(false);
        if (rando == prediction)
        {
            dodgeTotal = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterDamage + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterBonusDamage;
            dodgeSuccess.SetActive(true);
        }
        else
        {
            dodgeTotal = 0;
            dodgeFail.SetActive(false);
        }

        StartCoroutine(dodgeDisposeTimer());
    }

    IEnumerator dodgeDisposeTimer()
    {
        yield return new WaitForSeconds(1);
        dodgeRollScreen.SetActive(false);
    }

    public void incrementprediction()
    {
        if(dodgeRollButton.activeInHierarchy)
            prediction++;
    }

    public void decrementprediction()
    {
        if (dodgeRollButton.activeInHierarchy)
            prediction--;
    }

    public void Items()
    {
        inventoryDisplay.SetActive(true);
    }

    public void Abilities()
    {
        abilityWindow.SetActive(true);
    }

    public void FWS()
    {
        dodgeFail.SetActive(false);
        dodgeSuccess.SetActive(false);
        fwsButton.SetActive(false);
        fwsNumber.GetComponent<Text>().text = "";
        fwsButton.SetActive(true);
        flowingWavesStance.SetActive(true);
    }

    public void FlowingWavesStance()
    {
        int rando = Random.Range(1, 7);
        fwsNumber.GetComponent<Text>().text = rando + "";
        fwsButton.SetActive(false);
        if (rando >= 4)
        {
            dodgeTotal = serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterDamage + serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterBonusDamage;
            dodgeSuccess.SetActive(true);
        }
        else
        {
            dodgeTotal = 0;
            dodgeFail.SetActive(false);
        }

        StartCoroutine(FlowingWavesStanceDisposeTimer());
    }

    IEnumerator FlowingWavesStanceDisposeTimer()
    {
        yield return new WaitForSeconds(1);
        flowingWavesStance.SetActive(false);
        hasDodged = true;
    }
}
