using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InvaderManager : MonoBehaviour
{
    public static InvaderManager I { get; set; }

    public float PlayerRespawnDelay = 3f;

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
    public Defender PlayerObject;

    public UnityEvent OnGameOver;

    public static void RegisterNewInvader(Invader invdr)
    {
        if(I && !I.invaders.Contains(invdr))
            I.invaders.Add(invdr);
    }

    public static void RemoveInvader(Invader invdr)
    {
        if (I)
        {
            I.invaders.Remove(invdr);
        }
    }

    public static bool IsInsideBounds(Vector2 p)
    {
        Vector2 t = new Vector2(I.transform.position.x, I.transform.position.y);
        Vector2 b = I.InvaderBounds;

        return  (t.x - b.x < p.x && p.x < t.x + b.x) && // X
                (t.y - b.y < p.y && p.y < t.y + b.y);   // Y
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
        PlayerObject.ResetObject();
        startTime = Time.time;
        updateRoutine = StartCoroutine(update());
    }

    public void SpawnPlayer()
    {
        if (0 < LifeCounter.I.LifeCount)
        {
            LifeCounter.I.LostLife();
            StartCoroutine(playerRespawn());
        }
        else if(OnGameOver != null)
        {
            OnGameOver.Invoke();
        }
    }

    private IEnumerator playerRespawn()
    {
        ClearObjects();
        yield return new WaitForSeconds(PlayerRespawnDelay);
        PlayerObject.ResetObject();
        StartGame();
    }
        
    private IEnumerator update()
    {
        while(true)
        {
            float waitDelay = SpawnInvader();
            yield return new WaitForSeconds(waitDelay);
        }
    }

    public float SpawnInvader()
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
