namespace Auth.Models
{
    public class Authorization
    {
        #region Constructor

        public Authorization()
            : this("OnBoarding", Action.ReadOnly) { }

        public Authorization(string target, Action action)
        {
            this.Target = target;
            this.Action = action;
        }

        #endregion Constructor

        public string Target { get; private set; }

        public Action Action { get; private set; }
    }
}