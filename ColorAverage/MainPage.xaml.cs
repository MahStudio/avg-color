using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Imaging;
using Windows.Storage.Streams;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace ColorAverage
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        string testlink = "https://simplesnapbacks.files.wordpress.com/2014/03/abstract-purple-background-wallpaper-hd.jpg";
        public MainPage()
        {
            this.InitializeComponent();
        }

        public Color GetPixel(byte[] pixels, int x, int y, uint width, uint height)
        {
            int i = x;
            int j = y;
            int k = (i * (int)width + j) * 4;
            var a = pixels[k + 3];
            var r = pixels[k + 2];
            var g = pixels[k + 1];
            var b = pixels[k + 0];
            return Color.FromArgb(a, r, g, b);
        }

        private async void ImageLink_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(ImageLink.Text != "")
            {
                try
                {
                    var random = await RandomAccessStreamReference.CreateFromUri(new Uri(ImageLink.Text, UriKind.RelativeOrAbsolute)).OpenReadAsync();

                    var bitmap = await BitmapDecoder.CreateAsync(random);
                    var ba = (await bitmap.GetPixelDataAsync()).DetachPixelData();
                    Color tempColor;
                    long sumA = 0, sumR = 0, sumG = 0, sumB = 0;
                    for (int i = 0; i < bitmap.PixelHeight; i++)
                    {
                        for (int j = 0; j < bitmap.PixelWidth; j++)
                        {
                            tempColor = GetPixel(ba, 1, 1, Convert.ToUInt32(j), Convert.ToUInt32(i));
                            sumA += Convert.ToInt64(tempColor.A);
                            sumR += Convert.ToInt64(tempColor.R);
                            sumG += Convert.ToInt64(tempColor.G);
                            sumB += Convert.ToInt64(tempColor.B);
                        }
                    }
                    Color result = new Color();
                    result.A = (byte)(sumA / (bitmap.PixelHeight * bitmap.PixelWidth));
                    result.R = (byte)(sumR / (bitmap.PixelHeight * bitmap.PixelWidth));
                    result.G = (byte)(sumG / (bitmap.PixelHeight * bitmap.PixelWidth));
                    result.B = (byte)(sumB / (bitmap.PixelHeight * bitmap.PixelWidth));
                    this.Background = new SolidColorBrush(result);

                }
                catch { }
            }
        }
    }
}
