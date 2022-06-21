using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class CheckForEndGame : MonoBehaviour
{
    // Start is called before the first frame update
    public CompletionState completionState;

    [SerializeField] private Animator animator;
    [SerializeField] private GameObject Eyes;
    private Material EyesMat;
    [SerializeField] private GameObject[] CreatureItems = new GameObject[4];
    private Material[] CreatureMats = new Material[4];

    public UnityEvent OnGameCompletion;

    private AudioSource audioSource;
    private AudioSource audioSourceSessionTermine;

    public BloomControllers BloomController;
    public Color EndColor;


    void Start()
    {
        EyesMat = Eyes.GetComponent<Renderer>().material;

        audioSource = GetComponent<AudioSource>();

        for (int i = 0; i < CreatureItems.Length; i++)
        {
            CreatureMats[i] = CreatureItems[i].GetComponent<Renderer>().material;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayTimeline();
        }
    }

    public void CheckForEnd()
    {
        if (completionState.IsCompleted())
        {
            PlayTimeline();
        }
    }

    public void PlayTimeline()
    {
        Debug.Log("play end tl");
        Sequence mySequence = DOTween.Sequence();
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(1, "_FinalProgress", 3f));
        }
        mySequence.AppendInterval(15.5f);
        mySequence.AppendInterval(1f);

        mySequence.AppendCallback(() =>
        {
            audioSource.Play();
        });
        mySequence.AppendInterval(2.2f);
        mySequence.AppendInterval(0f);
        mySequence.AppendCallback(() =>
        {
            animator.SetBool("isTaz", true);
            BloomController.SetBloomColor(EndColor);
        });
        // lightning effect
        mySequence.AppendInterval(0.1f);
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(0, "_uProgress", 1.6f));
            mySequence.Join(creatureMat.DOFloat(0, "_EmissionMapIntensity", 1.6f));
            mySequence.Join(creatureMat.DOFloat(0, "_EmissionZoneIntensity", 1.6f));
        }

        mySequence.AppendInterval(7.5f);
        mySequence.AppendInterval(0f);
        mySequence.AppendCallback(() =>
        {
            animator.SetBool("IsBackToFetus", true);
            audioSourceSessionTermine.Play();
        });
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(0, "_DissolveProgress", 3f).SetDelay(0.3f));
        }

        mySequence.AppendInterval(0f);
        mySequence.AppendInterval(5f);
        mySequence.AppendCallback(OnGameCompletion.Invoke);
    }
}
