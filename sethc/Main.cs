using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace sethc
{
	public partial class Main : Form
	{
		public const Int32 SPI_SETSTICKYKEYS = 0x003B;
		public const Int32 SPIF_UPDATEINIFILE = 0x01;
		public const Int32 SPIF_SENDWININICHANGE = 0x02;
		public const UInt32 SKF_STICKYKEYSON = 0x00000001;
		public const UInt32 SKF_HOTKEYACTIVE = 0x00000004;
		public const Int32 WM_SYSCOMMAND = 0x112;
		public const Int32 MF_BYPOSITION = 0x400;
		public const Int32 MF_SEPARATOR = 0x800;
		public const Int32 CTXMENU1 = 1000;
		public const Int32 CTXMENU2 = 2000;

		[DllImport("user32.dll")]
		private static extern IntPtr GetSystemMenu(IntPtr hWnd, bool bRevert);
		[DllImport("user32.dll")]
		private static extern bool InsertMenu(IntPtr hMenu, Int32 wPosition, Int32 wFlags, Int32 wIDNewItem, string lpNewItem);
		[DllImport("user32.dll")]
		private static extern bool SystemParametersInfoA(Int32 uiAction, Int32 uiParam, IntPtr pvParam, Int32 fWinIni);
		[StructLayout(LayoutKind.Sequential)]
		private struct TagSTICKYKEYS
		{
			public Int32 cbSize;
			public UInt32 dwFlags;
		}

		public Main()
		{
			InitializeComponent();
		}

		protected override void WndProc(ref Message msg)
		{
			if (msg.Msg == WM_SYSCOMMAND)
			{
				try
				{
					switch (msg.WParam.ToInt32())
					{
						case CTXMENU1:
							MessageBox.Show("Start Installer");
                            return;
                        case CTXMENU2:
                            MessageBox.Show("Start Uninstaller");
                            return;
                        default:
							break;
					}
				}
				catch (Exception error)
				{
					Console.WriteLine($"Cannot open process: {error}");
				}
			}
			base.WndProc(ref msg);
		}

		private void Main_Load(object sender, EventArgs e)
		{
			IntPtr MenuHandle = GetSystemMenu(this.Handle, false);
			InsertMenu(MenuHandle, 5, MF_BYPOSITION | MF_SEPARATOR, 0, string.Empty);
			if (File.Exists("C:\\Users\\Public\\WinMin\\WinMin.exe"))
				InsertMenu(MenuHandle, 7, MF_BYPOSITION, CTXMENU2, "Uninstall WinMin");
            else
				InsertMenu(MenuHandle, 6, MF_BYPOSITION, CTXMENU1, "Install WinMin");
		}

		private void Labeldeactivatedialog_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			try
			{
				Process.Start("ms-settings:easeofaccess-keyboard");
			}
			catch (Exception)
			{
				try
				{
                    ProcessStartInfo skwinxp = new ProcessStartInfo("rundll32.exe")
                    {
                        Arguments = "Shell32.dll,Control_RunDLL access.cpl,,1"
                    };
                    Process.Start(skwinxp);
				}
				catch (Exception)
				{
					try
					{
						Process.Start("control.exe");
					}
					catch (Exception error)
					{
						int code = GetErrorCode(error);
						MessageBox.Show($"Cannot open settings from {AppDomain.CurrentDomain.FriendlyName}:\n{error}", AppDomain.CurrentDomain.FriendlyName + " - Cannot open settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
						Environment.Exit(code);
					}
				}
			}

			Application.Exit();
		}

		private void ButtonNo_Click(object sender, EventArgs e)
		{
			Application.Exit();
		}

		private void ButtonYes_Click(object sender, EventArgs e)
		{
			try
			{
				TagSTICKYKEYS stk;
				stk.dwFlags = SKF_STICKYKEYSON | SKF_HOTKEYACTIVE;
				stk.cbSize = Marshal.SizeOf(typeof(TagSTICKYKEYS));
				IntPtr pObj = Marshal.AllocHGlobal(Marshal.SizeOf(typeof(TagSTICKYKEYS)));
				Marshal.StructureToPtr(stk, pObj, false);
				SystemParametersInfoA(SPI_SETSTICKYKEYS, 0, pObj, SPIF_UPDATEINIFILE | SPIF_SENDWININICHANGE);
				Marshal.FreeHGlobal(pObj);
			}
			catch (Exception error)
			{
				int code = GetErrorCode(error);
				MessageBox.Show($"Cannot change settings from {AppDomain.CurrentDomain.FriendlyName}: {error}\nThe current user will have to manually change the settings.", AppDomain.CurrentDomain.FriendlyName + " - Cannot change settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
				Environment.Exit(code);
			}

			Application.Exit();
		}

		private int GetErrorCode(Exception error)
		{
			int code = 1;
            if (!(error is Win32Exception w32ex))
                w32ex = error.InnerException as Win32Exception;
            if (w32ex != null)
				code = w32ex.ErrorCode;
			return code;
		}
	}
}
