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
using System.Windows.Shell;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// BlackCatControl.xaml 的交互逻辑
    /// </summary>
    public partial class BlackCatControl : UserControl
    {
        public BlackCatControl()
        {
            InitializeComponent();
        }



        #region [注册的的事件]
        //当点击猫咪按钮的时候，触发此方法
        private void CatButtonControl_OnClick(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            /* 停止计时，返回到设置时间的界面 */
            AppManager.AppSystems.TimeSystem.StopHandle();

            /* 如果是[番茄钟]模式，就把番茄钟也复位 */
            if (AppManager.AppDatas.SettingData.PomodoroEnabled == true)
            {
                AppManager.AppSystems.PomodoroSystem.ResetPomodoro();
            }

            /* 显示[设定界面]（普通倒计时 或者 番茄钟） */
            AppManager.MainWindow.OpenSetupUi(true);

            /* 把黄猫咪的按钮，设置为暂停按钮 */
            AppManager.MainWindow.YellowCatUserControl.IsPauseButton = true;

            /* 关闭暂停界面 */
            AppManager.MainWindow.PausedUiUserControl.OpenOrClose(false);

            //播放音效
            AppManager.AppSystems.AudioSystem.PlayAudio(AudioType.DefaultButtonUp);


            /* 通知窗口 相关 */
            AppManager.AppSystems.NotificationSystem.CloseAllNotification();//关闭所有的通知窗口

            /* 任务栏 相关 */
            AppManager.AppSystems.TaskbarSystem.SetProgressValueAndState(0, TaskbarItemProgressState.Paused);
        }


        //当鼠标进入[猫咪身体]时
        private void CatGrid_OnMouseEnter(object sender, MouseEventArgs e)
        {
            //播放音效
            AppManager.AppSystems.AudioSystem.PlayAudio(AudioType.CatUp);
        }

        //当鼠标移出[猫咪身体]时
        private void CatGrid_OnMouseLeave(object sender, MouseEventArgs e)
        {
            //播放音效
            if (AppManager.MainWindow.TimingUserControl.Visibility != Visibility.Visible &&
                AppManager.MainWindow.PomodoroUserControl.Visibility != Visibility.Visible)
            {
                AppManager.AppSystems.AudioSystem.PlayAudio(AudioType.CatDown);
            }

        }
        #endregion


    }
}
