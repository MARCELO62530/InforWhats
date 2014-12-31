namespace WhatsAppApi.Helper
{
    public class DebugAdapter
    {
        protected static DebugAdapter _instance;
        public static DebugAdapter Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new DebugAdapter();
                }
                return _instance;
            }
        }

        public event OnPrintDebugDelegate OnPrintDebug;
        internal void fireOnPrintDebug(object value)
        {
            if (OnPrintDebug != null)
            {
                OnPrintDebug(value);
            }
        }

        public delegate void OnPrintDebugDelegate(object value);
    }
}
