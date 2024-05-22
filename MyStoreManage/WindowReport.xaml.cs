using Microsoft.EntityFrameworkCore;
using MyStoreManage.Models;
using MyStoreManage.session_login;
using System.Windows;
using System.Windows.Controls;


namespace MyStoreManage
{
    /// <summary>
    /// Interaction logic for WindowReport.xaml
    /// </summary>
    public partial class WindowReport : Window
    {
        private readonly MyStoreContext _storeContext;
        public WindowReport(MyStoreContext storeContext)
        {
            InitializeComponent();
            _storeContext = storeContext;
            HandleStaffNameNavigate();
            HandleGirdView();

            dpEndDate.SelectedDate = DateTime.Now.Date;
            dpStartDate.SelectedDate = DateTime.Now.Date.AddMonths(-1);
            HandleButtonAdmin();
        }

        public void HandleStaffNameNavigate()
        {
            txblStaffNameNavigate.Text = SessionService.Instance.GetNameInSession();
        }

        public void HandleGirdView()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 1)
            {
                //lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).ToList();

                //list order trong vong 1 thang ke tu ngay hien tai
                DateTime currentDate = DateTime.Now;
                DateTime oneMonthAgo = currentDate.AddMonths(-1);
                lvOrders.ItemsSource = _storeContext.Orders
                                        .Include(x => x.Staff)
                                        .Where(od => od.OrderDate >= oneMonthAgo && od.OrderDate <= currentDate)
                                        .ToList();

            }
            else if (role == 2)
            {
                var idStaff = SessionService.Instance.GetStaffIdInSession();
                lvOrders.ItemsSource = _storeContext.Orders.Include(x => x.Staff).Where(od => od.StaffId == idStaff).ToList();
            }

        }

        public void HandleLoadGirdViewDetail(int idDetail)
        {
            lvOrderDetails.ItemsSource = _storeContext.OrderDetails.Where(od => od.OrderId == idDetail).ToList();
        }

        private void DetailButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var orderId = (int)button.Tag;

            HandleLoadGirdViewDetail(orderId);
        }

        private void btnSearch_Click(object sender, RoutedEventArgs e)
        {
            string searchQuery = txtSearch.Text.Trim();

            if (int.TryParse(searchQuery, out int searchId))
            {
                // Tìm kiếm theo id
                var filteredOrders = _storeContext.Orders
                    .Include(x => x.Staff)
                    .Where(od => od.StaffId == searchId)
                    .ToList();

                lvOrders.ItemsSource = filteredOrders;
            }
            else
            {
                // Tìm kiếm theo name
                var filteredOrders = _storeContext.Orders
                    .Include(x => x.Staff)
                    .Where(od => od.Staff.Name.Contains(searchQuery))
                    .ToList();

                lvOrders.ItemsSource = filteredOrders;
            }
        }

        public void HandleButtonAdmin()
        {
            var role = SessionService.Instance.GetRoleInSession();
            if (role == 1)
            {
                //
            }
            else if (role == 2)
            {
                lbsearchID.Visibility = Visibility.Hidden;
                txtSearch.Visibility = Visibility.Hidden;
                btnSearch.Visibility = Visibility.Hidden;
            }

        }

        private void btnSearchDate_Click(object sender, RoutedEventArgs e)
        {
            DateTime endDate = dpEndDate.SelectedDate ?? DateTime.Now.Date;
            DateTime startDate = dpStartDate.SelectedDate ?? endDate.AddMonths(-1);

            var filteredOrders = _storeContext.Orders
                .Include(x => x.Staff)
                .Where(od => od.OrderDate >= startDate.Date && od.OrderDate <= endDate.Date)
                .ToList();

            lvOrders.ItemsSource = filteredOrders;
        }

        private void btnOpenOrdersManage_Click(object sender, RoutedEventArgs e)
        {
            var windowOrderManage = new WindowOrderManage(_storeContext);
            this.Close();
            windowOrderManage.Show();
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

        private void btnOpenReport_Click(object sender, RoutedEventArgs e)
        {
            var windowReport = new WindowReport(_storeContext);
            this.Close();
            windowReport.Show();
            e.Handled = true;
        }

        private void btnOpenStaffManage_Click(object sender, RoutedEventArgs e)
        {
            var windowStaffManage = new WindowStaffManage(_storeContext);
            this.Close();
            windowStaffManage.Show();
            e.Handled = true;
        }

        private void btnOpenMyAccount_Click(object sender, RoutedEventArgs e)
        {
            var WindowMyProfile = new WindowMyProfile(_storeContext);
            this.Close();
            WindowMyProfile.Show();
            e.Handled = true;
        }
    }
}
