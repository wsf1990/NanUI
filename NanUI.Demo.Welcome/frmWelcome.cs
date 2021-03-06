﻿using Chromium;
using NetDimension.NanUI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NanUI.Demo.Welcome
{
	public partial class frmWelcome : HtmlUIForm    //继承HtmlUIForm
	{
		frmAbout aboutForm = null;
		public frmWelcome()
			: base("embedded://www/index.html") //设定启示页面，scheme是embedded就是我们在Main里注册的当前程序集资源
		{
			InitializeComponent();

			//在js中注册一个方法来打开About窗口
			GlobalObject.AddFunction("showAboutForm").Execute += (sender, args) =>
			{
				ShowAboutNanUI();
				//ShowAboutWindow();
			};

			GlobalObject.AddFunction("showDevTools").Execute += (sender, args) =>
			{
				ShowDevTools();
			};


			//网页加载完成时触发事件
			LoadHandler.OnLoadEnd += (sender, args) =>
			{
			//判断下触发的事件是不是主框架的
			if (args.Frame.IsMain)
				{
				//执行JS，将当前的CEF运行版本等信息通过JS加载到网页上
				var js = $"$client.setRuntimeInfo({{ api: ['{CfxRuntime.ApiHash(0)}', '{CfxRuntime.ApiHash(1)}'], cef:'{CfxRuntime.GetCefVersion()}', chrome:'{CfxRuntime.GetChromeVersion()}',os:'{CfxRuntime.PlatformOS}', arch:'{CfxRuntime.PlatformArch}'}});";
					ExecuteJavascript(js);
				}


			};

		}

		private void ShowAboutWindow()
		{
			//因为当前环境中的JS代码跑在另外的线程上，所以在Control上扩展个UpdateUI方法，简化InvokeRequired流程
			this.UpdateUI(() =>
			{
			//显示字窗体的过程，不解释
			if (aboutForm == null || aboutForm.IsDisposed)
				{
					aboutForm = new frmAbout();
					aboutForm.Show(this);
				}
				else
				{
					aboutForm.Activate();
				}

			});
		}
	}
}
