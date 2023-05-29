using UnityEngine;

namespace Assets.Scripts.Components.Audio
{
    class PlaySfxSound : MonoBehaviour
    {
        [SerializeField] private AudioClip _clip;
        private AudioSource _source;

    }
}
