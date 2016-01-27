using System.Diagnostics.Contracts;
using System.Linq;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace PgenUWP
{
    public static class ItemClickCommand
    {
        public static readonly DependencyProperty CommandProperty =
            DependencyProperty.RegisterAttached(
                "Command", 
                typeof(ICommand),
                typeof(ItemClickCommand), 
                new PropertyMetadata(null, OnCommandPropertyChanged));

        public static void SetCommand(DependencyObject dependencyObject, ICommand value)
        {
            dependencyObject.SetValue(CommandProperty, value);
        }

        public static ICommand GetCommand(DependencyObject dependencyObject)
        {
            return (ICommand)dependencyObject.GetValue(CommandProperty);
        }

        private static void OnCommandPropertyChanged(
            DependencyObject dependencyObject,
            DependencyPropertyChangedEventArgs e)
        {
            var targetObject = dependencyObject as ListViewBase;
            Contract.Assert(targetObject != null);
            Contract.Assert(
                targetObject.SelectionMode == ListViewSelectionMode.Single,
                "Item click command can be used only with single selection list view base object.");
            (dependencyObject as ListViewBase).Tapped += OnItemTapped;
        }

        private static void OnItemTapped(object sender, TappedRoutedEventArgs e)
        {
            var control = sender as ListViewBase;
            var command = GetCommand(control);
            if (control == null || command == null)
            {
                return;
            }

            var selectedItem = control.SelectedItems.FirstOrDefault();
            if (selectedItem != null && command.CanExecute(selectedItem))
            {
                command.Execute(selectedItem);
            }
        }
    }
}
