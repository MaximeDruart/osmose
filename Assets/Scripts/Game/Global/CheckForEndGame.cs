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
    public UnityEvent OnGameCompletionStart;

    public AudioSource audioSource;
    public AudioSource audioSourceSessionTermine;

    public BloomControllers BloomController;
    public Color EndColor;

    [Header("Particles")]

    public ParticleSystem elec1;
    public ParticleSystem elec2;


    void Start()
    {
        EyesMat = Eyes.GetComponent<SkinnedMeshRenderer>().material;

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
        mySequence.AppendInterval(5f);
        mySequence.AppendInterval(0f);
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(1, "_FinalProgress", 3f));
        }
        mySequence.AppendInterval(10.5f);
        mySequence.AppendInterval(1f);

        mySequence.AppendCallback(() =>
        {
            audioSource.Play();
        });
        mySequence.AppendInterval(4.2f);
        mySequence.AppendInterval(0f);
        mySequence.AppendCallback(() =>
        {
            OnGameCompletionStart.Invoke();
            animator.SetBool("isTaz", true);
            BloomController.SetBloomColor(EndColor);
            elec1.Play();
            elec2.Play();

        });
        // lightning effect
        mySequence.AppendInterval(0.1f);
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(0, "_uProgress", 1.6f));
            mySequence.Join(creatureMat.DOFloat(0, "_EmissionMapIntensity", 1.6f));
            mySequence.Join(creatureMat.DOFloat(0, "_EmissionZoneIntensity", 1.6f));
        }

        mySequence.AppendInterval(5.5f);
        mySequence.AppendInterval(0f);
        mySequence.AppendCallback(() =>
        {
            animator.SetBool("IsBackToFetus", true);
            EyesMat.DOFade(0, 0.5f);
            elec1.Stop();
            elec2.Stop();
        });
        foreach (var creatureMat in CreatureMats)
        {
            mySequence.Join(creatureMat.DOFloat(0, "_DissolveProgress", 3f).SetDelay(0.3f));
        }
        mySequence.AppendInterval(2f);
        mySequence.AppendCallback(() => audioSourceSessionTermine.Play());
        mySequence.AppendInterval(7f);
        mySequence.AppendInterval(0f);
        mySequence.AppendCallback(OnGameCompletion.Invoke);
    }
}
