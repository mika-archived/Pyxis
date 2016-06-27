using System;
using System.Globalization;
using System.Threading.Tasks;

using Windows.UI.Xaml.Controls;

using Pyxis.Services.Interfaces;
using Pyxis.ViewModels.Base;

namespace Pyxis.Services
{
    public class DialogService : IDialogService
    {
        private Type GetTypeFromToken(string dialogToken)
        {
            var assemblyQualifiedAppType = GetType().AssemblyQualifiedName;
            var pageNameWithParameter = assemblyQualifiedAppType.Replace(GetType().FullName,
                                                                         typeof(App).Namespace +
                                                                         ".Views.{0}Dialog");

            var viewFullName = string.Format(CultureInfo.InvariantCulture, pageNameWithParameter, dialogToken);
            var viewType = Type.GetType(viewFullName);

            if (viewType == null)
                throw new ArgumentException(string.Format("{0}'{1}' is not found.", nameof(dialogToken), dialogToken));

            return viewType;
        }

        #region Implementation of IDialogService

        public async Task<ContentDialogResult> ShowDialogAsync(string dialogToken)
        {
            var type = GetTypeFromToken(dialogToken);
            var instance = Activator.CreateInstance(type) as ContentDialog;
            if (instance == null)
                throw new NotSupportedException();
            return await instance.ShowAsync();
        }

        public async Task<object> ShowDialogAsync(string dialogToken, object parameter)
        {
            var type = GetTypeFromToken(dialogToken);
            var instance = Activator.CreateInstance(type) as ContentDialog;
            if (instance == null)
                throw new NotSupportedException();
            (instance.DataContext as DialogViewModel)?.OnInitialize(parameter);
            await instance.ShowAsync();
            (instance.DataContext as DialogViewModel)?.OnFinalize();
            return (instance.DataContext as DialogViewModel)?.ResultValue;
        }

        #endregion
    }
}