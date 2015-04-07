using System;

namespace XNA_GSM.Utils
{
    public class BindingProperty<T>
    {
        public BindingProperty(T value)
        {
            _oldValue = value;
            _value = value;
        }

        private BindingProperty(Action<T> parent, T value)
        {
            _value = value;
            _oldValue = value;
            PrivatePropertyChanged += parent;
        }

        private T _value;
        private T _oldValue;

        private bool _isMyInvocation;

        public T Value
        {
            get { return _value; }
            set
            {
                _isMyInvocation = true;
                SetValueAndRaisePrivateEvent(value);
            }
        }

        private void SetValueAndRaisePrivateEvent(T value)
        {
            if (!_isMyInvocation && PropertyChanged != null)
                PropertyChanged(_oldValue, value);
            _isMyInvocation = false;
            _oldValue = _value;
            _value = value;
            PrivatePropertyChanged(value);
        }

        private void SetValueAndRaisePublicEvent(T value)
        {
            if (PropertyChanged != null)
                PropertyChanged(_oldValue, value);
            _value = value;
        }

        private event Action<T> PrivatePropertyChanged;
        public event Action<T, T> PropertyChanged;

        public BindingProperty<T> NewBoundProperty()
        {
            BindingProperty<T> bindingProperty = new BindingProperty<T>(SetValueAndRaisePrivateEvent, Value);
            PrivatePropertyChanged += bindingProperty.SetValueAndRaisePublicEvent;
            return bindingProperty;
        }
        public override string ToString()
        {
            return _value.ToString();
        }
    }
}
