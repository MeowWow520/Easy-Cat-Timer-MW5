using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// 设置的数据
    /// </summary>
    public class SettingData : INotifyPropertyChanged
    {
        /* 设置相关 */
        private int volume;//音量
        private LanguageType language;//语言
        private bool topmost;//窗口是否置顶

        /* 番茄钟相关 */
        private bool pomodoroEnabled;//是否使用[番茄钟]模式？
        private int pomodoroWorkMinutes;//[工作]的时长（单位：分钟）
        private int pomodoroShortBreakMinutes;//[短休息]的时长（单位：分钟）
        private int pomodoroLongBreakMinutes;//[长休息]的时长（单位：分钟）
        private int pomodoroRoundsBeforeLongBreak;//每完成几轮[工作]，就[长休息]一次？
        private bool pomodoroAutoStartNext;//一个阶段结束后，是否自动开始下一个阶段？



        #region 公开属性
        /// <summary>
        /// 音量
        /// </summary>
        public int Volume
        {
            get { return volume; }
            set
            {
                volume = value;
                PropertyChange("Volume");//更新UI
                AppManager.AppSystems.AudioSystem.OnVolumeChange(volume);//触发[音量更改]的事件
            }
        }

        /// <summary>
        /// 语言
        /// </summary>
        public LanguageType Language
        {
            get { return language; }
            set
            {
                language = value;
                PropertyChange("Language");//更新UI
            }
        }

        /// <summary>
        /// 窗口是否置顶？
        /// </summary>
        public bool Topmost
        {
            get { return topmost; }
            set
            {
                topmost = value;
                PropertyChange("Topmost");//更新UI
            }
        }


        /// <summary>
        /// 是否使用[番茄钟]模式？
        /// （如果为false，就是原来那个简单的倒计时）
        /// </summary>
        public bool PomodoroEnabled
        {
            get { return pomodoroEnabled; }
            set
            {
                pomodoroEnabled = value;
                PropertyChange("PomodoroEnabled");//更新UI
            }
        }

        /// <summary>
        /// [工作]的时长（单位：分钟）
        /// </summary>
        public int PomodoroWorkMinutes
        {
            get { return pomodoroWorkMinutes; }
            set
            {
                pomodoroWorkMinutes = value;
                PropertyChange("PomodoroWorkMinutes");//更新UI
            }
        }

        /// <summary>
        /// [短休息]的时长（单位：分钟）
        /// </summary>
        public int PomodoroShortBreakMinutes
        {
            get { return pomodoroShortBreakMinutes; }
            set
            {
                pomodoroShortBreakMinutes = value;
                PropertyChange("PomodoroShortBreakMinutes");//更新UI
            }
        }

        /// <summary>
        /// [长休息]的时长（单位：分钟）
        /// </summary>
        public int PomodoroLongBreakMinutes
        {
            get { return pomodoroLongBreakMinutes; }
            set
            {
                pomodoroLongBreakMinutes = value;
                PropertyChange("PomodoroLongBreakMinutes");//更新UI
            }
        }

        /// <summary>
        /// 每完成几轮[工作]，就[长休息]一次？
        /// </summary>
        public int PomodoroRoundsBeforeLongBreak
        {
            get { return pomodoroRoundsBeforeLongBreak; }
            set
            {
                pomodoroRoundsBeforeLongBreak = value;
                PropertyChange("PomodoroRoundsBeforeLongBreak");//更新UI
            }
        }

        /// <summary>
        /// 一个阶段结束后，是否自动开始下一个阶段？
        /// （如果为false，就要用户自己点[开始]）
        /// </summary>
        public bool PomodoroAutoStartNext
        {
            get { return pomodoroAutoStartNext; }
            set
            {
                pomodoroAutoStartNext = value;
                PropertyChange("PomodoroAutoStartNext");//更新UI
            }
        }
        #endregion

        #region 构造方法
        public SettingData()
        {
            volume = 100;
            language = LanguageType.Chinese;
            topmost = false;

            /* 番茄钟的默认值：25分钟工作 + 5分钟短休息 + 15分钟长休息，每4轮长休息一次 */
            pomodoroEnabled = false;
            pomodoroWorkMinutes = 25;
            pomodoroShortBreakMinutes = 5;
            pomodoroLongBreakMinutes = 15;
            pomodoroRoundsBeforeLongBreak = 4;
            pomodoroAutoStartNext = true;
        }
        #endregion 构造方法


        #region 数据的双向绑定-更新方法
        /// <summary>
        /// 当属性改变的时候，就触发此方法
        /// </summary>
        /// <param name="propertyName">发生改变的属性的名字</param>
        private void PropertyChange(string propertyName)
        {
            if (PropertyChanged != null)//如果此事件被监听
            {
                //就发送通知
                //参数1：是哪个数据类的对象发生了改变？
                //参数2：发生改变的属性名
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        /// <summary>
        /// 系统会自动监听此事件
        /// 如果此事件触发了，系统就会去通知相应的控件
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}
