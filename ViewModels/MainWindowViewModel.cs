// ============================================================================
// 
// XAML を動的に読み込んで UI を構築するサンプルプログラム
// 
// ============================================================================

// ----------------------------------------------------------------------------
// 
// ----------------------------------------------------------------------------

using Livet;
using Livet.Commands;

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup;
using System.Xml;

namespace TestXamlReader.ViewModels
{
	public class MainWindowViewModel : ViewModel
	{
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

		// タブアイテム
		public ObservableCollection<TabItem> TabItems { get; set; } = new();

		// 選択タブ
		private Int32 _selectedTabIndex;
		public Int32 SelectedTabIndex
		{
			get => _selectedTabIndex;
			set => RaisePropertyChangedIfSet(ref _selectedTabIndex, value);
		}

		// --------------------------------------------------------------------
		// コマンド
		// --------------------------------------------------------------------

		#region タブを増やすボタンの制御
		private ViewModelCommand? _buttonAddTabItemClickedCommand;

		public ViewModelCommand ButtonAddTabItemClickedCommand
		{
			get
			{
				if (_buttonAddTabItemClickedCommand == null)
				{
					_buttonAddTabItemClickedCommand = new ViewModelCommand(ButtonAddTabItemClicked);
				}
				return _buttonAddTabItemClickedCommand;
			}
		}

		public void ButtonAddTabItemClicked()
		{
			try
			{
				// リソースから XAML を読み込む
				Assembly assembly = Assembly.GetExecutingAssembly();
				Stream? stream = assembly.GetManifestResourceStream("TestXamlReader.Views.DynamicTabItem.xaml");
				if (stream == null)
				{
					throw new Exception("リソースを読み込めませんでした：");
				}
				using StreamReader reader = new(stream);
				using XmlReader xml = XmlReader.Create(reader.BaseStream);

				// タブを生成
				FrameworkElement? element = XamlReader.Load(xml) as FrameworkElement;
				if (element == null)
				{
					throw new Exception("リソースからコントロールを生成できませんでした：");
				}
				TabItem tabItem = new()
				{
					Header = "タブ " + TabItems.Count,
					Content = element,
				};
				TabItems.Add(tabItem);

				// 追加したタブを選択
				SelectedTabIndex = TabItems.Count - 1;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message);
			}
		}
		#endregion

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

		// ====================================================================
		// public メンバー関数
		// ====================================================================

		// --------------------------------------------------------------------
		// 初期化
		// --------------------------------------------------------------------
		public void Initialize()
		{
		}
	}
}
