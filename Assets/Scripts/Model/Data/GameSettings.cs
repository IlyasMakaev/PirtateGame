using UnityEngine;

namespace Scripts.Model.Data
{
    [CreateAssetMenu(menuName = "Data/GameSettings", fileName = "GameSettings")]
    public class GameSettings : ScriptableObject
    {
        [SerializeField] private float Music;
        [SerializeField] private float Sfx;

        private void OnEnable()
        {
            PlayerPrefs.SetFloat("music", 20f);
            PlayerPrefs.Save();
        }
    }
}
