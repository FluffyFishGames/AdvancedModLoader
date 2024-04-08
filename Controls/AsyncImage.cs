using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Metadata;
using Avalonia.Controls.Primitives;
using Avalonia.Labs.Controls.Cache;
using Avalonia.Media;
using Avalonia.Media.Imaging;
using Avalonia.Platform;
using Avalonia.Threading;

namespace AdvancedModLoader.Controls
{
    [TemplatePart("PART_Image", typeof(Avalonia.Controls.Image))]
    [TemplatePart("PART_Placeholder", typeof(Avalonia.Controls.ContentControl))]
    public partial class AsyncImage : TemplatedControl
    {
        protected Avalonia.Controls.Image? ImagePart { get; private set; }
        protected Avalonia.Controls.ContentControl? PlaceholderPart { get; private set; }

        private bool IsInitialized = false;
        private CancellationTokenSource? TokenSource;
        private AsyncImageState _State;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            ImagePart = e.NameScope.Get<Avalonia.Controls.Image>("PART_Image");
            PlaceholderPart = e.NameScope.Get<ContentControl>("PART_Placeholder");

            TokenSource = new CancellationTokenSource();
            IsInitialized = true;

            if (Source != null)
            {
                SetSource(Source);
            }
        }

        private async void SetSource(object? source)
        {
            if (!IsInitialized)
            {
                return;
            }

            TokenSource?.Cancel();
            TokenSource = new CancellationTokenSource();

            AttachSource(null);

            if (source == null)
                return;

            State = AsyncImageState.Loading;

            if (Source == null)
                return;

            var uri = Source;
            try
            {
                var bitmap = await Source.LoadImageAsync(TokenSource.Token);
                AttachSource(bitmap);
            }
            catch (Exception ex)
            {
                await Dispatcher.UIThread.InvokeAsync(() => {
                    State = AsyncImageState.Failed;
                    RaiseEvent(new AsyncImageFailedEventArgs(ex));
                });
            }
        }

        private void AttachSource(IImage? image)
        {
            if (ImagePart != null)
            {
                ImagePart.Source = image;
            }

            TokenSource?.Cancel();
            TokenSource = new CancellationTokenSource();

            if (image == null)
            {
                State = AsyncImageState.Unloaded;

                ImageTransition?.Start(ImagePart, PlaceholderPart, true, TokenSource.Token);
            }
            else if (image.Size != default)
            {
                State = AsyncImageState.Loaded;

                ImageTransition?.Start(PlaceholderPart, ImagePart, true, TokenSource.Token);

                RaiseEvent(new Avalonia.Interactivity.RoutedEventArgs(OpenedEvent));
            }
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SourceProperty)
            {
                SetSource(Source);
            }
        }
    }
}