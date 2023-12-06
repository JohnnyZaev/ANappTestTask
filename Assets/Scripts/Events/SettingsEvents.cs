using System;

namespace Events
{
    /// <summary>
    /// Public static delegates to manage settings changes (note these are "events" in the conceptual sense
    /// and not the strict C# sense).
    /// </summary>
    public static class SettingsEvents 
    {
        // Notify set up script that UI is ready
        public static Action SettingsInitialized;

        // Presenter --> View: update UI sliders
        public static Action<float> MasterSliderSet;
        public static Action<float> SfxSliderSet;
        public static Action<float> MusicSliderSet;

        // View -> Presenter: update UI sliders
        public static Action<float> MasterSliderChanged;
        public static Action<float> SfxSliderChanged;
        public static Action<float> MusicSliderChanged;

        //// Presenter -> Model: update volume settings
        public static Action<float> MasterVolumeChanged;
        public static Action<float> SfxVolumeChanged;
        public static Action<float> MusicVolumeChanged;

        // Model -> Presenter: model values changed (e.g. loading saved values)
        public static Action<float> ModelMasterVolumeChanged;
        public static Action<float> ModelSfxVolumeChanged;
        public static Action<float> ModelMusicVolumeChanged;

    }
}
