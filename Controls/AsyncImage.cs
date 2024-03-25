/*
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
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

        private bool IsInitialized;
        private CancellationTokenSource? TokenSource;
        private AsyncImageState _state;

        protected override void OnApplyTemplate(TemplateAppliedEventArgs e)
        {
            base.OnApplyTemplate(e);

            ImagePart = e.NameScope.Get<Image>("PART_Image");
            PlaceholderPart = e.NameScope.Get<Image>("PART_PlaceholderImage");

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
            {
                return;
            }

            State = AsyncImageState.Loading;
            if (Source is IImage image)
            {
                AttachSource(image);
                return;
            }

            if (Source == null)
            {
                return;
            }

            var uri = Source;

            if (uri != null && uri.IsAbsoluteUri)
            {
                if (uri.Scheme == "http" || uri.Scheme == "https")
                {
                    Bitmap? bitmap = null;
                    // Android doesn't allow network requests on the main thread, even though we are using async apis.
#if NET6_0_OR_GREATER
                    if (OperatingSystem.IsAndroid())
                    {
                        await Task.Run(async () =>
                        {
                            try
                            {
                                bitmap = await LoadImageAsync(uri, TokenSource.Token);
                            }
                            catch (Exception ex)
                            {
                                await Dispatcher.UIThread.InvokeAsync(() => {
                                    State = AsyncImageState.Failed;

                                    RaiseEvent(new AsyncImageFailedEventArgs(ex));
                                });
                            }
                        });
                    }
                    else
#endif
                    {
                        try
                        {
                            bitmap = await LoadImageAsync(uri, TokenSource.Token);
                        }
                        catch (Exception ex)
                        {
                            await Dispatcher.UIThread.InvokeAsync(() => {
                                State = AsyncImageState.Failed;

                                RaiseEvent(new AsyncImageFailedEventArgs(ex));
                            });
                        }
                    }

                    AttachSource(bitmap);
                }
                else if (uri.Scheme == "avares")
                {
                    try
                    {
                        AttachSource(new Bitmap(AssetLoader.Open(uri)));
                    }
                    catch (Exception ex)
                    {
                        State = AsyncImageState.Failed;

                        RaiseEvent(new AsyncImageFailedEventArgs(ex));
                    }
                }
                else if (uri.Scheme == "file" && File.Exists(uri.LocalPath))
                {
                    AttachSource(new Bitmap(uri.LocalPath));
                }
                else
                {
                    RaiseEvent(new AsyncImageFailedEventArgs(new UriFormatException($"Uri has unsupported scheme. Uri:{source}")));
                }
            }
            else
            {
                RaiseEvent(new AsyncImageFailedEventArgs(new UriFormatException($"Relative paths aren't supported. Uri:{source}")));
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

        private async Task<Bitmap> LoadImageAsync(Uri? url, CancellationToken token)
        {
            if (await ProvideCachedResourceAsync(url, token) is { } bitmap)
            {
                return bitmap;
            }
#if NET6_0_OR_GREATER
            using var client = new HttpClient();
            var stream = await client.GetStreamAsync(url, token).ConfigureAwait(false);

            await using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream, token).ConfigureAwait(false);
#elif NETSTANDARD2_0
            using var client = new HttpClient();
            var response = await client.GetAsync(url, token).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            using var memoryStream = new MemoryStream();
            await stream.CopyToAsync(memoryStream).ConfigureAwait(false);
#endif

            memoryStream.Position = 0;
            return new Bitmap(memoryStream);
        }

        protected override void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            base.OnPropertyChanged(change);

            if (change.Property == SourceProperty)
            {
                SetSource(Source);
            }
        }

        protected virtual async Task<Bitmap?> ProvideCachedResourceAsync(Uri? imageUri, CancellationToken token)
        {
            if (IsCacheEnabled && imageUri != null)
            {
                return await ImageCache.Instance.GetFromCacheAsync(imageUri, cancellationToken: token).ConfigureAwait(false);
            }
            return null;
        }
    }
}
*/