using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using ViewFaceCore.Core;
using ViewFaceCore.Models;

namespace Test5.Services
{
    public class FaceService
    {

       
        public FaceService()
        {
          
        }
        public string  GetFaceInfo(BitmapSource bitmap)
        {
            private readonly static string imagePath = @"images/Jay_3.jpg";
        private readonly static string outputPath = @"images/Jay_out.jpg";

        static void Main(string[] args)
        {
            using var bitmap = (Bitmap)Image.FromFile(imagePath);
            using FaceDetector faceDetector = new FaceDetector();
            FaceInfo[] infos = faceDetector.Detect(bitmap);
            //输出人脸信息
            Console.WriteLine($"识别到的人脸数量：{infos.Length} 个人脸信息：\n");
            Console.WriteLine($"No.\t人脸置信度\t位置信息");
            for (int i = 0; i < infos.Length; i++)
            {
                Console.WriteLine($"{i}\t{infos[i].Score:f8}\t{infos[i].Location}");
            }
            //画方框，标记人脸
            using (Graphics g = Graphics.FromImage(bitmap))
            {
                g.DrawRectangles(new Pen(Color.Red, 4), infos.Select(p => new RectangleF(p.Location.X, p.Location.Y, p.Location.Width, p.Location.Height)).ToArray());
            }
            bitmap.Save(outputPath);
            Console.WriteLine($"输出图片已保存至：{outputPath}");
            Console.WriteLine();
        }
    }
}
