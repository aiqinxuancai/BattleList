using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
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

        public MainWindow()
        {
            InitializeComponent();
            battleListManager = new BattleListManager();
            battleListManager.LoadData();


            //dataGridMain.DataContext = battleListManager.m_battleList;
            dataGridMain.ItemsSource = battleListManager.m_battleList;
        }





    }
}
