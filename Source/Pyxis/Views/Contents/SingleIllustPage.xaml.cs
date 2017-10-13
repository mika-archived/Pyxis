using Windows.UI.Xaml.Controls;

using Pyxis.ViewModels.Contents;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pyxis.Views.Contents
{
    public sealed partial class SingleIllustPage : UserControl
    {
        public SingleIllustPageViewModel ViewModel => DataContext as SingleIllustPageViewModel;

        public SingleIllustPage()
        {
            InitializeComponent();
        }
    }
}