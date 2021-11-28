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

				// ViewModel を紐付
				DynamicTabItemViewModel dynamicTabItemViewModel = new();
				CompositeDisposable.Add(dynamicTabItemViewModel);
				tabItem.DataContext = dynamicTabItemViewModel;
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
