using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class abilityListener : MonoBehaviour
{
    private GameObject serverInfo;
    private GameObject[] players;
    private int playerID;
    private string characterName;
    private bool rallyCast;
    private bool holyRejuv;
    private bool battleMedicCast;
    private bool ultimateSacrifice;
    private bool spiritBomb;
    private bool heavBless;
    private bool hitOph;
    private bool healed;
    private bool scrollClr;
    private List<string> senders = new List<string>();
    private bool coolingDown = false;
    private float pause = 0.5f;
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
            coolingDown = false;
            for (int j = 0; j < senders.Count; j++)
            {
                if (senders[j] == players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name)
                    coolingDown = true;
            }
            if (!coolingDown)
            {
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming)
                {
                    if (i == 0)
                    {
                        if (!scrollClr)
                            StartCoroutine(Deactivate("ClearScroll"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount > 0)
                {
                    if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget == characterName && !healed)
                    {
                        Heal(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount);
                    }

                    if (i == 0)
                    {
                        if (!healed)
                            StartCoroutine(Deactivate("Heal"));
                    }
                    else
                    {
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                    }
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitOphretes)
                {
                    if (i == 0)
                    {
                        if (!hitOph)
                            StartCoroutine(Deactivate("HitOphretes"));
                    }
                    else
                    {
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                        TakeDamage(3);
                    }
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HeavensBlessing)
                {
                    if (!heavBless)
                        Heal(6);
                    if (i == 0)
                    {
                        if (!heavBless)
                            StartCoroutine(Deactivate("HeavensBlessing"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyCast)
                {
                    if (!rallyCast)
                    {
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Rally = true;
                        Heal(12);
                    }
                    if (i == 0)
                    {
                        if (!rallyCast)
                            StartCoroutine(Deactivate("RallyCast"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));

                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HolyRejuvenation)
                {
                    if (!holyRejuv)
                        Heal(9);
                    if (i == 0)
                    {
                        if (!holyRejuv)
                            StartCoroutine(Deactivate("HolyRejuvenation"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast)
                {
                    if (!battleMedicCast)
                        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedic = true;
                    if (i == 0)
                    {
                        if (!battleMedicCast)
                            StartCoroutine(Deactivate("BattleMedic"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastUltimateSacrifice)
                {
                    if (!ultimateSacrifice)
                    {
                        Heal(999);
                        int ac = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges);
                        ac++;
                        players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges = ac + "";
                    }
                    if (i == 0)
                    {
                        if (!ultimateSacrifice)
                            StartCoroutine(Deactivate("UltimateSacrifice"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBombHeal)
                {
                    if (!spiritBomb)
                    {
                        int intellect = int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod);
                        Heal(intellect / 4);
                    }
                    if (i == 0)
                    {
                        if (!spiritBomb)
                            StartCoroutine(Deactivate("SpiritBomb"));
                    }
                    else
                        StartCoroutine(AddSender(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name));
                }
            }
        }
    }

    IEnumerator Deactivate(string s)
    {
        switch (s)
        {
            case "ClearScroll":
                scrollClr = true;
                yield return new WaitForSeconds(pause);
                scrollClr = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().UsingScrollOfHarming = false;
                break;
            case "Heal":
                healed = true;
                yield return new WaitForSeconds(pause);
                healed = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealTarget = null;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HealAmount = 0;
                break;
            case "HitOphretes":
                hitOph = true;
                yield return new WaitForSeconds(pause);
                hitOph = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HitOphretes = false;
                break;
            case "HeavensBlessing":
                heavBless = true;
                yield return new WaitForSeconds(pause);
                heavBless = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HeavensBlessing = false;
                break;
            case "RallyCast":
                rallyCast = true;
                yield return new WaitForSeconds(pause);
                rallyCast = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().RallyCast = false;
                break;
            case "HolyRejuvenation":
                holyRejuv = true;
                yield return new WaitForSeconds(pause);
                holyRejuv = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HolyRejuvenation = false;
                break;
            case "BattleMedic":
                battleMedicCast = true;
                yield return new WaitForSeconds(pause);
                battleMedicCast = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().BattleMedicCast = false;
                break;
            case "UltimateSacrifice":
                ultimateSacrifice = true;
                yield return new WaitForSeconds(pause);
                ultimateSacrifice = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().HasCastUltimateSacrifice = false;
                break;
            case "SpiritBomb":
                spiritBomb = true;
                yield return new WaitForSeconds(pause);
                spiritBomb = false;
                players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpiritBombHeal = false;
                break;
        }
    }

    IEnumerator AddSender(string s)
    {
        senders.Add(s);
        yield return new WaitForSeconds(2*pause);
        senders.Remove(s);
    }

    void Heal(int amount)
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        int maxHealth = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth);

        //The Shadow Ability - Lightless Void
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 24 && amount != 1)
            amount = 0;

        //Spider Queen Aranne Ability - Venomous Bite
        if (serverInfo.GetComponent<BoltEntity>().GetState<IServerInfo>().MonsterAbility == 28)
            players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Poisoned = false;

        health += amount;
        if (health > maxHealth)
            health = maxHealth;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }

    void TakeDamage(int amount)
    {
        int health = int.Parse(players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health);
        health -= amount;
        players[playerID].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health = health + "";
    }
}
