using MahApps.Metro.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        private readonly ICollectionView _dataSource;

        public MainWindow()
        {
            InitializeComponent();
            battleListManager = new BattleListManager();
            battleListManager.LoadData();

            //_dataSource = new CollectionView(battleListManager.m_battleList);
            //_dataSource.Filter = (i => ((JObject)i)["MapPointName"].ToString().Contains("Boss"));
            //dataGridMain.DataContext = _dataSource;
            dataGridMain.ItemsSource = battleListManager.m_battleList;//battleListManager.m_battleList;
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
    }
}
