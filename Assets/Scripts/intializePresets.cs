using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class intializePresets : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        string fileName = "night_elf_info";
        string primaryStat = "Lone Wolf";
        string abilityCharges = "When facing a monster alone, gain a +2 bonus to power rolls";
        string healthDice = "+0";
        string armorProficiency = "+3";
        string weaponProficiency = "-1";
        string aaa = "+0";

        var write = File.CreateText(fileName);
        write.WriteLine(primaryStat);
        write.WriteLine(abilityCharges);
        write.WriteLine(healthDice);
        write.WriteLine(armorProficiency);
        write.WriteLine(weaponProficiency);
        write.WriteLine(aaa);
        write.Close();
    }
}
