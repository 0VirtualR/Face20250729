using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Face.Views
{
    /// <summary>
    /// CameraView.xaml 的交互逻辑
    /// </summary>
    public partial class CameraView : UserControl
    {
        public CameraView()
        {
            InitializeComponent();
            this.Loaded += (s, e) => StartScanAnimation();
        }
        // 启动扫描动画
        private void StartScanAnimation()
        {
            line2.RenderTransform.BeginAnimation(TranslateTransform.YProperty, null);

            // 重置位置
            ((TranslateTransform)line2.RenderTransform).Y = -573;
            //DoubleAnimation animation = new DoubleAnimation
            //{
            //    From = -600,
            //    To = 600,
            //    Duration = TimeSpan.FromSeconds(2),
            //    RepeatBehavior = RepeatBehavior.Forever
            //};

            //ScanTransform.BeginAnimation(TranslateTransform.YProperty, animation);
        }

    }
}
