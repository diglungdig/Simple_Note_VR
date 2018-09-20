using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A simple notification solution for VR, utilizing Unity's text mesh feature. It simulates google tilt brush's notifcation system.
/// To use, simply call SimpleNoteVR.Instance.Notify("Hello Worlds");
/// Developed by Wei(github.com/diglungdig)
/// </summary>
public class SimpleNoteVR : MonoBehaviour
{
    //Singleton class
    public static SimpleNoteVR Instance;

    public Color TextColor = Color.white;
    public Color FrameColor = Color.white;
    public Font TextFont;
    public Sprite BackgroundSprite;

    [Range(0.4f, 5f), Header("Distance to Camera")]
    public float DistanceMultiplier = 0.4f;

    [Range(1f, 30f), Header("Character Size")]
    public float CharacterSizeMultiplier = 1f;

    [Range(1f, 10f), Header("Background Width Multiplier")]
    public float WidthMultiplier = 1f;

    #region Private Variables
    private Camera MainCam;
    private TextMesh tmesh;
    private Rigidbody rigid;
    private SpriteRenderer BorderSprite;
    private Coroutine Cached;

    private float DistanceCached;
    private float CharacterSizeCached;

    private bool Notify_Holding = false;
    #endregion

    #region Singleton build-ups
    void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
    void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }
    #endregion

    // Use this for initialization
    void Start()
    {
        MainCam = Camera.main;
        tmesh = gameObject.GetComponent<TextMesh>();

        if (tmesh == null)
        {
            tmesh = gameObject.AddComponent<TextMesh>();
        }
        tmesh.characterSize = 0.01f;
        tmesh.anchor = TextAnchor.MiddleCenter;
        tmesh.alignment = TextAlignment.Center;
        tmesh.fontSize = 30;
        tmesh.color = TextColor;
        tmesh.font = TextFont;

        GetComponent<MeshRenderer>().material = TextFont.material;

        BorderSprite = gameObject.GetComponentInChildren<SpriteRenderer>();
        if (BackgroundSprite != null)
        {
            BorderSprite.sprite = BackgroundSprite;
        }
        BorderSprite.color = FrameColor;

        CharacterSizeCached = tmesh.characterSize;
    }

    /// <summary>
    /// Call this function to trigger notification
    /// </summary>
    /// <param name="words"></param>
    public void Notify(string words, float lingeringTime)
    {
        if (Cached != null)
        {
            StopCoroutine(Cached);
            ResetValues();
        }

        //Calculate position to appear
        transform.position = MainCam.transform.position + MainCam.transform.forward * DistanceMultiplier;
        transform.rotation = Quaternion.LookRotation(transform.position - MainCam.transform.position);

        //Calculate border length to match word length
        BorderSprite.size = new Vector2((words.Length * CharacterSizeMultiplier) / 30f, 0.1f * CharacterSizeMultiplier * WidthMultiplier);

        tmesh.text = words;
        tmesh.characterSize = CharacterSizeCached * CharacterSizeMultiplier;

        Cached = StartCoroutine(Recycle(lingeringTime, 2f));
    }

    /// <summary>
    /// Pop out a notifcation and hold it
    /// </summary>
    /// <param name="words"></param>
    public void Notify_Hold(string words)
    {
        if (Cached != null)
        {
            StopCoroutine(Cached);
            ResetValues();
        }
        //Calculate position to appear
        transform.position = MainCam.transform.position + MainCam.transform.forward * DistanceMultiplier;
        transform.rotation = Quaternion.LookRotation(transform.position - MainCam.transform.position);

        //Calculate border length to match word length
        BorderSprite.size = new Vector2((words.Length * CharacterSizeMultiplier) / 30f, 0.1f * CharacterSizeMultiplier * WidthMultiplier);

        tmesh.text = words;
        tmesh.characterSize = CharacterSizeCached * CharacterSizeMultiplier;

        Notify_Holding = true;
    }

    /// <summary>
    /// Release the held notification
    /// </summary>
    public void Notify_Release()
    {
        if (Notify_Holding)
        {
            Cached = StartCoroutine(Recycle(0f, 2f));
        }
        else
        {
            Debug.Log("Not holding: no need to release");
        }

    }

    /// <summary>
    /// Recycle the notification
    /// </summary>
    /// <param name="WaitTimer"></param>
    /// <param name="DropTimer"></param>
    /// <returns></returns>
    private IEnumerator Recycle(float WaitTimer, float DropTimer)
    {
        yield return new WaitForSeconds(WaitTimer);

        rigid = gameObject.AddComponent<Rigidbody>();
        rigid.AddForce(Vector3.up * 1.5f, ForceMode.Impulse);
        rigid.AddTorque(Random.onUnitSphere, ForceMode.Impulse);
        rigid.interpolation = RigidbodyInterpolation.Interpolate;
        rigid.mass = 120f;
        rigid.useGravity = true;

        yield return new WaitForSeconds(DropTimer);

        ResetValues();
    }

    /// <summary>
    /// Reset properties
    /// </summary>
    private void ResetValues()
    {
        if (rigid != null)
        {
            Destroy(rigid);
        }
        tmesh.text = "";
        BorderSprite.size = Vector2.zero;
    }

    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.Space))
    //    {
    //        //SimpleNoteVR.Instance.Notify("Testisdadsdafeagew ffrfderare adedaewdewadng this it");
    //        Instance.Notify_Hold("Testing hold");
    //    }
    //    if (Input.GetKeyDown(KeyCode.Backspace))
    //    {
    //        Instance.Notify_Release();
    //    }
    //}
}
