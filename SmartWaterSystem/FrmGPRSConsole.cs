﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace SmartWaterSystem
{
    public partial class FrmGPRSConsole : DevExpress.XtraEditors.XtraForm
    {
        NLog.Logger logger = NLog.LogManager.GetLogger("FrmGPRSConsole");
        private const int MaxLine = 1000;

        public FrmGPRSConsole()
        {
            InitializeComponent();

            timerCtrl.Interval = 100;
            timerCtrl.Tick += new EventHandler(timerCtrl_Tick);
            timerCtrl.Enabled = true;

            GlobalValue.MSMQMgr.MSMQEvent += new MSMQHandler(MSMQMgr_MSMQEvent);
        }

        private void FrmGPRSConsole_Load(object sender, EventArgs e)
        {
            FrmGPRSConsole.CheckForIllegalCrossThreadCalls = false;
            ShowCtrlMsg();

            txtControl.SelectionStart = txtControl.Text.Length;
            txtControl.ScrollToCaret();
            txtControl.SelectedText = "";

            GlobalValue.MSMQMgr.GetServiceStopMsg();
        }

        void MSMQMgr_MSMQEvent(object sender, MSMQEventArgs e)
        {
            if (e.msmqEntity != null)
            {
                if (e.msmqEntity.MsgType == Entity.ConstValue.MSMQTYPE.Message)
                {
                    SetCtrlMsg(e.msmqEntity.Msg);
                }
            }
        }

        private void SetCtrlMsg(string msg)
        {
            if (!string.IsNullOrEmpty(msg))
            {
                if (!msg.EndsWith("\r\n"))
                    msg += "\r\n";
                lstCtrlMsg.Add(msg);
                ctrlMsgChange = true;
            }
        }

        void timerCtrl_Tick(object sender, EventArgs e)
        {
            if (ctrlMsgChange && txtControl.Visible)
            {
                //去除超过MaxLine行数部分
                int outliencount = lstCtrlMsg.Count - MaxLine;
                if (outliencount > 0)
                {
                    lstCtrlMsg.RemoveRange(0, outliencount);
                }

                ShowCtrlMsg();
                ctrlMsgChange = false;
            }
        }

        private delegate void AddMsgHandle(string msg);
        List<string> lstCtrlMsg = new List<string>();
        bool ctrlMsgChange = false;  //是否有更新消息
        public void ShowCtrlMsg()
        {
            try
            {
                string ctrlmsg = "";
                for (int i = 0; i < lstCtrlMsg.Count; i++)
                {
                    ctrlmsg += lstCtrlMsg[i];
                }
                txtControl.ResetText();
                txtControl.Text+=ctrlmsg;
                txtControl.SelectionStart = txtControl.Text.Length;
                txtControl.ScrollToCaret();
                txtControl.SelectedText = "";
            }
            catch (Exception ex)
            {
                logger.ErrorException("ShowCtrlMsg", ex);
                MessageBox.Show("输出消息时发生异常", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void FrmGPRSConsole_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }


    }
}