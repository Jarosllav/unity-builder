using System;

namespace nobodyworks.builder.utilities
{
    public class Condition : IDisposable
    {
        private Func<bool> _delegates;

        public void Dispose()
        {
            _delegates = null;
        }
        
        public void Subscribe(Func<bool> condition)
        {   
            _delegates += condition;
        }
        
        public void Unsubscribe(Func<bool> condition)
        {
            _delegates -= condition;
        }

        public bool AllTrue()
        {
            if (_delegates == null)
            {
                return true;
            }

            foreach (var handler in _delegates.GetInvocationList())
            {
                if (!(bool)handler.DynamicInvoke())
                {
                    return false;
                }
            }

            return true;
        }
    }
    
    
    public class Condition<Arg1> : IDisposable
        where Arg1 : class
    {
        private Func<Arg1, bool> _delegate;

        public void Dispose()
        {
            _delegate = null;
        }
        
        public void Subscribe(Func<Arg1, bool> condition)
        {   
            _delegate += condition;
        }
        
        public void Unsubscribe(Func<Arg1, bool> condition)
        {
            _delegate -= condition;
        }

        public bool AllTrue(Arg1 arg)
        {
            if (_delegate == null)
            {
                return true;
            }

            foreach (var handler in _delegate.GetInvocationList())
            {
                if (!(bool)handler.DynamicInvoke(arg))
                {
                    return false;
                }
            }

            return true;
        }
    }
}