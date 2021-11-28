// ============================================================================
// 
// 動的タブの ViewModel
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Livet;
using Livet.Commands;

using System;
using System.Collections.ObjectModel;

namespace TestXamlReader.ViewModels
{
	public class DynamicTabItemViewModel : ViewModel
	{
		// ====================================================================
		// コンストラクター・デストラクター
		// ====================================================================

		// --------------------------------------------------------------------
		// コンストラクター
		// --------------------------------------------------------------------
		public DynamicTabItemViewModel()
		{
			for (Int32 i = 0; i < 10; i++)
			{
				ListItems.Add("ドラッグで順番を入れ替えられるアイテム " + i.ToString());
			}
		}

		// ====================================================================
		// public プロパティー
		// ====================================================================

		// --------------------------------------------------------------------
		// View 通信用のプロパティー
		// --------------------------------------------------------------------

		// ランダム値
		private Int32 _randomValue;
		public Int32 RandomValue
		{
			get => _randomValue;
			set => RaisePropertyChangedIfSet(ref _randomValue, value);
		}

		// リストアイテム
		public ObservableCollection<String> ListItems { get; set; } = new();

		// --------------------------------------------------------------------
		// コマンド
		// --------------------------------------------------------------------

		#region ランダムに値を設定ボタンの制御
		private ViewModelCommand? _buttonRandomClickedCommand;

		public ViewModelCommand ButtonRandomClickedCommand
		{
			get
			{
				if (_buttonRandomClickedCommand == null)
				{
					_buttonRandomClickedCommand = new ViewModelCommand(ButtonRandomClicked);
				}
				return _buttonRandomClickedCommand;
			}
		}

		public void ButtonRandomClicked()
		{
			RandomValue = Random.Shared.Next();
		}
		#endregion
	}
}
