using QFramework;
using UnityEngine;
using UnityEngine.UI;

namespace QFramework.Example
{


	public partial class UIStackPanel2 : UIPanel
	{
		protected override void OnInit(IUIData uiData = null)
		{
			mData = uiData as UIStackPanel2Data ?? new UIStackPanel2Data();
			// please add init code here
		}


		protected override void OnOpen(IUIData uiData = null)
		{
		}


		protected override void OnShow()
		{
		}


		protected override void OnHide()
		{
		}


		protected override void OnClose()
		{
		}
	}
	public class UIStackPanel2Data : UIPanelData
	{
	}
}
