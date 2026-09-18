using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// 番茄钟的数据
    /// （这里存的是[运行时]的状态，不会被存档）
    /// </summary>
    public class PomodoroData : INotifyPropertyChanged
    {
        /* 番茄钟相关 */
        private PomodoroStage currentStage;//当前所处的阶段
        private int currentRound;//当前是这一轮循环里的第几轮（从1开始，最大是[长休息间隔]）
        private int completedWorkCount;//从开始到现在，一共完成了几个[工作]阶段
        private int roundsSinceLongBreak;//距离上一次[长休息]，已经完成了几个[工作]阶段

        private string stageName = "";//当前阶段的名字（给界面显示用）
        private string roundText = "";//当前轮次的文字（给界面显示用）



        #region 公开属性
        /// <summary>
        /// 当前所处的阶段
        /// </summary>
        public PomodoroStage CurrentStage
        {
            get { return currentStage; }
            set
            {
                currentStage = value;
                PropertyChange("CurrentStage");
            }
        }

        /// <summary>
        /// 当前是这一轮循环里的第几轮（从1开始）
        /// （最大值就是[长休息间隔]，不会一直往上涨）
        /// </summary>
        public int CurrentRound
        {
            get { return currentRound; }
            set
            {
                currentRound = value;
                PropertyChange("CurrentRound");
            }
        }

        /// <summary>
        /// 从开始到现在，一共完成了几个[工作]阶段
        /// </summary>
        public int CompletedWorkCount
        {
            get { return completedWorkCount; }
            set
            {
                completedWorkCount = value;
                PropertyChange("CompletedWorkCount");
            }
        }

        /// <summary>
        /// 距离上一次[长休息]，已经完成了几个[工作]阶段
        /// （完成到[长休息间隔]个的时候，就该[长休息]了；[长休息]之后会重新从0开始数）
        /// </summary>
        public int RoundsSinceLongBreak
        {
            get { return roundsSinceLongBreak; }
            set
            {
                roundsSinceLongBreak = value;
                PropertyChange("RoundsSinceLongBreak");
            }
        }

        /// <summary>
        /// 当前阶段的名字（给界面显示用）
        /// </summary>
        public string StageName
        {
            get { return stageName; }
        }

        /// <summary>
        /// 当前轮次的文字（给界面显示用）
        /// </summary>
        public string RoundText
        {
            get { return roundText; }
        }
        #endregion

        #region 公开方法
        /// <summary>
        /// 刷新给界面显示的两个文字
        /// （切换阶段、切换语言的时候都要调用）
        /// </summary>
        /// <param name="_stageName">阶段的名字</param>
        /// <param name="_roundText">轮次的文字</param>
        public void RefreshTexts(string _stageName, string _roundText)
        {
            stageName = _stageName;
            roundText = _roundText;

            PropertyChange("StageName");
            PropertyChange("RoundText");
        }
        #endregion

        #region 构造方法
        public PomodoroData()
        {
            currentStage = PomodoroStage.None;
            currentRound = 1;
            completedWorkCount = 0;
            roundsSinceLongBreak = 0;
        }
        #endregion



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
