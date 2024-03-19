using AdvancedModLoader.Image;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Logging;
using Avalonia.Media;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace AdvancedModLoader.Components
{
    /// <summary>
    /// A helper component which will displays a loading indicator while a provided IAsyncImage is loading and displaying the loaded image when finished.
    /// </summary>
    public partial class ImageLoader : ContentControl
    {
        /// <summary>
        /// Defines the IAsyncImage to load.
        /// </summary>
        public static readonly StyledProperty<IAsyncImage?> SourceProperty =AvaloniaProperty.Register<ImageLoader, IAsyncImage?>(nameof(Source));

        /// <summary>
        /// Contains the state of the current source IsLoading property.
        /// </summary>
        public static readonly DirectProperty<ImageLoader, bool> IsLoadingProperty = AvaloniaProperty.RegisterDirect<ImageLoader, bool>(nameof(IsLoading), image => image.Source?.IsLoading() ?? false);

        /// <summary>
        /// Contains the loaded image.
        /// </summary>
        public static readonly DirectProperty<ImageLoader, IImage?> LoadedImageProperty = AvaloniaProperty.RegisterDirect<ImageLoader, IImage?>(nameof(LoadedImage), image => image._LoadedImage);

        /// <summary>
        /// Cancellation token for loading an image.
        /// </summary>
        protected CancellationTokenSource? LoadToken;

        public ImageLoader()
        {
            InitializeComponent();
        }

        private IImage? _LoadedImage;

        /// <summary>
        /// The currently loaded image.
        /// </summary>
        public IImage? LoadedImage
        {
            get => _LoadedImage;
            internal set => SetAndRaise(LoadedImageProperty, ref _LoadedImage, value);
        }
        
        /// <summary>
        /// The loading state of the IAsyncImage
        /// </summary>
        public bool IsLoading
        {
            get => Source?.IsLoading() ?? false;
        }

        /// <summary>
        /// The current IAsyncImage
        /// </summary>
        public IAsyncImage? Source
        {
            get => GetValue(SourceProperty);
            set => SetValue(SourceProperty, value);
        }

        protected override async void OnPropertyChanged(AvaloniaPropertyChangedEventArgs change)
        {
            if (change.Property == SourceProperty)
            {
                LoadedImage = await this.LoadImage();
            }
            base.OnPropertyChanged(change);
        }

        /// <summary>
        /// Loads an image. All caching etc. should be done in your implementation of IAsyncImage.
        /// </summary>
        /// <returns>The loaded image</returns>
        protected async Task<IImage?> LoadImage()
        {
            var cancelToken = new CancellationTokenSource();
            var oldCancelToken = Interlocked.Exchange(ref LoadToken, cancelToken);
            try
            {
                oldCancelToken?.Cancel();
            }
            catch (ObjectDisposedException) 
            { 
            }

            if (Source == null)
                return null;

            // adding property changed listeners
            Source.PropertyChanged += Source_PropertyChanged;
            var ret = await Source.LoadImageAsync(cancelToken);
            // we should remove property changed listeners
            Source.PropertyChanged -= Source_PropertyChanged;

            return ret;
        }

        private void Source_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "IsLoading")
                RaisePropertyChanged<bool>(IsLoadingProperty, false, true);
        }
    }
}
