using Indulged.API.Networking;
using Indulged.API.Storage;
using Indulged.API.Storage.Events;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at http://go.microsoft.com/fwlink/?LinkId=234236

namespace Indulged.UI.Detail.Sections
{
    public sealed partial class EXIFSection : DetailSectionBase
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public EXIFSection()
        {
            this.InitializeComponent();
        }

        public override void AddEventListeners()
        {
            base.AddEventListeners();
            StorageService.Instance.EXIFUpdated += OnEXIFUpdated;
        }

        public override void RemoveEventListeners()
        {
            base.RemoveEventListeners();
            StorageService.Instance.EXIFUpdated -= OnEXIFUpdated;
        }

        protected override async void OnPhotoChanged()
        {
            base.OnPhotoChanged();
            if (Photo.EXIF != null)
            {
                EXIFLabel.Text = this.GetEXIFString();
            }
            else
            {
                EXIFLabel.Text = "Loading EXIF data...";
            }

            // Always refresh EXIF data
            var status = await APIService.Instance.GetEXIFAsync(Photo.ResourceId);
            if (!status.Success)
            {
                EXIFLabel.Text = "Cannot get EXIF data: " + status.ErrorMessage;
            }
        }

        private void OnEXIFUpdated(object sender, StorageEventArgs e)
        {
            if (e.PhotoId == Photo.ResourceId)
            {
                EXIFLabel.Text = GetEXIFString();
            }            
        }

        private string GetEXIFString()
        {
            if (Photo.EXIF.Count == 0)
            {
                return "This photo does not contain metadata";
            }

            string result = "";

            if (Photo.EXIF.ContainsKey("Model"))
                result = "This photo was taken with " + Photo.EXIF["Model"] + ". ";

            if (Photo.EXIF.ContainsKey("Aperture"))
                result += "Aperture was " + Photo.EXIF["Aperture"] + ". ";

            if (Photo.EXIF.ContainsKey("Exposure Program"))
                result += "Exposure mode was " + Photo.EXIF["Exposure Program"] + ". ";

            if (Photo.EXIF.ContainsKey("ISO Speed") && Photo.EXIF.ContainsKey("Exposure"))
                result += "ISO  " + Photo.EXIF["ISO Speed"] + " at " + Photo.EXIF["Exposure"] + ".";

            return result;

        }

    }
}
