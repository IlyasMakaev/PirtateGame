using UnityEngine;

namespace Scripts.Components 
{
    class DoInteractionComponent : MonoBehaviour
    {
        public void DoInteraction(GameObject go)
        {
            var interactble = go.GetComponent<InteractbleComponent>();
            if(interactble != null)
            {
                interactble.Interact();
            }
        }
    }
}
