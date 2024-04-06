using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using System;
using System.Threading.Tasks;

namespace DispatherQueueSample

{
    public static class DispatcherQueueExtension
    {
        public static bool TryEnqueue(this DispatcherQueue dispatcherQueue,
            Action enqeueuingAction, Action<Exception> exceptionAction)
        {
            return dispatcherQueue.TryEnqueue(() =>
            {
                try
                {
                    enqeueuingAction();
                }
                catch (Exception ex)
                {
                    exceptionAction(ex);
                }
            });
        }
    }

    public partial class MainWindowViewMoel : ObservableObject
    {
        private readonly DispatcherQueue dispatcherQueue;

        public MainWindowViewMoel()
        {
            dispatcherQueue = DispatcherQueue.GetForCurrentThread();
        }

        [ObservableProperty]
        private string text = string.Empty;

        [RelayCommand]
        private async Task UpdateText()
        {
            await Task.Run(() =>
            {
                dispatcherQueue.TryEnqueue(() =>
                {
                    Text = DateTime.Now.ToString();
                    throw new Exception("Something went wrong!");
                },
                ex =>
                {
                    Text = ex.Message;
                });
            });
        }
    }
}
