using System.ComponentModel;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// 应用的数据
    /// </summary>
    public class AppDatas
    {
        /*所有的数据*/
        private TimeData timeData;//时间
        private SettingData settingData;//设置
        private StateData stateData;//状态
        private PomodoroData pomodoroData;//番茄钟


        #region 公开属性
        /// <summary>
        /// 时间的数据
        /// </summary>
        public TimeData TimeData
        {
            get { return timeData; }
        }

        /// <summary>
        /// 设置的数据
        /// </summary>
        public SettingData SettingData
        {
            get { return settingData; }
        }

        /// <summary>
        /// 状态的数据
        /// </summary>
        public StateData StateData
        {
            get { return stateData; }
        }

        /// <summary>
        /// 番茄钟的数据
        /// </summary>
        public PomodoroData PomodoroData
        {
            get { return pomodoroData; }
        }
        #endregion

        #region 构造方法
        public AppDatas()
        {
            timeData = new TimeData();
            settingData = new SettingData();
            stateData = new StateData();
            pomodoroData = new PomodoroData();
        }
        #endregion

    }
}
