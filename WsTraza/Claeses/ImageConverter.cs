using SkiaSharp;
using Svg.Skia;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;

namespace WsTraza.Claeses
{
    public class ImageConverter
    {
        public string  ConvertPngToJpgWithWhiteBackground(string base64Image)
        {
            var imageBytes = Convert.FromBase64String(base64Image);
            using var ms = new MemoryStream(imageBytes);
            using var image = Image.FromStream(ms);
            // Crear un nuevo Bitmap con el tamaño original de la imagen y fondo blanco
            var resizedImage = new Bitmap(image.Width, image.Height);
            using (var graphics = Graphics.FromImage(resizedImage))
            {
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.Clear(Color.White); // Establecer el fondo blanco
                graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            }

            using (var msResized = new MemoryStream())
            {
                // Guardar la imagen convertida como JPG
                resizedImage.Save(msResized, System.Drawing.Imaging.ImageFormat.Jpeg);
                return Convert.ToBase64String(msResized.ToArray());
            }
        }

    }
}
