using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Scripts.Model.Data.Properties
{
    public abstract class PersistentProperty<TPropertyType>
    {
        [SerializeField] private TPropertyType _value;
        private TPropertyType _defaultValue;

        public delegate void OnPropertyChanged(TPropertyType newValue, TPropertyType oldvalue);
        public event OnPropertyChanged OnChanged;

        public PersistentProperty(TPropertyType defaultValue)
        {
            _defaultValue = defaultValue;
        }


        public TPropertyType Value
        {
            get => _value;
            set
            {
                var isEquals = _value.Equals(value);
                if (isEquals) return;

                var oldVaue = _value;
                Write(value);
                _value = value;

                OnChanged?.Invoke(value, oldVaue);
            }
        }

        protected void Init()
        {
            _value = Read(_defaultValue);
        }

        protected abstract void Write(TPropertyType value);
        protected abstract TPropertyType Read(TPropertyType defaultValue);
    }
}
