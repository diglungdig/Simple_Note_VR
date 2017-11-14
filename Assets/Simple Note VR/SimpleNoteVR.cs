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
    [Range(0.4f, 1f), Header("Distance to Camera")]
    public float Multiplier;

    #region Private Variables
    private Camera MainCam;
    private TextMesh tmesh;
    private Rigidbody rigid;
    private SpriteRenderer BorderSprite;
    private Coroutine Cached;
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
        BorderSprite.color = FrameColor;
    }

    /// <summary>
    /// Call this function to trigger notification
    /// </summary>
    /// <param name="words"></param>
    public void Notify(string words)
    {
        if (Cached != null)
        {
            StopCoroutine(Cached);
            ResetValues();
        }

        //Calculate position to appear
        transform.position = MainCam.transform.position + MainCam.transform.forward * Multiplier;
        transform.rotation = Quaternion.LookRotation(transform.position - MainCam.transform.position);

        //Calculate border length to match word length
        BorderSprite.size = new Vector2((words.Length) / 30f, 0.1f);

        tmesh.text = words;

        Cached = StartCoroutine(Recycle(1.5f, 2f));
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
        rigid.AddTorque(Vector3.left, ForceMode.Impulse);
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
}
