using System;

using Windows.UI.Xaml.Navigation;

using Prism.Windows.Navigation;

using Pyxis.Models.Parameters;

namespace Pyxis.Navigation
{
    public class PyxisNavigatingFromEventArgs : EventArgs
    {
        private readonly NavigatingFromEventArgs _e;
        private TransitionParameter _parsedParameter;

        public bool Cancel
        {
            get { return _e.Cancel; }
            set { _e.Cancel = value; }
        }

        public NavigationMode NavigationMode
        {
            get { return _e.NavigationMode; }
            set { _e.NavigationMode = value; }
        }

        public object Parameter
        {
            get { return _e.Parameter; }
            set { _e.Parameter = value; }
        }

        public Type SourcePageType => _e.SourcePageType;

        public PyxisNavigatingFromEventArgs(NavigatingFromEventArgs e)
        {
            _e = e;
        }

        public T ParsedQuery<T>() where T : TransitionParameter
            => (T) (_parsedParameter ?? (_parsedParameter = TransitionParameter.FromQuery<T>(Parameter.ToString())));
    }
}