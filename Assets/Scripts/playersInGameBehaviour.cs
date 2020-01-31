using UnityEngine;
using System.Collections;
using System.IO;

public class playersInGameBehaviour : Bolt.EntityBehaviour<IPlayersInGame>
{
    public override void Attached()
    {

        string fileName = "playersInGame";
        var sr = File.OpenText(fileName);
        if (entity.IsOwner)
            state.Amount = sr.ReadLine();
        sr.Close();
        if (state.Amount == "0")
        {
            Destroy(gameObject);
        }
    }

    public override void SimulateOwner()
    {
     
    }

}
