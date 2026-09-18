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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// ClockControl.xaml 的交互逻辑
    /// </summary>
    public partial class ClockControl : UserControl
    {
        /* 属性: 时间(Time)
                阶段的名字(StageText)  —— 番茄钟用
                轮次(RoundText)        —— 番茄钟用 */

        public ClockControl()
        {
            InitializeComponent();
        }


        #region 依赖项属性：Time
        /// <summary>
        /// 依赖项属性：时间
        /// </summary>
        public static DependencyProperty TimeProperty;

        /// <summary>
        /// 公开属性：时间
        /// </summary>
        public DayTime Time
        {
            get { return (DayTime)GetValue(TimeProperty); }
            set { SetValue(TimeProperty, value); }
        }

        /// <summary>
        /// 依赖项属性发生改变时，触发的事件：
        /// 当TimeProperty依赖项属性，的属性值发生改变的时候，调用这个方法
        /// </summary>
        /// <param name="sender">依赖项对象</param>
        /// <param name="e">依赖项属性改变事件 的参数（里面有这个属性的新的值，和旧的值）</param>
        private static void OnTimeChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {

        }
        #endregion


        #region 依赖项属性：StageText（番茄钟：阶段的名字）
        /// <summary>
        /// 依赖项属性：阶段的名字（番茄钟用）
        /// </summary>
        public static DependencyProperty StageTextProperty;

        /// <summary>
        /// 公开属性：阶段的名字（番茄钟用）
        /// </summary>
        public string StageText
        {
            get { return (string)GetValue(StageTextProperty); }
            set { SetValue(StageTextProperty, value); }
        }
        #endregion


        #region 依赖项属性：RoundText（番茄钟：轮次）
        /// <summary>
        /// 依赖项属性：轮次（番茄钟用）
        /// </summary>
        public static DependencyProperty RoundTextProperty;

        /// <summary>
        /// 公开属性：轮次（番茄钟用）
        /// </summary>
        public string RoundText
        {
            get { return (string)GetValue(RoundTextProperty); }
            set { SetValue(RoundTextProperty, value); }
        }
        #endregion


        #region 静态构造方法：注册依赖项属性 和 路由事件
        /// <summary>
        /// 静态构造方法：在里面注册依赖项属性 和 路由事件
        /// </summary>
        static ClockControl()
        {
            /*注册依赖项属性*/
            //注册TimeProperty
            TimeProperty = DependencyProperty.Register(
                "Time", //属性的名字
                typeof(DayTime),//属性的类型
                typeof(ClockControl),//这个属性属于哪个控件？
                new FrameworkPropertyMetadata(//属性的初始值和回调函数
                    //初始值
                    (DayTime)new DayTime(0),
                    //当属性的值发生改变时，调用什么方法？
                    new PropertyChangedCallback(OnTimeChanged))
            );

            //注册StageTextProperty（番茄钟：阶段的名字）
            StageTextProperty = DependencyProperty.Register(
                "StageText", typeof(string), typeof(ClockControl),
                new FrameworkPropertyMetadata((string)"", new PropertyChangedCallback(OnPomodoroTextChanged))
            );

            //注册RoundTextProperty（番茄钟：轮次）
            RoundTextProperty = DependencyProperty.Register(
                "RoundText", typeof(string), typeof(ClockControl),
                new FrameworkPropertyMetadata((string)"", new PropertyChangedCallback(OnPomodoroTextChanged))
            );
        }
        #endregion


        #region 私有方法
        /// <summary>
        /// 当[阶段的名字]或者[轮次]发生改变时，触发此方法
        /// （如果两个都是空的，就把这一整块隐藏起来，让界面和原来一样）
        /// </summary>
        private static void OnPomodoroTextChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            ClockControl _clockControl = sender as ClockControl;
            if (_clockControl == null) return;

            bool _isShow = (string.IsNullOrEmpty(_clockControl.StageText) == false) ||
                           (string.IsNullOrEmpty(_clockControl.RoundText) == false);

            _clockControl.PomodoroTextPanel.Visibility = (_isShow == true)
                ? Visibility.Visible
                : Visibility.Collapsed;
        }
        #endregion
    }
}
