using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scripts.UI
{
    public class ButtonUI : MonoBehaviour
    {
        [SerializeField]

        private Action action = null;
        private Button button = null;

        private void Awake()
        {
            button = GetComponent<Button>();
            button.onClick.AddListener(Action);
        }

        private void Action()
        {
            action?.Invoke();
        }

        public void SetAction(Action action)
        {
            this.action = action;
        }


    }

}
