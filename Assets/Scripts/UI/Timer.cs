using TMPro;
using UnityEngine;

public class Timer : MonoBehaviour
{
    [SerializeField] TMP_Text _text;

    float _timer;

    void Start()
    {
        _timer = 60;
    }

    void Update()
    {
        _timer -= Time.deltaTime;
        _text.text = Mathf.Ceil(_timer).ToString();
    }
}