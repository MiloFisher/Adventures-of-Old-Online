using UnityEngine;
using System.Collections;

public class gamePieceBehaviour : Bolt.EntityBehaviour<IGamePieceState>
{
	public override void Attached()
	{
		state.SetTransforms(state.GamePieceTransform, transform);
	}

	public override void SimulateOwner()
	{
		var speed = 4f;
		var movement = Vector3.zero;

		if (Input.GetKey(KeyCode.W)) { movement.y += 1; }
		if (Input.GetKey(KeyCode.S)) { movement.y -= 1; }
		if (Input.GetKey(KeyCode.A)) { movement.x -= 1; }
		if (Input.GetKey(KeyCode.D)) { movement.x += 1; }

		if (movement != Vector3.zero)
		{
			transform.position = transform.position + (movement.normalized * speed * BoltNetwork.FrameDeltaTime);
		}
	}
}
