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
    /// Interaction logic for WindowProductManage.xaml
    /// </summary>
    public partial class WindowProductManage : Window
    {
        private readonly MyStoreContext _context;
        public WindowProductManage(MyStoreContext context)
        {
            InitializeComponent();
            _context = context;
            HandleBeforeLoadGirdView();
            HandleComboboxCategory();
            HandleStaffNameNavigate();
            HandleButttonRole();
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
        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }
        public void HandleComboboxCategory()
        {
            cbbCategory.ItemsSource = _context.Categories.ToList();
        }
        public void HandleBeforeLoadGirdView()
        {
            lvProducts.ItemsSource = _context.Products.Include(x => x.Category).ToList();
        }

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(SessionService.Instance.GetNameInSession());
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
            var windowCategoryManage = new WindowCategoryManage(_context);
            this.Close();
            windowCategoryManage.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_context);
            this.Close();
            windowOrderManage.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_context);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            SessionService.Instance.ClearSession();
            var mainWindow = new MainWindow(_context);
            this.Close();
            mainWindow.Show();
            e.Handled = true;
        }

        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_context);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }

        private void btnOpenOrdersReport_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
