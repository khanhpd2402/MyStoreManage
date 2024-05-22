using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
using MyStoreManage.session_login;
using System.Windows;

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
        private void btnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var windowReport = new WindowReport(_context);
            this.Close();
            windowReport.Show();
            e.Handled = true;
        }

        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_context);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }
    }
}
