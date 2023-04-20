using UnityEngine;
using System.Collections;

//音频管理，在这里加入素材
public class SoundManager : MonoBehaviour
{
    static SoundManager instance;

    public static SoundManager GetInstance()
    {
        if (instance == null)
        {
            var k = GameObject.Find("ManagerObject").AddComponent<SoundManager>();
            instance = k;

        }
        return instance;
    }

    public AudioClip ShootSound;
    public AudioClip ConfirmSound;
    public AudioClip ObjectDestroyedSound;

    private AudioSource source;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            source = GetComponent<AudioSource>();
            Debug.Log("SoundManager initialized");
        }
        ShootSound = Resources.Load<AudioClip>("music/11805");
    }

    //TODO::Add sound effects
    public void PlayShootSound()
    {
        source.PlayOneShot(ShootSound);
        return;
    }

    public void PlayConfirmSound()
    {
        return;
        source.PlayOneShot(ConfirmSound);
    }

    public void PlayObjectDestroyedSound()
    {
        return;
        source.PlayOneShot(ObjectDestroyedSound);
    }
}
