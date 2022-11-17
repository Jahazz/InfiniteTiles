using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public enum OffMeshLinkMoveMethod
{
	Teleport,
	NormalSpeed,
	Parabola
}

[RequireComponent(typeof(UnityEngine.AI.NavMeshAgent))]
public class AgentLinkMover : MonoBehaviour
{
	public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
	IEnumerator Start ()
	{
		UnityEngine.AI.NavMeshAgent agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
		agent.autoTraverseOffMeshLink = false;
		while (true)
		{
			if (agent.isOnOffMeshLink)
			{
				if (method == OffMeshLinkMoveMethod.NormalSpeed)
					yield return StartCoroutine(NormalSpeed(agent));
				else if (method == OffMeshLinkMoveMethod.Parabola)
					yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
			}
			yield return null;
		}
	}
	IEnumerator NormalSpeed (NavMeshAgent agent)
	{
		OffMeshLinkData data = agent.currentOffMeshLinkData;
		TileConnector currentConnector = data.offMeshLink.GetComponent<TileConnector>();
		//CustomTile startingTile = 
		Vector3 endPos = transform.forward + transform.position;

		while (Vector3.Distance(agent.transform.position , endPos) < 0.1f)
		{
			agent.transform.position = Vector3.Lerp(agent.transform.position, endPos, agent.speed * Time.deltaTime);
			yield return null;
		}

		agent.CompleteOffMeshLink();
	}
	IEnumerator Parabola (UnityEngine.AI.NavMeshAgent agent, float height, float duration)
	{
		UnityEngine.AI.OffMeshLinkData data = agent.currentOffMeshLinkData;
		Vector3 startPos = agent.transform.position;
		Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
		float normalizedTime = 0.0f;
		while (normalizedTime < 1.0f)
		{
			float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
			agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
			normalizedTime += Time.deltaTime / duration;
			yield return null;
		}
	}
}