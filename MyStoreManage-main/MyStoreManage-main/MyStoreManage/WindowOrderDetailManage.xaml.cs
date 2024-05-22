using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
using MyStoreManage.session_login;
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
    /// Interaction logic for WindowOrderDetailManage.xaml
    /// </summary>
    public partial class WindowOrderDetailManage : Window
    {
        private readonly MyStoreContext _storeContext;
        public WindowOrderDetailManage(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleStaffNameNavigate();
            HandleButttonRole();
        }
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }
        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {

        }
        public void HandleButttonRole()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 1)
            {
                btnOpenOrdersManage.Visibility = Visibility.Hidden;
            }
            else if (role == 2)
            {
                btnOpenProductsManage.Visibility = Visibility.Hidden;
                btnOpenCategoriesManage.Visibility = Visibility.Hidden;
                btnOpenStaffManage.Visibility = Visibility.Hidden;
            }
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
            SessionService.Instance.ClearSession();
            var mainWindow = new MainWindow(_storeContext);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenProductsManage_Click(object sender, RoutedEventArgs e)
        {
            var windowProductManage = new WindowProductManage(_storeContext);
            this.Close();
            windowProductManage.Show();
            e.Handled = true;
        }
        private void btnOpenCategoriesManage_Click(object sender, RoutedEventArgs e)
        {
            var windowCategoryManage = new WindowCategoryManage(_storeContext);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }
        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_storeContext);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
