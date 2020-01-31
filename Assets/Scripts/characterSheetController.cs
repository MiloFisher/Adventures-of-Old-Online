using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class characterSheetController : MonoBehaviour
{
    public string characterName;
    public GameObject grayOverlay;
    public GameObject Name;
    public GameObject Image;
    public GameObject Race;
    public GameObject Class;
    public GameObject AbilityCharges;
    public GameObject Damage;
    public GameObject Gold;
    public GameObject PhysicalPower;
    public GameObject SpellPower;
    public GameObject DamageReduction;
    public GameObject Level;
    public GameObject Strength;
    public GameObject Dexterity;
    public GameObject Intellect;
    public GameObject Health;
    public GameObject MaxHealth;
    //public GameObject TemporaryEffects;
    private GameObject[] players;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        players = GameObject.FindGameObjectsWithTag("Player Info");
        for (int i = 0; i < players.Length; i++)
        {
            if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name == characterName)
            {
                Name.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Name;
                Race.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Race;
                Class.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Class;
                Damage.GetComponent<Text>().text = (int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Damage) + int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageMod)) + "";
                Gold.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Gold;
                PhysicalPower.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().PhysicalPower;
                SpellPower.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().SpellPower;
                DamageReduction.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DamageReduction;
                Level.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Level;
                Strength.GetComponent<Text>().text = (int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Strength) + int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().StrengthMod)) + "";
                Dexterity.GetComponent<Text>().text = (int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Dexterity) + int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().DexterityMod)) + "";
                Intellect.GetComponent<Text>().text = (int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Intellect) + int.Parse(players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IntellectMod)) + "";
                Health.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Health;
                MaxHealth.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().MaxHealth;

                Sprite image = (Sprite)Resources.Load("Images/" + players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().Image, typeof(Sprite));
                Image.GetComponent<Image>().sprite = image;

                AbilityCharges.GetComponent<Text>().text = players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().AbilityCharges;

                if (players[i].GetComponent<BoltEntity>().GetState<IPlayerInfo>().IsDead)
                    grayOverlay.SetActive(true);
                else
                    grayOverlay.SetActive(false);
            }
        }
    }
}
