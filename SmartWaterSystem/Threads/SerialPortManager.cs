﻿using System;
using Common;
using System.Threading;
using Protocol;
using System.Data;
using Entity;
using System.Collections.Generic;
using BLL;

namespace SmartWaterSystem
{
    /// <summary>
    /// TransStatus
    /// </summary>
    public enum TransStatus
    {
        Start,
        Success,
        Fail
    }

    public enum SerialPortType : uint
    {
        None = 0,
        NoiseEnable,             //设置启动
        //NoiseStop,              //设置停止
        NoiseGetStandValue,     //噪声获取启动值(标准值)
        NoiseSetStandValue,     //噪声设置启动值(标准值)

        NoiseClearData,         //设置清除数据
        NoiseReadParm,          //读取参数数据
        NoiseSetParm,           //设置参数数据
        NoiseCtrlReadParm,      //噪声远传控制器读取参数数据
        NoiseCtrlSetParm,       //噪声远传控制器设置参数数据

        NoiseCtrlReset,         //噪声远传控制器复位
        NoiseCtrlReadFiledStrength, //读取噪声远传控制器场强\电压

        NoiseReadData,          //读取数据
        NoiseBatchWrite,        //批量设置
        UniversalReset, //通用终端复位
        UniversalSetTime,   //设置通用终端时间
        UniversalSetEnableCollect, //设置通用终端启用采集

        UniversalEnableAlarm,   //设置通用终端是否启用实时报警

        UniversalSetCollectConfig,  //设置通用终端采集配置功能
        UniversalSetSim_Interval,  //设置通用终端模拟量时间间隔
        UniversalSetPluse_Interval, //设置通用终端脉冲量时间间隔
        UniversalSet485_Interval,  //设置通用终端RS485时间间隔
        UniversalSetModbusProtocol,   //设置通用终端Modbus协议

        UniversalSetModbusExeFlag,     //设置通用终端Modbus执行标志
        UniversalSetBasicInfo,      //通用终端设置基本信息,包括手机号、通信方式、波特率、ip、端口号
        UniversalReadBasicInfo,       //通用终端读取基本信息，包括手机号、通信方式、波特率、ip、端口号
        UniversalCalibrationSimualte1, //通用终端校准第一路模拟量
        UniversalReadVer,           //通用终端读取版本号

        UniversalReadFiledStrength, //通用终端读取场强
        UniversalCalibrationSimualte2,  //通用终端校准第二路模拟量
        UniversalPluseBasic,    //通用终端设置脉冲基准数
        Universal651ChPwd,      //通用终端SL651修改密码
        Universal651ReadBasicInfo, //通用终端SL651读取基本信息

        Universal651SetBasicInfo,   //通用终端SL651设置基本信息
        Universal651ReadRunInfo,    //通用终端SL651读取运行信息
        Universal651SetRunInfo,     //通用终端SL651设置运行信息
        Universal651QueryElements,  //通用终端SL651查询要素实时数据
        Universal651QueryPrecipitation, //通用终端SL651查询时段降水量

        Universal651SetPreConstCtrl,    //通用终端SL651设置水量定值控制
        Universal651QueryManualSetParm, //通用终端SL651查询人工置数
        Universal651SetManualSetParm,   //通用终端SL651设置人工置数
        Universal651SetCalibration,     //通用终端SL651设置水位校准值
        Universal651ReadTimeintervalReportTime, //通用终端SL651读取均匀时段报上传时间

        Universal651SetTimeintervalReportTime, //通用终端SL651设置均匀时段报上传时间
        Universal651QueryTime,      //通用终端SL651查询时间
        Universal651QueryVer,       //通用终端SL651查询版本
        Universal651QueryCurData,   //通用终端SL651查询实时数据
        Universal651QueryEvent,     //通用终端SL651查询事件

        Universal651QueryAlarm,     //通用终端SL651查询状态和报警
        Universal651SetTime,        //通用终端SL651设置时间
        Universal651InitFlash,      //通用终端SL651初始化FLASH
        Universal651Init,           //通用终端SL651恢复出厂
        UniversalCallData,      //通用终端招测数据(串口)

        UniversalCallEnable,    //通用终端招测使能
        OLWQReset, //水质终端复位
        OLWQSetTime,   //设置水质终端时间
        OLWQSetEnableCollect, //设置水质终端启用采集
        OLWQSetBasicInfo,      //水质终端设置基本信息,包括通信方式、波特率、ip、端口号

        OLWQReadBaicInfo,       //水质终端读取基本信息，包括通信方式、波特率、ip、端口号
        OLWQCallData,           //水质终端招测数据(串口)
        HydrantReset,           //消防栓复位
        HydrantSetTime,         //设置消防栓时间
        HydrantSetBasicInfo,    //消防栓设置基本信息，包括通讯方式、ip、端口号

        HydrantReadBasicInfo,   //消防栓读取基本信息，包括通讯方式、ip、端口号
        HydrantReadHistory,     //读取消防栓历史数据
        HydrantSetEnableCollect,//消防栓启动采集开关
    }

    public class SerialPortEventArgs : EventArgs
    {
        private SerialPortType _type = SerialPortType.None;
        public SerialPortType OptType
        {
            get { return _type; }
        }

        private TransStatus _transStatus;
        /// <summary>
        /// 传输状态
        /// </summary>
        public TransStatus TransactStatus
        {
            get { return _transStatus; }
        }

        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
        }

        private object _tag = null;
        public object Tag
        {
            get { return _tag; }
        }

        public SerialPortEventArgs(SerialPortType Type, TransStatus status, string msg, object obj)
        {
            this._type = Type;
            this._transStatus = status;
            this._msg = msg;
            this._tag = obj;
        }
    }

    public class SerialPortScheduleEventArgs : EventArgs
    {
        public SerialPortScheduleEventArgs(SerialPortType Type)
        {
            this._type = Type;
        }

        public SerialPortScheduleEventArgs(SerialPortType Type, string msg)
        {
            this._type = Type;
            this._msg = msg;
        }

        private SerialPortType _type = SerialPortType.None;
        public SerialPortType OptType
        {
            get { return _type; }
        }

        private string _msg = "";
        public string Msg
        {
            get { return _msg; }
            set { _msg = value; }
        }
    }

    public delegate void SerialPortHandle(object sender, SerialPortEventArgs e);
    public delegate void SerialPortScheduleHandle(object sender, SerialPortScheduleEventArgs e);
    public class SerialPortManager : SerialPortRW
    {
        private NLog.Logger logger = NLog.LogManager.GetLogger("SerialPortMgr");
        private const int eventcount = 2;
        public event SerialPortHandle SerialPortEvent;

        public SerialPortType SendType;

        /// <summary>
        /// 用于通知UI多个通信动作是的进度(读写)
        /// </summary>
        public event SerialPortScheduleHandle SerialPortScheduleEvent;
        private IntPtr[] hEvent = new IntPtr[eventcount];
        public SerialPortManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                hEvent[i] = Win32.CreateEvent(IntPtr.Zero, false, false, "");
            }
        }

        ~SerialPortManager()
        {
            for (int i = 0; i < eventcount; i++)
            {
                Win32.CloseHandle(hEvent[i]);
            }
        }

        public virtual void OnSerialPortEvent(SerialPortEventArgs e)
        {
            if (SerialPortEvent != null)
                SerialPortEvent(this, e);
        }

        public virtual void OnSerialPortScheduleEvent(SerialPortScheduleEventArgs e)
        {
            if (SerialPortScheduleEvent != null)
                SerialPortScheduleEvent(this, e);
        }

        public void Start()
        {
            Thread t = new Thread(new ThreadStart(SerialPortThread));
            t.IsBackground = true;
            t.Start();

            SerialPortLoop();

            GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived += GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;
        }

        public void Stop()
        {
            Win32.SetEvent(hEvent[0]);
        }

        public void Send(SerialPortType type)
        {
            SendType = type;
            Win32.SetEvent(hEvent[1]);
        }

        private void SerialPortThread()
        {
            while (true)
            {
                uint evt = 0;
                evt=Win32.WaitForMultipleObjects(eventcount, hEvent, false, Win32.INFINITE);
                bool result = false;  //-1:执行失败;1:执行成功;0:无执行返回
                string msg = "";
                object obj = null;

                OnSerialPortEvent(new SerialPortEventArgs((SerialPortType)evt, TransStatus.Start, "", null));
                GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived -= GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;  //发送命令时去除委托

                if (evt == 0)
                {
                    Thread.CurrentThread.Abort();
                    return;
                }
                switch ((uint)SendType)
                {
                    case (uint)SerialPortType.None:
                        Thread.CurrentThread.Abort();
                        break;
                    case (uint)SerialPortType.NoiseCtrlReset:
                        {
                            NoiseCtrl ctrl = new NoiseCtrl();
                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在复位远传控制器..."));
                            result = ctrl.Reset(GlobalValue.NoiseSerialPortOptData.ID);
                        }
                        break;
                    case (uint)SerialPortType.NoiseCtrlReadFiledStrength:
                        #region 通用终端读取场强\电压
                        {
                            try
                            {
                                NoiseCtrl ctrl = new NoiseCtrl();
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器场强\\电压..."));

                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                result_data.FieldStrength = ctrl.ReadFieldStrength(GlobalValue.NoiseSerialPortOptData.ID);
                                result = true;
                                obj = result_data;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseCtrlReadParm:
                        #region 噪声记录仪控制器读取参数
                        {
                            try
                            {
                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                NoiseCtrl ctrl = new NoiseCtrl();
                                if (GlobalValue.NoiseSerialPortOptData.IsOptID)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器ID..."));
                                    result_data.ID = ctrl.ReadNoiseCtrlID();
                                }
                                // 读取时间
                                if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器时间..."));
                                    byte[] tt1 = ctrl.ReadTime(GlobalValue.NoiseSerialPortOptData.ID);
                                    result_data.dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tt1[0], tt1[1], tt1[2]);
                                }
                                if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteId)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器对应记录仪ID..."));
                                    result_data.RemoteId = ctrl.ReadCtrlNoisLogID(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                // 读取远传功能
                                if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器远传开关状态..."));
                                    result_data.RemoteSwitch = ctrl.ReadRemote(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                if(GlobalValue.NoiseSerialPortOptData.IsOptComType)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器远传通讯方式..."));
                                    result_data.ComType = ctrl.ReadComType(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                // 读取远传通讯时间
                                if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传控制器通讯时间..."));
                                    result_data.ComTime = Convert.ToInt32(ctrl.ReadRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID));
                                }
                                if (GlobalValue.NoiseSerialPortOptData.IsOptIP)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传IP地址..."));
                                    result_data.IP = ctrl.ReadIP(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                if (GlobalValue.NoiseSerialPortOptData.IsOptPort)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传端口号..."));
                                    result_data.Port = Convert.ToInt32(ctrl.ReadPort(GlobalValue.NoiseSerialPortOptData.ID));
                                }

                                result = true;
                                obj = result_data;
                                //result = ctrl.WriteWireless((short)GlobalValue.NoiseSerialPortOptData.RemoteId, (int)GlobalValue.NoiseSerialPortOptData.Frequency * 1000, GlobalValue.NoiseSerialPortOptData.Rate,
                                //    GlobalValue.NoiseSerialPortOptData.Power, GlobalValue.NoiseSerialPortOptData.Baud, GlobalValue.NoiseSerialPortOptData.WakeTime);
                                //if (!result)
                                //    break;
                                //result = ctrl.WriteGPRSBaurate((short)GlobalValue.NoiseSerialPortOptData.RemoteId, GlobalValue.NoiseSerialPortOptData.GprsBaud);
                                //}
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                logger.ErrorException("NoiseCtrlReadParm", ex);
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseCtrlSetParm:
                        #region 噪声记录仪控制器远传设置(开关、ip、port)
                        {
                            try
                            {
                                NoiseCtrl ctrl = new NoiseCtrl();
                                if (GlobalValue.NoiseSerialPortOptData.IsOptID)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传控制器ID..."));
                                    result = ctrl.WriteNoiseCtrlID(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.SetID);
                                }
                                else
                                {
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteId)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传控制器对应记录仪ID..."));
                                        result = ctrl.WriteCtrlToNoiseLogID(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteId);
                                    }
                                    // 设置时间
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置时间..."));
                                        result = ctrl.WriteTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.dt);
                                        if (!result)
                                            break;
                                    }
                                    // 设置远传功能
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传开关状态..."));
                                        result = ctrl.WriteRemoteSwitch(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteSwitch);
                                        if (!result)
                                            break;
                                    }
                                    if(GlobalValue.NoiseSerialPortOptData.IsOptComType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置通讯方式..."));
                                        result = ctrl.WriteComType(GlobalValue.NoiseSerialPortOptData.ID, (byte)GlobalValue.NoiseSerialPortOptData.ComType);
                                        if (!result)
                                            break;
                                    }
                                    // 设置远传通讯时间
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传通讯时间..."));
                                        result = ctrl.WriteRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.ComTime);
                                        if (!result)
                                            break;
                                    }
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传控制器远传地址IP..."));
                                        result = ctrl.Write_IP(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.IP);
                                    }
                                    
                                    if (GlobalValue.NoiseSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传控制器远传端口号..."));
                                        result = ctrl.WritePortName(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Port.ToString());
                                    }
                                }
                                obj = null;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                logger.ErrorException("NoiseCtrlSetParm", ex);
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseEnable:
                        #region 噪声记录仪开启
                        {
                            try
                            {
                                short[] Originaldata = null;
                                result = GlobalValue.Noiselog.CtrlStartOrStop(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Enable, out Originaldata);
                                obj = null;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseGetStandValue:
                        #region 获取噪声记录仪启动值(标准值)
                        {
                            try
                            {
                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                result_data.StandValue = GlobalValue.Noiselog.ReadNoiseStandValue(GlobalValue.NoiseSerialPortOptData.ID);
                                result = true;
                                obj = result_data;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseSetStandValue:
                        #region 设置噪声记录仪启动值(标准值)
                        {
                            try
                            {
                                result = GlobalValue.Noiselog.WriteStandValue(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.StandValue);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseClearData:
                        #region 噪声记录仪清除数据
                        {
                            try
                            {
                                result = GlobalValue.Noiselog.ClearData(GlobalValue.NoiseSerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseReadParm:
                        #region 噪声记录仪读取参数
                        {
                            try
                            {
                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                result_data.ID = GlobalValue.NoiseSerialPortOptData.ID;
                                // 读取时间
                                if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取时间..."));
                                    byte[] tt1 = GlobalValue.Noiselog.ReadTime(GlobalValue.NoiseSerialPortOptData.ID);
                                    result_data.dt = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, tt1[0], tt1[1], tt1[2]);
                                }
                                // 读取远传通讯时间
                                //if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                                //{
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传通讯时间..."));
                                //    result_data.ComTime = Convert.ToInt32(GlobalValue.Noiselog.ReadRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID));
                                //}
                                // 读取记录时间段
                                if (GlobalValue.NoiseSerialPortOptData.IsOptColTime)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取记录时间段..."));
                                    byte[] tt = GlobalValue.Noiselog.ReadStartEndTime(GlobalValue.NoiseSerialPortOptData.ID);
                                    result_data.colstarttime = Convert.ToInt32(tt[0]);
                                    result_data.colendtime = Convert.ToInt32(tt[1]);
                                }
                                // 读取采集间隔
                                if (GlobalValue.NoiseSerialPortOptData.IsOptInterval)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取采集时间间隔..."));
                                    result_data.Interval = GlobalValue.Noiselog.ReadInterval(GlobalValue.NoiseSerialPortOptData.ID);
                                }
                                // 读取远传功能
                                //if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                                //{
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传开关状态..."));
                                //    result_data.RemoteSwitch = GlobalValue.Noiselog.ReadRemote(GlobalValue.NoiseSerialPortOptData.ID);
                                //}
                                //if (result_data.RemoteSwitch)
                                //{
                                //    NoiseCtrl ctrl = new NoiseCtrl();

                                //    //串口读取远传控制器设备与记录仪设备对应的ID号
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传ID..."));
                                //    result_data.RemoteId = (short)ctrl.ReadCtrlNoisLogID(GlobalValue.NoiseSerialPortOptData.ID);

                                //    // 读取远传端口
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传端口..."));
                                //    result_data.Port = Convert.ToInt32(ctrl.ReadPort(GlobalValue.NoiseSerialPortOptData.ID));

                                //    // 读取远传地址
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传地址..."));
                                //    result_data.IP = ctrl.ReadIP(GlobalValue.NoiseSerialPortOptData.ID);

                                //    //读取无线参数
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取远传无线参数..."));
                                //    int[] wirelessparm = ctrl.ReadWireless(GlobalValue.NoiseSerialPortOptData.ID);
                                //    //收发频率（3byte）+无线速率（1byte）+发射功率（1byte）+串口速率（1byte）+唤醒时间（1byte）
                                //    if (wirelessparm != null && wirelessparm.Length == 5)
                                //    {
                                //        result_data.Frequency = Convert.ToDouble(wirelessparm[0]) / 1000;
                                //        result_data.Rate = wirelessparm[1];
                                //        result_data.Power = wirelessparm[2];
                                //        result_data.Baud = wirelessparm[3];
                                //        result_data.WakeTime = wirelessparm[4];
                                //    }

                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在读取GPRS通讯波特率..."));
                                //    result_data.GprsBaud = ctrl.ReadGPRSBaudrate(GlobalValue.NoiseSerialPortOptData.ID);
                                //}
                                result = true;
                                obj = result_data;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseSetParm:
                        #region 噪声记录仪设置参数
                        {
                            try
                            {
                                NoiseSerialPortOptEntity result_data = new NoiseSerialPortOptEntity();
                                result_data.ID = GlobalValue.NoiseSerialPortOptData.ID;
                                // 设置时间
                                if (GlobalValue.NoiseSerialPortOptData.IsOptDT)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置时间..."));
                                    result = GlobalValue.Noiselog.WriteTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.dt);
                                    if (!result)
                                        break;
                                }
                                
                                // 设置远传通讯时间
                                //if (GlobalValue.NoiseSerialPortOptData.IsOptComTime)
                                //{
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传通讯时间..."));
                                //    result = GlobalValue.Noiselog.WriteRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.ComTime);
                                //    if (!result)
                                //        break;
                                //}
                                
                                // 设置记录时间段
                                if (GlobalValue.NoiseSerialPortOptData.IsOptColTime)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置记录时间段..."));
                                    result = GlobalValue.Noiselog.WriteStartEndTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.colstarttime, GlobalValue.NoiseSerialPortOptData.colendtime);
                                    if (!result)
                                        break;
                                }
                                
                                // 设置采集间隔
                                if (GlobalValue.NoiseSerialPortOptData.IsOptInterval)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置采集时间间隔..."));
                                    result = GlobalValue.Noiselog.WriteInterval(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Interval);
                                    if (!result)
                                        break;
                                }
                                
                                // 设置远传功能
                                //if (GlobalValue.NoiseSerialPortOptData.IsOptRemoteSwitch)
                                //{
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseReadParm, "正在设置远传开关状态..."));
                                //    result = GlobalValue.Noiselog.WriteRemoteSwitch(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteSwitch);
                                //    if (!result)
                                //        break;
                                //}
                                result = true;
                                obj = result_data;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseReadData:
                        #region 读取噪声记录仪数据
                        {
                            try
                            {
                                short datasum = 0, standvalue = 0;
                                GlobalValue.Noiselog.ReadNoiseLogDataSum2Standvalue(GlobalValue.NoiseSerialPortOptData.ID, out datasum, out standvalue);
                                decimal dpacksum = (decimal)datasum / 256;
                                int packsum = (int)Math.Ceiling(dpacksum);
                                NoiseDataBaseHelper.SaveStandData(GlobalValue.NoiseSerialPortOptData.ID, standvalue);  //保存启动值
                                short[] arr = GlobalValue.Noiselog.ReadData(GlobalValue.NoiseSerialPortOptData.ID, packsum);
                                result = true;
                                obj = arr;
                            }
                            catch (ArgumentNullException argex)
                            {
                                result = false;
                                msg = "记录仪" + GlobalValue.NoiseSerialPortOptData.ID + argex.ParamName;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.NoiseBatchWrite:
                        #region 噪声记录仪批量设置参数
                        {
                            try
                            {
                                // 设置时间
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]时间..."));
                                result = GlobalValue.Noiselog.WriteTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.dt);
                                //if (result)
                                //{
                                //    // 设置远传通讯时间
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]远传通讯时间..."));
                                //    result = GlobalValue.Noiselog.WriteRemoteSendTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.ComTime);
                                //}
                                if (result)
                                {
                                    // 设置记录时间段
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]记录时间段..."));
                                    result = GlobalValue.Noiselog.WriteStartEndTime(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.colstarttime, GlobalValue.NoiseSerialPortOptData.colendtime);
                                }
                                if (result)
                                {
                                    // 设置采集间隔
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]采集间隔..."));
                                    result = GlobalValue.Noiselog.WriteInterval(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Interval);
                                }
                                //if (result)
                                //{
                                //    // 设置远传功能
                                //    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]远传功能..."));
                                //    result = GlobalValue.Noiselog.WriteRemoteSwitch(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.RemoteSwitch);
                                //}
                                if (result)
                                {
                                    // 设置开关
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.NoiseBatchWrite, "正在设置记录仪[" + GlobalValue.NoiseSerialPortOptData.ID + "]开关..."));
                                    short[] origitydata = null;
                                    result = GlobalValue.Noiselog.CtrlStartOrStop(GlobalValue.NoiseSerialPortOptData.ID, GlobalValue.NoiseSerialPortOptData.Enable, out origitydata);
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReset:
                        #region 通用终端复位
                        {
                            try
                            {
                                result = GlobalValue.Universallog.Reset(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResetType);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetTime:
                        #region 设置通用终端时间
                        {
                            try
                            {
                                result = GlobalValue.Universallog.SetTime(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, DateTime.Now);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReadVer:
                        #region 通用终端读取版本号
                        {
                            try
                            {
                                GlobalValue.UniSerialPortOptData.Ver = GlobalValue.Universallog.ReadVer(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                                else
                                    result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReadFiledStrength:
                        #region 通用终端读取场强\电压
                        {
                            try
                            {
                                GlobalValue.UniSerialPortOptData.FieldStrength = GlobalValue.Universallog.ReadFieldStrength(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetEnableCollect:
                        #region 通用终端启用采集
                        {
                            try
                            {
                                result = GlobalValue.Universallog.EnableCollect(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalEnableAlarm:
                        #region 通用终端是否启用报警
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置报警次数..."));
                                    byte data =Convert.ToByte(GlobalValue.UniSerialPortOptData.AlarmLen);
                                    result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_ALARMLEN, data);
                                }
                                OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置报警..."));
                                result = GlobalValue.Universallog.EnableAlarm(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.EnableAlarm);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalReadBasicInfo:
                        #region 通用终端读取基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.UniSerialPortOptData.ID = GlobalValue.Universallog.ReadId(GlobalValue.UniSerialPortOptData.DevType);
                                }
                                else
                                {
                                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取设备时间..."));
                                        GlobalValue.UniSerialPortOptData.DT = GlobalValue.Universallog.ReadTime(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptCellPhone)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取报警手机号码..."));
                                        GlobalValue.UniSerialPortOptData.CellPhone = GlobalValue.Universallog.ReadCellPhone(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptComType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端通信方式..."));
                                        GlobalValue.UniSerialPortOptData.ComType = GlobalValue.Universallog.ReadComType(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_NetWorkType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端联网模式..."));
                                        GlobalValue.UniSerialPortOptData.NetWorkType = GlobalValue.Universallog.ReadNetworkType(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端通信IP地址..."));
                                        GlobalValue.UniSerialPortOptData.IP = GlobalValue.Universallog.ReadIP(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端通信端口号..."));
                                        GlobalValue.UniSerialPortOptData.Port = GlobalValue.Universallog.ReadPort(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_HeartInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端心跳间隔..."));
                                        GlobalValue.UniSerialPortOptData.HeartInterval = GlobalValue.Universallog.ReadHeartInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Baud485)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端485波特率..."));
                                        GlobalValue.UniSerialPortOptData.Baud485 = GlobalValue.Universallog.Read485Baud(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptModbusExeFlag)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取modbus执行标识..."));
                                        GlobalValue.UniSerialPortOptData.ModbusExeFlag = GlobalValue.Universallog.ReadModbusExeFlag(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_VolInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端电压时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.VolInterval = GlobalValue.Universallog.ReadVolInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_VolLower)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端电压报警下限..."));
                                        GlobalValue.UniSerialPortOptData.VolLower = GlobalValue.Universallog.ReadVolLower(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_SMSInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端短信发送间隔..."));
                                        GlobalValue.UniSerialPortOptData.SMSInterval = GlobalValue.Universallog.ReadSMSInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_PluseUnit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端脉冲计数单位..."));
                                        GlobalValue.UniSerialPortOptData.PluseUnit = GlobalValue.Universallog.ReadPluseUnit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端报警次数..."));
                                        GlobalValue.UniSerialPortOptData.AlarmLen = GlobalValue.Universallog.ReadAlarmLen(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Range)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端量程..."));
                                        GlobalValue.UniSerialPortOptData.Range = GlobalValue.Universallog.ReadRange(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端压力偏移量..."));
                                        GlobalValue.UniSerialPortOptData.PreOffset = GlobalValue.Universallog.ReadPreOffset(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端压力报警上限值..."));
                                        GlobalValue.UniSerialPortOptData.UpLimit = GlobalValue.Universallog.ReadLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.UpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端压力报警上限投退状态..."));
                                        GlobalValue.UniSerialPortOptData.UpLimitEnable = GlobalValue.Universallog.ReadAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.UpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端压力报警下限值..."));
                                        GlobalValue.UniSerialPortOptData.LowLimit = GlobalValue.Universallog.ReadLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.LowAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端压力报警下限投退状态..."));
                                        GlobalValue.UniSerialPortOptData.LowLimitEnable = GlobalValue.Universallog.ReadAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.LowAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端斜率报警上限值..."));
                                        GlobalValue.UniSerialPortOptData.SlopUpLimit = GlobalValue.Universallog.ReadLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopUpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端斜率报警上限投退状态..."));
                                        GlobalValue.UniSerialPortOptData.SlopUpLimitEnable = GlobalValue.Universallog.ReadAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopUpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端斜率报警下限值..."));
                                        GlobalValue.UniSerialPortOptData.SlopLowLimit = GlobalValue.Universallog.ReadLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopLowAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端斜率报警下限投退状态..."));
                                        GlobalValue.UniSerialPortOptData.SlopLowLimitEnable = GlobalValue.Universallog.ReadAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopLowAlarm);
                                    }
                                    
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端采集配置..."));
                                        byte data = GlobalValue.Universallog.ReadCollectConfig(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                        if ((data & 0x01) == 0x01)
                                            GlobalValue.UniSerialPortOptData.Collect_DigitPre = true;
                                        if ((data & 0x02) == 0x02)
                                            GlobalValue.UniSerialPortOptData.Collect_Pluse = true;
                                        if ((data & 0x04) == 0x04)
                                            GlobalValue.UniSerialPortOptData.Collect_Simulate1 = true;
                                        if ((data & 0x08) == 0x08)
                                            GlobalValue.UniSerialPortOptData.Collect_Simulate2 = true;
                                        if ((data & 0x10) == 0x10)
                                            GlobalValue.UniSerialPortOptData.Collect_RS485 = true;
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端模拟量时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.Simulate_Interval = GlobalValue.Universallog.ReadSimualteInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PluseInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端脉冲量时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.Pluse_Interval = GlobalValue.Universallog.ReadPluseInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Interval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端RS485时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.RS485_Interval = GlobalValue.Universallog.ReadRS485Interval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Protocol)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在读取终端modbus协议..."));
                                        GlobalValue.UniSerialPortOptData.RS485Protocol = GlobalValue.Universallog.ReadModbusProtocol(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                    }
                                }
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                                else
                                    result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalSetBasicInfo:
                        #region 设置通用终端基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.Universallog.SetID(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                }
                                else
                                {
                                    if (GlobalValue.UniSerialPortOptData.IsOptCellPhone)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置报警手机号码..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType,GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_CELLPHONE, System.Text.Encoding.Default.GetBytes(GlobalValue.UniSerialPortOptData.CellPhone));
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptComType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端通信方式..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID,  (byte)UNIVERSAL_COMMAND.SET_COMTYPE, (byte)GlobalValue.UniSerialPortOptData.ComType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_NetWorkType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端联网模式..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_NETWORKTYPE,(byte)GlobalValue.UniSerialPortOptData.NetWorkType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端通信IP地址..."));
                                        result = GlobalValue.Universallog.SetIP(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.IP);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端通信端口号..."));
                                        string str_port = GlobalValue.UniSerialPortOptData.Port.ToString();
                                        byte[] data = new byte[str_port.Length];
                                        for (int i = 0; i < str_port.Length; i++)
                                        {
                                            data[i] = (byte)str_port[i];
                                        }
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_PORT, data);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_HeartInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端心跳时间间隔..."));
                                        byte[] data = BitConverter.GetBytes((short)GlobalValue.UniSerialPortOptData.HeartInterval);
                                        Array.Reverse(data);
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_HEART, data);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Baud485)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端485波特率..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_485BAUD, (byte)GlobalValue.UniSerialPortOptData.Baud485);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptModbusExeFlag)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置Modbus执行标识..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_MODBUSEXEFLAG, GlobalValue.UniSerialPortOptData.ModbusExeFlag ? (byte)0x01 : (byte)0x00);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_VolInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置电压时间间隔..."));
                                        byte[] data = BitConverter.GetBytes((short)GlobalValue.UniSerialPortOptData.VolInterval);
                                        Array.Reverse(data);
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_VOLINTERVAL, data);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_VolLower)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置电压报警下限..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_VOLLOWER, (byte)GlobalValue.UniSerialPortOptData.VolLower);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_SMSInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置短信发送间隔..."));
                                        byte[] data = BitConverter.GetBytes((short)GlobalValue.UniSerialPortOptData.SMSInterval);
                                        Array.Reverse(data);
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_SMSINTERVAL, data);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_PluseUnit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置脉冲计数单位..."));
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_PLUSEUNIT, (byte)GlobalValue.UniSerialPortOptData.PluseUnit);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_AlarmLen)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置报警次数..."));
                                        byte data = Convert.ToByte(GlobalValue.UniSerialPortOptData.AlarmLen);
                                        result = GlobalValue.Universallog.Set(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)UNIVERSAL_COMMAND.SET_ALARMLEN, data);
                                    }

                                    double simrange = -1;
                                    //判断是否需要读取量程用于设置时的计算
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimit || GlobalValue.UniSerialPortOptData.IsOpt_LowLimit
                                        || GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit || GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit)
                                    {
                                        if (GlobalValue.UniSerialPortOptData.FlagType == UniversalFlagType.Simulate1|| GlobalValue.UniSerialPortOptData.FlagType == UniversalFlagType.Simulate2) //模拟量1、2才需要
                                        {
                                            OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在读取量程..."));
                                            simrange = GlobalValue.Universallog.ReadRange(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.FlagType);
                                        }
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Range)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置量程..."));
                                        result = GlobalValue.Universallog.SetRange(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Range, GlobalValue.UniSerialPortOptData.FlagType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置偏移值..."));
                                        result = GlobalValue.Universallog.SetPreOffset(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PreOffset, GlobalValue.UniSerialPortOptData.FlagType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置上限值..."));
                                        result = GlobalValue.Universallog.SetLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.UpLimit,simrange, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.UpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_UpLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置报警上限投退..."));
                                        result = GlobalValue.Universallog.SetAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.UpLimitEnable,GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.UpAlarm );
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置下限值..."));
                                        result = GlobalValue.Universallog.SetLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.LowLimit, simrange, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.LowAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_LowLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置报警下限投退..."));
                                        result = GlobalValue.Universallog.SetAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.LowLimitEnable, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.LowAlarm);
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置斜率上限值..."));
                                        result = GlobalValue.Universallog.SetLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.SlopUpLimit, simrange, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopUpAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopUpLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置斜率报警上限投退..."));
                                        result = GlobalValue.Universallog.SetAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.SlopUpLimitEnable, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopUpAlarm);
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置斜率上限值..."));
                                        result = GlobalValue.Universallog.SetLimit(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.SlopLowLimit, simrange, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopLowAlarm);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SlopLowLimitEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置斜率报警上限投退..."));
                                        result = GlobalValue.Universallog.SetAlarmEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.SlopLowLimitEnable, GlobalValue.UniSerialPortOptData.FlagType, UniversalAlarmType.SlopLowAlarm);
                                    }

                                    
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端采集配置..."));
                                        Int16 data = 0;
                                        if (GlobalValue.UniSerialPortOptData.Collect_DigitPre)
                                            data |= 0x01;
                                        if (GlobalValue.UniSerialPortOptData.Collect_Pluse)
                                            data |= 0x02;
                                        if (GlobalValue.UniSerialPortOptData.Collect_Simulate1)
                                            data |= 0x04;
                                        if (GlobalValue.UniSerialPortOptData.Collect_Simulate2)
                                            data |= 0x08;
                                        if (GlobalValue.UniSerialPortOptData.Collect_RS485)
                                            data |= 0x10;
                                        result = GlobalValue.Universallog.SetCollectConfig(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, (byte)data);
                                    }

                                    if (GlobalValue.UniSerialPortOptData.IsOpt_SimualteInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端模拟量时间间隔..."));
                                        result = GlobalValue.Universallog.SetSimulateInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Simulate_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PluseInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端脉冲量时间间隔..."));
                                        result = GlobalValue.Universallog.SetPluseInterval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Pluse_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Interval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端RS485时间间隔..."));
                                        result = GlobalValue.Universallog.SetRS485Interval(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.RS485_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_RS485Protocol)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalSetBasicInfo, "正在设置终端modbus协议..."));
                                        result = GlobalValue.Universallog.SetModbusProtocol(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.RS485Protocol);
                                    }
                                }
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalCalibrationSimualte1:
                        #region 校准通用终端第1路模拟量
                        {
                            try
                            {
                                result = GlobalValue.Universallog.CalibartionSimulate1(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalCalibrationSimualte2:
                        #region 校准通用终端第2路模拟量
                        {
                            try
                            {
                                result = GlobalValue.Universallog.CalibartionSimulate2(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalPluseBasic:
                        #region 设置通用终端脉冲基准数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.SetPluseBasic1)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置脉冲一路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PluseBasic1, 1);
                                }
                                if (GlobalValue.UniSerialPortOptData.SetPluseBasic2)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置脉冲二路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PluseBasic2, 2);
                                }
                                if (GlobalValue.UniSerialPortOptData.SetPluseBasic3)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置脉冲三路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PluseBasic3, 3);
                                }
                                if (GlobalValue.UniSerialPortOptData.SetPluseBasic4)
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置脉冲四路基准..."));
                                    result = GlobalValue.Universallog.SetPluseBasic(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PluseBasic4, 4);
                                }
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        break;
                    #endregion
                    case (uint)SerialPortType.UniversalCallData:
                        #region 读取通用终端招测数据
                        {
                            try
                            {
                                BLL.TerminalDataBLL dataBll = new BLL.TerminalDataBLL();
                                DataTable dt_config = null; Dictionary<int, string> lstAlarmType = null;
                                if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                {
                                    dt_config = dataBll.GetUniversalDataConfig((GlobalValue.UniSerialPortOptData.DevType == ConstValue.DEV_TYPE.Data_CTRL) ? TerType.PreTer : TerType.UniversalTer);
                                    if (dt_config == null || dt_config.Rows.Count == 0)
                                    {
                                        result = false;
                                        msg = "终端没有配置采集参数,请先配置!";
                                        break;
                                    }
                                    lstAlarmType = dataBll.GetAlarmType();
                                }
                                Package package = new Package();
                                List<string> lstmsg = new List<string>();
                                if (GlobalValue.SerialPortCallDataType.GetPre)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_Pre1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取压力失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("压力无数据");
                                        }
                                        else
                                            dataBll.AnalysisPre(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetSim1)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_Sim1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取模拟量1路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("模拟量1路无数据");
                                        }
                                        else
                                            dataBll.AnalysisSim(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetSim2)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_Sim2);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取模拟量2路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("模拟量2路无数据");
                                        }
                                        else
                                            dataBll.AnalysisSim(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetPluse)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_Pluse);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取脉冲失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("招测脉冲无数据");
                                        }
                                        else
                                            dataBll.AnalysisPluse(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetRS4851)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_RS4851, 15, 1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取RS485 1路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("招测RS485 1路无数据");
                                        }
                                        else
                                        {
                                            if (package.Data.Length == 20)  //长度为20的作为流量解析
                                                dataBll.AnalysisRS485Flow(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);
                                            else
                                                dataBll.AnalysisRS485(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);
                                        }

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetRS4852)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_RS4852, 15, 1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取RS485 2路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("招测RS485 2路无数据");
                                        }
                                        else
                                            dataBll.AnalysisRS485(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetRS4853)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_RS4853, 15, 1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取RS485 3路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("招测RS485 3路无数据");
                                        }
                                        else
                                            dataBll.AnalysisRS485(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                if (GlobalValue.SerialPortCallDataType.GetRS4854)
                                {
                                    package = GlobalValue.Universallog.GetCallDataPackage(GlobalValue.UniSerialPortOptData.ID, UNIVERSAL_COMMAND.CallData_RS4854, 15, 1);
                                    if (GlobalValue.Universallog.RWType == RWFunType.SerialPort)
                                    {
                                        if (!package.IsSuccess || package.Data == null)
                                        {
                                            lstmsg.Add("获取RS485 4路失败");
                                        }
                                        else if (package.Data.Length == 0)
                                        {
                                            lstmsg.Add("招测RS485 4路无数据");
                                        }
                                        else
                                            dataBll.AnalysisRS485(GlobalValue.UniSerialPortOptData.ID, package, dt_config, ref lstmsg, lstAlarmType);

                                        foreach (string msgresult in lstmsg)
                                        {
                                            serialPortUtil.AppendBufLine(msgresult);
                                        }
                                        lstmsg.Clear();
                                    }
                                }
                                
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                                else
                                    result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.UniversalCallEnable:
                        #region 设置通用终端招测使能
                        {
                            try
                            {
                                result = GlobalValue.Universallog.SetCallEnable(GlobalValue.UniSerialPortOptData.DevType, GlobalValue.UniSerialPortOptData.ID,GlobalValue.UniSerialPortOptData.CallEnable);
                                msg = SocketSend();
                                if (!string.IsNullOrEmpty(msg))
                                    result = false;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        break;
                    #endregion
                    #region 通用终端SL651操作
                    case (uint)SerialPortType.Universal651ChPwd:        //设置通用终端SL651密码
                    case (uint)SerialPortType.Universal651ReadBasicInfo://读取通用终端SL651基本信息
                    case (uint)SerialPortType.Universal651SetBasicInfo: //设置通用终端SL651基本信息
                    case (uint)SerialPortType.Universal651ReadRunInfo:  //读取通用终端SL651运行配置
                    case (uint)SerialPortType.Universal651SetRunInfo:   //设置通用终端SL651运行配置
                    case (uint)SerialPortType.Universal651QueryPrecipitation:   //通用终端SL651查询时段降水量
                    case (uint)SerialPortType.Universal651InitFlash:      //通用终端SL651初始化FLASH
                    case (uint)SerialPortType.Universal651Init:           //通用终端SL651恢复出厂
                        #region 通用终端SL651命令(需要发送确认帧)
                        {
                            SL651SendCmd(out result, out msg, out obj);
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.Universal651QueryElements://通用终端SL651查询指定要素 
                    case (uint)SerialPortType.Universal651QueryCurData:   //通用终端SL651查询实时数据
                    case (uint)SerialPortType.Universal651QueryVer:       //通用终端SL651查询版本 
                    case (uint)SerialPortType.Universal651QueryAlarm:     //通用终端SL651查询状态和报警
                    case (uint)SerialPortType.Universal651QueryEvent:     //通用终端SL651查询事件
                    case (uint)SerialPortType.Universal651SetTime:        //通用终端SL651设置时间   
                    case (uint)SerialPortType.Universal651QueryTime:      //通用终端SL651查询时间
                    case (uint)SerialPortType.Universal651QueryManualSetParm:   //通用终端SL651查询人工置数
                    case (uint)SerialPortType.Universal651SetManualSetParm:     //通用终端SL651设置人工置数
                    case (uint)SerialPortType.Universal651SetCalibration:       //通用终端SL651设置水位校准值
                    case (uint)SerialPortType.Universal651ReadTimeintervalReportTime:   //通用终端SL651读取均匀时段报上传时间
                    case (uint)SerialPortType.Universal651SetTimeintervalReportTime:    //通用终端SL651设置均匀时段报上传时间
                    case (uint)SerialPortType.Universal651SetPreConstCtrl://通用终端SL651设置水量定值控制命令
                        #region 通用终端SL651命令(不需要发送确认帧)
                        {
                            SL651SendCmd(out result, out msg, out obj);
                        }
                        #endregion
                        break;
                    #endregion
                    case (uint)SerialPortType.OLWQReset:
                        #region 水质终端复位
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.Reset(GlobalValue.UniSerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetTime:
                        #region 设置水质终端时间
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.SetTime(GlobalValue.UniSerialPortOptData.ID, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetEnableCollect:
                        #region 水质终端启用采集
                        {
                            try
                            {
                                result = GlobalValue.OLWQlog.EnableCollect(GlobalValue.UniSerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQReadBaicInfo:
                        #region 水质终端读取基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.UniSerialPortOptData.ID = GlobalValue.OLWQlog.ReadId();
                                }
                                else
                                {
                                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取设备时间..."));
                                        GlobalValue.UniSerialPortOptData.DT = GlobalValue.OLWQlog.ReadTime(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端通信IP地址..."));
                                        GlobalValue.UniSerialPortOptData.IP = GlobalValue.OLWQlog.ReadIP(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端通信端口号..."));
                                        GlobalValue.UniSerialPortOptData.Port = GlobalValue.OLWQlog.ReadPort(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端供电方式..."));
                                        GlobalValue.UniSerialPortOptData.PowerSupplyType = GlobalValue.OLWQlog.ReadPowerSupplyType(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端采集配置..."));
                                        byte data = GlobalValue.OLWQlog.ReadCollectConfig(GlobalValue.UniSerialPortOptData.ID);
                                        if ((data & 0x01) == 0x01)
                                            GlobalValue.UniSerialPortOptData.Collect_Turbidity = true;
                                        if ((data & 0x02) == 0x02)
                                            GlobalValue.UniSerialPortOptData.Collect_ResidualC1 = true;
                                        if ((data & 0x04) == 0x04)
                                            GlobalValue.UniSerialPortOptData.Collect_PH = true;
                                        if ((data & 0x08) == 0x08)
                                            GlobalValue.UniSerialPortOptData.Collect_Conductivity = true;
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTerAddr)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端地址..."));
                                        GlobalValue.UniSerialPortOptData.TerAddr = GlobalValue.OLWQlog.ReadTerAddr(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptCenterAddr)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取中心站地址..."));
                                        GlobalValue.UniSerialPortOptData.CenterAddr = GlobalValue.OLWQlog.ReadCenterAddr(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPwd)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取密码..."));
                                        GlobalValue.UniSerialPortOptData.Pwd = GlobalValue.OLWQlog.ReadPassword(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptWorkType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取工作方式..."));
                                        GlobalValue.UniSerialPortOptData.WorkType = GlobalValue.OLWQlog.ReadWorkType(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptGprsSwitch)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取GPRS开关状态..."));
                                        GlobalValue.UniSerialPortOptData.GprsSwitch = GlobalValue.OLWQlog.ReadGPRSSwitch(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptClearInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端清洗间隔..."));
                                        GlobalValue.UniSerialPortOptData.ClearInterval = GlobalValue.OLWQlog.ReadClearInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptDataInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端数据加报时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.DataInterval = GlobalValue.OLWQlog.ReadDataInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取温度上限..."));
                                        GlobalValue.UniSerialPortOptData.TempUpLimit = GlobalValue.OLWQlog.ReadTempUpLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取温度下限..."));
                                        GlobalValue.UniSerialPortOptData.TempLowLimit = GlobalValue.OLWQlog.ReadTempLowLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempAddtion)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取温度加报阀值..."));
                                        GlobalValue.UniSerialPortOptData.TempAddtion = GlobalValue.OLWQlog.ReadTempAddtion(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPHUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取PH上限..."));
                                        GlobalValue.UniSerialPortOptData.PHUpLimit = GlobalValue.OLWQlog.ReadPHUpLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPHLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取PH下限..."));
                                        GlobalValue.UniSerialPortOptData.PHLowLimit = GlobalValue.OLWQlog.ReadPHLowLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取电导率上限..."));
                                        GlobalValue.UniSerialPortOptData.ConductivityUpLimit = GlobalValue.OLWQlog.ReadConductivityUpLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取电导率下限..."));
                                        GlobalValue.UniSerialPortOptData.ConductivityLowLimit = GlobalValue.OLWQlog.ReadConductivityLowLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端浊度上限..."));
                                        GlobalValue.UniSerialPortOptData.TurbidityUpLimit = GlobalValue.OLWQlog.ReadTurbidityUpLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端浊度下限..."));
                                        GlobalValue.UniSerialPortOptData.TurbidityLowLimit = GlobalValue.OLWQlog.ReadTurbidityLowLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯下限值..."));
                                        GlobalValue.UniSerialPortOptData.ResidualClLowLimit = GlobalValue.OLWQlog.ReadResidualClLowLimit(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClZero)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯零点..."));
                                        GlobalValue.UniSerialPortOptData.ResidualClZero = GlobalValue.OLWQlog.ReadResidualClZero(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClStandValue)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯校准值..."));
                                        GlobalValue.UniSerialPortOptData.ResidualClStandValue = GlobalValue.OLWQlog.ReadResidualClStandValue(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClSensitivity)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯灵敏度..."));
                                        GlobalValue.UniSerialPortOptData.ResidualClSensitivity = GlobalValue.OLWQlog.ReadResidualClSensitivity(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_ResidualClInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端余氯时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.ResidualCl_Interval = GlobalValue.OLWQlog.ReadResidualClInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端浊度时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.Turbidity_Interval = GlobalValue.OLWQlog.ReadTurbidityInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PHInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端PH时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.PH_Interval = GlobalValue.OLWQlog.ReadPHInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_ConductivityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在读取终端电导率时间间隔..."));
                                        GlobalValue.UniSerialPortOptData.Conductivity_Interval = GlobalValue.OLWQlog.ReadConductivityInterval(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                }
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQSetBasicInfo:
                        #region 设置水质终端基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.OLWQlog.SetID(GlobalValue.UniSerialPortOptData.ID);
                                }
                                else
                                {
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端通信IP地址..."));
                                        result = GlobalValue.OLWQlog.SetIP(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.IP);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端通信端口号..."));
                                        result = GlobalValue.OLWQlog.SetPort(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Port);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPowerSupplyType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端供电方式..."));
                                        result = GlobalValue.OLWQlog.SetPowerSupplyType(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PowerSupplyType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_CollectConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端采集配置..."));
                                        Int16 data = 0;
                                        if (GlobalValue.UniSerialPortOptData.Collect_Turbidity)
                                            data |= 0x01;
                                        if (GlobalValue.UniSerialPortOptData.Collect_ResidualC1)
                                            data |= 0x02;
                                        if (GlobalValue.UniSerialPortOptData.Collect_PH)
                                            data |= 0x04;
                                        if (GlobalValue.UniSerialPortOptData.Collect_Conductivity)
                                            data |= 0x08;
                                        result = GlobalValue.OLWQlog.SetCollectConfig(GlobalValue.UniSerialPortOptData.ID, (byte)data);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTerAddr)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端地址..."));
                                        result = GlobalValue.OLWQlog.SetTerAddr(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TerAddr[4], GlobalValue.UniSerialPortOptData.TerAddr[3],
                                            GlobalValue.UniSerialPortOptData.TerAddr[2], GlobalValue.UniSerialPortOptData.TerAddr[1], GlobalValue.UniSerialPortOptData.TerAddr[0]);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptCenterAddr)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置中心站地址..."));
                                        result = GlobalValue.OLWQlog.SetCenterAddr(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.CenterAddr);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPwd)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置密码..."));
                                        result = GlobalValue.OLWQlog.SetPwd(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Pwd[1], GlobalValue.UniSerialPortOptData.Pwd[0]);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptWorkType)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置工作方式..."));
                                        result = GlobalValue.OLWQlog.SetWorkType(GlobalValue.UniSerialPortOptData.ID, (ushort)GlobalValue.UniSerialPortOptData.WorkType);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptGprsSwitch)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置GPRS开关..."));
                                        result = GlobalValue.OLWQlog.SetGPRSSwitch(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.GprsSwitch);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptClearInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端清洗间隔..."));
                                        result = GlobalValue.OLWQlog.SetClearInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ClearInterval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptDataInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端加报时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetDataInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.DataInterval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置温度上限..."));
                                        result = GlobalValue.OLWQlog.SetTempUpLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TempUpLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置温度下限..."));
                                        result = GlobalValue.OLWQlog.SetTempLowLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TempLowLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTempAddtion)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置温度加报阀值..."));
                                        result = GlobalValue.OLWQlog.SetTempAddtion(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TempAddtion);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPHUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置PH上限..."));
                                        result = GlobalValue.OLWQlog.SetPHUpLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PHUpLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPHLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置PH下限..."));
                                        result = GlobalValue.OLWQlog.SetPHLowLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PHLowLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置电导率上限..."));
                                        result = GlobalValue.OLWQlog.SetConductivityUpLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ConductivityUpLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptConductivityLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置电导率下限..."));
                                        result = GlobalValue.OLWQlog.SetConductivityLowLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ConductivityLowLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityUpLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端浊度上限..."));
                                        result = GlobalValue.OLWQlog.SetTurbidityUpLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TurbidityUpLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptTurbidityLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端浊度下限..."));
                                        result = GlobalValue.OLWQlog.SetTurbidityLowLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.TurbidityLowLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClLowLimit)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置余氯下限值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClLowLimit(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResidualClLowLimit);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClZero)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端零点值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClZero(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResidualClZero);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClStandValue)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端校准值..."));
                                        result = GlobalValue.OLWQlog.SetResidualClStandValue(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResidualClStandValue);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptResidualClSensitivity)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端灵敏度..."));
                                        result = GlobalValue.OLWQlog.SetResidualClSensitivity(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResidualClSensitivity);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_ResidualClInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端余氯采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetResidualClInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ResidualCl_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_TurbidityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端浊度采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetTurbidityInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Turbidity_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PHInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端PH采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetPHInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PH_Interval);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_ConductivityInterval)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.OLWQReadBaicInfo, "正在设置终端电导率采集时间间隔..."));
                                        result = GlobalValue.OLWQlog.SetConductivityInterval(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Conductivity_Interval);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.OLWQCallData:
                        #region 读取水质终端招测数据
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData == null || GlobalValue.SerialPortCallDataType == null)
                                {
                                    result = false;
                                    msg = "终端没有配置采集类型,请先配置!";
                                }
                                else
                                {
                                    DataTable dt_config = (new BLL.TerminalDataBLL()).GetUniversalDataConfig(Entity.TerType.UniversalTer);
                                    if (dt_config != null && dt_config.Rows.Count > 0)
                                    {
                                        obj = GlobalValue.OLWQlog.ReadCallData(GlobalValue.UniSerialPortOptData.ID, dt_config);
                                        result = true;
                                    }
                                    else
                                    {
                                        result = false;
                                        msg = "终端没有配置采集参数";
                                    }
                                }

                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantReset:
                        #region 消防栓复位
                        {
                            try
                            {
                                result = GlobalValue.Hydrantlog.Reset(GlobalValue.UniSerialPortOptData.ID);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantSetTime:
                        #region 设置消防栓时间
                        {
                            try
                            {
                                result = GlobalValue.Hydrantlog.SetTime(GlobalValue.UniSerialPortOptData.ID, DateTime.Now);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantReadBasicInfo:
                        #region 消防栓读取基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.UniSerialPortOptData.ID = GlobalValue.Hydrantlog.ReadId();
                                }
                                else
                                {
                                    if (GlobalValue.UniSerialPortOptData.IsOptDT)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓时间..."));
                                        GlobalValue.UniSerialPortOptData.DT = GlobalValue.Hydrantlog.ReadTime(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Numofturns)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓开启圈数..."));
                                        GlobalValue.UniSerialPortOptData.Numofturns = GlobalValue.Hydrantlog.ReadNumofturns(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_RealTimeData)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓实时压力值..."));
                                        GlobalValue.UniSerialPortOptData.RealTimeData = GlobalValue.Hydrantlog.ReadPreRealTimeData(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_ComTime)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓远传时间..."));
                                        GlobalValue.UniSerialPortOptData.ComTime = GlobalValue.Hydrantlog.ReadComTime(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓通信IP地址..."));
                                        GlobalValue.UniSerialPortOptData.IP = GlobalValue.Hydrantlog.ReadIP(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓通信端口号..."));
                                        GlobalValue.UniSerialPortOptData.Port = GlobalValue.Hydrantlog.ReadPort(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_HydrantEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓开关状态..."));
                                        GlobalValue.UniSerialPortOptData.HydrantEnable = GlobalValue.Hydrantlog.ReadEnableCollect(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_Range)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓压力量程..."));
                                        GlobalValue.UniSerialPortOptData.Range = GlobalValue.Hydrantlog.ReadPreRange(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓压力偏移量..."));
                                        GlobalValue.UniSerialPortOptData.PreOffset = GlobalValue.Hydrantlog.ReadPreOffset(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取压力配置..."));
                                        GlobalValue.UniSerialPortOptData.PreConfig = GlobalValue.Hydrantlog.ReadPreConfig(GlobalValue.UniSerialPortOptData.ID);
                                    }
                                    
                                }
                                result = true;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantSetBasicInfo:
                        #region 设置消防栓基础参数
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    result = GlobalValue.Hydrantlog.SetID(GlobalValue.UniSerialPortOptData.ID);
                                }
                                else
                                {
                                    
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_ComTime)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓远传时间..."));
                                        result = GlobalValue.Hydrantlog.SetComTime(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.ComTime);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptIP)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓通信IP地址..."));
                                        result = GlobalValue.Hydrantlog.SetIP(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.IP);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOptPort)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓通信端口号..."));
                                        result = GlobalValue.Hydrantlog.SetPort(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Port);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_HydrantEnable)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓开关状态..."));
                                        result = GlobalValue.Hydrantlog.SetEnableCollect(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.HydrantEnable);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_Range)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓压力量程..."));
                                        result = GlobalValue.Hydrantlog.SetPreRange(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.Range);
                                    }
                                    if(GlobalValue.UniSerialPortOptData.IsOpt_PreOffset)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantSetBasicInfo, "正在设置消防栓压力偏移值..."));
                                        result = GlobalValue.Hydrantlog.SetPreOffset(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PreOffset);
                                    }
                                    if (GlobalValue.UniSerialPortOptData.IsOpt_PreConfig)
                                    {
                                        OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.UniversalReadBasicInfo, "正在设置消防栓压力配置..."));
                                        result = GlobalValue.Hydrantlog.SetPreConfig(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.PreConfig);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantReadHistory:
                        #region 消防栓读取历史数据
                        {
                            try
                            {
                                if (GlobalValue.UniSerialPortOptData.IsOptID)  //没有ID时，只读写ID
                                {
                                    GlobalValue.UniSerialPortOptData.ID = GlobalValue.Hydrantlog.ReadId();
                                }
                                else
                                {
                                    OnSerialPortScheduleEvent(new SerialPortScheduleEventArgs(SerialPortType.HydrantReadBasicInfo, "正在读取消防栓时间..."));
                                    GlobalValue.Hydrantlog.ReadHistory(GlobalValue.UniSerialPortOptData.ID, GlobalValue.UniSerialPortOptData.HydrantHistoryOpt);
                                }
                                result = true;
                            }
                            catch (ArgumentNullException argex)
                            {
                                result = true;
                                msg = argex.ParamName;
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                    case (uint)SerialPortType.HydrantSetEnableCollect:
                        #region 消防栓启用采集
                        {
                            try
                            {
                                result = GlobalValue.Hydrantlog.SetEnableCollect(GlobalValue.UniSerialPortOptData.ID, true);
                            }
                            catch (Exception ex)
                            {
                                result = false;
                                msg = ex.Message;
                            }
                        }
                        #endregion
                        break;
                }
                GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived += GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;  //发送完成后,恢复委托
                OnSerialPortEvent(new SerialPortEventArgs(SendType, result ? TransStatus.Success : TransStatus.Fail, msg, obj));
            }
        }

        /// <summary>
        /// 将消息显示到FrmConsole页面
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="multiPack">是否多包</param>
        private void ShowMsgControl(Package651 pack, string msg, bool multiPack = false)
        {
            string str_senddt = string.Format("{0}-{1}-{2} {3}:{4}:{5}", String.Format("{0:X2}", pack.dt[0]), String.Format("{0:X2}", pack.dt[1])
                                            , String.Format("{0:X2}", pack.dt[2]), String.Format("{0:X2}", pack.dt[3]), String.Format("{0:X2}", pack.dt[4]), String.Format("{0:X2}", pack.dt[5]));
            string str = string.Format("中心站地址:{0},遥测站地址:A1-A5[{1},{2},{3},{4},{5}],密码:{6},功能码:{7}({8}),上/下行:{9},",
                                            Convert.ToInt16(pack.CenterAddr), String.Format("{0:X2}", pack.A1), String.Format("{0:X2}", pack.A2), String.Format("{0:X2}", pack.A3), String.Format("{0:X2}", pack.A4), String.Format("{0:X2}", pack.A5),
                                            "0x" + String.Format("{0:X2}", pack.PWD[0]) + String.Format("{0:X2}", pack.PWD[1]), "0x" + String.Format("{0:X2}", pack.FUNCODE), SL651AnalyseElement.GetFuncodeName(pack.FUNCODE), pack.IsUpload ? "上行" : "下行") +
                                            string.Format("报文长度:{0},报文起始符:{1},发报时间:{2},{3}校验码:{4}",
                                            (!multiPack ? pack.DataLength.ToString() : "多包"), "0x" + String.Format("{0:X2}", pack.CStart), str_senddt, msg, ConvertHelper.ByteToString(pack.CS, pack.CS.Length));
            GlobalValue.portUtil.AppendBufLine(str);
        }

        private void SL651SendCmd(out bool result, out string msg, out object obj)
        {
            result = false;  //-1:执行失败;1:执行成功;0:无执行返回
            msg = "";
            obj = null;
            try
            {
                SPLoopRunning = false;
                Thread.Sleep(20);
                bool readnextpack = false; //是否读取下一个包(多包时,第一包readnextpack = false,后面readnextpack = true)
                GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived -= GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;
                GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived += GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;
                List<Package651> packsresp = GlobalValue.SerialPortMgr.Read(GlobalValue.SerialPort651OptData, 4, 1, true, readnextpack);
                if (packsresp != null && packsresp.Count > 0)
                {
                    List<byte> lstData = new List<byte>();
                    lstData.AddRange(packsresp[0].Data);
                    for (int i = 0; i < packsresp.Count; i++)
                    {
                        string packcountmsg = "";
                        if (packsresp[0].SumPackCount > 1)
                            packcountmsg = "总包数:" + packsresp[i].SumPackCount + "、当前第" + packsresp[i].CurPackCount + "包";

                        GlobalValue.portUtil.AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff") + string.Format(" 收到{0}:{1}", packcountmsg, ConvertHelper.ByteArrayToHexString(packsresp[i].OriginalData)));
                        if (i > 0)
                            lstData.AddRange(packsresp[i].Data);  //多包的时候将多包合并
                        
                    }
                    Universal651SerialPortEntity spEntity = null;
                    string anamsg = SL651AnalyseElement.AnalyseElement(packsresp[0].FUNCODE, lstData.ToArray(), packsresp[0].dt, ref spEntity);
                    ShowMsgControl(packsresp[0], anamsg, (packsresp.Count > 1 ? true : false));
                    bool needconfirm = Package651.NeedResp(packsresp[0].FUNCODE);
                    if (needconfirm && (packsresp.Count == 1 || (packsresp[packsresp.Count - 1].CurPackCount > 0 && packsresp[packsresp.Count - 1].CurPackCount == packsresp[packsresp.Count - 1].SumPackCount)))
                    {
                        Package651 tmp = GlobalValue.SerialPort651OptData;
                        tmp.dt[0] = ConvertHelper.StringToByte((DateTime.Now.Year - 2000).ToString())[0];
                        tmp.dt[1] = ConvertHelper.StringToByte(DateTime.Now.Month.ToString().PadLeft(2, '0'))[0];
                        tmp.dt[2] = ConvertHelper.StringToByte(DateTime.Now.Day.ToString().PadLeft(2, '0'))[0];
                        tmp.dt[3] = ConvertHelper.StringToByte(DateTime.Now.Hour.ToString().PadLeft(2, '0'))[0];
                        tmp.dt[4] = ConvertHelper.StringToByte(DateTime.Now.Minute.ToString().PadLeft(2, '0'))[0];
                        tmp.dt[5] = ConvertHelper.StringToByte(DateTime.Now.Second.ToString().PadLeft(2, '0'))[0];
                        tmp.Data = null;
                        tmp.CS = null;
                        byte[] lens = new byte[2];
                        if (packsresp[packsresp.Count - 1].CurPackCount > 0 && packsresp[packsresp.Count - 1].CurPackCount == packsresp[packsresp.Count - 1].SumPackCount)  //多包时,回复的帧中带包序号
                        {
                            tmp.CurPackCount = packsresp[packsresp.Count - 1].CurPackCount;
                            tmp.SumPackCount = packsresp[packsresp.Count - 1].CurPackCount;
                            lens = BitConverter.GetBytes((ushort)(11));  //多了三个字节的包总数及序列号
                            tmp.CStart = PackageDefine.CStart_Pack;   //多包起始符不一样，不是02 是16
                        }
                        else
                            lens = BitConverter.GetBytes((ushort)(8));
                        tmp.L0 = lens[0];
                        tmp.L1 = lens[1];
                        if (SL651AllowOnLine)
                            tmp.End = PackageDefine.ESC;
                        else
                            tmp.End = PackageDefine.EOT;
                        byte[] bsenddata = tmp.ToResponseArray();
                        tmp.CS = Package651.crc16(bsenddata, bsenddata.Length);
                        Thread.Sleep(20);
                        GlobalValue.SerialPortMgr.Read(tmp, 3, 1, false);
                    }
                    result = true;
                    msg = "";
                    obj = spEntity;
                    GlobalValue.Universallog.serialPortUtil.lstResult651.Clear();
                }
            }
            catch (Exception ex)
            {
                result = false;
                msg = ex.Message;
            }
            finally
            {
                Thread.Sleep(30);
                SPLoopRunning = true;
            }
        }

        private void SerialPortLoop()
        {
            Thread t = new Thread(new ThreadStart(SerialPortLoopThread));
            t.IsBackground = true;
            t.Start();
            SPLoopRunning = true;
        }

        public bool SPLoopRunning = false;  //是否允许运行
        List<Package651> lstResult651 = null;  //返回结果集651(支持多包)
        public bool SL651AllowOnLine = false;
        private void SerialPortLoopThread()
        {
            try
            {
                lstResult651 = GlobalValue.Universallog.serialPortUtil.lstResult651;
                lstResult651.Clear();
                //GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived -= GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;
                //GlobalValue.Universallog.serialPortUtil.serialPort.DataReceived += GlobalValue.Universallog.serialPortUtil.SerialPort_DataReceived;

                while (true)
                {
                    try
                    {
                        Thread.Sleep(20);
                        if (SPLoopRunning)
                        {
                            if (GlobalValue.Universallog.serialPortUtil.IsComClosing)//关闭窗口
                            {
                                break;
                            }

                            if (lstResult651.Count > 0)
                            {
                                if (lstResult651 != null && lstResult651.Count > 0)
                                {
                                    int proccount = 0;
                                    for (int i = 0; i < lstResult651.Count; i++)
                                    {
                                        //确定是上行还是下行帧,只监听上行帧
                                        if (lstResult651[i].IsUpload)
                                        {
                                            GlobalValue.portUtil.AppendBufLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss ffff")+" 监听到帧:" + ConvertHelper.ByteArrayToHexString(lstResult651[i].OriginalData));
                                            Universal651SerialPortEntity spEntity = null;
                                            if (lstResult651[i].Data != null && lstResult651[i].Data.Length > 0)
                                            {
                                                List<byte> lstData = new List<byte>();
                                                lstData.AddRange(lstResult651[i].Data);
                                                string anamsg = SL651AnalyseElement.AnalyseElement(lstResult651[i].FUNCODE, lstData.ToArray(), lstResult651[i].dt, ref spEntity);
                                                ShowMsgControl(lstResult651[i], anamsg, (lstResult651.Count > 1 ? true : false));
                                            }
                                            else
                                            {
                                                string anamsg = SL651AnalyseElement.AnalyseElement(lstResult651[i].FUNCODE, null, lstResult651[i].dt, ref spEntity);
                                                ShowMsgControl(lstResult651[i], anamsg, (lstResult651.Count > 1 ? true : false));
                                            }

                                            SerialPortType evt = GetSPType(lstResult651[i].FUNCODE);
                                            if (evt != SerialPortType.None)
                                            {
                                                spEntity.IsSPLoop = true;
                                                OnSerialPortEvent(new SerialPortEventArgs(evt, TransStatus.Success, "", spEntity));
                                            }
                                            if (Package651.NeedResp(lstResult651[i].FUNCODE))
                                            {
                                                Package651 tmp = lstResult651[i];
                                                tmp.dt[0] = ConvertHelper.StringToByte((DateTime.Now.Year - 2000).ToString())[0];
                                                tmp.dt[1] = ConvertHelper.StringToByte(DateTime.Now.Month.ToString().PadLeft(2, '0'))[0];
                                                tmp.dt[2] = ConvertHelper.StringToByte(DateTime.Now.Day.ToString().PadLeft(2, '0'))[0];
                                                tmp.dt[3] = ConvertHelper.StringToByte(DateTime.Now.Hour.ToString().PadLeft(2, '0'))[0];
                                                tmp.dt[4] = ConvertHelper.StringToByte(DateTime.Now.Minute.ToString().PadLeft(2, '0'))[0];
                                                tmp.dt[5] = ConvertHelper.StringToByte(DateTime.Now.Second.ToString().PadLeft(2, '0'))[0];
                                                tmp.Data = null;
                                                tmp.CS = null;
                                                byte[] lens = BitConverter.GetBytes((ushort)(8));
                                                tmp.L0 = lens[0];
                                                tmp.L1 = lens[1];
                                                if (SL651AllowOnLine)
                                                    tmp.End = PackageDefine.ESC;
                                                else
                                                    tmp.End = PackageDefine.EOT;
                                                byte[] bsenddata = tmp.ToResponseArray();
                                                tmp.CS = Package651.crc16(bsenddata, bsenddata.Length);
                                                Thread.Sleep(20);
                                                GlobalValue.SerialPortMgr.Read(tmp, 3, 2, false);
                                            }
                                        }
                                        proccount++;
                                    }
                                    if (lstResult651.Count >= proccount)
                                    {
                                        try
                                        {
                                            lstResult651.RemoveRange(0, proccount);
                                        }
                                        catch { }
                                    }
                                }
                            }
                        }
                    }
                    catch(Exception ex)
                    {
                        logger.ErrorException("SerialPortLoopThread", ex);
                        lstResult651.Clear();
                    }
                }
            }
            catch (Exception e)
            {
                logger.ErrorException("SerialPortLoopThread", e);
            }
        }

        private SerialPortType GetSPType(byte funcode)
        {
            switch ((SL651_COMMAND)funcode)
            {
                case SL651_COMMAND.ChPwd:
                    return SerialPortType.Universal651ChPwd;        //设置通用终端SL651密码
                case SL651_COMMAND.ReadBasiConfig:
                    return SerialPortType.Universal651ReadBasicInfo;//读取通用终端SL651基本信息
                case SL651_COMMAND.SetBasicConfig:
                    return SerialPortType.Universal651SetBasicInfo; //设置通用终端SL651基本信息
                case SL651_COMMAND.ReadRunConfig:
                    return SerialPortType.Universal651ReadRunInfo;  //读取通用终端SL651运行配置
                case SL651_COMMAND.SetRunConfig:
                    return SerialPortType.Universal651SetRunInfo;   //设置通用终端SL651运行配置
                case SL651_COMMAND.PrecipitationConstantCtrl:
                    return SerialPortType.Universal651SetPreConstCtrl;//通用终端SL651设置水量定值控制命令
                case SL651_COMMAND.QueryPrecipitation:
                    return SerialPortType.Universal651QueryPrecipitation;   //通用终端SL651查询时段降水量
                case SL651_COMMAND.InitFlash:
                    return SerialPortType.Universal651InitFlash;      //通用终端SL651初始化FLASH
                case SL651_COMMAND.Init:
                    return SerialPortType.Universal651Init;           //通用终端SL651恢复出厂
                case SL651_COMMAND.QueryElements:
                    return SerialPortType.Universal651QueryElements;//通用终端SL651查询指定要素 
                case SL651_COMMAND.QueryCurData:
                    return SerialPortType.Universal651QueryCurData;   //通用终端SL651查询实时数据
                case SL651_COMMAND.QueryVer:
                    return SerialPortType.Universal651QueryVer;       //通用终端SL651查询版本 
                case SL651_COMMAND.QueryAlarm:
                    return SerialPortType.Universal651QueryAlarm;     //通用终端SL651查询状态和报警
                case SL651_COMMAND.QueryEvent:
                    return SerialPortType.Universal651QueryEvent;     //通用终端SL651查询事件
                case SL651_COMMAND.SetTime:
                    return SerialPortType.Universal651SetTime;        //通用终端SL651设置时间   
                case SL651_COMMAND.QueryTime:
                    return SerialPortType.Universal651QueryTime;      //通用终端SL651查询时间
                case SL651_COMMAND.QueryManualSetParm:
                    return SerialPortType.Universal651QueryManualSetParm;   //通用终端SL651查询人工置数
                case SL651_COMMAND.SetManualSetParm:
                    return SerialPortType.Universal651SetManualSetParm;     //通用终端SL651设置人工置数
                case SL651_COMMAND.SetCalibration1:
                    return SerialPortType.Universal651SetCalibration;       //通用终端SL651设置水位校准值
                case SL651_COMMAND.ReadTimeIntervalReportTime:
                    return SerialPortType.Universal651ReadTimeintervalReportTime;   //通用终端SL651读取均匀时段报上传时间
                case SL651_COMMAND.SetTimeIntervalReportTime:
                    return SerialPortType.Universal651SetTimeintervalReportTime;    //通用终端SL651设置均匀时段报上传时间
                default:
                    return SerialPortType.None;
            }
        }

        private string SocketSend()
        {
            try
            {
                if (GlobalValue.Universallog.RWType== RWFunType.GPRS && GlobalValue.Universallog.lstCmdPack != null && GlobalValue.Universallog.lstCmdPack.Count > 0)
                {
                    SocketEntity msmqentity = new SocketEntity();
                    msmqentity.MsgType = ConstValue.MSMQTYPE.P68_Cmd;
                    msmqentity.Packs = GlobalValue.Universallog.lstCmdPack.ToArray();
                    return GlobalValue.SocketMgr.SendMessage2(msmqentity);
                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                logger.ErrorException("SocketSend", ex);
                return "发送发送异常,ex:"+ex.Message;
            }
        }
    }
}
