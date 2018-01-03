using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvaderManager : MonoBehaviour
{
    public static InvaderManager I { get; set; }

    public Vector2 InvaderBounds;

    [Header("All curves are sampled x as time and y as amount")]
    public AnimationCurve MaxOpponentsOnBoard;
    public AnimationCurve SpawnDelay;
    public AnimationCurve SpawnDelayDelta;

    private int maxOpponentsOnBoard;
    private float startTime = 0f;

    private Coroutine updateRoutine = null;

    private List<Invader> invaders = new List<Invader>();

    public Invader InvaderPrefab;

    public static void RegisterNewInvader(Invader invdr)
    {
        I.invaders.Add(invdr);
    }

    public static void RemoveInvader(Invader invdr)
    {
        if (I)
        {
            I.invaders.Remove(invdr);
        }
    }

    public static bool IsInsideBounds(Vector2 position)
    {
        return true;
    }

    private void Awake()
    {
        I = this;
        invaders = new List<Invader>();
    }

    public void ClearObjects()
    {
        for (; 0 < invaders.Count;)
        {
            if (invaders[0] == null)
                invaders.RemoveAt(0);
            else
                invaders[0].DisableAndReuse();
        }

        if (updateRoutine != null)
            StopCoroutine(updateRoutine);
    }

    public void StartGame()
    {
        // Init
        startTime = Time.time;
        StartCoroutine(update());
    }

    private IEnumerator update()
    {
        while(true)
        {
            float waitDelay = SpawnInvader();
            yield return new WaitForSeconds(waitDelay);
        }
    }

    private float SpawnInvader()
    {
        float time = Time.time - startTime;
        maxOpponentsOnBoard = Mathf.RoundToInt(MaxOpponentsOnBoard.Evaluate(time));
        float delay = SpawnDelay.Evaluate(time);
        float delayDelta = SpawnDelayDelta.Evaluate(time);

        delay += Random.Range(-delayDelta, delayDelta);

        if(invaders.Count < maxOpponentsOnBoard)
        {
            Invader invdr = ObjectPool.CreateObject<Invader>(InvaderPrefab);

            invdr.ResetObject();

            Vector2 pos = new Vector2(transform.position.x, transform.position.y) + new Vector2(Random.Range(-InvaderBounds.x, InvaderBounds.x), Random.Range(-InvaderBounds.y, InvaderBounds.y));
            invdr.transform.position = pos;

            Debug.Log(invaders.Count);
        }

        return delay;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(InvaderBounds.x * 2f, InvaderBounds.y * 2f, 0f));
    }
}
