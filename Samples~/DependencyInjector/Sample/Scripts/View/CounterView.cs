using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DependencyInjector.Examples
{
    public class CounterView : MonoBehaviour, ICounterView
    {
        [Header("References")] 
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Button _button; 

        public Action OnAddNumberClick { get; set; }

        private void Awake()
        {
            _button.onClick.AddListener(AddNumber);
        }

        private void AddNumber()
        {
            OnAddNumberClick?.Invoke();
        }

        public void ShowCount(string count)
        {
            _countText.text = count;
        }
    }
}