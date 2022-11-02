using UnityEngine;
using UnityEngine.Events;

public class Switch : MonoBehaviour
{
    public Vector2 Size;
    public UnityEvent Activate;
    public UnityEvent Deactivate;
    private Transform _player;
    private bool _isActivated;

    void Start()
    {
        var playerObj = GameObject.Find("Player");
        if (playerObj != null)
        {
            _player = playerObj.transform;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)
            && _player != null
            && Mathf.Abs(transform.position.x - _player.position.x) <= Size.x / 2
            && Mathf.Abs(transform.position.y - _player.position.y) <= Size.y / 2)
        {

            var rod = transform.Find("Lever");

            if (_isActivated)
            {
                _isActivated = false;
                if (rod != null)
                {
                    rod.transform.eulerAngles = new Vector3(0, 0, 30);
                }
                Deactivate.Invoke();
            }
            else
            {
                _isActivated = true;
                if (rod != null)
                {
                    rod.transform.eulerAngles = new Vector3(0, 0, -30);
                }
                Activate.Invoke();
            }
        }
    }
}
