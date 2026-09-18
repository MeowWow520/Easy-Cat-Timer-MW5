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
    /// PomodoroControl.xaml 的交互逻辑
    /// </summary>
    public partial class PomodoroControl : UserControl
    {
        /* 4个设置的取值范围 */
        private const int workMinMinutes = 1;
        private const int workMaxMinutes = 180;
        private const int breakMinMinutes = 1;
        private const int breakMaxMinutes = 120;
        private const int roundsMin = 1;
        private const int roundsMax = 12;


        public PomodoroControl()
        {
            InitializeComponent();
        }



        #region 公开方法 -[打开和关闭]
        /// <summary>
        /// 打开或者关闭 此界面
        /// </summary>
        /// <param name="_isOpen">是否打开？</param>
        public void OpenOrClose(bool _isOpen)
        {
            if (_isOpen == true)
            {
                Open();
            }
            else
            {
                Close();
            }
        }


        /// <summary>
        /// 打开 此界面
        /// </summary>
        private void Open()
        {
            this.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// 关闭 此界面
        /// </summary>
        private void Close()
        {
            this.Visibility = Visibility.Collapsed;
        }
        #endregion


        #region 控件的事件 -[开始]
        /// <summary>
        /// 当点击[开始]按钮时
        /// </summary>
        private void StartButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            //播放音效
            AppManager.AppSystems.AudioSystem.PlayAudio(AudioType.DefaultButtonUp);

            //如果有"等待开始"的阶段就开始它，否则从头开始一个番茄钟
            AppManager.AppSystems.PomodoroSystem.StartOrContinue();
        }
        #endregion


        #region 控件的事件 -[增加或减少 4个设置]
        /* [工作]的时长 */
        private void WorkUpButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessWorkMinutes(1);
        }
        private void WorkDownButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessWorkMinutes(-1);
        }


        /* [短休息]的时长 */
        private void ShortBreakUpButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessShortBreakMinutes(1);
        }
        private void ShortBreakDownButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessShortBreakMinutes(-1);
        }


        /* [长休息]的时长 */
        private void LongBreakUpButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessLongBreakMinutes(1);
        }
        private void LongBreakDownButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessLongBreakMinutes(-1);
        }


        /* [长休息]的间隔（几轮） */
        private void RoundsUpButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessRounds(1);
        }
        private void RoundsDownButton_Click(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            AddOrLessRounds(-1);
        }
        #endregion


        #region 私有方法 -[修改4个设置]
        /// <summary>
        /// 增加或者减少 [工作]的时长
        /// </summary>
        /// <param name="_changeMinutes">要改变的分钟数（+1 或者 -1）</param>
        private void AddOrLessWorkMinutes(int _changeMinutes)
        {
            int _minutes = AppManager.AppDatas.SettingData.PomodoroWorkMinutes + _changeMinutes;
            AppManager.AppDatas.SettingData.PomodoroWorkMinutes = Tools.Clamp(_minutes, workMinMinutes, workMaxMinutes);

            PlayNumberAudio();
        }


        /// <summary>
        /// 增加或者减少 [短休息]的时长
        /// </summary>
        /// <param name="_changeMinutes">要改变的分钟数（+1 或者 -1）</param>
        private void AddOrLessShortBreakMinutes(int _changeMinutes)
        {
            int _minutes = AppManager.AppDatas.SettingData.PomodoroShortBreakMinutes + _changeMinutes;
            AppManager.AppDatas.SettingData.PomodoroShortBreakMinutes = Tools.Clamp(_minutes, breakMinMinutes, breakMaxMinutes);

            PlayNumberAudio();
        }


        /// <summary>
        /// 增加或者减少 [长休息]的时长
        /// </summary>
        /// <param name="_changeMinutes">要改变的分钟数（+1 或者 -1）</param>
        private void AddOrLessLongBreakMinutes(int _changeMinutes)
        {
            int _minutes = AppManager.AppDatas.SettingData.PomodoroLongBreakMinutes + _changeMinutes;
            AppManager.AppDatas.SettingData.PomodoroLongBreakMinutes = Tools.Clamp(_minutes, breakMinMinutes, breakMaxMinutes);

            PlayNumberAudio();
        }


        /// <summary>
        /// 增加或者减少 [长休息]的间隔（几轮）
        /// </summary>
        /// <param name="_changeRounds">要改变的轮数（+1 或者 -1）</param>
        private void AddOrLessRounds(int _changeRounds)
        {
            int _rounds = AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak + _changeRounds;
            AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak = Tools.Clamp(_rounds, roundsMin, roundsMax);

            PlayNumberAudio();
        }


        /// <summary>
        /// 播放[数字改变]的音效
        /// </summary>
        private void PlayNumberAudio()
        {
            AppManager.AppSystems.AudioSystem.PlayAudio(AudioType.AddOrlessNumberSoundPlayer);
        }
        #endregion
    }
}
