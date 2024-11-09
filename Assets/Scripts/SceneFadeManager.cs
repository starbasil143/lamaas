using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class SceneFadeManager : MonoBehaviour
{
    public static SceneFadeManager instance;

    public Image _fadeOutImage;
    public float _fadeOutSpeed = 5f;
    public float _fadeInSpeed = 5f;

    public bool isFadingOut { get; private set; }
    public bool isFadingIn { get; private set; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        _fadeOutImage.fillOrigin = 0;
        _fadeOutImage.fillAmount = 0f;
    }

    private void Update()
    {
        if (isFadingOut)
        {
            if (_fadeOutImage.fillAmount < 1f)
            {
                _fadeOutImage.fillAmount += Time.deltaTime * _fadeOutSpeed;
            }
            else
            {
                isFadingOut = false;
            }
        }

        if (isFadingIn)
        {
            if (_fadeOutImage.fillAmount > 0f)
            {
                _fadeOutImage.fillAmount -= Time.deltaTime * _fadeInSpeed;
            }
            else
            {
                isFadingIn = false;
            }
        }
    }

    public void StartFadeOut()
    {
        isFadingOut = true;
        _fadeOutImage.fillOrigin = 0;
        _fadeOutImage.fillAmount = 0f;
    }
    public void StartFadeIn()
    {
        isFadingIn = true;
        _fadeOutImage.fillOrigin = 1;
        _fadeOutImage.fillAmount = 1f;
    }

}
