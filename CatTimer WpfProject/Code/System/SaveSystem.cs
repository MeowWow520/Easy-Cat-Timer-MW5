using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// 保存&读取 系统
    /// </summary>
    public class SaveSystem
    {
        /// <summary>
        /// 保存
        /// </summary>
        public void Save()
        {
            Properties.Settings.Default.Volume = AppManager.AppDatas.SettingData.Volume;//是否有声音？
            Properties.Settings.Default.Language = (int)AppManager.AppDatas.SettingData.Language;//语言
            Properties.Settings.Default.Topmost = AppManager.AppDatas.SettingData.Topmost;//窗口是否置顶

            Properties.Settings.Default.PomodoroEnabled = AppManager.AppDatas.SettingData.PomodoroEnabled;//是否使用番茄钟模式？
            Properties.Settings.Default.PomodoroWorkMinutes = AppManager.AppDatas.SettingData.PomodoroWorkMinutes;//工作的时长
            Properties.Settings.Default.PomodoroShortBreakMinutes = AppManager.AppDatas.SettingData.PomodoroShortBreakMinutes;//短休息的时长
            Properties.Settings.Default.PomodoroLongBreakMinutes = AppManager.AppDatas.SettingData.PomodoroLongBreakMinutes;//长休息的时长
            Properties.Settings.Default.PomodoroRoundsBeforeLongBreak = AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak;//每几轮长休息一次
            Properties.Settings.Default.PomodoroAutoStartNext = AppManager.AppDatas.SettingData.PomodoroAutoStartNext;//是否自动开始下一阶段

            Properties.Settings.Default.Save();
        }

        
        /// <summary>
        /// 读取
        /// </summary>
        public void Load()
        {
            AppManager.AppDatas.SettingData.Volume = Properties.Settings.Default.Volume;
            AppManager.AppDatas.SettingData.Language = (LanguageType) Properties.Settings.Default.Language;
            AppManager.AppDatas.SettingData.Topmost = Properties.Settings.Default.Topmost;

            AppManager.AppDatas.SettingData.PomodoroEnabled = Properties.Settings.Default.PomodoroEnabled;
            AppManager.AppDatas.SettingData.PomodoroWorkMinutes = Properties.Settings.Default.PomodoroWorkMinutes;
            AppManager.AppDatas.SettingData.PomodoroShortBreakMinutes = Properties.Settings.Default.PomodoroShortBreakMinutes;
            AppManager.AppDatas.SettingData.PomodoroLongBreakMinutes = Properties.Settings.Default.PomodoroLongBreakMinutes;
            AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak = Properties.Settings.Default.PomodoroRoundsBeforeLongBreak;
            AppManager.AppDatas.SettingData.PomodoroAutoStartNext = Properties.Settings.Default.PomodoroAutoStartNext;

            //更改UI
            AppManager.AppSystems.LanguageSystem.SetLanguage(AppManager.AppDatas.SettingData.Language);

        }
    }
}
