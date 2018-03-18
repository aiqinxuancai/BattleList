using BattleListMainWindow.Service;
using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BattleListMainWindow
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private BattleListManager battleListManager ;

        private SQLiteShowList sqliteShowList;

        public MainWindow()
        {
            InitializeComponent();

            sqliteShowList = new SQLiteShowList();
            sqliteShowList.Init();

            //生成Load层
            Grid gridLoading = new Grid
            {
                Background =  new SolidColorBrush(Colors.White)
            };

            TextBlock textBlockLoad = new TextBlock {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 16,
                Text = "Loading..."
            };

            gridLoading.Children.Add(textBlockLoad);
            
            this.gridMain.Children.Add(gridLoading);

            //载入数据
            Task.Run(() => {
                DataView dataView = sqliteShowList.LoadData();
                this.Dispatcher.Invoke(() => {
                    dataGridMain.ItemsSource = dataView;
                    this.gridMain.Children.Remove(gridLoading);
                });

            });

        }



        private void buttonStatisticalData_Click(object sender, RoutedEventArgs e)
        {
            this.ToggleFlyout(0);
        }

        private void ToggleFlyout(int index)
        {
            var flyout = this.Flyouts.Items[index] as Flyout;
            if (flyout == null)
            {
                return;
            }

            flyout.IsOpen = !flyout.IsOpen;
        }

        private void buttonOpenBattleInfo_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            dynamic data = button.DataContext;

            var info = data["FullBattleData"];

            textBlockBattleInfo.Text = info;
            this.ToggleFlyout(0);
        }
    }
}
