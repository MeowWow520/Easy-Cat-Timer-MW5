using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shell;

namespace CatTimer_WpfProject
{
    /// <summary>
    /// 番茄钟的系统
    /// （负责：阶段之间的切换。至于"每秒扣时间"，还是由TimeSystem负责）
    /// </summary>
    public class PomodoroSystem
    {
        #region 公开方法 -[开始一个番茄钟]
        /// <summary>
        /// 点[开始]按钮的时候调用：
        /// 如果当前有一个"等待开始"的阶段（比如关掉了[自动进入下一阶段]），就开始那个阶段；
        /// 否则，就从头开始一个番茄钟。
        /// </summary>
        public void StartOrContinue()
        {
            //如果当前不在番茄钟里，就从头开始一个
            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.None)
            {
                StartPomodoro();
                return;
            }

            //当前已经有一个"等待开始"的阶段了（时长已经装好了），直接开始它
            StartCurrentStage();
        }


        /// <summary>
        /// 开始一个番茄钟
        /// （永远从[工作]阶段开始）
        /// </summary>
        public void StartPomodoro()
        {
            //把轮次复位
            AppManager.AppDatas.PomodoroData.CompletedWorkCount = 0;
            AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak = 0;
            AppManager.AppDatas.PomodoroData.CurrentRound = 1;

            //进入[工作]阶段，并且立刻开始倒计时
            EnterStage(PomodoroStage.Work, true);
        }
        #endregion


        #region 公开方法 -[阶段结束 / 跳过 / 重置]
        /// <summary>
        /// 当[一个阶段]的倒计时归零时，触发此方法
        /// （由TimeSystem在倒计时结束时调用）
        /// </summary>
        public void OnStageFinished()
        {
            //如果没有在跑番茄钟，就什么都不做
            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.None) return;


            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.Work)
            {
                //完成了一个[工作]阶段
                AppManager.AppDatas.PomodoroData.CompletedWorkCount += 1;
                AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak += 1;

                //当前是这一轮循环里的第几轮？
                //（注意：要放在 GetBreakStage() 之前算，因为那个方法可能会把计数器清零）
                AppManager.AppDatas.PomodoroData.CurrentRound = AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak;

                //进入休息：是[短休息]还是[长休息]，由 GetBreakStage() 决定
                EnterStage(GetBreakStage(), AppManager.AppDatas.SettingData.PomodoroAutoStartNext);
            }
            else
            {
                //休息结束了：进入下一轮[工作]
                AppManager.AppDatas.PomodoroData.CurrentRound = AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak + 1;

                EnterStage(PomodoroStage.Work, AppManager.AppDatas.SettingData.PomodoroAutoStartNext);
            }
        }


        /// <summary>
        /// 跳过当前阶段
        /// （注意：跳过[工作]阶段，不算完成，完成的个数不会增加）
        /// </summary>
        public void SkipStage()
        {
            //如果没有在跑番茄钟，就什么都不做
            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.None) return;

            //先停掉当前的倒计时
            AppManager.AppSystems.TimeSystem.StopHandle();

            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.Work)
            {
                //跳过[工作]：直接去休息（因为没完成，所以完成的个数不变）
                EnterStage(GetBreakStage(), AppManager.AppDatas.SettingData.PomodoroAutoStartNext);
            }
            else
            {
                //跳过[休息]：直接去下一轮[工作]
                AppManager.AppDatas.PomodoroData.CurrentRound = AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak + 1;

                EnterStage(PomodoroStage.Work, AppManager.AppDatas.SettingData.PomodoroAutoStartNext);
            }
        }


        /// <summary>
        /// 重置番茄钟
        /// （点黑猫[重置]、切换番茄钟模式的时候会调用）
        /// </summary>
        public void ResetPomodoro()
        {
            AppManager.AppDatas.PomodoroData.CurrentStage = PomodoroStage.None;
            AppManager.AppDatas.PomodoroData.CompletedWorkCount = 0;
            AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak = 0;
            AppManager.AppDatas.PomodoroData.CurrentRound = 1;

            RefreshTexts();
        }
        #endregion


        #region 公开方法 -[刷新界面上的文字]
        /// <summary>
        /// 刷新[阶段的名字]和[轮次]这两个文字
        /// （切换阶段、切换语言的时候都要调用）
        /// </summary>
        public void RefreshTexts()
        {
            /* 如果没有在跑番茄钟，就把两个文字都清空 */
            if (AppManager.AppDatas.PomodoroData.CurrentStage == PomodoroStage.None)
            {
                AppManager.AppDatas.PomodoroData.RefreshTexts("", "");
                return;
            }


            /* 阶段的名字 */
            string _stageName = "";
            switch (AppManager.AppDatas.PomodoroData.CurrentStage)
            {
                case PomodoroStage.Work:
                    _stageName = GetText("Pomodoro.Stage.Work.Text");
                    break;

                case PomodoroStage.ShortBreak:
                    _stageName = GetText("Pomodoro.Stage.ShortBreak.Text");
                    break;

                case PomodoroStage.LongBreak:
                    _stageName = GetText("Pomodoro.Stage.LongBreak.Text");
                    break;
            }


            /* 轮次 */
            int _round = AppManager.AppDatas.PomodoroData.CurrentRound;
            int _totalRound = AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak;
            if (_totalRound <= 0)
            {
                //防止[长休息间隔]被改成0的时候，显示出"第 1 / 0 轮"
                _totalRound = 1;
            }

            string _roundFormat = GetText("Pomodoro.Round.Format");
            string _roundText = "";
            if (string.IsNullOrEmpty(_roundFormat) == false)
            {
                _roundText = string.Format(_roundFormat, _round, _totalRound);
            }


            AppManager.AppDatas.PomodoroData.RefreshTexts(_stageName, _roundText);
        }
        #endregion


        #region 私有方法 -[阶段切换]
        /// <summary>
        /// 进入某一个阶段
        /// </summary>
        /// <param name="_stage">要进入的阶段</param>
        /// <param name="_isAutoStart">进入之后，是否立刻开始倒计时？</param>
        private void EnterStage(PomodoroStage _stage, bool _isAutoStart)
        {
            //如果进入的是[长休息]，说明这一轮循环结束了，计数要重新开始
            if (_stage == PomodoroStage.LongBreak)
            {
                AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak = 0;
            }

            //修改当前阶段
            AppManager.AppDatas.PomodoroData.CurrentStage = _stage;

            //把这个阶段的时长，写进时间数据里
            int _seconds = GetStageSeconds(_stage);
            AppManager.AppDatas.TimeData.CurrentTime.DayToSecond = _seconds;
            AppManager.AppDatas.TimeData.InputTime.DayToSecond = _seconds;

            //刷新界面上的文字
            RefreshTexts();

            //任务栏进度条归零（不管是自动开始还是等用户点开始，都先归零）
            AppManager.AppSystems.TaskbarSystem.SetProgressValueAndState(0, TaskbarItemProgressState.Paused);

            if (_isAutoStart == true)
            {
                //立刻开始倒计时，并且隐藏[设定界面]
                StartCurrentStage();
            }
            else
            {
                //不自动开始：显示[设定界面]，等用户自己点[开始]
                AppManager.MainWindow.OpenSetupUi(true);
            }
        }


        /// <summary>
        /// 开始[当前已经装好的]这个阶段
        /// </summary>
        private void StartCurrentStage()
        {
            //开始倒计时，并且隐藏[设定界面]
            AppManager.AppSystems.TimeSystem.StartHandle();
            AppManager.MainWindow.OpenSetupUi(false);
        }


        /// <summary>
        /// 根据[距离上一次长休息，已经完成了几个工作阶段]，决定接下来的休息是[短休息]还是[长休息]
        /// </summary>
        private PomodoroStage GetBreakStage()
        {
            int _roundsBeforeLongBreak = AppManager.AppDatas.SettingData.PomodoroRoundsBeforeLongBreak;
            int _roundsSinceLongBreak = AppManager.AppDatas.PomodoroData.RoundsSinceLongBreak;

            //这一轮循环里已经完成了 N 个[工作]阶段，就该[长休息]了
            if (_roundsBeforeLongBreak > 0 && _roundsSinceLongBreak >= _roundsBeforeLongBreak)
            {
                return PomodoroStage.LongBreak;
            }

            return PomodoroStage.ShortBreak;
        }


        /// <summary>
        /// 获取某一个阶段的时长（单位：秒）
        /// </summary>
        /// <param name="_stage">阶段</param>
        private int GetStageSeconds(PomodoroStage _stage)
        {
            int _minutes = 0;

            switch (_stage)
            {
                case PomodoroStage.Work:
                    _minutes = AppManager.AppDatas.SettingData.PomodoroWorkMinutes;
                    break;

                case PomodoroStage.ShortBreak:
                    _minutes = AppManager.AppDatas.SettingData.PomodoroShortBreakMinutes;
                    break;

                case PomodoroStage.LongBreak:
                    _minutes = AppManager.AppDatas.SettingData.PomodoroLongBreakMinutes;
                    break;
            }

            return _minutes * 60;
        }


        /// <summary>
        /// 从资源字典里取出一个文字
        /// （界面上的文字是跟着语言走的，所以要动态取）
        /// </summary>
        /// <param name="_key">文字的Key</param>
        private string GetText(string _key)
        {
            if (AppManager.MainApp == null) return "";

            object _value = AppManager.MainApp.TryFindResource(_key);
            if (_value is string)
            {
                return (string)_value;
            }

            return "";
        }
        #endregion
    }
}
