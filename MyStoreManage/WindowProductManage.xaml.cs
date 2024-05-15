using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
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
using System.Windows.Shapes;

namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowProductManage.xaml
    /// </summary>
    public partial class WindowProductManage : Window
    {
        private readonly MyStoreContext _storeContext;
        public WindowProductManage(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleBeforeLoadGirdView();
            HandleComboboxCategory();
        }
        public void HandleComboboxCategory()
        {

            cbbCategory.ItemsSource = _storeContext.Categories.ToList();
        }
        public void HandleBeforeLoadGirdView()
        {
            lvProducts.ItemsSource = _storeContext.Products.Include(x => x.Category).ToList();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {

        }
        //ToolBar
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_storeContext);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_storeContext);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_storeContext);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = new MainWindow(_storeContext);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }
    }
}
