using System;
using System.ComponentModel;
using Android.Content;
using Android.Runtime;
using Plugin.Iconize;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
#if USE_FASTRENDERERS
using LabelRenderer = Xamarin.Forms.Platform.Android.FastRenderers.LabelRenderer;
#else
using LabelRenderer = Xamarin.Forms.Platform.Android.LabelRenderer;
#endif

[assembly: ExportRenderer(typeof(IconLabel), typeof(IconLabelRenderer))]
namespace Plugin.Iconize
{
    /// <summary>
    /// Defines the <see cref="IconLabel" /> renderer.
    /// </summary>
#if USE_FASTRENDERERS
    /// <seealso cref="Xamarin.Forms.Platform.Android.FastRenderers.LabelRenderer" />
#else
    /// <seealso cref="Xamarin.Forms.Platform.Android.LabelRenderer" />
#endif
    public class IconLabelRenderer : LabelRenderer
    {
        /// <summary>
        /// Gets the label.
        /// </summary>
        /// <value>
        /// The label.
        /// </value>
        private IconLabel Label => Element as IconLabel;

        /// <summary>
        /// Initializes a new instance of the <see cref="IconLabelRenderer"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public IconLabelRenderer(Context context)
            : base(context)
        {
            // Intentionally left blank
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IconLabelRenderer"/> class.
        /// </summary>
        /// <param name="javaReference"></param>
        /// <param name="transfer"></param>
        /// <remarks>Fix for Xamarin.Forms 3.1.0.697729 (not needed with Xamarin.Forms 3.0.0.561731)</remarks>
        [Obsolete]
        public IconLabelRenderer(IntPtr javaReference, JniHandleOwnership transfer) { }

        /// <inheritdoc />
        protected override void OnElementChanged(ElementChangedEventArgs<Label> e)
        {
            try
            {
                base.OnElementChanged(e);

                if (Label == null)
                    return;

                UpdateText();
            }
            catch (Exception)
            {
                // Do nothing
            }
        }

        /// <inheritdoc />
        protected override void OnElementPropertyChanged(Object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (Label == null)
                return;

            switch (e.PropertyName)
            {
                case nameof(IconLabel.FontSize):
                case nameof(IconLabel.TextColor):
                    UpdateText();
                    break;
            }
        }

        /// <inheritdoc />
        protected override void OnAttachedToWindow()
        {
            base.OnAttachedToWindow();

            //if (Element == null || Control == null)
            //    return;

#if USE_FASTRENDERERS
            TextChanged += OnTextChanged;
#else
            Control.TextChanged += OnTextChanged;
#endif
        }

        /// <inheritdoc />
        protected override void OnDetachedFromWindow()
        {
            if (Element == null)
                return;

#if USE_FASTRENDERERS
            TextChanged -= OnTextChanged;
#else
            Control.TextChanged -= OnTextChanged;
#endif
            base.OnDetachedFromWindow();
        }

        private void OnTextChanged(Object sender, Android.Text.TextChangedEventArgs e)
        {
            UpdateText();
        }

        private void UpdateText()
        {
#if USE_FASTRENDERERS
            TextChanged -= OnTextChanged;
#else
            Control.TextChanged -= OnTextChanged;
#endif

            var icon = Iconize.FindIconForKey(Label.Text);
            if (icon != null)
            {
#if USE_FASTRENDERERS
                Text = $"{icon.Character}";
                Typeface = Iconize.FindModuleOf(icon).ToTypeface(Context);
#else
                Control.Text = $"{icon.Character}";
                Control.Typeface = Iconize.FindModuleOf(icon).ToTypeface(Context);
#endif
            }
#if USE_FASTRENDERERS
            TextChanged += OnTextChanged;
#else
            Control.TextChanged += OnTextChanged;
#endif
        }
    }
}